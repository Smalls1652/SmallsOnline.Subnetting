using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class SmallestByte
    {
        public SmallestByte(byte[] byteArray)
        {
            GetSmallestByte(byteArray);
        }

        public byte Value
        {
            get => _value;
        }

        public int Position
        {
            get => _position;
        }

        private byte _value;
        private int _position;

        private void GetSmallestByte(byte[] byteArray)
        {
            bool smallestFound = false;
            for (int i = 0; i < byteArray.Length; i++)
            {
                // If the value of the index is not equal to 0 or 255, then set it as the smallest byte.
                if (byteArray[i] != byte.MinValue && byteArray[i] != byte.MaxValue)
                {
                    _value = byteArray[i];
                    _position = i;
                    smallestFound = true;
                }
            }

            // If no smallest byte was found, then forcefully set it to 255 and the last position of the byte array.
            // This is an issue with /24, /16, and /8 networks.
            // I need to determine a solution to this in a clean way.
            if (!smallestFound)
            {
                _value = byte.MaxValue;
                _position = 3;
            }
        }
    }
}