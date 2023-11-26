using HandHistories.Objects.Cards;
using NUnit.Framework;
using System;

namespace HandHistories.Objects.UnitTests.Cards
{
    [TestFixture]
    public class CardEqualityTests
    {
        [Test]
        public void Card_TestEquality()
        {
            if (new Card('2', 'c') != new Card("2", "C"))
            {
                throw new Exception("Card: 2c is not equal 2c.");
            }
        }
        [Test]
        public void HoleCards_TestEquality_ReturnTrue()
        {
            HoleCards holeCards1 = HoleCards.ForHoldem(new Card('2', 'c'), new Card('A', 'S'));
            HoleCards holeCards2 = HoleCards.ForHoldem(new Card('a', 'S'), new Card('2', 'c'));

            Assert.IsTrue(holeCards1.Equals(holeCards2));
            Assert.IsTrue(holeCards1 == holeCards2);
        }


        [Test]
        public void HoleCards_TestEquality_ReturnFalse()
        {
            HoleCards holeCards1 = HoleCards.ForHoldem(new Card('k', 'c'), new Card('A', 'S'));
            HoleCards holeCards2 = HoleCards.ForHoldem(new Card('a', 'S'), new Card('2', 'c'));

            Assert.IsFalse(holeCards1.Equals(holeCards2));
            Assert.IsFalse(holeCards1 == holeCards2);
        }

    }
}
