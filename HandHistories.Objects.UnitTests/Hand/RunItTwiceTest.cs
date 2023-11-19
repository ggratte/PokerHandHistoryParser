using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandHistories.Objects.UnitTests.Hand
{
    [TestFixture]
    public class RunItTwiceTest
    {
        [Test]
        public void RunItTwice_TestEquality_ReturnsTrue() 
        {
            RunItTwice r1 = new RunItTwice()
            {
                Board = BoardCards.FromCards("AcKdQd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>() 
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 120m, 0)
                }
            };

            RunItTwice r2 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QdAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 120m, 0)
                }
            };

            Assert.IsTrue(r1.Equals(r1));
            Assert.IsTrue(r1.Equals(r2));
        }

        [Test]
        public void RunItTwice_TestEquality_ReturnsFalse()
        {
            RunItTwice r1 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QdAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 120m, 0)
                }
            };

            RunItTwice r2 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QcAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 120m, 0)
                }
            };

            RunItTwice r3 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QdAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player2", WinningsActionType.WINS, 120m, 0)
                }
            };

            RunItTwice r4 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QdAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 120m, 0),
                    new WinningsAction("player2", WinningsActionType.WINS, 120m, 0)
                }
            };

            RunItTwice r5 = new RunItTwice()
            {
                Board = BoardCards.FromCards("QdAckd"),
                Actions = new List<HandAction>()
                {

                },
                Winners = new List<WinningsAction>()
                {
                    new WinningsAction("player1", WinningsActionType.WINS, 121m, 0)
                }
            };

            Assert.IsFalse(r1.Equals(r2));
            Assert.IsFalse(r1.Equals(r3));
            Assert.IsFalse(r1.Equals(r4));
            Assert.IsFalse(r1.Equals(r5));
            Assert.IsFalse(r2.Equals(r3));
            Assert.IsFalse(r2.Equals(r4));
            Assert.IsFalse(r2.Equals(r5));
            Assert.IsFalse(r3.Equals(r4));
            Assert.IsFalse(r3.Equals(r5));
            Assert.IsFalse(r4.Equals(r5));
        }
    }
}
