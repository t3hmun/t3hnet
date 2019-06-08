namespace t3hnet
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>Startup is a singleton service that is used to configure the pipeline and its services.</summary>
    /// <remarks> WebHost resolves Startup and then calls ConfigureServices during its initialisation, which is invoked by the
    ///     WebHostBuilder. Configure is called later when the WebHost is started.</remarks>
    public class Startup
    {
        /// <summary>Called during WebHostBuilder.Build to register the services for the pipeline.</summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>Called during IWebHost.Start to build the pipeline.</summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async context => { await context.Response.WriteAsync("Salutations"); });
        }
    }
}