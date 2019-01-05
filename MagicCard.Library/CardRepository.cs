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

        public ICard CreateCard(string accountNumber, decimal startingBalance, string pin)
        {
            #region Parameter validation

            if (startingBalance < 0)
                throw new ArgumentException("A positive starting balance must be specified", nameof(startingBalance));

            if (String.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException(nameof(accountNumber), "An account number must be specified");

            if (String.IsNullOrWhiteSpace(pin))
                throw new ArgumentNullException(nameof(pin));

            if (!int.TryParse(pin, out int result))
                throw new ArgumentException("Pin is not a number", nameof(pin));

            // Check the pin number is a positive integer.
            if (result < 0)
                throw new ArgumentException("Pin number must be a positive integer", nameof(pin));

            #endregion            // Check to see if the card already exsits.

            if (_cards.ContainsKey(accountNumber))
                throw new ArgumentException("A card for that account already exists.", nameof(accountNumber));

            // TryAdd() will return false if another user has created a card on a separate thread, or
            // atomically add it.
            var card = new Card(startingBalance, pin, accountNumber);
            if (!_cards.TryAdd(accountNumber, card))
                throw new ArgumentException("A card for that account already exists.", nameof(accountNumber));

            return card;
        }

        public ICard GetCard(string accountNumber)
        {
            if (String.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException(nameof(accountNumber), "An account number must be specified");

            _cards.TryGetValue(accountNumber, out ICard card);
            return card;
        }
    }
}