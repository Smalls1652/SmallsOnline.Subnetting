using System;
using System.Net;

namespace SmallsOnline.Subnetting.Lib.Core
{
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
            double bytesFilled = Math.Round(Math.Log(maxAddresses, 256), MidpointRounding.ToZero);

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

        private static double GetBitBlockFromCidr(double cidrNotation)
        {
            return _maxBits - cidrNotation;
        }
    }
}