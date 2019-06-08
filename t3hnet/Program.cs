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

            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("config/hostConfig.json").Build());
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("config/webConfig.json");
            });

            builder.ConfigureServices(ConfigureCommanderServices());

            builder.UseKestrel();
            builder.UseStartup<Startup>();

            return builder;
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