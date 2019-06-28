namespace t3hnet
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using t3hnet.Control;

    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: const config file strings.

            // The first thing we do is create the logger and wrap the entire application in the logger.
            // This makes sure that application level errors are logged - failures in DI registrations and basic config.
            // The logger is one of the rare bits of cross cutting infrastructure that lives above the IoC container.
            // Its best to adhere to the Microsoft ILogger interface, leaves options open.
            // This global catch-all logger is the only place where Serilog is used directly with the Microsoft ILogger interface.
            // The Serilog logger usage is all shoved into testable methods because we can't have our root live problem diagnostic fail. 

            var logger = RootLogger.CreateLogger();
            logger.Information("==Start==");

            RootLogger.TryCatchLogFlushThrow(()=>
            {
                var webHostBuilder = CreateWebHostBuilder(args);
                var webHost = webHostBuilder.Build();
                logger.Information("Starting WebHost...");
                webHost.Start();
                logger.Information("... WebHost started.");
                using (var scope = webHost.Services.CreateScope())
                {
                    var session = scope.ServiceProvider.GetService<IServerCommander>();
                    logger.Information("Running session.");
                    session.Start();
                    logger.Information("Session ended.");
                }
            }, logger);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();

            // Load Configs.
            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("config/hostConfig.json").Build());
            // AppConfiguration is layered on top of a copy of the hostConfig.
            // The configs-delegates are run after the host config is built.
            builder.ConfigureAppConfiguration(configBuilder => { configBuilder.AddJsonFile("config/webConfig.json"); });

            // ContentRoot: AppContext.BaseDirectory is the default (WebHostBuilder.BuildCommonServices() called by IWebHost.Build()).
            builder.UseContentRoot(AppContext.BaseDirectory);
            // WebRoot: wwwroot is the default (HostingEnvironmentExtensions.Initialize() called by WebHostBuilder.BuildCommonServices())
            builder.UseWebRoot("wwwroot");


            // Register Services with IoC container.
            builder.ConfigureServices(ConfigureCommanderServices());
            // WebHost calls the 2 methods of Startup to configure the pipeline during Start().
            builder.ConfigureServices(AddStartup());
            // The DI registration for IServer.
            builder.UseKestrel();

            return builder;
        }

        private static Action<IServiceCollection> AddStartup()
        {
            return di =>
            {
                //This is what UseStartup<>() does if you fulfill the interface.
                //However if it doesn't then it uses convention based startup using the environment to decide the method name.
                //The convention based startup also allow dependency injection into the Configure(...) methods.
                di.AddSingleton<IStartup, Startup>();
            };
        }

        private static Action<IServiceCollection> ConfigureCommanderServices()
        {
            return di =>
            {
                di.AddScoped<IServerCommandCommunication, ConsoleServerCommandCommunication>();
                di.AddScoped<INow, DateTimeNow>();
                di.AddScoped<IServerCommander, ServerCommander>();
            };
        }
    }
}