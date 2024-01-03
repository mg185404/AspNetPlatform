using static System.Net.Mime.MediaTypeNames;

namespace Platform
{
    public class TerminalQueryStringMiddleware
    {
        private RequestDelegate? next;
        public TerminalQueryStringMiddleware()
        {

        }

        //Terminal class based Middleware
        //The component will forward requests only when the constructor has been provided with a non-null
        //value for the nextDelegate parameter

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
            && context.Request.Query["custom"] == "true")
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "text/plain";

                }
                await context.Response.WriteAsync("Terminal Class-based Middleware \n");
            }
            if (next != null)
            {
                await next(context);
            }
        }
    }
}

