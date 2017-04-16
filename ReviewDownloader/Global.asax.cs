using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ReviewDownloader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // CONFIGURE SERILOG
            Log.Logger = new LoggerConfiguration()
            .WriteTo
            .RollingFile(new JsonFormatter(renderMessage: true), @"C:\inetpub\wwwroot\ReviewCuratorPublish\App_Data\Logs\{Date}.txt")
            .CreateLogger();
        }
    }
}
