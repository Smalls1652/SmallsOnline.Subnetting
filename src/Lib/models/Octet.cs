using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    /// <summary>
    /// A representation of an octet.
    /// </summary>
    public class Octet
    {
        /// <summary>
        /// Create with the position of the octet and it's byte value.
        /// </summary>
        /// <param name="position">The position of the octet.</param>
        /// <param name="byteItem">The byte value of the octet.</param>
        public Octet(int position, byte byteItem)
        {
            octetPosition = position;
            binaryValue = new(byteItem);
        }

        /// <summary>
        /// The position of the octet.
        /// </summary>
        public int OctetPosition
        {
            get => octetPosition;
        }

        /// <summary>
        /// The binary value of the octet.
        /// </summary>
        public BinaryNumber BinaryValue
        {
            get => binaryValue;
        }

        /// <summary>
        /// If bits have been used.
        /// </summary>
        public bool BitsUsed
        {
            get => binaryValue.GetUsedBits() > 0;
        }

        private readonly int octetPosition;
        private readonly BinaryNumber binaryValue;

        /// <summary>
        /// Return the byte value of the octet.
        /// </summary>
        /// <returns>The byte value of the octet.</returns>
        public byte ToByte()
        {
            return Convert.ToByte(binaryValue.GetUnusedBits());
        }

        /// <summary>
        /// Returns the value of the octet as a string.
        /// </summary>
        /// <returns>The value of the octet as a string.</returns>
        public override string ToString()
        {
            return $"{binaryValue.GetUnusedBits()}";
        }
    }
}