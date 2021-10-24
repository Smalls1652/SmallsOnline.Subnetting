using System;
using System.Collections;
using System.Collections.Generic;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class BinaryNumber
    {
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

        public BitRepresentation[] BitValues
        {
            get => bitValues;
        }

        private readonly BitRepresentation[] bitValues;

        public int GetUnusedBits()
        {
            int bitsUnused = 0;

            foreach (BitRepresentation bitItem in bitValues)
            {
                bitsUnused += bitItem.BitActualValue;
            }

            return bitsUnused;
        }

        public int GetUsedBits()
        {
            int bitsUsed = 255;

            foreach (BitRepresentation bitItem in bitValues)
            {
                bitsUsed -= bitItem.BitActualValue;
            }

            return bitsUsed;
        }

        public override string ToString()
        {
            return $"{GetUnusedBits()}";
        }
    }
}