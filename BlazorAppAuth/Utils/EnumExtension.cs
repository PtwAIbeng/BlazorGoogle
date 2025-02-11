using System.Text.RegularExpressions;

namespace BlazorAppAuth.Utils
{
    public static class EnumExtension
    {
        public static string GetProperName(this Enum value)
        {
            if (value == null)
                return string.Empty;

            // Convert the enum name to a string
            string enumName = value.ToString();

            // Use Regex to separate words based on capital letters
            string properName = Regex.Replace(
                enumName,
                "([A-Z])",
                " $1"
            ).Trim();

            return properName;
        }
    }
}
