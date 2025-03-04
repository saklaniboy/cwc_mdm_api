using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cwcerp.MDM_API
{
    public class HttpMethodFilterMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpMethodFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Head || context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                return;
            }

            await _next(context);
        }
    }
}
