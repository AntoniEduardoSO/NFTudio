namespace NFTudio.Core.Extensions;
public static class StringExtensions
{
    public static bool HasValue(this string? s) =>
        !string.IsNullOrWhiteSpace(s);
}