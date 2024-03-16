using System.Net;

namespace SmallsOnline.Subnetting.Lib.Models;

/// <summary>
/// A representation of an IPv4 subnet mask.
/// </summary>
public class IPv4SubnetMask
{
    /// <summary>
    /// Create from a CIDR notation.
    /// </summary>
    /// <param name="cidr">The CIDR notation of the subnet mask.</param>
    public IPv4SubnetMask(double cidr)
    {
        WildcardMask = GetWildcardMaskFromCidr(cidr);
        Octets = GetSubnetMaskOctetsFromWildcardMask(WildcardMask);
        CidrNotation = cidr;
        TotalAddresses = (int)GetMaxAddressesFromCidr(cidr);
    }

    /// <summary>
    /// Create from a byte array.
    /// </summary>
    /// <param name="bytes">The byte array of the subnet mask.</param>
    public IPv4SubnetMask(byte[] bytes)
    {
        Octets = GetOctetsFromBytes(bytes);
        WildcardMask = new(bytes);
        CidrNotation = GetCidrMaskFromSubnetMask(Octets);
        TotalAddresses = (int)GetMaxAddressesFromCidr(CidrNotation);
    }

    /// <summary>
    /// The octets of the subnet mask.
    /// </summary>
    public Octet[] Octets { get; }

    /// <summary>
    /// The wildcard mask of the subnet mask.
    /// </summary>
    public IPv4WildcardMask WildcardMask { get; }

    /// <summary>
    /// The CIDR notation of the subnet mask.
    /// </summary>
    public double CidrNotation { get; }

    /// <summary>
    /// The total amount of addresses available from the subnet mask.
    /// </summary>
    public int TotalAddresses { get; }

    /// <summary>
    /// Gets the last octet that was borrowed from.
    /// </summary>
    /// <returns>The last octet borrowed from.</returns>
    public Octet GetLastUsedOctet()
    {
        List<Octet> octetsList = new(Octets);

        Octet lastUsedOctet = octetsList.FindAll((octetItem) => octetItem.BitsUsed == true)[0];

        return lastUsedOctet;
    }

    /// <summary>
    /// Returns the subnet mask as a byte array.
    /// </summary>
    /// <returns>A byte array of the subnet mask.</returns>
    public byte[] ToBytes()
    {
        byte[] byteArray = new byte[4];
        for (int i = 0; i < Octets.Length; i++)
        {
            byteArray[i] = (byte)(byte.MinValue + Octets[i].ToByte());
        }

        return byteArray;
    }

    /// <summary>
    /// Returns the subnet mask as an IPAddress.
    /// </summary>
    /// <returns>An IPAddress type.</returns>
    public IPAddress ToIPAddress()
    {
        return new(ToBytes());
    }

    /// <summary>
    /// Displays the subnet mask as a string.
    /// </summary>
    /// <returns>A string representation of the subnet mask.</returns>
    public override string ToString()
    {
        return string.Join(".", ToBytes());
    }

    /// <summary>
    /// Gets the value of the octets for the subnet mask from a byte array.
    /// </summary>
    /// <param name="bytes">A byte array of the subnet mask.</param>
    /// <returns>An array of the octets.</returns>
    private static Octet[] GetOctetsFromBytes(byte[] bytes)
    {
        Octet[] octets = new Octet[4];
        for (int i = 0; i < octets.Length; i++)
        {
            octets[i] = new(i + 1, bytes[i]);
        }

        return octets;
    }

    /// <summary>
    /// Calculate the total amount of addresses available from a CIDR notation.
    /// </summary>
    /// <param name="cidr">The CIDR notation of the subnet mask.</param>
    /// <returns>The total amount of addresses.</returns>
    private static double GetMaxAddressesFromCidr(double cidr)
    {
        // Get the maximum amount of addresses that can be used.
        // Calculated by: 2^(32-cidrNotation)
        return Math.Pow(2, GetBitBlockFromCidr(cidr));
    }

    private static double GetBitBlockFromCidr(double cidr)
    {
        return 32 - cidr;
    }

    /// <summary>
    /// Get the wildcard mask of the subnet mask from a CIDR notation.
    /// </summary>
    /// <param name="cidr">The CIDR notation of the subnet mask.</param>
    /// <returns>The wildcard mask of the subnet mask.</returns>
    private static IPv4WildcardMask GetWildcardMaskFromCidr(double cidr)
    {
        byte[] wildcardByteArray = new byte[4];

        double maxAddresses = GetMaxAddressesFromCidr(cidr);

        // Get the amount of bytes filled.
        // This is calculated by getting the log of the max addresses from the base of 256 and rounding to the lowest number.
        double bytesFilled = Math.Floor(Math.Log(maxAddresses, 256));

        // Get the amount of bits used.
        // This is calculated by: maxAddresses / 256^bytesFilled
        double bitsUsed = maxAddresses / Math.Pow(256, bytesFilled);

        // Determine the position to fill the amount of bits used.
        int byteArrayPosition = bytesFilled switch
        {
            0 => 3,
            1 => 2,
            2 => 1,
            3 => 0,
            _ => 0
        };

        // Set the position in the wildcard byte array to the amount of bits used.
        // The value of 'bitsUsed' is subtracted by 1, since a byte is denoted from 0 - 255.
        wildcardByteArray[byteArrayPosition] = (byte)(bitsUsed - 1);

        // If needed, fill the remaining bytes to the right of the previously modified byte position.
        for (int i = byteArrayPosition + 1; i < wildcardByteArray.Length; i++)
        {
            wildcardByteArray[i] = byte.MaxValue;
        }

        return new(wildcardByteArray, true);
    }

    /// <summary>
    /// Get the octets of a subnet mask from the wildcard mask.
    /// </summary>
    /// <param name="wildcardMask">The wildcard mask of a subnet mask.</param>
    /// <returns>An array of octets of the subnet mask.</returns>
    private static Octet[] GetSubnetMaskOctetsFromWildcardMask(IPv4WildcardMask wildcardMask)
    {
        Octet[] octets = new Octet[4];
        for (int i = wildcardMask.WildcardBytes.Length - 1; i >= 0; i--)
        {
            octets[i] = new(i + 1, (byte)(byte.MaxValue - wildcardMask.WildcardBytes[i]));
        }

        return octets;
    }

    /// <summary>
    /// Get the CIDR notation from the subnet mask's octets.
    /// </summary>
    /// <param name="octets">The octets of the subnet mask.</param>
    /// <returns>The CIDR notation of the subnet mask.</returns>
    private static double GetCidrMaskFromSubnetMask(Octet[] octets)
    {
        List<BitRepresentation> bitList = new();
        foreach (Octet octetItem in octets)
        {
            bitList.AddRange(octetItem.BinaryValue.BitValues);
        }
        int countOfUsedBits = bitList.FindAll((item) => item.BitIsOn == false).Count;

        return 32 - countOfUsedBits;
    }

    /// <summary>
    /// Calculate the total amount of addresses from the octets of a subnet mask.
    /// </summary>
    /// <param name="octet">The octets of the subnet mask.</param>
    /// <returns>The total amount of addresses.</returns>
    private static int GetTotalAddressesFromSubnetMask(Octet octet)
    {
        // Note:
        // ------
        // This is still bugging out.
        // 255.255.255.0 (/24), 255.255.0 (/16), and 255.0.0.0 (/8) are still returning 0.
        // Since I've gotten the CIDR notation being calculated already, I've switched to using
        // the GetMaxAddresses_FromCidr() method instead. Will keep looking into fixing this.
        // ------

        return (int)(Math.Pow(256, octet.OctetPosition - 1) * octet.BinaryValue.GetUnusedBits());
    }
}
