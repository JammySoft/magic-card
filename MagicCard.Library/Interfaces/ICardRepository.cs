namespace MagicCard.Library.Interfaces
{
    public interface ICardRepository 
    {
      /// <summary>
      /// Create or retrieve an ICard.
      /// </summary>
      /// <param name="startingBalance">The starting balance to use if we create a card.</param>
      /// <param name="pin">The pin to use if creating a card.</param>
      /// <param name="accountNumber">The account number associated with the card.</param>
      /// <returns>An <see cref="ICard"/> interface.</returns>
        ICard GetOrAddCard(decimal startingBalance, string pin, string accountNumber);
    }
}
