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
            Assert.IsTrue(expectedHandHistory.CommunityCards == handHistory.CommunityCards);
            Assert.IsTrue(expectedHandHistory.Players.Equals(handHistory.Players));
            Assert.AreEqual(expectedHandHistory.Hero, handHistory.Hero);
            Assert.AreEqual(expectedHandHistory.NumPlayersActive, handHistory.NumPlayersActive);
            Assert.AreEqual(expectedHandHistory.RunItMultipleTimes.Length, handHistory.RunItMultipleTimes.Length);

            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[0].Equals(handHistory.RunItMultipleTimes[0]));
            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[1].Equals(handHistory.RunItMultipleTimes[1]));
            Assert.IsTrue(expectedHandHistory.RunItMultipleTimes[2].Equals(handHistory.RunItMultipleTimes[2]));

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
                CommunityCards = BoardCards.FromCards("5d7hks"),
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
                        Board = BoardCards.FromCards("ks7h5d"),
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
                CommunityCards = BoardCards.ForPreflop(),
                Players = new PlayerList(new List<Player>
                {
                    new Player("79adcegg", 173.04m, 1),
                    new Player("853aak6d", 100m, 2),
                    new Player("ac12334d", 190.65m, 4),
                    new Player("1234gsaa", 85.59m, 5),
                    new Player("Hero", 300.5m, 6, HoleCards.FromCards("5h2c")),
                }),
                Hero = new Player("Hero", 300.5m, 6, HoleCards.FromCards("2c5h")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = null,
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

        [Test]
        public void MultiplePosts()
        {
            HandHistorySummary expectedSummary = new HandHistorySummary()
            {
                GameDescription = new GameDescriptor()
                {
                    PokerFormat = PokerFormat.CashGame,
                    GameType = GameType.NoLimitHoldem,
                    Limit = Limit.FromSmallBlindBigBlind(0.25m, 0.5m, Currency.USD),
                    SeatType = SeatType.FromMaxPlayers(6),
                    Site = SiteName.GGPoker,
                    TableType = TableType.FromTableTypeDescriptions(TableTypeDescription.Regular)
                },
                DateOfHandUtc = new DateTime(2017, 11, 30, 19, 22, 11),
                DealerButtonPosition = 4,
                HandId = HandID.From(11358),
                NumPlayersSeated = 5,
                TableName = "NLHGold12",
                TotalPot = 13.3m,
                Rake = 1.42m,
                Jackpot = 0,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards("Ts7h5d2hKc"),
                Players = new PlayerList(new List<Player>
                {
                    new Player("Hero", 25.31m, 1, HoleCards.FromCards("7d2c")),
                    new Player("123dca2d", 40m, 2),
                    new Player("casf123d", 58.14m, 4),
                    new Player("facdasfwc", 29.61m, 5),
                    new Player("fadfvasf", 42.3m, 6),
                }),
                Hero = new Player("Hero", 25.31m, 1, HoleCards.FromCards("7d2c")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("Ts7h5d2hKc"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("123dca2d", WinningsActionType.WINS, 11.88m, 0)
                        }
                    },
                    new RunItTwice {},
                    new RunItTwice {}
                },
                Winners = new List<WinningsAction>() { new WinningsAction("123dca2d", WinningsActionType.WINS, 11.88m, 0) },
                HandActions = new List<HandAction>() {
                    new HandAction("facdasfwc", HandActionType.SMALL_BLIND, -0.25m, Street.Preflop),
                    new HandAction("fadfvasf", HandActionType.BIG_BLIND, -0.5m, Street.Preflop),
                    new HandAction("123dca2d", HandActionType.POSTS, -0.5m, Street.Preflop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("123dca2d", HandActionType.CHECK, 0, Street.Preflop),
                    new HandAction("casf123d", HandActionType.CALL, 0.5m, Street.Preflop),
                    new HandAction("facdasfwc", HandActionType.CALL, 0.25m, Street.Preflop),
                    new HandAction("fadfvasf", HandActionType.CHECK, 0, Street.Preflop),
                    new HandAction("facdasfwc", HandActionType.CHECK, 0, Street.Flop),
                    new HandAction("fadfvasf", HandActionType.CHECK, 0, Street.Flop),
                    new HandAction("123dca2d", HandActionType.BET, 1m, Street.Flop),
                    new HandAction("casf123d", HandActionType.FOLD, 0m, Street.Flop),
                    new HandAction("facdasfwc", HandActionType.CALL, 1m, Street.Flop),
                    new HandAction("fadfvasf", HandActionType.CALL, 1m, Street.Flop),
                    new HandAction("facdasfwc", HandActionType.CHECK, 0, Street.Turn),
                    new HandAction("fadfvasf", HandActionType.CHECK, 0, Street.Turn),
                    new HandAction("123dca2d", HandActionType.BET, 3.1m, Street.Turn),
                    new HandAction("facdasfwc", HandActionType.CALL, 3.1m, Street.Turn),
                    new HandAction("fadfvasf", HandActionType.CALL, 3.1m, Street.Turn),
                    new HandAction("facdasfwc", HandActionType.CHECK, 0, Street.River),
                    new HandAction("fadfvasf", HandActionType.CHECK, 0, Street.River),
                    new HandAction("123dca2d", HandActionType.BET, 8.2m, Street.River),
                    new HandAction("facdasfwc", HandActionType.FOLD, 0m, Street.River),
                    new HandAction("fadfvasf", HandActionType.FOLD, 0m, Street.River),
                    new HandAction("123dca2d", HandActionType.UNCALLED_BET, 8.2m, Street.River),
                }
            };
            TestFullHandHistorySummary(expectedSummary, "MultiplePosts");
            TestHandHistory(expectedHandHistory, "MultiplePosts");
        }

        [Test]
        public void TestRunTwice()
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
                DateOfHandUtc = new DateTime(2018, 11, 11, 22, 08, 11),
                DealerButtonPosition = 1,
                HandId = HandID.From(1587634),
                NumPlayersSeated = 4,
                TableName = "NLHGold15",
                TotalPot = 194.36m,
                Rake = 5m,
                Jackpot = 1,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards("2d3c8sJh3d"),
                Players = new PlayerList(new List<Player>
                {
                    new Player("8acec3m6", 306.88m, 1),
                    new Player("52jj36aa", 97.18m, 3, HoleCards.FromCards("KcAs")),
                    new Player("Hero", 117.09m, 4, HoleCards.FromCards("AcAd")),
                    new Player("mn19dde5", 63.41m, 6),
                }),
                Hero = new Player("Hero", 117.09m, 4, HoleCards.FromCards("AcAd")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("2d3c8sJh3d"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("Hero", WinningsActionType.WINS, 94.18m, 0)
                        }
                    },
                    new RunItTwice 
                    {
                        Board = BoardCards.FromCards("6d4s4h3hTs"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("Hero", WinningsActionType.WINS, 94.18m, 0)
                        }
                    
                    },
                    new RunItTwice {}
                },
                Winners = new List<WinningsAction>() { 
                    new WinningsAction("Hero", WinningsActionType.WINS, 94.18m, 0), 
                    new WinningsAction("Hero", WinningsActionType.WINS, 94.18m, 0), 
                },
                HandActions = new List<HandAction>() {
                    new HandAction("52jj36aa", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                    new HandAction("Hero", HandActionType.BIG_BLIND, -1, Street.Preflop),
                    new HandAction("mn19dde5", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("8acec3m6", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("52jj36aa", HandActionType.RAISE, 2.50m, Street.Preflop),
                    new HandAction("Hero", HandActionType.RAISE, 8, Street.Preflop),
                    new HandAction("52jj36aa", HandActionType.RAISE, 94.18m, Street.Preflop, true, 0),
                    new HandAction("Hero", HandActionType.CALL, 88.18m, Street.Preflop),
                    new HandAction("52jj36aa", HandActionType.SHOW, Street.Showdown), 
                    new HandAction("Hero", HandActionType.SHOW, Street.Showdown), 
                }
            };
            TestFullHandHistorySummary(expectedSummary, "RunTwice");
            TestHandHistory(expectedHandHistory, "RunTwice");
        }

        [Test]
        public void RunThreeTimes()
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
                DateOfHandUtc = new DateTime(2018, 11, 16, 13, 15, 16),
                DealerButtonPosition = 2,
                HandId = HandID.From(1245993),
                NumPlayersSeated = 6,
                TableName = "NLHGold14",
                TotalPot = 55.04m,
                Rake = 2.7m,
                Jackpot = 1,
                Bingo = 0
            };

            HandHistory expectedHandHistory = new HandHistory()
            {
                CommunityCards = BoardCards.FromCards("Th9c5s6hJc"),
                Players = new PlayerList(new List<Player>
                {
                    new Player("88kkaa62", 110.97m, 1, HoleCards.FromCards("Js Jd")),
                    new Player("25611aab", 179.23m, 2),
                    new Player("Hero", 118.31m, 3, HoleCards.FromCards("8d3s")),
                    new Player("thg124da", 100, 4),
                    new Player("22ab6yhc", 26.77m, 5, HoleCards.FromCards("Kc Qs")),
                    new Player("ma2fcaaf", 114.72m, 6)
                }),
                Hero = new Player("Hero", 118.31m, 3, HoleCards.FromCards("8d3s")),
                RunItMultipleTimes = new RunItTwice[]
                {
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("Th9c5s6hJc"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("22ab6yhc", WinningsActionType.WINS, 17.12m, 0)
                        }
                    },
                    new RunItTwice 
                    {
                        Board = BoardCards.FromCards("Th9c5s4dkd"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("22ab6yhc", WinningsActionType.WINS, 17.11m, 0)
                        }
                    
                    },
                    new RunItTwice
                    {
                        Board = BoardCards.FromCards("Th9c5sAd2h"),
                        Actions = new List<HandAction> { },
                        Winners = new List<WinningsAction>
                        {
                            new WinningsAction("88kkaa62", WinningsActionType.WINS, 17.11m, 0)
                        }
                    }
                },
                Winners = new List<WinningsAction>() { 
                    new WinningsAction("22ab6yhc", WinningsActionType.WINS, 17.12m, 0),
                    new WinningsAction("22ab6yhc", WinningsActionType.WINS, 17.11m, 0),
                    new WinningsAction("88kkaa62", WinningsActionType.WINS, 17.11m, 0)
                },
                HandActions = new List<HandAction>() {
                    new HandAction("Hero", HandActionType.SMALL_BLIND, -0.5m, Street.Preflop),
                    new HandAction("thg124da", HandActionType.BIG_BLIND, -1, Street.Preflop),
                    new HandAction("22ab6yhc", HandActionType.CALL, 1, Street.Preflop),
                    new HandAction("ma2fcaaf", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("88kkaa62", HandActionType.RAISE, 5m, Street.Preflop),
                    new HandAction("25611aab", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("Hero", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("thg124da", HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction("22ab6yhc", HandActionType.CALL, 4, Street.Preflop),
                    new HandAction("22ab6yhc", HandActionType.CHECK, 0, Street.Flop), 
                    new HandAction("88kkaa62", HandActionType.BET, 11.5m, Street.Flop), 
                    new HandAction("22ab6yhc", HandActionType.RAISE, 21.77m, Street.Flop, true), 
                    new HandAction("88kkaa62", HandActionType.CALL, 10.27m, Street.Flop), 
                    new HandAction("22ab6yhc", HandActionType.SHOW, Street.Showdown), 
                    new HandAction("88kkaa62", HandActionType.SHOW, Street.Showdown) 
                }
            };
            TestFullHandHistorySummary(expectedSummary, "RunThreeTimes");
            TestHandHistory(expectedHandHistory, "RunThreeTimes");
        }
    }
}
