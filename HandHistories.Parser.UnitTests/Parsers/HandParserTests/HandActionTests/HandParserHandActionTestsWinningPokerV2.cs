using System.Collections.Generic;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using NUnit.Framework;
using System;

namespace HandHistories.Parser.UnitTests.Parsers.HandParserTests.HandActionTests
{
    [TestFixture]
    class HandParserHandActionTestsWinningPokerV2Impl : HandParserHandActionTests
    {
        public HandParserHandActionTestsWinningPokerV2Impl()
            : base("WinningPokerV2")
        {
        }
        
        protected override List<HandAction> ExpectedHandActionsBasicHand
        {
            get
            {
                Assert.Ignore();
                throw new NotImplementedException();
            }
        }

        protected override List<WinningsAction> ExpectedWinnersHandActionsBasicHand
        {
            get
            {
                Assert.Ignore();
                throw new NotImplementedException();
            }
        }

        protected override List<HandAction> ExpectedHandActionsFoldedPreflop
        {
            get
            {
                return new List<HandAction>()
                {
                    new HandAction("Player4", HandActionType.SMALL_BLIND, 0.01m, Street.Preflop),
                    new HandAction("Player5", HandActionType.BIG_BLIND, 0.02m, Street.Preflop),
                    new HandAction("Player2", HandActionType.POSTS, 0.02m, Street.Preflop),

                    new HandAction("Player1", HandActionType.FOLD, 0m, Street.Preflop),
                    new HandAction("Player2", HandActionType.CHECK, 0m, Street.Preflop),
                    new HandAction("Player3", HandActionType.RAISE, 0.08m, Street.Preflop),
                    new HandAction("Player4", HandActionType.RAISE, 0.25m, Street.Preflop),
                    new HandAction("Player5", HandActionType.FOLD, 0m, Street.Preflop),
                    new HandAction("Player2", HandActionType.FOLD, 0m, Street.Preflop),
                    new HandAction("Player3", HandActionType.FOLD, 0m, Street.Preflop),
                    new HandAction("Player4", HandActionType.UNCALLED_BET, 0.18m, Street.Preflop),
                };
            }
        }

        protected override List<WinningsAction> ExpectedWinnersHandActionsFoldedPreflop
        {
            get { return new List<WinningsAction>() { new WinningsAction("Player4", WinningsActionType.WINS, 0.20m, 0) }; }
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
            get
            {
                Assert.Ignore();
                throw new NotImplementedException();
            }
        }

        protected override List<HandAction> ExpectedHandActionsAllInHand
        {
            get
            {
                return new List<HandAction>()
                {
                    new HandAction("Player 2", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                    new HandAction("Player 1", HandActionType.BIG_BLIND, 1m, Street.Preflop),

                    new HandAction("Player 2", HandActionType.RAISE, 2.5m, Street.Preflop),
                    new HandAction("Player 1", HandActionType.RAISE, 8m, Street.Preflop),
                    new HandAction("Player 2", HandActionType.CALL, 6m, Street.Preflop),

                    new HandAction("Player 1", HandActionType.BET, 18m, Street.Flop),
                    new HandAction("Player 2", HandActionType.CALL, 18m, Street.Flop),

                    new HandAction("Player 1", HandActionType.BET, 54m, Street.Turn),
                    new HandAction("Player 2", HandActionType.CALL, 13m, Street.Turn, AllInAction: true),
                    new HandAction("Player 1", HandActionType.UNCALLED_BET, 41m, Street.Turn),

                    new HandAction("Player 1", HandActionType.SHOW, Street.Showdown),
                    new HandAction("Player 2", HandActionType.SHOW, Street.Showdown),
                };
            }
        }

        protected override List<WinningsAction> ExpectedWinnersHandActionsAllInHand
        {
            get { return new List<WinningsAction>() { new WinningsAction("Player 2", WinningsActionType.WINS, 79.50m, 0) }; }
        }

        protected override List<HandAction> ExpectedHandActionsUncalledBetHand
        {
            get
            {
                return new List<HandAction>()
                {
                    new HandAction("Player 1", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                    new HandAction("Player 2", HandActionType.BIG_BLIND, 1m, Street.Preflop),

                    new HandAction("Player 1", HandActionType.RAISE, 2.5m, Street.Preflop),
                    new HandAction("Player 2", HandActionType.CALL, 1.5m, Street.Preflop),

                    new HandAction("Player 2", HandActionType.CHECK, 0m, Street.Flop),
                    new HandAction("Player 1", HandActionType.BET, 3.75m, Street.Flop),
                    new HandAction("Player 2", HandActionType.FOLD, 18m, Street.Flop),
                    new HandAction("Player 1", HandActionType.UNCALLED_BET, 3.75m, Street.Flop),
                };
            }
        }

        protected override List<WinningsAction> ExpectedWinnersHandActionsUncalledBetHand
        {
            get { throw new NotImplementedException(); }
        }

        protected override List<HandAction> ExpectedOmahaHiLoHand
        {
            get
            {
                Assert.Ignore();
                throw new NotImplementedException();
            }
        }

        protected override List<WinningsAction> ExpectedWinnersOmahaHiLoHand
        {
            get 
            {
                Assert.Ignore();
                throw new NotImplementedException();
            }
        }
    }
}
