namespace SmallsOnline.Subnetting.Lib.Models;

/// <summary>
/// A representation of the wildcard mask of an IPv4 subnet mask.
/// </summary>
public class IPv4WildcardMask
{
    /// <summary>
    /// Generate the wildcard mask from a byte array.
    /// </summary>
    /// <param name="bytes">A byte array of a subnet mask or wildcard mask.</param>
    /// <param name="isAlreadyCalculated">Whether the input byte array has already been calculated.</param>
    public IPv4WildcardMask(byte[] bytes, bool isAlreadyCalculated = false)
    {
        if (isAlreadyCalculated)
        {
            WildcardBytes = bytes;
        }
        else
        {
            WildcardBytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                WildcardBytes[i] = (byte)(byte.MaxValue - bytes[i]);
            }
        }
    }

    /// <summary>
    /// The bytes of the wildcard mask.
    /// </summary>
    public byte[] WildcardBytes { get; }

    /// <summary>
    /// Displays the wildcard mask as a string.
    /// </summary>
    /// <returns>A string representation of the wildcard mask.</returns>
    public override string ToString()
    {
        return string.Join(".", WildcardBytes);
    }
}
