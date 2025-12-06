using System.Text;
using System.Text.RegularExpressions;

namespace Application.Common.Utilities;

public static class StringHelper
{
    private static readonly Random Random = new();
    public static int Generate(int min, int max) => Random.Next(min, max);

    // ref: https://stackoverflow.com/questions/2920744/how-to-generate-a-slug-from-a-string-in-c
    public static string ToSlugName(string value)
    {
        value = value.ToLowerInvariant();

        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);  //Remove all accents
        value = Encoding.ASCII.GetString(bytes);
  
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);  //Replace spaces

        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);  //Remove invalid chars

        value = value.Trim('-', '_'); //Trim dashes from end
    
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);  //Replace double occurences of - or _

        return value;
    }
}
