namespace WaterCalculator.Common.Extensions
{
    public static class StringExtensions
    {
        extension(string text)
        {
            public string GetShortDescription(int maxLength)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return string.Empty;

                if (text.Length <= maxLength)
                    return text;

                return $"{text[..maxLength]}...";
            }
        }
    }
}
