using System;

namespace MagicCard.Library.Interfaces
{
    public interface ICardRepository
    {
        /// <summary>
        /// Create an ICard.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the card.</param>
        /// <param name="startingBalance">The starting balance for the card.</param>
        /// <param name="pin">The pin to assign to the card.</param>
        /// <returns>An <see cref="ICard"/> interface.</returns>
        /// <exception cref="ArgumentException">Thrown if the starting balance is negative or the pin is invalid.</exception>
        /// <exception cref="ArgumentNullException">Account number or pin is not specified.</exception>
        /// <remarks>Used to create a new ICard.</remarks>
        ICard CreateCard(string accountNumber, decimal startingBalance, string pin);

        /// <summary>
        /// Return an existing ICard.
        /// </summary>
        /// <param name="accountNumber">The account number to find the ICard for.</param>
        /// <returns>An ICard interface pointing to the card instance, or null if a card for that account cannot be found.</returns>
        ICard GetCard(string accountNumber);
    }
}