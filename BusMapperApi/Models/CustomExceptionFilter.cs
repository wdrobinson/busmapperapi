using log4net;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace BusMapperApi.Models
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomExceptionFilter));

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Log.Error("Error caught in CustomExceptionFilter: " + actionExecutedContext.Exception);

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent("An error has occured", System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            base.OnException(actionExecutedContext);
        }
    }
}