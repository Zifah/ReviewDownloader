using Newtonsoft.Json;
using ReviewCurator.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewDownloader.Models
{
    public class ReviewDownloadRequest
    {
        [JsonProperty("productUrl")]
        public string ProductUrl { set; get; }

        [JsonProperty("count")]
        public int Count { set; get; }

        [JsonProperty("email")]
        public string Email { set; get; }
    }
}