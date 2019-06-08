namespace t3hnet.Control
{
    using System;

    internal class DateTimeNow : INow
    {
        public string Time()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }
}