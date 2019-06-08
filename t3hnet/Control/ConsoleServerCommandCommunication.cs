namespace t3hnet.Control
{
    using System;

    internal class ConsoleServerCommandCommunication : IServerCommandCommunication
    {
        public string NextCommand()
        {
            return Console.ReadLine();
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}