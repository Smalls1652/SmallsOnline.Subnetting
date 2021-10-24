using System;

namespace SmallsOnline.Subnetting.Lib.Models
{
    public class BitRepresentation
    {
        public BitRepresentation(int position, bool isOn)
        {
            bitPosition = position;
            bitIsOn = isOn;
        }

        public int BitPosition
        {
            get => bitPosition;
        }

        public int BitValue
        {
            get => (int)Math.Pow(2, bitPosition);
        }

        public bool BitIsOn
        {
            get => bitIsOn;
        }

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