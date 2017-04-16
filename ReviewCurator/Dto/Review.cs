using Newtonsoft.Json;
using System;

namespace ReviewCurator.Dto
{
    public class Review
    {
        [JsonProperty("username")]
        public string UserName { set; get; }
        [JsonIgnore]
        public string UserProfileLink { set; get; }
        [JsonProperty("title")]
        public string Title { set; get; }
        [JsonProperty("review")]
        public string ReviewComment { set; get; }
        [JsonProperty("rating")]
        public int StarRating { set; get; }
        [JsonProperty("link")]
        public string ReviewLink { set; get; }
        [JsonProperty("date")]
        public DateTime? Date { set; get; }
    }
}
