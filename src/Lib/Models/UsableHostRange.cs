using System.Net;

namespace SmallsOnline.Subnetting.Lib.Models;

/// <summary>
/// A representation of the usable host range of a subnet.
/// </summary>
public class UsableHostRange
{
    /// <summary>
    /// Create from the network and broadcast address.
    /// </summary>
    /// <param name="networkAddress">The network address of the subnet.</param>
    /// <param name="broadcastAddress">The broadcast address of the subnet.</param>
    public UsableHostRange(IPAddress networkAddress, IPAddress broadcastAddress)
    {
        FirstUsableHostAddress = GetFirstUsableHostAddress(networkAddress);
        LastUsableHostAddress = GetLastUsableHostAddress(broadcastAddress);
    }

    /// <summary>
    /// The first usable host address in the subnet.
    /// </summary>
    public IPAddress FirstUsableHostAddress { get; }

    /// <summary>
    /// The last usable host address in the subnet.
    /// </summary>
    public IPAddress LastUsableHostAddress { get; }

    /// <summary>
    /// Get the first usable host address in the subnet.
    /// </summary>
    /// <param name="networkAddress">The network address of the subnet.</param>
    /// <returns>The first usable host address in the subnet.</returns>
    private static IPAddress GetFirstUsableHostAddress(IPAddress networkAddress)
    {
        byte[] networkAddressBytes = networkAddress.GetAddressBytes();

        return new(
            address: [
                networkAddressBytes[0],
                networkAddressBytes[1],
                networkAddressBytes[2],
                (byte)(networkAddressBytes[3] + 1)
            ]
        );
    }

    /// <summary>
    /// Get the last usable host address in the subnet.
    /// </summary>
    /// <param name="broadcastAddress">The broadcast address of the subnet.</param>
    /// <returns>The last usable host address in the subnet.</returns>
    private static IPAddress GetLastUsableHostAddress(IPAddress broadcastAddress)
    {
        byte[] broadcastAddressBytes = broadcastAddress.GetAddressBytes();

        return new(
            address: [
                broadcastAddressBytes[0],
                broadcastAddressBytes[1],
                broadcastAddressBytes[2],
                (byte)(broadcastAddressBytes[3] - 1)
            ]
        );
    }

    public override string ToString()
    {
        return $"{FirstUsableHostAddress} - {LastUsableHostAddress}";
    }
}
