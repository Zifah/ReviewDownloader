using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ReviewDownloader.Filters
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Log.Error(actionExecutedContext.Exception, "An unhandled error occurred.{Exception}");
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {

                Content = new StringContent("An unhandled exception has occurred. Contact System Administrator")
            };
        }
    }
}