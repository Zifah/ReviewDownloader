using ReviewCurator.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net;
using ReviewCurator.Utility;
using System.Threading;
using RestSharp.Extensions.MonoHttp;

namespace ReviewCurator.Service
{
    public class AmazonReviewService : IReviewService
    {
        private const string _reviewListDivId = "cm_cr-review_list";
        private const string _reviewTitleDataHook = "review-title";
        private const string _reviewDataHook = "review";
        private const string _reviewAuthorDataHook = "review-author";
        private const string _reviewStarsDataHook = "review-star-rating";
        private const string _reviewDateDataHook = "review-date";
        private const string _reviewBodyDataHook = "review-body";
        private const int _maxPageSize = 50;
        private const int _maxResults = 5000;
        private const string _recent = "recent";
        private const string _utf8 = "UTF8";
        private const string _dataHook = "data-hook";
        private const string _href = "href";
        private const string _div = "div";
        private const string _span = "span";
        private const string _a = "a";
        private const string _i = "i";
        private const string _avpOnlyReviews = "avp_only_reviews";
        private const string _class = "class";
        private const string _serviceName = "Amazon";

        public string Name
        {
            get
            {
                return _serviceName;
            }
        }

        private IList<Review> ProcessReviews(HtmlNode reviewsListDiv)
        {
            // get all the nodes with _reviewDataHook
            var reviewNodes = reviewsListDiv.GetAllChildrenWithAttributeAs(_dataHook, _reviewDataHook, _div, true);
            var derivedNodes = new List<Review>();
            //for each node, get the title, username, rating, body, date and url

            reviewNodes.ToList().ForEach(node =>
            {
                var urlAndTitleLink = node.GetFirstChildWithAttributeAs(_dataHook, _reviewTitleDataHook, _a, true);
                var ratingSpan = node.GetFirstChildWithAttributeAs(_dataHook, _reviewStarsDataHook, _i, true);
                var userNameLink = node.GetFirstChildWithAttributeAs(_dataHook, _reviewAuthorDataHook, _a, true);
                var onDateSpan = node.GetFirstChildWithAttributeAs(_dataHook, _reviewDateDataHook, _span, true);
                var contentSpan = node.GetFirstChildWithAttributeAs(_dataHook, _reviewBodyDataHook, _span, true);

                derivedNodes.Add(new Review
                {
                    UserName = userNameLink == null ? null : HttpUtility.HtmlDecode(userNameLink.InnerText),
                    UserProfileLink = userNameLink == null ? null : $"https://www.amazon.com{userNameLink.GetAttributeValue(_href, null)}",
                    ReviewComment = contentSpan == null ? null : HttpUtility.HtmlDecode(contentSpan.InnerText),
                    Title = urlAndTitleLink == null ? null : HttpUtility.HtmlDecode(urlAndTitleLink.InnerText),
                    ReviewLink = urlAndTitleLink == null ? null : $"https://www.amazon.com{urlAndTitleLink.GetAttributeValue(_href, null)}",
                    Date = onDateSpan == null ? null : (DateTime?)DateTime.Parse(onDateSpan.InnerText.Substring(3)), //remove the first three characters: "on ",
                    StarRating = ratingSpan == null ? 0 : Convert.ToInt32(ratingSpan.Descendants(_span).First().InnerText.Substring(0, 1))
                });
            });

            return derivedNodes;
        }

        private IList<Review> ProcessReviews(string reviewListDiv)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(reviewListDiv);
            return ProcessReviews(doc.DocumentNode);
        }

        private List<string> GenerateUrls(string productPageUrl, out string firstReviewsPageDivContent, int maxResults)
        {
            string reviewsResource = "/product-reviews/";
            string reviewUrl = productPageUrl.Replace("/dp/", reviewsResource).Replace("?", "/?").Replace("#", "/#");
            var endOfUrl = reviewUrl.IndexOf("/", reviewUrl.IndexOf(reviewsResource) + reviewsResource.Length);
            reviewUrl = reviewUrl.Substring(0, endOfUrl);

            var urls = new List<string> { };
            firstReviewsPageDivContent = null;
            // download the page
            // ie=UTF8&pageNumber=1&pageSize=50&sortBy=recent&reviewerType=avp_only_reviews
            var firstReviewsPageDiv = GetPageReviewsList(reviewUrl, new { ie = _utf8, pageSize = _maxPageSize, sortBy = _recent, reviewerType = _avpOnlyReviews });

            if (firstReviewsPageDiv == null)
            {
                throw new ReviewDownloadException("Could not get reviews. Either the URL is invalid, this robot has been blocked, Amazon has updated their markup or the product has no reviews");
            }

            firstReviewsPageDivContent = firstReviewsPageDiv.OuterHtml;
            var reviewCountSpan = firstReviewsPageDiv
                .Descendants(_span)
                .FirstOrDefault(x => x.Attributes[_class].Value == "a-size-base");

            if (reviewCountSpan == null)
            {
                // HTML has changed
                return urls;
            }

            int numberOfReviews = Convert.ToInt32(reviewCountSpan.InnerText.Replace(" reviews", "").Split(' ').Last().Replace(",", ""));
            numberOfReviews = Math.Min(numberOfReviews, maxResults);
            int numberOfPages = (numberOfReviews / _maxPageSize) + ((numberOfReviews % _maxPageSize) == 0 ? 0 : 1);

            // for each page number, generate appropriate URL
            for (int i = 2; i <= numberOfPages; i++)
            {
                urls.Add($"{reviewUrl}/ref=cm_cr_arp_d_paging_btm_{i}?ie={_utf8}&pageNumber={i}&pageSize={_maxPageSize}&sortBy={_recent}&reviewerType={_avpOnlyReviews}");
            }

            return urls;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <exception cref="ReviewDownloadException">This is thrown when there is an issue downloading the HTML content</exception>
        /// <returns></returns>
        private string DownloadPage(string url, Object queryString)
        {
            var webClient = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddObject(queryString);
            var response = webClient.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ReviewDownloadException($"Sorry. We can't reach {_serviceName} at this time. Please, try later");

            return response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewPageUrl"></param>
        /// <param name="queryString"></param>
        /// <exception cref="ReviewDownloadException">This is thrown when there is an issue downloading the HTML content</exception>
        /// <returns></returns>
        private HtmlNode GetPageReviewsList(string reviewPageUrl, Object queryString)
        {
            // download the page
            // ie=UTF8&pageNumber=98&pageSize=50&sortBy=recent&reviewerType=avp_only_reviews
            string pageContent = DownloadPage(reviewPageUrl, queryString);
            // get the number of reviews
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);
            return doc.GetElementbyId(_reviewListDivId);
        }
        
        public void GetReviewsFromUrl(List<Review> reviews, string url, int delaySeconds = 0, bool useAsync = false, int maxResults = _maxResults)
        {
            string firstPageReviewsDiv = null;
            var reviewPageUrls = GenerateUrls(url, out firstPageReviewsDiv, maxResults);

            if (firstPageReviewsDiv == null)
                return;

            var firstPageReviews = ProcessReviews(firstPageReviewsDiv);
            reviews.AddRange(firstPageReviews);

            if (useAsync)
            {
                // The source of your work items, create a sequence of Task instances.
                Task[] tasks = Enumerable.Range(0, reviewPageUrls.Count).Select(i =>
                    // Create task here.
                    Task.Run(() =>
                    {
                        Thread.Sleep(delaySeconds * 1000);
                        PopulateReviews(new GenerateReviewsInput
                        {
                            url = reviewPageUrls[i],
                            reviewList = reviews
                        });
                    })
                ).ToArray();

                //// Wait on all the tasks.
                Task.WaitAll(tasks);
                reviews = reviews.OrderByDescending(x => x.Date).ToList();
            }

            else
            {
                reviewPageUrls.ForEach(u =>
                {
                    PopulateReviews(new GenerateReviewsInput
                    {
                        url = u,
                        reviewList = reviews
                    });
                    Thread.Sleep(delaySeconds * 1000);
                });
            }
        }

        private static void PopulateReviews(object state)
        {
            // get the journal type
            var input = state as GenerateReviewsInput;
            var amazonService = new AmazonReviewService();
            var reviewList = amazonService.GetPageReviewsList(input.url, new { });
            var processedReviews = amazonService.ProcessReviews(reviewList);
            input.reviewList.AddRange(processedReviews);
        }

        private class GenerateReviewsInput
        {
            public string url { set; get; }
            public List<Review> reviewList { set; get; }
        }

    }
}
