using System;
using System.Net;


namespace SmallsOnline.Subnetting.Lib.Models
{
    public class IPv4Subnet
    {
        public IPv4Subnet(IPAddress ipAddress, double cidr)
        {
             _subnetMask = new(cidr);
            _networkAddress = GetSubnetBoundary(ipAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        public IPv4Subnet(IPAddress ipAddress, IPv4SubnetMask subnetMask)
        {
             _subnetMask = subnetMask;
            _networkAddress = GetSubnetBoundary(ipAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

        public IPv4Subnet(string networkString)
        {
            ParsedNetAddressString parsedNetAddress = new(networkString);
            _subnetMask = new(parsedNetAddress.CidrNotation);
            _networkAddress = GetSubnetBoundary(parsedNetAddress.IPAddress, _subnetMask);
            _broadcastAddress = GetBroadcastAddress(_networkAddress, _subnetMask);
            _usableHostRange = new(_networkAddress, _broadcastAddress);
        }

         public IPAddress NetworkAddress
        {
            get => _networkAddress;
        }

        public IPv4SubnetMask SubnetMask
        {
            get => _subnetMask;
        }

        public double CidrMask
        {
            get => _subnetMask.CidrNotation;
        }

        public IPAddress BroadcastAddress
        {
            get => _broadcastAddress;
        }

        public double TotalAddresses
        {
            get => _subnetMask.TotalAddresses;
        }

        public double UsableAddresses
        {
            get => _subnetMask.TotalAddresses - 2;
        }

        public UsableHostRange UsableHostRange
        {
            get => _usableHostRange;
        }

        private readonly IPAddress _networkAddress;
        private readonly IPv4SubnetMask _subnetMask;
        private readonly IPAddress _broadcastAddress;
        private readonly UsableHostRange _usableHostRange;

        public override string ToString()
        {
            return $"{_networkAddress}/{CidrMask}";
        }

        private static IPAddress GetSubnetBoundary(IPAddress ipAddress, IPv4SubnetMask subnetMask)
        {
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.ToBytes();
            Octet lastUsedOctet = subnetMask.GetLastUsedOctet();

            byte[] netAddressBytes = new byte[4];
            for (int i = 0; i < netAddressBytes.Length; i++)
            {
                if (i < lastUsedOctet.OctetPosition - 1)
                {
                    netAddressBytes[i] = ipAddressBytes[i];
                }
                else
                {
                    netAddressBytes[i] = ipAddressBytes[i] &= subnetMaskBytes[i];
                }
            }

            return new(netAddressBytes);
        }

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