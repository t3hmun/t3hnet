namespace t3hnet.Control
{
    public interface INow
    {
        /// <summary>A string of local HH:mm:ss (24 hour time).</summary>
        /// <returns>HH:mm:ss</returns>
        string Time();
    }
}