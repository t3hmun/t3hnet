namespace t3hnet.Control
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class ServerCommander : IDisposable, IServerCommander
    {
        private readonly IServerCommandCommunication _cmd;
        private readonly IConfiguration _conf;
        private readonly INow _now;

        public ServerCommander(IServerCommandCommunication cmd, IConfiguration conf, INow now)
        {
            _cmd = cmd;
            _conf = conf;
            _now = now;
        }

        public void Dispose()
        {
            _cmd.WriteLine($"[{_now.Time()}] Interactive session stopping.");
        }

        public void Start()
        {
            _cmd.WriteLine($"Server started {_now.Time()} with config:");
            foreach (var (key, value) in _conf.AsEnumerable()) _cmd.WriteLine($"Key: {key}, Value: {value}");

            var exit = false;
            while (!exit)
            {
                _cmd.WriteLine("Waiting for input.");
                var input = _cmd.NextCommand();
                switch (input)
                {
                    case "exit":
                        exit = true;
                        break;
                    default:
                        _cmd.WriteLine($"[{_now.Time()}] Command not recognised. Type exit to quit.");
                        break;
                }
            }
        }
    }
}