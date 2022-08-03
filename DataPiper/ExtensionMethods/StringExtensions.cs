using System.Globalization;

namespace DataPiper
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string str)
        {
            str = str.ToLower(); //TextInfo.ToTitleCase doesn't work on all caps
            return new CultureInfo("en-US").TextInfo.ToTitleCase(str);
        }
    }
}
