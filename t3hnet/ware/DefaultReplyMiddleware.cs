namespace t3hnet.ware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class DefaultReplyMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>This is intended to be the first middleware (therefore last on the way back), sets a default response if no
        ///     other middleware touches the response.</summary>
        /// <param name="next">Next middleware in the pipeline.</param>
        public DefaultReplyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Give all other middleware a chance to set a response.
            await _next(context);


            // An untouched response is null length, 0 length would be a valid empty reply.
            if (context.Response.ContentLength == null)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("nope");
            }
        }
    }
}