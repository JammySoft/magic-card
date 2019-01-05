using System;
using Autofac;
using Xunit;
using MagicCard.Library.Interfaces;
using MagicCard.Library.IoC;

namespace MagicCard.UnitTests
{
  /// <summary>
  /// Tests for creating instances of Cards.
  /// </summary>
  public class CardRepositoryTests
  {
    private readonly ICardRepository _cardRepository;

    public CardRepositoryTests()
    {
      // Setup Ioc container.
      var bldr = new ContainerBuilder();
      bldr.RegisterModule<DefaultModule>();
      var container = bldr.Build();

      this._cardRepository = container.Resolve<ICardRepository>();
    }

    /// <summary>
    /// Create a card and make sure the properties are as expected.
    /// </summary>
    [Fact]
    public void CreateCardTest()
    {
      // Create a card
      var card = _cardRepository.GetOrAddCard(100m, "1234", "34567890");

      // Check balance
      Assert.Equal(100m, card.Balance);

      // Chec account number.
      Assert.Equal("34567890", card.AccountNumber);
    }

    /// <summary>
    /// Get a second a reference to the card but make sure it is the same instance.
    /// </summary>
    [Fact]
    public void CreateSecondCardIsSameInstanceTest()
    {
      // Create first card.
      var card1 = _cardRepository.GetOrAddCard(100m, "1234", "34567890");

      // Create second card - should be the same instance.
      var card2 = _cardRepository.GetOrAddCard(100m, "1234", "34567890");

      Assert.True(Object.ReferenceEquals(card1, card2));
    }

    /// <summary>
    /// Create a second card using different accounts and make sure they are not the same instance.
    /// </summary>
    [Fact]
    public void CreateDifferentCardIsDifferentInstanceTest()
    {
      // Create first card.
      var card1 = _cardRepository.GetOrAddCard(100m, "1234", "34567890");

      // Create second card with different account number.
      var card2 = _cardRepository.GetOrAddCard(100m, "1234", "34567891");

      // Confirm it is a different account.
      Assert.False(Object.ReferenceEquals(card1, card2));

      // Balances should both be 100.
      Assert.Equal(100m, card1.Balance);
      Assert.Equal(100m, card2.Balance);

      // Account numbers should be different.
      Assert.Equal("34567890", card1.AccountNumber);
      Assert.Equal("34567891", card2.AccountNumber);
    }

    /// <summary>
    /// Trying to create a card with a negative balance should fail.
    /// </summary>
    [Fact]
    public void CreateCardWithNegativeBalanceFailsTest()
    {
      // Try and create card.
      Assert.Throws<ArgumentException>(() =>
      {
        var card1 = _cardRepository.GetOrAddCard(-100m, "1234", "34567890");
      });
    }

    /// <summary>
    /// Trying to create a card without a pin should fail.
    /// </summary>
    [Fact]
    public void CreateCardWithoutPinFailsTest()
    {
      // Try and create card.
      Assert.Throws<ArgumentNullException>(() =>
      {
        var card1 = _cardRepository.GetOrAddCard(100m, null, "34567890");
      });
    }

    /// <summary>
    /// Trying to create a card with non-numeric pin should fail.
    /// </summary>
    [Fact]
    public void CreateCardWithNonNumericPinFailsTest()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var card1 = _cardRepository.GetOrAddCard(100m, "ABCD", "34567890");
      });
    }
    
    /// <summary>
    /// Trying to create a card with negative pin should fail.
    /// </summary>
    [Fact]
    public void CreateCardWithNegativePinFailsTest()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        var card1 = _cardRepository.GetOrAddCard(100m, "-234", "34567890");
      });
    }
  }
}
