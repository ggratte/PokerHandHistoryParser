using System;
using System.Collections.Generic;
using System.Text;
using HandHistories.Parser.Parsers.FastParser.GGPoker;
using HandHistories.Parser.UnitTests.Parsers.Base;
using NUnit.Framework;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Parser.UnitTests.Utils.Pot;
using HandHistories.Objects.Actions;

namespace HandHistories.Parser.UnitTests.Parsers.FastParserTests.GGPoker
{
    using Parser = GGPokerFastParserImpl;

    [TestFixture]
    class GGPokerHandSummaryParserExtraTests: HandHistoryParserBaseTests
    {
        public GGPokerHandSummaryParserExtraTests() : base("GGPoker")
        {

        }

        private HandHistorySummary TestFullHandHistorySummary(HandHistorySummary expectedSummary, string fileName)
        {
            string handText = SampleHandHistoryRepository.GetHandExample(PokerFormat.CashGame, Site, "GeneralHands", fileName);

            HandHistorySummary actualSummary = GetSummmaryParser().ParseFullHandSummary(handText, true);

            Assert.AreEqual(expectedSummary.GameDescription, actualSummary.GameDescription);
            Assert.AreEqual(expectedSummary.DealerButtonPosition, actualSummary.DealerButtonPosition);
            Assert.AreEqual(expectedSummary.DateOfHandUtc, actualSummary.DateOfHandUtc);
            Assert.AreEqual(expectedSummary.HandId, actualSummary.HandId);
            Assert.AreEqual(expectedSummary.NumPlayersSeated, actualSummary.NumPlayersSeated);
            Assert.AreEqual(expectedSummary.TableName, actualSummary.TableName);
            Assert.AreEqual(expectedSummary.TotalPot, actualSummary.TotalPot);
            Assert.AreEqual(expectedSummary.Rake, actualSummary.Rake);
            Assert.AreEqual(expectedSummary.Jackpot, actualSummary.Jackpot);
            Assert.AreEqual(expectedSummary.Bingo, actualSummary.Bingo);

            return actualSummary;
        }

        private HandHistory TestHandHistory(HandHistory expectedHandHistory, string fileName)
        {
            string handText = SampleHandHistoryRepository.GetHandExample(PokerFormat.CashGame, Site, "GeneralHands", fileName);

            HandHistory handHistory = GetParser().ParseFullHandHistory(handText, true);
           
            Assert.IsTrue(expectedHandHistory.CommunityCards.Equals(handHistory.CommunityCards));
            Assert.IsTrue(expectedHandHistory.Players.Equals(handHistory.Players));
            Assert.AreEqual(expectedHandHistory.Hero, handHistory.Hero);
            Assert.AreEqual(expectedHandHistory.NumPlayersActive, handHistory.NumPlayersActive);
            Assert.AreEqual(expectedHandHistory.RunItMultipleTimes.Length, handHistory.RunItMultipleTimes.Length);

            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[0].Equals(handHistory.RunItMultipleTimes[0]));
            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[1].Equals(handHistory.RunItMultipleTimes[1]));
            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[2].Equals(handHistory.RunItMultipleTimes[2]));

            //Assert.IsTrue(expectedHandHistory.HandActions.Equals(handHistory.HandActions));
            TestWinners(expectedHandHistory.Winners, handHistory.Winners);
            TestActions(expectedHandHistory.HandActions, handHistory.HandActions);

            return handHistory;
        }

        private void TestWinners(List<WinningsAction> expectedWinningsActions, List<WinningsAction> winningsActions) 
        { 
            Assert.AreEqual(expectedWinningsActions.Count, winningsActions.Count);
            for (int i = 0; i < expectedWinningsActions.Count; i++) 
            {
                Assert.AreEqual(expectedWinningsActions[i], winningsActions[i]);
            }
        }

        private void TestActions(List<HandAction> expectedHandActions, List<HandAction> handActions)
        {
            Assert.AreEqual(handActions.Count, expectedHandActions.Count);
            for (int i = 0; i < expectedHandActions.Count; i++)
            {
                Assert.AreEqual(expectedHandActions[i], handActions[i]);
            }
        }

        [Test]
        public void ZoomHand()
        {
            HandHistorySummary expectedSummary = new HandHistorySummary()
            {
                GameDescription = new GameDescriptor()
                {
                    PokerFormat = PokerFormat.CashGame,
                    GameType = GameType.NoLimitHoldem,
                    Limit = Limit.FromSmallBlindBigBlind(0.05m, 0.1m, Currency.USD),
                    SeatType = SeatType.FromMaxPlayers(6),
                    Site = SiteName.GGPoker,
                    TableType = TableType.FromTableTypeDescriptions(TableTypeDescription.Zoom)
                },
                DateOfHandUtc = new DateTime(2018, 3, 30, 3, 44, 33),
                DealerButtonPosition = 1,
                HandId = HandID.From(2345481320),
                NumPlayersSeated = 6,
                TableName = "RushAndCash143219",
                TotalPot = 1,
                Rake = 0.05m,
                Jackpot = 0,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards("8dJc2s"),
                Players = new PlayerList(new List<Player>
                {
                    new Player("Hero", 22, 3, HoleCards.FromCards("Ac3d")),
                    new Player("2734abdf", 22.17m, 2),
                    new Player("1234gsaa", 14.67m, 1),
                    new Player("788bnr4e", 12.43m, 4),
                    new Player("gm27b4sg", 8.9m, 5),
                    new Player("153ddcg3", 7.83m, 6),
                }),
                Hero = new Player("Hero", 22, 3, HoleCards.FromCards("3DAC")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("2sjc8D"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("2734abdf", WinningsActionType.WINS, 0.95m, 0)
                        }
                    },
                    new RunItTwice {},
                    new RunItTwice {}
                },
                Winners = new List<WinningsAction>() { new WinningsAction("2734abdf", WinningsActionType.WINS, 0.95m, 0)},
                HandActions = new List<HandAction>() { 
                    new HandAction("2734abdf", HandActionType.SMALL_BLIND, -0.05m, Street.Preflop),
                    new HandAction("Hero", HandActionType.BIG_BLIND, -0.1m, Street.Preflop),
                    new HandAction("788bnr4e", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("gm27b4sg", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("153ddcg3", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("1234gsaa", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("2734abdf", HandActionType.RAISE, 0.25m, Street.Preflop),
                    new HandAction("Hero", HandActionType.CALL, 0.2m, Street.Preflop),
                    new HandAction("2734abdf", HandActionType.CHECK, 0, Street.Flop), 
                    new HandAction("Hero", HandActionType.BET, 0.2m, Street.Flop),
                    new HandAction("2734abdf", HandActionType.RAISE, 0.7m, Street.Flop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Flop),
                    new HandAction("2734abdf", HandActionType.UNCALLED_BET, 0.5m, Street.Flop),
                }
            };
            TestFullHandHistorySummary(expectedSummary, "RushAndCash");
            TestHandHistory(expectedHandHistory, "RushAndCash");
        }


        [Test]
        public void TestRegularHand()
        {
            HandHistorySummary expectedSummary = new HandHistorySummary()
            {
                GameDescription = new GameDescriptor()
                {
                    PokerFormat = PokerFormat.CashGame,
                    GameType = GameType.NoLimitHoldem,
                    Limit = Limit.FromSmallBlindBigBlind(0.5m, 1m, Currency.USD),
                    SeatType = SeatType.FromMaxPlayers(6),
                    Site = SiteName.GGPoker,
                    TableType = TableType.FromTableTypeDescriptions(TableTypeDescription.Regular)
                },
                DateOfHandUtc = new DateTime(2017, 11, 12, 11, 43, 27),
                DealerButtonPosition = 2,
                HandId = HandID.From(23847510),
                NumPlayersSeated = 5,
                TableName = "NLHGold11",
                TotalPot = 2,
                Rake = 0,
                Jackpot = 0,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards(String.Empty),
                Players = new PlayerList(new List<Player>
                {
                    new Player("79adcegg", 173.04m, 1),
                    new Player("853aak6d", 100m, 2),
                    new Player("ac12334d", 190.65m, 4),
                    new Player("1234gsaa", 85.59m, 5),
                    new Player("Hero", 300.5m, 6, HoleCards.FromCards("5h2h")),
                }),
                Hero = new Player("Hero", 300.5m, 6, HoleCards.FromCards("2h5h")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards(String.Empty),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("ac12334d", WinningsActionType.WINS, 2m, 0)
                        }
                    },
                    new RunItTwice {},
                    new RunItTwice {}
                },
                Winners = new List<WinningsAction>() { new WinningsAction("ac12334d", WinningsActionType.WINS, 2m, 0)},
                HandActions = new List<HandAction>() { 
                    new HandAction("ac12334d", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                    new HandAction("1234gsaa", HandActionType.BIG_BLIND, -1, Street.Preflop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("79adcegg", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("853aak6d", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("ac12334d", HandActionType.RAISE, 3m, Street.Preflop),
                    new HandAction("1234gsaa", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("ac12334d", HandActionType.UNCALLED_BET, 2.5m, Street.Preflop),
                }
            };
            TestFullHandHistorySummary(expectedSummary, "Regular");
            TestHandHistory(expectedHandHistory, "Regular");
        }

        [Test]
        public void Straddle()
        {
            HandHistorySummary expectedSummary = new HandHistorySummary()
            {
                GameDescription = new GameDescriptor()
                {
                    PokerFormat = PokerFormat.CashGame,
                    GameType = GameType.NoLimitHoldem,
                    Limit = Limit.FromSmallBlindBigBlind(0.5m, 1m, Currency.USD),
                    SeatType = SeatType.FromMaxPlayers(6),
                    Site = SiteName.GGPoker,
                    TableType = TableType.FromTableTypeDescriptions(TableTypeDescription.Regular)
                },
                DateOfHandUtc = new DateTime(2018, 12, 11, 16, 21, 51),
                DealerButtonPosition = 6,
                HandId = HandID.From(582731),
                NumPlayersSeated = 6,
                TableName = "NLHGold7",
                TotalPot = 28.9m,
                Rake = 1.44m,
                Jackpot = 0,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards("ThJh7dAc"),
                Players = new PlayerList(new List<Player>
                {
                    new Player("xnvdam1", 103.5m, 1),
                    new Player("3fak3jfj2", 106.89m, 2),
                    new Player("Hero", 100.23m, 3, HoleCards.FromCards("TcKs")),
                    new Player("62dfdfaf", 105.63m, 4),
                    new Player("fdafscsa", 100, 5),
                    new Player("123dca2d", 133.34m, 6),
                }),
                Hero = new Player("Hero", 100.23m, 3, HoleCards.FromCards("KsTc")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("ThJh7dAc"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("xnvdam1", WinningsActionType.WINS, 27.46m, 0)
                        }
                    },
                    new RunItTwice {},
                    new RunItTwice {}
                },
                Winners = new List<WinningsAction>() { new WinningsAction("xnvdam1", WinningsActionType.WINS, 27.46m, 0)},
                HandActions = new List<HandAction>() { 
                    new HandAction("xnvdam1", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                    new HandAction("3fak3jfj2", HandActionType.BIG_BLIND, -1, Street.Preflop),
                    new HandAction("62dfdfaf", HandActionType.STRADDLE, -2, Street.Preflop),
                    new HandAction("fdafscsa", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("123dca2d", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("xnvdam1", HandActionType.RAISE, 6.50m, Street.Preflop),
                    new HandAction("3fak3jfj2", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("62dfdfaf", HandActionType.CALL, 5m, Street.Preflop),
                    new HandAction("xnvdam1", HandActionType.BET, 6.95m, Street.Flop),
                    new HandAction("62dfdfaf", HandActionType.CALL, 6.95m, Street.Flop),
                    new HandAction("xnvdam1", HandActionType.BET, 22.68m, Street.Turn), 
                    new HandAction("62dfdfaf", HandActionType.FOLD, 0, Street.Turn),
                    new HandAction("xnvdam1", HandActionType.UNCALLED_BET, 22.68m, Street.Turn), 
                }
            };
            TestFullHandHistorySummary(expectedSummary, "Straddle");
            TestHandHistory(expectedHandHistory, "Straddle");
        }
    }
}
