using System;
using System.Net;
using System.Collections.Generic;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class IPv4SubnetMask
    {
        public IPv4SubnetMask(double cidr)
        {
            wildcardMask = GetWildcardMask_FromCidr(cidr);
            octets = GetSubnetMaskOctets_FromWildcardMask(wildcardMask);
            cidrNotation = cidr;
            totalAddresses = (int)GetMaxAddresses_FromCidr(cidr);
        }

        public IPv4SubnetMask(byte[] bytes)
        {
            octets = GetOctets_FromBytes(bytes);
            wildcardMask = new(bytes);
            cidrNotation = GetCidrMask_FromSubnetMask(octets);
            totalAddresses = GetTotalAddresses_FromSubnetMask(GetLastUsedOctet());
        }

        public Octet[] Octets
        {
            get => octets;
        }

        public IPv4WildcardMask WildcardMask
        {
            get => wildcardMask;
        }

        public double CidrNotation
        {
            get => cidrNotation;
        }

        public int TotalAddresses
        {
            get => totalAddresses;
        }

        private readonly Octet[] octets;
        private readonly IPv4WildcardMask wildcardMask;
        private readonly double cidrNotation;
        private readonly int totalAddresses;

        public Octet GetLastUsedOctet()
        {
            List<Octet> octetsList = new(octets);

            Octet lastUsedOctet = octetsList.FindAll((octetItem) => octetItem.BitsUsed == true)[0];

            return lastUsedOctet;
        }

        public byte[] ToBytes()
        {
            byte[] byteArray = new byte[4];
            for (int i = 0; i < octets.Length; i++)
            {
                byteArray[i] = (byte)(byte.MinValue + octets[i].ToByte());
            }

            return byteArray;
        }

        public IPAddress ToIPAddress()
        {
            return new(ToBytes());
        }

        public override string ToString()
        {
            return string.Join(".", ToBytes());
        }

        private static Octet[] GetOctets_FromBytes(byte[] bytes)
        {
            Octet[] _octets = new Octet[4];
            for (int i = 0; i < _octets.Length; i++)
            {
                _octets[i] = new(i + 1, bytes[i]);
            }

            return _octets;
        }

        private static double GetMaxAddresses_FromCidr(double cidr)
        {
            // Get the maximum amount of addresses that can be used.
            // Calculated by: 2^(32-cidrNotation)
            return Math.Pow(2, GetBitBlock_FromCidr(cidr));
        }

        private static double GetBitBlock_FromCidr(double cidr)
        {
            return 32 - cidr;
        }

        private static IPv4WildcardMask GetWildcardMask_FromCidr(double cidr)
        {
            byte[] wildcardByteArray = new byte[4];

            double maxAddresses = GetMaxAddresses_FromCidr(cidr);
            
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

        private static Octet[] GetSubnetMaskOctets_FromWildcardMask(IPv4WildcardMask _wildcardMask)
        {
            Octet[] _octets = new Octet[4];
            for (int i = _wildcardMask.WildcardBytes.Length - 1; i >= 0; i--)
            {
                _octets[i] = new(i + 1, (byte)(byte.MaxValue - _wildcardMask.WildcardBytes[i]));
            }

            return _octets;
        }

        private static double GetCidrMask_FromSubnetMask(Octet[] _octets)
        {
            List<BitRepresentation> bitList = new();
            foreach (Octet octetItem in _octets)
            {
                bitList.AddRange(octetItem.BinaryValue.BitValues);
            }
            int countOfUsedBits = bitList.FindAll((item) => item.BitIsOn == false).Count;

            return 32 - countOfUsedBits;
        }

        private static int GetTotalAddresses_FromSubnetMask(Octet _octet)
        {
            return (int)(Math.Pow(256, _octet.OctetPosition - 1) * _octet.BinaryValue.GetUnusedBits());
        }
    }
}