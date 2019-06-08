namespace t3hnet
{
    using Microsoft.Extensions.Configuration;

    class ConsoleServerCommandCommunication : IServerCommandCommunication
    {
        public ConsoleServerCommandCommunication()
        {
         }

        public string NextCommand()
        {
            return System.Console.ReadLine();
        }

        public void Write(string text)
        {
            System.Console.Write(text);
        }

        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}