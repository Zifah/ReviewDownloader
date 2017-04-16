using Newtonsoft.Json;
using ReviewCurator.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewDownloader.Models
{
    public class ReviewDownloadResponse
    {
        public ReviewDownloadResponse()
        {
            FirstFewReviews = new List<Review>();
        }

        [JsonProperty("reviewsFileUrl")]
        public string ReviewsFileUrl { set; get; }
        [JsonProperty("firstFewReviews")]
        public List<Review> FirstFewReviews { set; get; }

        [JsonProperty("errorDisplayMessage")]
        public string ErrorDisplayMessage { set; get; }

        [JsonProperty("reviewSource")]
        public string ReviewSource { set; get; }
    }
}