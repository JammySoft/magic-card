using System;
using Autofac;
using MagicCard.Library.IoC;
using Xunit;
using MagicCard.Library.Interfaces;

namespace MagicCard.UnitTests
{
  /// <summary>
  /// Simple tests for using the ICard interface.
  /// </summary>
  public class CardMethodTests
  {
    private readonly ICardRepository _cardRepository;
    
    public CardMethodTests()
    {
      // Setup Ioc container.
      var bldr = new ContainerBuilder();
      bldr.RegisterModule<DefaultModule>();
      var container = bldr.Build();

      this._cardRepository = container.Resolve<ICardRepository>();
    }

    /// <summary>
    /// Make a withdrawal and check the balance is as expected.
    /// </summary>
    [Fact]
    public void WithdrawTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      // Make a withdrawl
      card.Withdraw("1234", 23.33m);

      // Check ending balance.
      Assert.Equal(976.67m, card.Balance);
    }

    /// <summary>
    /// Make two withdrawals using the same card and ensure the ending and
    /// interim balances are as expected.
    /// </summary>
    [Fact]
    public void SecondWithdrawTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      // Make a withdrawl
      card.Withdraw("1234", 100m);

      // Check ending balance.
      Assert.Equal(900m, card.Balance);

      // Make second withdrawal.
      card.Withdraw("1234", 200m);

      // Check ending balance
      Assert.Equal(700m, card.Balance);
    }

    /// <summary>
    /// Make a deposit and make sure the ending deposit is as expected.
    /// </summary>
    [Fact]
    public void DepositTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      // Make a deposit
      card.Deposit(100m);

      // Check ending balance.
      Assert.Equal(1100m, card.Balance);
    }

    /// <summary>
    /// Make successive deposits and check the interim and end balances.
    /// </summary>
    [Fact]
    public void SecondDepositTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      // Make a deposit
      card.Deposit(100m);

      // Check ending balance.
      Assert.Equal(1100m, card.Balance);

      // Make second deposit
      card.Deposit(11m);

      // Check ending balance.
      Assert.Equal(1111m, card.Balance);
    }

    /// <summary>
    /// Attempt to make a withdrawal without specifying a pin.
    /// </summary>
    [Fact]
    public void WithdrawWithNullPinFailsTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      Assert.Throws<ArgumentException>(() => { card.Withdraw(null, 1000); });
    }

    /// <summary>
    /// Attempt to make a withdrawal specifying an empty pin.
    /// </summary>
    [Fact]
    public void WithdrawWithEmptyStringPinFailsTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      Assert.Throws<ArgumentException>(() => { card.Withdraw("", 1000); });
    }

    /// <summary>
    /// Attempt to make a withdrawal specifying an invalid pin.
    /// </summary>
    [Fact]
    public void WithdrawWithIncorrectPinFailsTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(1000m, "1234", "34567890");

      // Check the starting balance
      Assert.Equal(1000m, card.Balance);

      // Attempt to withdraw with incorrect pin.
      Assert.Throws<ArgumentException>(() => { card.Withdraw("7777", 1000); });
    }
  }
}
