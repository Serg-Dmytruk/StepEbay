namespace StepEbay.Common.Helpers
{
    public static class UnixHelper
    {
        private static readonly DateTime Unix = new(1970, 1, 1);

        public static long ToUnix(this DateTime dateTime)
        {
            return (long)(dateTime - Unix).TotalSeconds < 1 ? 10 : (long)(dateTime - Unix).TotalSeconds;
        }

        public static DateTime ToDateTime(this long seconds)
        {
            return Unix.AddSeconds(seconds);
        }
    }
}
