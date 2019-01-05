using System;namespace MagicCard.Library.Interfaces{    /// <summary>    /// Interface for card implementations.    /// </summary>    public interface ICard    {        /// <summary>        /// Account number associated with the card.        /// </summary>        string AccountNumber { get; }        /// <summary>        /// Current available balance on the card.        /// </summary>        decimal Balance { get; }        /// <summary>        /// Top-up the card with an amount.        /// </summary>        /// <param name="amount">The amount to add to the balance.</param>        /// <returns>The new balance.</returns>        decimal Deposit(decimal amount);        /// <summary>        /// Withdraw an amount from the card.        /// </summary>        /// <param name="pin">The pin number to verify.</param>        /// <param name="amount">The amount to withdraw.</param>        /// <returns>The remaining balance.</returns>        /// <exception cref="ArgumentException">Thrown if the pin is invalid or if the withdrawal amount is negative.</exception>        /// <exception cref="InvalidOperationException">Thrown if the resulting balance would be below zero.</exception>        decimal Withdraw(string pin, decimal amount);    }}