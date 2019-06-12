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
            // Now all the other middleware have run and had a change to set the response.

            // If the end of the pipeline is reached the response is automatically changed to 404.

            if (!context.Response.HasStarted && context.Response.StatusCode == 404)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("nope");
            }
        }
    }
}