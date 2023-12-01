using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.FastParser.GGPoker;
using NUnit.Framework;
using HandHistories.Parser.UnitTests.Parsers.Base;

namespace HandHistories.Parser.UnitTests.Parsers.FastParserTests.GGPoker
{
    using Parser = GGPokerFastParserImpl;
    [TestFixture]
    class GGPokerFastParserActionTests : HandHistoryParserBaseTests
    {
        public GGPokerFastParserActionTests()
            : base("GGPoker")
        {
        }

        protected GGPokerFastParserImpl GetGGPokerFastParser()
        {
            return new GGPokerFastParserImpl();
        }

        [Test]
        public void ParseRegularActionLine_Raise_Works()
        {
            HandAction handAction =
                GetGGPokerFastParser().ParseRegularActionLine(@"1232423sf: raises $1.1 to $2.1", 9, Street.Preflop);

            Assert.AreEqual(new HandAction("1232423sf", HandActionType.RAISE, 2.1m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_RaiseAllIn_Works()
        {
            HandAction handAction =
                 GetGGPokerFastParser().ParseRegularActionLine(@"012345678: raises $141.28 to $148.78 and is all-in", 9, Street.Flop);

            Assert.AreEqual(new HandAction("012345678", HandActionType.RAISE, 148.78m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_BetsAllIn_Works()
        {
            HandAction handAction =
                 GetGGPokerFastParser().ParseRegularActionLine(@"43rfn3x88: bets $3.03 and is all-in", 9, Street.Flop);

            Assert.AreEqual(new HandAction("43rfn3x88", HandActionType.BET, 3.03m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_CallsAllIn_Works()
        {
            HandAction handAction =
                 GetGGPokerFastParser().ParseRegularActionLine(@"8cd0f41: calls $74.1 and is all-in", 7, Street.Flop);

            Assert.AreEqual(new HandAction("8cd0f41", HandActionType.CALL, 74.1m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Calls_Works()
        {
            HandAction handAction =
               GetGGPokerFastParser().ParseRegularActionLine(@"ad0294d4: calls $1.84", 8, Street.Turn);

            Assert.AreEqual(new HandAction("ad0294d4", HandActionType.CALL, 1.84m, Street.Turn), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Checks_Works()
        {
            HandAction handAction =
               GetGGPokerFastParser().ParseRegularActionLine(@"tr280688: checks", 8, Street.Turn);

            Assert.AreEqual(new HandAction("tr280688", HandActionType.CHECK, 0m, Street.Turn), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Bets_Works()
        {
            HandAction handAction =
               GetGGPokerFastParser().ParseRegularActionLine(@"MS13ZEN: bets $1.76", 7, Street.River);

            Assert.AreEqual(new HandAction("MS13ZEN", HandActionType.BET, 1.76m, Street.River), handAction);
        }


        [Test]
        public void ParseRegularActionLine_PaysCashoutFee_Works()
        {
            HandAction handAction =
               GetGGPokerFastParser().ParseRegularActionLine(@"MS13ZEN: Pays Cashout Risk ($22.68)", 7, Street.River);

            Assert.AreEqual(new HandAction("MS13ZEN", HandActionType.PAYS_INSURANCE_FEE, 22.68m, Street.Showdown), handAction);
        }


        [Test]
        public void ParsePostingActionLine_SmallBlind_Works()
        {
            bool bigBlindPosted = false;
            HandAction handAction = Parser.ParsePostingActionLine(@"1236460a: posts small blind $0.5", 8, ref bigBlindPosted);

            Assert.AreEqual(new HandAction("1236460a", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_BigBlind_Works()
        {
            bool bigBlindPosted = false;
            HandAction handAction = Parser.ParsePostingActionLine(@"gaydaddy: posts big blind $1.23", 8, ref bigBlindPosted);

            Assert.AreEqual(new HandAction("gaydaddy", HandActionType.BIG_BLIND, 1.23m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_PostsBigBlind_WithExistingBigBlind_Works()
        {
            bool bigBlindPosted = true;
            HandAction handAction = Parser.ParsePostingActionLine(@"test1234: posts big blind $3", 8, ref bigBlindPosted);

            Assert.AreEqual(new HandAction("test1234", HandActionType.POSTS, 3, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_Straddle_Works()
        {
            bool bigBlindPosted = false;
            HandAction handAction = Parser.ParsePostingActionLine(@"12365032: straddle $2", 8, ref bigBlindPosted);

            Assert.AreEqual(new HandAction("12365032", HandActionType.STRADDLE, 2.0m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_MissedBlind_Works()
        {
            bool bigBlindPosted = false;
            HandAction handAction = Parser.ParsePostingActionLine(@"af3acs39: posts missed blind $0.5", 8, ref bigBlindPosted);

            Assert.AreEqual(new HandAction("af3acs39", HandActionType.POSTS_DEAD, 0.5m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Folds_Works()
        {
            HandAction handAction =
                GetGGPokerFastParser().ParseRegularActionLine(@"gaydaddy: folds", 8, Street.Preflop);

            Assert.AreEqual(new HandAction("gaydaddy", HandActionType.FOLD, 0, Street.Preflop), handAction);
        }

        [Test]
        public void ParseUncalledBetLine_Works()
        {
            HandAction handAction =
                GetGGPokerFastParser().ParseUncalledBetLine(@"Uncalled bet ($6) returned to Hero", Street.Preflop);

            Assert.AreEqual(new HandAction("Hero", HandActionType.UNCALLED_BET, 6, Street.Preflop), handAction);
        }

        [Test]
        public void ParseWinnings_Works()
        {
            WinningsAction handAction =
                GetGGPokerFastParser().ParseWinnings(@"a232scs collected $1236.25 from pot");

            Assert.AreEqual(new WinningsAction("a232scs", WinningsActionType.WINS, 1236.25m, 0), handAction);
        }

        [Test]
        public void ParseWinnings_WithColon_Works()
        {
            WinningsAction handAction =
                GetGGPokerFastParser().ParseWinnings(@"wo_olly:D collected $0.57 from pot");

            Assert.AreEqual(new WinningsAction("wo_olly:D", WinningsActionType.WINS, 0.57m, 0), handAction);
        }

        [Test]
        public void ParseWinnings_ReceivesCashOut_Works()
        {
            WinningsAction handAction =
                GetGGPokerFastParser().ParseWinnings(@"wo_olly:D: Receives Cashout ($9.98)");

            Assert.AreEqual(new WinningsAction("wo_olly:D", WinningsActionType.INSURANCE, 9.98m, 0), handAction);
        }

        [Test]
        public void ParseMiscShowdownLine_Shows_Works()
        {
            HandAction handAction =
                GetGGPokerFastParser().ParseMiscShowdownLine(@"RECHUK: shows [Ac Qh] (a full house, Aces full of Queens)", 6);

            Assert.AreEqual(new HandAction("RECHUK", HandActionType.SHOW, 0m, Street.Showdown), handAction);
        }
    }
}
