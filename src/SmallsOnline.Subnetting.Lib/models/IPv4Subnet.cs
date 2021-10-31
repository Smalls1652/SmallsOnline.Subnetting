using System;
using System.Net;

namespace SmallsOnline.Subnetting.Lib.Models
{
    using SmallsOnline.Subnetting.Lib.Enums;
    /// <summary>
    /// A representation of an IPv4 subnet.
    /// </summary>
    public class IPv4Subnet
    {
        /// <summary>
        /// Create from an IP address and CIDR mask.
        /// </summary>
        /// <param name="ipAddress">An IP address in a subnet.</param>
        /// <param name="cidr">The CIDR mask for the subnet.</param>
        public IPv4Subnet(IPAddress ipAddress, double cidr)
        {
            _subnetMask = new(cidr);
            _networkAddress = GetSubnetBoundary(ipAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        /// <summary>
        /// Create from an IP address and CIDR mask.
        /// </summary>
        /// <param name="ipAddress">An IP address in a subnet.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        public IPv4Subnet(IPAddress ipAddress, IPv4SubnetMask subnetMask)
        {
            _subnetMask = subnetMask;
            _networkAddress = GetSubnetBoundary(ipAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

         /// <summary>
        /// Create from an IP address and CIDR mask.
        /// </summary>
        /// <param name="ipAddress">An IP address in a subnet.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        public IPv4Subnet(IPAddress ipAddress, IPAddress subnetMask)
        {
            _subnetMask = new(subnetMask.GetAddressBytes());
            _networkAddress = GetSubnetBoundary(ipAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        /// <summary>
        /// Create from a string of a network.
        /// For example:
        /// 10.21.6.0/18
        /// </summary>
        /// <param name="networkString">A network written in a string format.</param>
        public IPv4Subnet(string networkString)
        {
            ParsedNetAddressString parsedNetAddress = new(networkString);

            _subnetMask = parsedNetAddress.ParsedType switch
            {
                ParsedNetAddressStringType.SubnetMask => new(parsedNetAddress.SubnetMask.GetAddressBytes()),
                _ => new(parsedNetAddress.CidrNotation)
            };

            _networkAddress = GetSubnetBoundary(parsedNetAddress.IPAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        /// <summary>
        /// The network address of the subnet.
        /// </summary>
        public IPAddress NetworkAddress
        {
            get => _networkAddress;
        }

        /// <summary>
        /// The subnet mask of the subnet.
        /// </summary>
        public IPv4SubnetMask SubnetMask
        {
            get => _subnetMask;
        }

        /// <summary>
        /// The CIDR mask of the subnet.
        /// </summary>
        public double CidrMask
        {
            get => _subnetMask.CidrNotation;
        }

        /// <summary>
        /// The broadcast address of the subnet.
        /// </summary>
        public IPAddress BroadcastAddress
        {
            get => _broadcastAddress;
        }

        /// <summary>
        /// The total amount of addresses in the subnet.
        /// </summary>
        public double TotalAddresses
        {
            get => _subnetMask.TotalAddresses;
        }

        /// <summary>
        /// The total amount of usable addresses in the subnet.
        /// </summary>
        public double UsableAddresses
        {
            get => _subnetMask.TotalAddresses - 2;
        }

        /// <summary>
        /// The range of hosts available for use in the subnet.
        /// </summary>
        public UsableHostRange UsableHostRange
        {
            get => _usableHostRange;
        }

        private readonly IPAddress _networkAddress;
        private readonly IPv4SubnetMask _subnetMask;
        private readonly IPAddress _broadcastAddress;
        private readonly UsableHostRange _usableHostRange;

        /// <summary>
        /// Display the subnet as a string.
        /// </summary>
        /// <returns>A string representation of the subnet with the network address and CIDR mask.</returns>
        public override string ToString()
        {
            return $"{_networkAddress}/{CidrMask}";
        }

        /// <summary>
        /// Gets the network address of the subnet from the supplied IP address and the subnet mask.
        /// </summary>
        /// <param name="ipAddress">The IP address in the subnet.</param>
        /// <param name="subnetMask">The subnet mask of the subnet.</param>
        /// <returns>The network address of the subnet.</returns>
        private static IPAddress GetSubnetBoundary(IPAddress ipAddress, IPv4SubnetMask subnetMask)
        {
            // Get the byte arrays of the IP address and the subnet mask.
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.ToBytes();

            // Get the last used octet in the subnet mask.
            Octet lastUsedOctet = subnetMask.GetLastUsedOctet();

            // Create the byte array for generating the network address.
            byte[] netAddressBytes = new byte[4];
            for (int i = 0; i < netAddressBytes.Length; i++)
            {
                if (i < lastUsedOctet.OctetPosition - 1)
                {
                    // If the current loop count is less than the last used octet's position
                    // then set the current index for the network address to the same value as
                    // the IP address.
                    netAddressBytes[i] = ipAddressBytes[i];
                }
                else
                {
                    // If the current loop count is less than the last used octet's position
                    // then set the current index for the network address to the value of a
                    // bitwise AND operation of the IP address and the subnet mask.
                    netAddressBytes[i] = ipAddressBytes[i] &= subnetMaskBytes[i];
                }
            }

            return new(netAddressBytes);
        }

        /// <summary>
        /// Gets the broadcast address of the subnet.
        /// </summary>
        /// <param name="networkAddress">The network address of the subnet.</param>
        /// <param name="subnetMask">The subnet mask of the subnet.</param>
        /// <returns>The broadcast address for the subnet.</returns>
        private static IPAddress GetBroadcastAddress(IPAddress networkAddress, IPv4SubnetMask subnetMask)
        {
            // Create an empty byte array for the broadcast address.
            byte[] broadcastAddressBytes = new byte[4];

            // Get the bytes for the wildcard subnet mask.
            byte[] networkAddressBytes = networkAddress.GetAddressBytes();
            byte[] wildcardBytes = subnetMask.WildcardMask.WildcardBytes;

            // Iterate through each index of the network address bytes and add the amount from the wildcard bytes.
            for (int i = 0; i < broadcastAddressBytes.Length; i++)
            {
                broadcastAddressBytes[i] = (byte)(networkAddressBytes[i] + wildcardBytes[i]);
            }

            return new(broadcastAddressBytes);
        }
    }
}