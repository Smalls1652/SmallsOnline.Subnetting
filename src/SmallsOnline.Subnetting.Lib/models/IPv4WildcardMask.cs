using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class IPv4WildcardMask
    {
        public IPv4WildcardMask(byte[] bytes, bool isAlreadyCalculated = false)
        {
            if (isAlreadyCalculated)
            {
                wildcardBytes = bytes;
            }
            else
            {
                wildcardBytes = new byte[4];
                for (int i = 0; i < bytes.Length; i++)
                {
                    wildcardBytes[i] = (byte)(byte.MaxValue - bytes[i]);
                }
            }
        }

        public byte[] WildcardBytes
        {
            get => wildcardBytes;
        }

        private readonly byte[] wildcardBytes;

        public override string ToString()
        {
            return string.Join<byte>(".", wildcardBytes);
        }
    }
}