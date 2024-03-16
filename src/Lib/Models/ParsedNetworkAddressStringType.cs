namespace SmallsOnline.Subnetting.Lib.Models;

/// <summary>
/// The type of parsed network address string.
/// </summary>
public enum ParsedNetworkAddressStringType
{
    /// <summary>
    /// A CIDR notation.
    /// </summary>
    CidrNotation = 0,

    /// <summary>
    /// A subnet mask.
    /// </summary>
    SubnetMask = 1
}
