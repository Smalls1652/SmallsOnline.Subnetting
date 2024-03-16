namespace SmallsOnline.Subnetting.Lib.Models;

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
        BitPosition = position;
        BitIsOn = isOn;
    }

    /// <summary>
    /// The position of the bit.
    /// </summary>
    public int BitPosition { get; }

    /// <summary>
    /// The value of the bit.
    /// 2^bitPosition
    /// </summary>
    public int BitValue
    {
        get => (int)Math.Pow(2, BitPosition);
    }

    /// <summary>
    /// If the bit is on or not.
    /// </summary>
    public bool BitIsOn { get; }

    /// <summary>
    /// The actual value of the bit if it's on or not.
    /// </summary>
    public int BitActualValue
    {
        get => BitIsOn ? BitValue : 0;
    }

    public override string ToString()
    {
        return $"{BitActualValue}";
    }
}
