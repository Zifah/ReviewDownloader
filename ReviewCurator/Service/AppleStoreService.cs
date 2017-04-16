using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using ReviewCurator.Dto;
using ReviewCurator.Dto.AppleReviewDto;
using ReviewCurator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReviewCurator.Service
{
    public class AppleStoreReviewService : IReviewService
    {
        private const int _maxResults = 500;
        private const string _serviceName = "Apple App Store";
        private string GetAppIdFromUrl(string productUrl)
        {
            var appId = Regex.Match(productUrl, "/app/.+/id(?<id>[0-9]+)").Groups["id"].Value;

            if (string.IsNullOrWhiteSpace(appId))
                throw new ReviewDownloadException("The provided URL cannot be processed. Are you sure this is a valid app URL?");

            return appId;
        }

        public string Name
        {
            get
            {
                return _serviceName;
            }
        }

        private void GetProductReviewsById(List<Review> reviews, string productId, int maxResults)
        {

            var firstPage = DownloadPage(1, productId);
            reviews.AddRange(GetFeedReviews(firstPage));

            var lastPageLink = firstPage.Link.Single(x => x.Rel == "last").Href;
            var lastPage = Convert.ToInt32(Regex.Match(lastPageLink, "/customerreviews/page=(?<lastPage>[1-9]{1}(0)?)/").Groups["lastPage"].Value);

            for (int i = 2; i <= lastPage; i++)
            {
                var theFeed = DownloadPage(i, productId);
                reviews.AddRange(GetFeedReviews(theFeed));

                if (reviews.Count >= maxResults)
                    break;
            }
            
        }

        private IEnumerable<Review> GetFeedReviews(Feed feed)
        {
            return feed.Entry.Where(x => x.Artist == null).Select(entry =>
            {
                return new Review
                {
                    ReviewComment = HttpUtility.HtmlDecode(entry.Content.First(x => x.Type == "text").Text),
                    Date = Convert.ToDateTime(entry.Updated),
                    StarRating = Convert.ToInt32(entry.Rating),
                    Title = HttpUtility.HtmlDecode(entry.Title),
                    UserName = HttpUtility.HtmlDecode(entry.Author.Name)
                };
            });
        }

        private Feed DownloadPage(int pageNumber, string productId)
        {
            string url = $"https://itunes.apple.com/us/rss/customerreviews/page={pageNumber}/id={productId}/sortby=mostrecent/xml";

            var webClient = new RestClient(url);
            var request = new RestRequest(Method.GET);

            var response = webClient.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                throw new ReviewDownloadException($"Sorry. We can't reach {_serviceName} at this time. Please, try later");

            var content = response.Content.Replace("$", "&amp;"); // make the XML serializable

            var feed = XmlHelper.Deserialize<Feed>(content);

            return feed;
        }

        /// <summary>
        /// Apple's RSS feed returns a maximum of 500 reviews per product
        /// </summary>
        /// <param name="url"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public void GetReviewsFromUrl(List<Review> reviews, string url, int delaySeconds = 0, bool useAsync = false, int maxResults = _maxResults)
        {
            GetProductReviewsById(reviews, GetAppIdFromUrl(url), maxResults);
        }
    }

}
