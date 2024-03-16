namespace SmallsOnline.Subnetting.Lib.Models;

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
        OctetPosition = position;
        BinaryValue = new(byteItem);
    }

    /// <summary>
    /// The position of the octet.
    /// </summary>
    public int OctetPosition { get; }

    /// <summary>
    /// The binary value of the octet.
    /// </summary>
    public BinaryNumber BinaryValue { get; }

    /// <summary>
    /// If bits have been used.
    /// </summary>
    public bool BitsUsed
    {
        get => BinaryValue.GetUsedBits() > 0;
    }

    /// <summary>
    /// Return the byte value of the octet.
    /// </summary>
    /// <returns>The byte value of the octet.</returns>
    public byte ToByte()
    {
        return Convert.ToByte(BinaryValue.GetUnusedBits());
    }

    /// <summary>
    /// Returns the value of the octet as a string.
    /// </summary>
    /// <returns>The value of the octet as a string.</returns>
    public override string ToString()
    {
        return $"{BinaryValue.GetUnusedBits()}";
    }
}
