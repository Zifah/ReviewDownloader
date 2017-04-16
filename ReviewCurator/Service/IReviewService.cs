using ReviewCurator.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewCurator.Service
{
    /// <summary>
    /// Interface for a class which gets reviews from a specific website
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">URL for review subject</param>
        /// <param name="delaySeconds">Seconds delay to introduce between each request</param>
        /// <param name="useAsync">Process the requests asychronously to improve turn-around-time</param>
        /// <param name="maxResults">Maximum number of results to retrieve</param>
        /// <returns></returns>
        void GetReviewsFromUrl(List<Review> reviews, string url, int delaySeconds, bool useAsync, int maxResults);

        string Name
        {
            get;
        }
    }
}
