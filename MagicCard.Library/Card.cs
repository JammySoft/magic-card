using System;
using MagicCard.Library.Interfaces;

namespace MagicCard.Library
{
    /// <summary>
    /// Implementation of <see cref="ICard"/>.
    /// </summary>
    internal sealed class Card : ICard
    {
        /// <summary>
        /// Current card balance.
        /// </summary>
        private decimal _balance;

        /// <summary>
        /// Object used for locking.
        /// </summary>
        private readonly object _lock = new Object();

        /// <summary>
        /// The card pin.
        /// </summary>
        private readonly string _pin;

        public Card(string accountNumber, decimal startingBalance, string pin)
        {
            this._balance = startingBalance;
            this._pin = pin;
            this.AccountNumber = accountNumber;
        }

        /// <summary>
        /// Current remaining balance on the card.
        /// </summary>
        public decimal Balance
        {
            get
            {
                lock (this._lock)
                {
                    return this._balance;
                }
            }
        }

        public string AccountNumber { get; }

        public decimal Withdraw(string pin, decimal withdrawAmount)
        {
            if (withdrawAmount <= 0)
                throw new ArgumentException("Can only withdraw amounts greater than zero.", nameof(withdrawAmount));

            ValidatePin(pin);

            lock (_lock)
            {
                // Calculate future balance and make sure it is greater than zero (no overdrafts allowed).
                var futureBalance = this._balance - withdrawAmount;
                if (futureBalance < 0)
                    throw new InvalidOperationException("You have no overdraft facility");

                this._balance = futureBalance;
                return this._balance;
            }
        }

        private void ValidatePin(string pin)
        {
            // In a real system the pin would probably not be stored just as a String.
            if (StringComparer.InvariantCultureIgnoreCase.Compare(this._pin, pin) != 0)
                throw new ArgumentException("Invalid pin specified");
        }

        public decimal Deposit(decimal depositAmount)
        {
            if (depositAmount <= 0)
                throw new ArgumentException("Can only deposit positive amounts", nameof(depositAmount));

            lock (_lock)
            {
                this._balance += depositAmount;
                return this._balance;
            }
        }
    }
}