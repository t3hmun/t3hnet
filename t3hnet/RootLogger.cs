namespace t3hnet
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    public static class RootLogger
    {
        public static ILogger CreateLogger()
        {
            var loggerConfig = new ConfigurationBuilder().AddJsonFile("config/loggerConfig.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(loggerConfig).CreateLogger();
            var logger = Log.Logger.ForContext<Program>().ForContext("Method", nameof(Program.Main));
            return logger;
        }

        public static void TryCatchLogFlushThrow(Action action, ILogger logger)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Main exception.");
                throw;
            }
            finally
            {
                Log.Logger.Information("==End==");
                Log.CloseAndFlush();
            }
        }
    }
}