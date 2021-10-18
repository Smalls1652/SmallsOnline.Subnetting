using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace SmallsOnline.Subnetting.Lib.Core
{
    using Models;

    public static class Calculator
    {
        // Define the max amount of bits that can be used.
        // 32 bits == 4 bytes
        private static readonly double _maxBits = 32;

        public static double GetMaxAddresses(double cidrNotation)
        {
            // Get the maximum amount of addresses that can be used.
            // Calculated by: 2^(32-cidrNotation)
            return Math.Pow(2, GetBitBlockFromCidr(cidrNotation));
        }

        public static IPAddress GetSubnetMask(double cidrNotation)
        {
            // Get the wildcard bytes and initialize a full byte array.
            byte[] wildcardBytes = GetWildCardBytes(cidrNotation);
            byte[] subnetMaskBytes = { 255, 255, 255, 255 };

            // Loop through each index of the subnetMaskBytes array and subtract the amount of bits used in the respective index from wildcardBytes.
            for (int i = 0; i < subnetMaskBytes.Length; i++)
            {
                // 255 - [wildcard bits]
                subnetMaskBytes[i] = (byte)(subnetMaskBytes[i] - wildcardBytes[i]);
            }

            // Create an IPAddress type from the subnet mask byte array.
            return new(subnetMaskBytes);
        }

        public static byte[] GetWildCardBytes(double cidrNotation)
        {
            // Initialize an empty 4 byte array.
            byte[] wildcardByteArray = new byte[4];

            // Get the max amount of addresses that can be used.
            double maxAddresses = GetMaxAddresses(cidrNotation);

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

            return wildcardByteArray;
        }

        public static IPAddress GetBroadcastAddress(IPAddress networkAddress, double cidrNotation)
        {
            // Create an empty byte array for the broadcast address.
            byte[] broadcastAddressBytes = new byte[4];

            // Get the bytes for the wildcard subnet mask.
            byte[] networkAddressBytes = networkAddress.GetAddressBytes();
            byte[] wildcardBytes = GetWildCardBytes(cidrNotation);

            // Iterate through each index of the network address bytes and add the amount from the wildcard bytes.
            for (int i = 0; i < broadcastAddressBytes.Length; i++)
            {
                broadcastAddressBytes[i] = (byte)(networkAddressBytes[i] + wildcardBytes[i]);
            }

            return new(broadcastAddressBytes);
        }

        public static IPAddress GetSubnetBoundary(IPAddress ipAddress, double cidrNotation)
        {
            // Get the bytes of both the provided IP address and the subnet mask.
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = GetSubnetMask(cidrNotation).GetAddressBytes();

            // Get the position and value of the smallest subnet mask byte.
            SmallestByte smallestSubnetMaskByte = new(subnetMaskBytes);

            // Get the least significant bit of the smallest subnet mask byte.
            int[] bitValues = GetBitsUsed(smallestSubnetMaskByte.Value);

            // Can't use ^1 for the last index of an array yet, so I have to do it the old way.
            // Gonna work on backporting System.Index and System.Range for netstandard2.0 targets.
            int leastSignificantBit = bitValues[bitValues.Length - 1];

            // Find the nearest byte in the IP address to ensure that it's in the right boundary.
            int nearestByte = 0;
            for (int i = 0; i <= ipAddressBytes[smallestSubnetMaskByte.Position]; i++)
            {
                if (i % leastSignificantBit == 0)
                {
                    nearestByte = i;
                }
            }

            // Create the new subnet boundary bytes.
            byte[] subnetBoundaryBytes = new byte[4];
            for (int i = 0; i < ipAddressBytes.Length; i++)
            {
                // If the current loop index is not equal to the position of the smallest subnet mask byte:
                // - Add the current IP address byte.
                // If it is:
                // - Add the nearest byte.
                if (i != smallestSubnetMaskByte.Position)
                {
                    subnetBoundaryBytes[i] = ipAddressBytes[i];
                }
                else
                {
                    subnetBoundaryBytes[i] = (byte)nearestByte;
                }
            }

            // Create the boundary IP address.
            IPAddress subnetBoundary = new(subnetBoundaryBytes);

            return subnetBoundary;
        }

        private static int[] GetBitsUsed(byte byteItem)
        {
            // Create a BitArray of the byte.
            BitArray bitArray = new(new byte[] { byteItem });

            // Iterate through each index of the bit array.
            // The loop goes in reverse.
            List<int> bitValues = new();
            for (int i = bitArray.Length - 1; i >= 0; i--)
            {
                // Convert the value of the item to an integer, which will either be a 0 or 1.
                int bitValue = Convert.ToInt32(bitArray[i]);

                // If the bit value is not 0, then add the full value of that bit.
                if (bitValue != 0)
                {
                    // 2^i
                    int bitValueEnriched = (int)Math.Pow(2, i);
                    bitValues.Add(bitValueEnriched);
                }
            }

            return bitValues.ToArray();
        }

        private static double GetBitBlockFromCidr(double cidrNotation)
        {
            return _maxBits - cidrNotation;
        }
    }
}