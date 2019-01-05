using System;
using System.Linq;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using Autofac;
using MagicCard.Library.Interfaces;
using MagicCard.Library.IoC;
using Xunit.Abstractions;

namespace MagicCard.UnitTests
{
    /// <summary>
    /// Tests for working with multiple cards.
    /// </summary>
    public class MultipleCardTests
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Some of these tests produce output so log via xUnit.
        /// </summary>
        /// <param name="output"></param>
        public MultipleCardTests(ITestOutputHelper output)
        {
            this._output = output;

            // Setup Ioc container.
            var bldr = new ContainerBuilder();
            bldr.RegisterModule<DefaultModule>();
            var container = bldr.Build();

            this._cardRepository = container.Resolve<ICardRepository>();
        }

        /// <summary>
        /// Make a withdrawal on two cards - resulting balance should be the same
        /// on both of the cards despite only one being used for the withdrawal.
        /// </summary>
        [Fact]
        public void WithdrawOnTwoCardsTest()
        {
            // Create a card
            var card1 = _cardRepository.CreateCard("34567890", 1000m, "1234");
            var card2 = _cardRepository.GetCard("34567890");

            Assert.True(Object.ReferenceEquals(card1, card2));

            // Check card 1 starting balance
            Assert.Equal(1000m, card1.Balance);

            // Check card 2 starting balance
            Assert.Equal(1000m, card2.Balance);

            // Make a withdrawl on card 1.
            card1.Withdraw("1234", 100m);

            // Check ending balance on card 1.
            Assert.Equal(900m, card1.Balance);

            // Check ending balance on card 2.
            Assert.Equal(900m, card2.Balance);
        }

        /// <summary>
        /// Create lots of cards and withdraw 1.0 from each but using one thread.
        /// This would be the case if one person had a pile of cards and used each
        /// one in turn.
        /// </summary>
        [Fact]
        public void WithdrawOnLotsOfCardsWithSingleThreadTest()
        {
            // Create a card
            var card1 = _cardRepository.CreateCard("34567890", 1000m, "1234");

            // Withdraw 1.0 one hundred times.
            for (int i = 0; i < 100; i++)
            {
                ICard card = _cardRepository.GetCard("34567890");
                card.Withdraw("1234", 1m);
            }

            Assert.Equal(900m, card1.Balance);
        }

        /// <summary>
        /// Withdraw random amounts from multiple cards on multiple threads.  This would simulate
        /// the case where lots of people had copies of the card associated with the same account, 
        /// and all made withdawals at approximately the same time.
        /// </summary>
        [Fact]
        public void WithdrawOnLotsOfCardsOnMultipleThreadTest()
        {
            var startingBalance = 10000m;

            // Create a card and set initial balance.
            var card1 = _cardRepository.CreateCard("34567890", startingBalance, "1234");

            // Create 100 other cards.
            var otherCards = Enumerable.Range(1, 100)
                .Select(idx => _cardRepository.GetCard("34567890"))
                .ToArray();

            var random = new Random();
            long runningTotal = 0;

            // Create many tasks to make withrawls on different threads.
            Task[] tasks = otherCards.Select(card => Task.Run(() =>
            {
                // Randomize the withdrawl amount.
                var amountToWithDraw = random.Next(1, 10);

                // Keep track of running total.
                Interlocked.Add(ref runningTotal, amountToWithDraw);

                // Make the withdrawal.
                decimal balance = card.Withdraw("1234", amountToWithDraw);

                // Show the thread being used in the logging output.
                this._output.WriteLine(
                    $"Withdrew {amountToWithDraw} on thread {Thread.CurrentThread.ManagedThreadId} - balance is {balance}");
            })).ToArray();

            Task.WaitAll(tasks);
            Assert.Equal(startingBalance - runningTotal, card1.Balance);
        }
    }
}