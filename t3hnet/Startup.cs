namespace t3hnet
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using t3hnet.ware;

    /// <summary>Startup is a singleton service that is used to configure the pipeline and its services.</summary>
    /// <remarks> WebHost resolves Startup and then calls ConfigureServices during its initialisation, which is invoked by the
    ///     WebHostBuilder. Configure is called later when the WebHost is started.</remarks>
    public class Startup : StartupBase
    {
        /// <summary>Called during WebHostBuilder.Build to register the services for the pipeline.</summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>Called during IWebHost.Start to build the pipeline.</summary>
        /// <param name="app"></param>
        public override void Configure(IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
            // The StaticFiles MiddleWare default to using the hostingEnvironment.WebRootFileProvider,
            // which is set up during WebHostEnvironmentExtensions.Initialize()<WebHost.BuildCommonServices()<WebHost.Build()
            app.UseMiddleware<StaticFileMiddleware>(Options.Create(new StaticFileOptions
                {FileProvider = hostingEnvironment.WebRootFileProvider}));
            app.UseMiddleware<DefaultReplyMiddleware>();
            //app.Run(async context => { await context.Response.WriteAsync("Salutations"); });
        }
    }
}