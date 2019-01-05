using System;

namespace MagicCard.Library
{
  /// <summary>
  /// Validates a users pin.
  /// </summary>
  internal static class PinValidator 
  {
    /// <summary>
    /// Validate that the pin is a string representing a positive integer.
    /// </summary>
    /// <param name="pin">The pin to validate.</param>
    public static void ValidatePin(string pin)
    {
      if (String.IsNullOrWhiteSpace(pin))
        throw new ArgumentNullException(nameof(pin));

      if (!int.TryParse(pin, out int result))
        throw new ArgumentException("Pin is not a number", nameof(pin));

      // Check the pin number is a positive integer.
      if (result < 0)
        throw new ArgumentException("Pin number must be a positive integer", nameof(pin));
    }
  }
}
