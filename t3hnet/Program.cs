namespace t3hnet
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using t3hnet.Control;

    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = CreateWebHostBuilder(args);
            var webHost = webHostBuilder.Build();
            webHost.Start();
            using (var scope = webHost.Services.CreateScope())
            {
                var session = scope.ServiceProvider.GetService<IServerCommander>();
                session.Start();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();

            // Load Configs.
            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("config/hostConfig.json").Build());
            builder.ConfigureAppConfiguration(configBuilder => { configBuilder.AddJsonFile("config/webConfig.json"); });

            // Register Services with IoC container.
            builder.ConfigureServices(ConfigureCommanderServices());
            builder.ConfigureServices(AddStartup()); //WebHost uses Startup to configure the pipeline during its initialisation.
            builder.UseKestrel(); //This is just DI registrations of server stuff.

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