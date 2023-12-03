using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using NUnit.Framework;

namespace HandHistories.Parser.UnitTests.Parsers.HandParserTests.HandActionTests
{
     [TestFixture]
    class HandParserHandActionTestsGGPokerImpl : HandParserHandActionTests
    {
         public HandParserHandActionTestsGGPokerImpl()
             : base("GGPoker")
        {
        }

        [Test]
        public void PaysCashoutFee_Works()
        {
            List<HandAction> expectedActions = new List<HandAction>()
            {
                new HandAction("tyx36123", HandActionType.SMALL_BLIND, -1m, Street.Preflop),
                    new HandAction("b6887901", HandActionType.BIG_BLIND, -2m, Street.Preflop),
                    new HandAction("vda35fd1", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("fma3fca1", HandActionType.RAISE, 4.4m, Street.Preflop),
                    new HandAction("oiesfcv1", HandActionType.CALL, 4.4m, Street.Preflop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("tyx36123", HandActionType.RAISE, 121.73m, Street.Preflop, true),
                    new HandAction("b6887901", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("fma3fca1", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("oiesfcv1", HandActionType.CALL, 118.33m, Street.Preflop),
                    new HandAction("tyx36123", HandActionType.SHOW, 0m, Street.Showdown),
                    new HandAction("oiesfcv1", HandActionType.SHOW, 0m, Street.Showdown),
                    new HandAction("oiesfcv1", HandActionType.PAYS_INSURANCE_FEE, 22.68m, Street.Showdown),
            };

            var expectedWinners = new List<WinningsAction>()
            {
                new WinningsAction("oiesfcv1", WinningsActionType.WINS, 243.86m, 0),
            };

            TestParseActions("PaysCashoutFee", expectedActions, expectedWinners);
        }

        [Test]
        public void RecivesCashout_Works()
        {
            List<HandAction> expectedActions = new List<HandAction>()
            {
                new HandAction("1xfd8bbx", HandActionType.SMALL_BLIND, -0.25m, Street.Preflop),
                new HandAction("88989dfa", HandActionType.BIG_BLIND, -0.5m, Street.Preflop),
                new HandAction("fdfa4dax", HandActionType.CALL, 0.5m, Street.Preflop),
                new HandAction("fdac21ts", HandActionType.RAISE, 1.82m, Street.Preflop),
                new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("c12290be", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("1xfd8bbx", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("88989dfa", HandActionType.CALL, 1.32m, Street.Preflop),
                new HandAction("fdfa4dax", HandActionType.RAISE, 6.5m, Street.Preflop, true),
                new HandAction("fdac21ts", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("88989dfa", HandActionType.CALL, 5.18m, Street.Preflop),

                new HandAction("fdfa4dax", HandActionType.SHOW, Street.Showdown),
                new HandAction("88989dfa", HandActionType.SHOW, Street.Showdown),
            };

            var expectedWinners = new List<WinningsAction>()
            {
                new WinningsAction("88989dfa", WinningsActionType.INSURANCE, 9.98m, 0),
                new WinningsAction("fdfa4dax", WinningsActionType.WINS, 14.8m, 0)
            };

            TestParseActions("ReceivesCashout", expectedActions, expectedWinners);
        }

        [Test]
        public void PostingDead_Works()
        {
            List<HandAction> expectedActions = new List<HandAction>()
            {
                new HandAction("142dsaca", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                new HandAction("343kk954", HandActionType.BIG_BLIND, -1, Street.Preflop),
                new HandAction("15yfdsaa", HandActionType.POSTS_DEAD, -0.5m, Street.Preflop),
                new HandAction("15yfdsaa", HandActionType.POSTS, -1, Street.Preflop),
                new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("b4mnx231", HandActionType.CALL, 1, Street.Preflop),
                new HandAction("15yfdsaa", HandActionType.RAISE, 4m, Street.Preflop),
                new HandAction("mkacefi1a", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("142dsaca", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("343kk954", HandActionType.FOLD, 0, Street.Preflop),
                new HandAction("b4mnx231", HandActionType.CALL, 4, Street.Preflop),

                new HandAction("b4mnx231", HandActionType.CHECK, 0, Street.Flop),
                new HandAction("15yfdsaa", HandActionType.BET, 12, Street.Flop),
                new HandAction("b4mnx231", HandActionType.FOLD, 0, Street.Flop),
                new HandAction("15yfdsaa", HandActionType.UNCALLED_BET, 12, Street.Flop),
            };

            var expectedWinners = new List<WinningsAction>()
            {
                 new WinningsAction("15yfdsaa", WinningsActionType.WINS, 11.4m, 0)
            };

            TestParseActions("PostingDead", expectedActions, expectedWinners);
        }

        protected override List<HandAction> ExpectedHandActionsBasicHand
         {
             get
             {
                 return new List<HandAction>()
                    {
                        new HandAction("ac12334d", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                        new HandAction("1234gsaa", HandActionType.BIG_BLIND, -1, Street.Preflop),
                        new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                        new HandAction("79adcegg", HandActionType.FOLD, 0, Street.Preflop),
                        new HandAction("853aak6d", HandActionType.FOLD, 0, Street.Preflop),
                        new HandAction("ac12334d", HandActionType.RAISE, 3m, Street.Preflop),
                        new HandAction("1234gsaa", HandActionType.FOLD, 0, Street.Preflop),
                        new HandAction("ac12334d", HandActionType.UNCALLED_BET, 2.5m, Street.Preflop),
                    };
             }
         }
        protected override List<WinningsAction> ExpectedWinnersHandActionsBasicHand
        {
            get { return new List<WinningsAction>() { new WinningsAction("ac12334d", WinningsActionType.WINS, 2m, 0), }; }
        }

         protected override List<HandAction> ExpectedHandActionsFoldedPreflop
         {
             get
             {
                Assert.Ignore();
                throw new NotImplementedException();
            }
         }

         protected override List<WinningsAction> ExpectedWinnersHandActionsFoldedPreflop
         {
             get {
                Assert.Ignore("Covered by BasicHand");
                throw new NotImplementedException();
            }
         }

         protected override List<HandAction> ExpectedHandActions3BetHand
         {
             get
             {
                Assert.Ignore();
                throw new NotImplementedException();
            }
         }

         protected override List<WinningsAction> ExpectedWinnersHandActions3BetHand
         {
             get {
                Assert.Ignore();
                throw new NotImplementedException();
            }
         }

         protected override List<HandAction> ExpectedHandActionsAllInHand
         {
             get
             {
                Assert.Ignore();
                throw new NotImplementedException();
            }
         }

         protected override List<WinningsAction> ExpectedWinnersHandActionsAllInHand
         {
             get {
                Assert.Ignore();
                throw new NotImplementedException();
            }
         }

         protected override List<HandAction> ExpectedHandActionsUncalledBetHand
         {
             get
             {
                Assert.Ignore("Covered by BasicHand");
                throw new NotImplementedException();
             }
         }

         protected override List<WinningsAction> ExpectedWinnersHandActionsUncalledBetHand
         {
             get { throw new NotImplementedException(); }
         }

         protected override List<HandAction> ExpectedOmahaHiLoHand
         {
             get { throw new NotImplementedException(); }
         }

         protected override List<WinningsAction> ExpectedWinnersOmahaHiLoHand
         {
             get { throw new NotImplementedException(); }
         }
    }
}
