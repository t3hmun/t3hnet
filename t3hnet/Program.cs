namespace t3hnet
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = CreateWebHostBuilder(args);
            var webHost = webHostBuilder.Build();
            webHost.Start();
            using (var scope = webHost.Services.CreateScope())
            {
                var conf = webHost.Services.GetService<IConfiguration>();
                Interactive(new ConsoleServerCommandCommunication(), conf, new DateTimeNow());
            }
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

        public static void Interactive(IServerCommandCommunication cmd, IConfiguration conf, INow now)
        {
            cmd.WriteLine($"Server started {now.Time()} with config:");
            foreach (var (key, value) in conf.AsEnumerable()) cmd.WriteLine($"Key: {key}, Value: {value}");

            var exit = false;
            while (!exit)
            {
                cmd.WriteLine("Waiting for input.");
                var input = cmd.NextCommand();
                switch (input)
                {
                    case "exit":
                        exit = true;
                        break;
                    default:
                        cmd.WriteLine($"[{now.Time()}] Command not recognised. Type exit to quit.");
                        break;
                }
            }
        }
    }
}