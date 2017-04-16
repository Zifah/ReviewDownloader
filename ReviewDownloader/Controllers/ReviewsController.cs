using ReviewDownloader.BusinessLogic;
using ReviewDownloader.Filters;
using ReviewDownloader.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ReviewDownloader.Controllers
{
    [UnhandledExceptionFilter]
    public class ReviewsController : ApiController
    {
        [ResponseType(typeof(ReviewDownloadResponse))]
        public HttpResponseMessage Post([FromBody]ReviewDownloadRequest input)
        {
            var downloadedReviews = new ReviewDownloadLogic().DownloadReviews(input);
            var finalResponse = Request.CreateResponse(downloadedReviews);
            return finalResponse;
        }
    }
}