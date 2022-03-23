namespace Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string str) => string.IsNullOrWhiteSpace(str);
    }
}
