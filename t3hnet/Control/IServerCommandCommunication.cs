namespace t3hnet.Control
{
    public interface IServerCommandCommunication
    {
        /// <summary>Read an input.</summary>
        /// <returns>Input text up to newline.</returns>
        string NextCommand();

        /// <summary>Communicates back to the user.</summary>
        /// <param name="text">Text to send to user.</param>
        void Write(string text);

        /// <summary>Communicates a line of text to the user.</summary>
        /// <param name="text">Text to send user.</param>
        void WriteLine(string text);
    }
}