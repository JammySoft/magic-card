using System;
using System.Collections.Concurrent;
using MagicCard.Library.Interfaces;

namespace MagicCard.Library
{
  /// <summary>
  /// Responsible for handing out instances of ICards.
  /// </summary>
  internal sealed class CardRepository : ICardRepository
  {
    /// <summary>
    /// Maintain dictionary of ICards keyed by the card's account number.
    /// </summary>
    private readonly ConcurrentDictionary<string, ICard> _cards = new ConcurrentDictionary<string, ICard>();

    public ICard GetOrAddCard(decimal startingBalance, string pin, string accountNumber)
    {
      // Validation goes here...      
      if (startingBalance < 0)
        throw new ArgumentException("A positive starting balance must be specified", nameof(startingBalance));

      if (String.IsNullOrWhiteSpace(accountNumber))
        throw new ArgumentNullException(nameof(accountNumber),"An account number must be specified");

      PinValidator.ValidatePin(pin);

      // Use ConcurrentDictionary to create a new card, or retrieve an existing one.
      ICard card = _cards.GetOrAdd(accountNumber, key => new Card(startingBalance, pin, accountNumber));
      return card;
    }
  }
}
