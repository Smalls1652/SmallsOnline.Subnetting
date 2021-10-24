using System;
using System.Collections;
using System.Collections.Generic;

namespace SmallsOnline.Subnetting.Lib.Models
{
    /// <summary>
    /// A representation of a binary number.
    /// </summary>
    public class BinaryNumber
    {
        /// <summary>
        /// Create from a byte.
        /// </summary>
        /// <param name="byteItem">A byte value.</param>
        public BinaryNumber(byte byteItem)
        {
            List<BitRepresentation> bitValuesList = new();

            BitArray bitArray = new(new byte[] { byteItem });
            for (int i = bitArray.Length - 1; i >= 0; i--)
            {
                bitValuesList.Add(new(i, bitArray[i]));
            }

            bitValues = bitValuesList.ToArray();
        }

        /// <summary>
        /// An array of the bits used for the binary number.
        /// </summary>
        public BitRepresentation[] BitValues
        {
            get => bitValues;
        }

        private readonly BitRepresentation[] bitValues;

        /// <summary>
        /// Get the amount of unused bits.
        /// </summary>
        /// <returns>The amount of unused bits.</returns>
        public int GetUnusedBits()
        {
            int bitsUnused = 0;

            foreach (BitRepresentation bitItem in bitValues)
            {
                bitsUnused += bitItem.BitActualValue;
            }

            return bitsUnused;
        }

        /// <summary>
        /// Get the amount of used bits.
        /// </summary>
        /// <returns>The amount of used bits.</returns>
        public int GetUsedBits()
        {
            int bitsUsed = 255;

            foreach (BitRepresentation bitItem in bitValues)
            {
                bitsUsed -= bitItem.BitActualValue;
            }

            return bitsUsed;
        }

        /// <summary>
        /// Return the amount of unused bits as a string.
        /// </summary>
        /// <returns>The amount of unused bits.</returns>
        public override string ToString()
        {
            return $"{GetUnusedBits()}";
        }
    }
}