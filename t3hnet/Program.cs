namespace t3hnet
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = CreateWebHostBuilder(args);
            var webHost = webHostBuilder.Build();
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.UseKestrel();
            builder.UseStartup<Startup>();
            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("config/hostConfig.json").Build());
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("config/webConfig.json");
            });
            return builder;
        }
    }
}