using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class Octet
    {
        public Octet(int position, byte byteItem)
        {
            octetPosition = position;
            binaryValue = new(byteItem);
        }

        public int OctetPosition
        {
            get => octetPosition;
        }

        public BinaryNumber BinaryValue
        {
            get => binaryValue;
        }

        public bool BitsUsed
        {
            get => binaryValue.GetUsedBits() > 0;
        }

        private readonly int octetPosition;
        private readonly BinaryNumber binaryValue;

        public byte ToByte()
        {
            return Convert.ToByte(binaryValue.GetUnusedBits());
        }

        public override string ToString()
        {
            return $"{binaryValue.GetUnusedBits()}";
        }
    }
}