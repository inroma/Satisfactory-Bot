namespace SatisfactoryBot.Helpers;

using System.Text.RegularExpressions;

internal static partial class RegexHelper
{
    [GeneratedRegex(@"^((https)://|)?((127\.)|(10\.)|(172\.1[6-9]\.)|(172\.2[0-9]\.)|(172\.3[0-1]\.)|(192\.168\.))")]
    internal static partial Regex LocalIpRegex();

    [GeneratedRegex(@"^((?:https://.)?(?:www\.)?(?:(?:(?:[-a-zA-Z0-9.]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-Z]*))|(?:(?:[a-fA-F0-9]*)?:){1,7}[a-fA-F0-9]*)|(?:25[0-5]|2[0-4]\d|1?\d{1,2})(?:\.(?:25[0-5]|2[0-4]\d|[0-1]?\d{1,2})){3})(?:(:[0-9]{4,5}))?/?)$")]
    internal static partial Regex IsValidAddress();

    [GeneratedRegex("(:[0-9]{4,5})/?$")]
    internal static partial Regex UrlEndingWithPort();
}
