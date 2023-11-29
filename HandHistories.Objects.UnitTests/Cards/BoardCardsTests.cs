using HandHistories.Objects.Cards;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace HandHistories.Objects.UnitTests.Cards
{
    [TestFixture]
    public class BoardCardsTests
    {
        private static Card C1 = new Card("A", "c");
        private static Card C2 = new Card("K", "d");
        private static Card C3 = new Card("2", "h");
        private static Card C4 = new Card("2", "d");
        private static Card C5 = new Card("9", "s");
        private static Card C6 = new Card("7", "s");


        [Test]
        public void BoardCardsTest_EqualityTest_withDefaultComparison_CompareWithSelf_ReturnTrue()
        {
            BoardCards emptyBoard = BoardCards.FromCards(String.Empty);
            BoardCards boardWithFlop = BoardCards.FromCards("AcQsTh");
            BoardCards boardWithTurn = BoardCards.FromCards("AcQsTh2h");
            BoardCards boardWithRiver = BoardCards.FromCards("AcQsTh2h3d");

            Assert.IsTrue(emptyBoard.Equals(emptyBoard));
            Assert.IsTrue(boardWithFlop.Equals(boardWithFlop));
            Assert.IsTrue(boardWithTurn.Equals(boardWithTurn));
            Assert.IsTrue(boardWithRiver.Equals(boardWithRiver));
        }

        [Test]
        public void BoardCardsTest_EqualityTest_withDefaultComparison_DifferentOrder_ReturnTrue()
        {
            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C3, C2);
            BoardCards b3 = BoardCards.ForFlop(C2, C1, C3);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C1);
            BoardCards b5 = BoardCards.ForFlop(C3, C1, C2);
            BoardCards b6 = BoardCards.ForFlop(C3, C2, C1);

            Assert.IsTrue(b1.Equals(b2));
            Assert.IsTrue(b1.Equals(b3));
            Assert.IsTrue(b1.Equals(b4));
            Assert.IsTrue(b1.Equals(b5));
            Assert.IsTrue(b1.Equals(b6));
            
            Assert.IsTrue(b2.Equals(b3));
            Assert.IsTrue(b2.Equals(b4));
            Assert.IsTrue(b2.Equals(b5));
            Assert.IsTrue(b2.Equals(b6));

            Assert.IsTrue(b3.Equals(b4));
            Assert.IsTrue(b3.Equals(b5));
            Assert.IsTrue(b3.Equals(b6));

            Assert.IsTrue(b4.Equals(b5));
            Assert.IsTrue(b4.Equals(b6));

            Assert.IsTrue(b5.Equals(b6));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithDefaultComparision_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C2, C4);
            BoardCards b3 = BoardCards.ForFlop(C1, C3, C4);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C4);

            Assert.IsFalse(b1.Equals(b2));
            Assert.IsFalse(b1.Equals(b3));
            Assert.IsFalse(b1.Equals(b4));
            Assert.IsFalse(b2.Equals(b3));
            Assert.IsFalse(b2.Equals(b4));
            Assert.IsFalse(b3.Equals(b4));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithDefaultComparision_ReturnsTrue()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C3, C2, C4);
            BoardCards b3 = BoardCards.ForTurn(C3, C2, C1, C4);

            Assert.IsTrue(b1.Equals(b2)); 
            Assert.IsTrue(b2.Equals(b3)); 
            Assert.IsTrue(b1.Equals(b3));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithDefaultComparision_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C2, C3, C5);
            BoardCards b3 = BoardCards.ForTurn(C1, C2, C5, C4);

            Assert.IsFalse(b1.Equals(b2));
            Assert.IsFalse(b1.Equals(b3));
            Assert.IsFalse(b2.Equals(b3));
        }

        [Test]
        public void BoardCardsTest_EqualityTestWithHoldemOmahaRule_CompareWithSelf_ReturnTrue()
        {
            BoardCards emptyBoard = BoardCards.FromCards(String.Empty);
            BoardCards boardWithFlop = BoardCards.FromCards("AcQsTh");
            BoardCards boardWithTurn = BoardCards.FromCards("AcQsTh2h");
            BoardCards boardWithRiver = BoardCards.FromCards("AcQsTh2h3d");

            Assert.IsTrue(emptyBoard.EqualsViaHoldemOmahaRule(emptyBoard));
            Assert.IsTrue(boardWithFlop.EqualsViaHoldemOmahaRule(boardWithFlop));
            Assert.IsTrue(boardWithTurn.EqualsViaHoldemOmahaRule(boardWithTurn));
            Assert.IsTrue(boardWithRiver.EqualsViaHoldemOmahaRule(boardWithRiver));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithHoldemOmahaRule_DifferentOrder_ReturnsTrue()
        {

            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C3, C2);
            BoardCards b3 = BoardCards.ForFlop(C2, C1, C3);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C1);
            BoardCards b5 = BoardCards.ForFlop(C3, C1, C2);
            BoardCards b6 = BoardCards.ForFlop(C3, C2, C1);

            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b2));
            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b3));
            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b4));
            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b5));
            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b6));
            
            Assert.IsTrue(b2.EqualsViaHoldemOmahaRule(b3));
            Assert.IsTrue(b2.EqualsViaHoldemOmahaRule(b4));
            Assert.IsTrue(b2.EqualsViaHoldemOmahaRule(b5));
            Assert.IsTrue(b2.EqualsViaHoldemOmahaRule(b6));

            Assert.IsTrue(b3.EqualsViaHoldemOmahaRule(b4));
            Assert.IsTrue(b3.EqualsViaHoldemOmahaRule(b5));
            Assert.IsTrue(b3.EqualsViaHoldemOmahaRule(b6));

            Assert.IsTrue(b4.EqualsViaHoldemOmahaRule(b5));
            Assert.IsTrue(b4.EqualsViaHoldemOmahaRule(b6));

            Assert.IsTrue(b5.EqualsViaHoldemOmahaRule(b6));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithHoldemOmahaRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C2, C4);
            BoardCards b3 = BoardCards.ForFlop(C1, C3, C4);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C4);

            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b2));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b4));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b4));
            Assert.IsFalse(b3.EqualsViaHoldemOmahaRule(b4));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithHoldemOmahaRule_ReturnsTrue()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C3, C2, C4);
            BoardCards b3 = BoardCards.ForTurn(C3, C2, C1, C4);

            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b2)); 
            Assert.IsTrue(b2.EqualsViaHoldemOmahaRule(b3)); 
            Assert.IsTrue(b1.EqualsViaHoldemOmahaRule(b3));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithHoldemOmahaRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C2, C3, C5);
            BoardCards b3 = BoardCards.ForTurn(C1, C2, C5, C4);

            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b2));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b3));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithHoldemOmahaRule_SameCardsWithDifferentOrder_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C4, C2, C3, C1);

            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b2));
        }


        [Test]
        public void BoardCardsTest_RiverEqualityTestWithHoldemOmahaRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForRiver(C1, C2, C3, C4, C5);
            BoardCards b2 = BoardCards.ForRiver(C1, C2, C3, C4, C6);
            BoardCards b3 = BoardCards.ForRiver(C1, C2, C3, C6, C5);

            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b2));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b3));
        }

        [Test]
        public void BoardCardsTest_RiverEqualityTestWithHoldemOmahaRule_SameCardsWithDifferentOrder_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForRiver(C1, C2, C3, C4, C5);
            BoardCards b2 = BoardCards.ForRiver(C4, C2, C3, C1, C5);
            BoardCards b3 = BoardCards.ForRiver(C1, C2, C3, C5, C4);
            BoardCards b4 = BoardCards.ForRiver(C1, C2, C5, C3, C4);

            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b2));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b1.EqualsViaHoldemOmahaRule(b4));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b3));
            Assert.IsFalse(b2.EqualsViaHoldemOmahaRule(b4));
            Assert.IsFalse(b3.EqualsViaHoldemOmahaRule(b4));
        }

        [Test]
        public void BoardCardsTest_EqualityTestWithCourchecvelRule_CompareWithSelf_ReturnTrue()
        {
            BoardCards emptyBoard = BoardCards.FromCards(String.Empty);
            BoardCards boardWithFlop = BoardCards.FromCards("AcQsTh");
            BoardCards boardWithTurn = BoardCards.FromCards("AcQsTh2h");
            BoardCards boardWithRiver = BoardCards.FromCards("AcQsTh2h3d");

            Assert.IsTrue(emptyBoard.EqualsViaCourchevelRule(emptyBoard));
            Assert.IsTrue(boardWithFlop.EqualsViaCourchevelRule(boardWithFlop));
            Assert.IsTrue(boardWithTurn.EqualsViaCourchevelRule(boardWithTurn));
            Assert.IsTrue(boardWithRiver.EqualsViaCourchevelRule(boardWithRiver));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithCourchecvelRule_DifferentOrder_ReturnsTrue()
        {

            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C3, C2);
            BoardCards b3 = BoardCards.ForFlop(C2, C1, C3);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C1);
            BoardCards b5 = BoardCards.ForFlop(C3, C1, C2);
            BoardCards b6 = BoardCards.ForFlop(C3, C2, C1);

            Assert.IsTrue(b1.EqualsViaCourchevelRule(b2));
            Assert.IsTrue(b3.EqualsViaCourchevelRule(b4));
            Assert.IsTrue(b5.EqualsViaCourchevelRule(b6));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithCourchecvelRule_DifferentOrder_ReturnsFalse()
        {

            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C3, C2);
            BoardCards b3 = BoardCards.ForFlop(C2, C1, C3);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C1);
            BoardCards b5 = BoardCards.ForFlop(C3, C1, C2);
            BoardCards b6 = BoardCards.ForFlop(C3, C2, C1);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b5));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b6));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b5));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b6));
            Assert.IsFalse(b3.EqualsViaCourchevelRule(b5));
            Assert.IsFalse(b3.EqualsViaCourchevelRule(b6));
        }

        [Test]
        public void BoardCardsTest_FlopEqualityTestWithCourchecvelRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForFlop(C1, C2, C3);
            BoardCards b2 = BoardCards.ForFlop(C1, C2, C4);
            BoardCards b3 = BoardCards.ForFlop(C1, C3, C4);
            BoardCards b4 = BoardCards.ForFlop(C2, C3, C4);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b2));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b3.EqualsViaCourchevelRule(b4));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithCourchecvelRule_ReturnsTrue()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C3, C2, C4);

            Assert.IsTrue(b1.EqualsViaCourchevelRule(b2));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithCourchecvelRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C1, C2, C3, C5);
            BoardCards b3 = BoardCards.ForTurn(C1, C2, C5, C4);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b2));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b3));
        }

        [Test]
        public void BoardCardsTest_TurnEqualityTestWithCourchecvelRule_SameCardsWithDifferentOrder_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForTurn(C1, C2, C3, C4);
            BoardCards b2 = BoardCards.ForTurn(C4, C2, C3, C1);
            BoardCards b3 = BoardCards.ForTurn(C1, C4, C3, C2);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b2));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b3));
        }


        [Test]
        public void BoardCardsTest_RiverEqualityTestWithCourchecvelRule_DifferentCards_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForRiver(C1, C2, C3, C4, C5);
            BoardCards b2 = BoardCards.ForRiver(C1, C2, C3, C4, C6);
            BoardCards b3 = BoardCards.ForRiver(C1, C2, C3, C6, C5);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b2));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b3));
        }

        [Test]
        public void BoardCardsTest_RiverEqualityTestWithCourchecvelRule_SameCardsWithDifferentOrder_ReturnsFalse()
        {
            BoardCards b1 = BoardCards.ForRiver(C1, C2, C3, C4, C5);
            BoardCards b2 = BoardCards.ForRiver(C4, C2, C3, C1, C5);
            BoardCards b3 = BoardCards.ForRiver(C1, C2, C3, C5, C4);
            BoardCards b4 = BoardCards.ForRiver(C1, C2, C5, C3, C4);

            Assert.IsFalse(b1.EqualsViaCourchevelRule(b2));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b1.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b3));
            Assert.IsFalse(b2.EqualsViaCourchevelRule(b4));
            Assert.IsFalse(b3.EqualsViaCourchevelRule(b4));
        }

    }

}
