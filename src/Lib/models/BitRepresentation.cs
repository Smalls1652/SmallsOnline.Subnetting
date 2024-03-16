using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    /// <summary>
    /// A representation of a bit.
    /// </summary>
    public class BitRepresentation
    {
        /// <summary>
        /// Create from the position of the bit and if it's on.
        /// </summary>
        /// <param name="position">The position of the bit.</param>
        /// <param name="isOn">The bit is on and used.</param>
        public BitRepresentation(int position, bool isOn)
        {
            bitPosition = position;
            bitIsOn = isOn;
        }

        /// <summary>
        /// The position of the bit.
        /// </summary>
        public int BitPosition
        {
            get => bitPosition;
        }

        /// <summary>
        /// The value of the bit.
        /// 2^bitPosition
        /// </summary>
        public int BitValue
        {
            get => (int)Math.Pow(2, bitPosition);
        }

        /// <summary>
        /// If the bit is on or not.
        /// </summary>
        public bool BitIsOn
        {
            get => bitIsOn;
        }

        /// <summary>
        /// The actual value of the bit if it's on or not.
        /// </summary>
        public int BitActualValue
        {
            get => bitIsOn ? BitValue : 0;
        }

        private readonly int bitPosition;
        private readonly bool bitIsOn;

        public override string ToString()
        {
            return $"{BitActualValue}";
        }
    }
}