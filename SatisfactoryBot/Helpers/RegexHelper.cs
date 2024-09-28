namespace SatisfactoryBot.Helpers;

using System.Text.RegularExpressions;

internal static partial class RegexHelper
{
    [GeneratedRegex("^((https|http)://|)((127\\.)|(10\\.)|(172\\.1[6-9]\\.)|(172\\.2[0-9]\\.)|(172\\.3[0-1]\\.)|(192\\.168\\.))")]
    internal static partial Regex LocalIpRegex();
}
