using System.Net;
using System.Text.RegularExpressions;

namespace SmallsOnline.Subnetting.Lib.Models;

/// <summary>
/// A representation of a parsed network address.
/// </summary>
public partial class ParsedNetworkAddressString
{
    /// <summary>
    /// Create from a network string.
    /// </summary>
    /// <param name="networkAddressString">The string of the network.</param>
    public ParsedNetworkAddressString(string networkAddressString)
    {
        Match networkAddressMatch = ParseNetworkAddressString(networkAddressString);

        IPAddress = GetIPAddressFromMatch(networkAddressMatch);
        ParsedType = GetParsedTypeFromMatch(networkAddressMatch);

        if (ParsedType == ParsedNetworkAddressStringType.CidrNotation)
        {
            CidrNotation = GetCidrNotationFromMatch(networkAddressMatch);
        }
        else
        {
            SubnetMask = GetSubnetMaskFromMatch(networkAddressMatch);
        }
    }

    /// <summary>
    /// The parsed IP address.
    /// </summary>
    public IPAddress IPAddress { get; }

    /// <summary>
    /// The parsed CIDR notation.
    /// </summary>
    public double CidrNotation { get; }

    /// <summary>
    /// The parsed subnet mask.
    /// </summary>
    public IPAddress? SubnetMask { get; }

    /// <summary>
    /// The parsed type of the network address string.
    /// </summary>
    public ParsedNetworkAddressStringType ParsedType { get; }

    /// <summary>
    /// Parse the network address string.
    /// </summary>
    /// <param name="networkAddressString">The network address string.</param>
    /// <returns>A <see cref="Match"/> item of the network address string.</returns>
    /// <exception cref="InvalidOperationException">The network address string is not valid.</exception>
    private static Match ParseNetworkAddressString(string networkAddressString)
    {
        Match networkAddressMatch = NetAddressRegex().Match(networkAddressString);

        if (!networkAddressMatch.Success)
        {
            throw new InvalidOperationException("The network address string is not valid.");
        }

        return networkAddressMatch;
    }

    /// <summary>
    /// Get the IP address from the match.
    /// </summary>
    /// <param name="networkAddressMatch">The match of the network address string.</param>
    /// <returns>The IP address from the match.</returns>
    private static IPAddress GetIPAddressFromMatch(Match networkAddressMatch)
    {
        return IPAddress.Parse(networkAddressMatch.Groups["netAddress"].Value);
    }

    /// <summary>
    /// Get the parsed type from the match.
    /// </summary>
    /// <param name="networkAddressMatch">The match of the network address string.</param>
    /// <returns>The parsed type from the match.</returns>
    /// <exception cref="InvalidOperationException">Could not determine the type of the network address string.</exception>
    private static ParsedNetworkAddressStringType GetParsedTypeFromMatch(Match networkAddressMatch)
    {
        if (networkAddressMatch.Groups["cidrNotation"].Success)
        {
            return ParsedNetworkAddressStringType.CidrNotation;
        }
        
        if (networkAddressMatch.Groups["subnetMask"].Success)
        {
            return ParsedNetworkAddressStringType.SubnetMask;
        }

        throw new InvalidOperationException("Could not determine the type of the network address string.");
    }

    /// <summary>
    /// Get the subnet mask from the match.
    /// </summary>
    /// <param name="networkAddressMatch">The match of the network address string.</param>
    /// <returns>The subnet mask from the match.</returns>
    private static IPAddress GetSubnetMaskFromMatch(Match networkAddressMatch)
    {
        return IPAddress.Parse(networkAddressMatch.Groups["subnetMask"].Value);
    }

    /// <summary>
    /// Get the CIDR notation from the match.
    /// </summary>
    /// <param name="networkAddressMatch">The match of the network address string.</param>
    /// <returns>The CIDR notation from the match.</returns>
    private static double GetCidrNotationFromMatch(Match networkAddressMatch)
    {
        return Convert.ToDouble(networkAddressMatch.Groups["cidrNotation"].Value);
    }

    [GeneratedRegex(
        pattern: @"^(?'netAddress'(?:\d{1,3}(?:\.|)){4})(?:(?:\/)(?'cidrNotation'\d{1,2})|(?:\/|\s)(?'subnetMask'(?:\d{1,3}(?:\.|)){4}))$"
    )]
    internal static partial Regex NetAddressRegex();
}
