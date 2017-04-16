using System;
using System.Collections.Generic;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using ReviewCurator.Dto;
using HtmlAgilityPack;
using RestSharp.Extensions.MonoHttp;
using ReviewCurator.Utility;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReviewCurator.Service
{
    public class PlayStoreReviewService : IReviewService
    {
        private const string _androidPackageNameRegex = "\\?id=(?<id>([A-Za-z]{1}[A-Za-z\\d_]*\\.)*[A-Za-z][A-Za-z\\d_]*)";
        private const int _maxResults = 5000;
        private const string _serviceName = "Google Play Store";

        public string Name
        {
            get
            {
                return _serviceName;
            }
        }

        public string DownloadPage(int pageNumber, string packageName)
        {
            var webClient = new RestClient("https://play.google.com/store/getreviews");
            var request = new RestRequest(Method.POST);
            request.AddObject(new GetReviewsRequest
            {
                id = packageName,
                pageNum = pageNumber,
                reviewSortOrder = 0,
                reviewType = 0,
                xhr = 1
            });

            var response = webClient.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ReviewDownloadException($"Sorry. We can't reach {_serviceName} at this time. Please, try later");

            var result = JsonConvert.DeserializeObject<List<string[]>>(response.Content.Substring(6));
            return result[0][2];
        }

        public void GetReviewsFromUrl(List<Review> reviews, string url, int delaySeconds = 0, bool useAsync = false, int maxResults = _maxResults)
        {
            string packageName = Regex.Match(url, _androidPackageNameRegex).Groups["id"].Value;

            if(string.IsNullOrWhiteSpace(packageName))
                throw new ReviewDownloadException("The URL cannot be processed. Are you sure this is a valid product URL?");
            
            
            bool exit = false;
            int pageNumber = 0;
            while (!exit)
            {
                var response = DownloadPage(pageNumber, packageName);
                if (!string.IsNullOrWhiteSpace(response) && maxResults > reviews.Count)
                {
                    var parsedResponse = ParseReviewPage(response);
                    reviews.AddRange(parsedResponse);
                    pageNumber++;
                    continue;
                }
                exit = true;
            }
        }

        public List<Review> ParseReviewPage(string reviewsPageHtml)
        {
            var parsedReviews = new List<Review>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(reviewsPageHtml);
            var reviewContainers = doc.DocumentNode.GetAllDivsWithClass("single-review", true).ToList();

            reviewContainers.ForEach(container => {
                var reviewContent = container.GetDivWithClass("review-body", false);
                var title = container.GetSpanWithClass("review-title", true).InnerText.Trim();
                var reviewUrl = HttpUtility.HtmlDecode(container.GetLinkWithClass("reviews-permalink", true).Attributes["href"].Value);
                var date = Convert.ToDateTime(container.GetSpanWithClass("review-date", true).InnerText.Trim());
                var stars = Convert.ToInt32(container.GetDivWithClass("tiny-star", false).Attributes["aria-label"].Value.Substring(6, 1));
                var authorName = container.GetSpanWithClass("author-name", true).InnerText.Trim();

                parsedReviews.Add(new Review
                {
                    Title = HttpUtility.HtmlDecode(title),
                    ReviewComment = HttpUtility.HtmlDecode(reviewContent.InnerText.Trim().Replace("Full Review", "").Trim()),
                    Date = date,
                    StarRating = stars,
                    ReviewLink = $"https://play.google.com{reviewUrl}",
                    UserName = HttpUtility.HtmlDecode(authorName)
                });
            });

            return parsedReviews;
        }
    }

    public class GetReviewsRequest
    {
        public int reviewType { set; get; }
        public int pageNum { set; get; }
        public string id { set; get; }
        public int reviewSortOrder { set; get; }
        public int xhr { set; get; }
    }
}
