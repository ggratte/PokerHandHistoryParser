﻿using System;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.Base;
using HandHistories.Parser.Parsers.FastParser.Entraction;
using HandHistories.Parser.Parsers.FastParser.FullTiltPoker;
using HandHistories.Parser.Parsers.FastParser.IPoker;
using HandHistories.Parser.Parsers.FastParser.Merge;
using HandHistories.Parser.Parsers.FastParser.MicroGaming;
using HandHistories.Parser.Parsers.FastParser.OnGame;
using HandHistories.Parser.Parsers.FastParser.PokerStars;
using HandHistories.Parser.Parsers.FastParser.Winamax;
using HandHistories.Parser.Parsers.FastParser._888;
using HandHistories.Parser.Parsers.FastParser.Winning;
using HandHistories.Parser.Parsers.FastParser.BossMedia;
using HandHistories.Parser.Parsers.FastParser.PartyPoker;
using HandHistories.Parser.Parsers.JSONParser.IGT;
using HandHistories.Parser.Parsers.LineCategoryParser.WinningPokerV2;
using HandHistories.Parser.Parsers.LineCategoryParser.PartyPoker;
using System.Text.RegularExpressions;
using HandHistories.Parser.Parsers.FastParser.GGPoker;

namespace HandHistories.Parser.Parsers.Factory
{
    public class HandHistoryParserFactoryImpl : IHandHistoryParserFactory
    {
        public HandHistoryParserFactoryImpl()
        {
        }

        public IHandHistoryParser GetFullHandHistoryParser(SiteName siteName)
        {
            switch (siteName)
            {
                case SiteName.PartyPokerEs:
                case SiteName.PartyPokerFr:
                case SiteName.PartyPokerNJ:
                case SiteName.PartyPokerIt:
                case SiteName.PartyPoker:
                    return new PartyPokerLineCatParserImpl();
                case SiteName.PokerStars:
                case SiteName.PokerStarsFr:
                case SiteName.PokerStarsIt:
                case SiteName.PokerStarsEs:
                case SiteName.PPPoker:
                    return new PokerStarsFastParserImpl(siteName);
                case SiteName.Merge:
                    return new MergeFastParserImpl();
                case SiteName.IPoker:
                    return new IPokerFastParserImpl();
                case SiteName.IPoker2:
                    return new IPokerFastParserImpl(true);
                case SiteName.OnGame:
                    return new OnGameFastParserImpl();
                case SiteName.OnGameFr:
                    return new OnGameFastParserImpl(SiteName.OnGameFr);
                case SiteName.OnGameIt:
                    return new OnGameFastParserImpl(SiteName.OnGameIt);                    
                case SiteName.Pacific:
                    return new Poker888FastParserImpl();
                case SiteName.Entraction:
                    return new EntractionFastParserImpl();
                case SiteName.FullTilt:
                    return new FullTiltPokerFastParserImpl();
                case SiteName.MicroGaming:
                    return new MicroGamingFastParserImpl();
                case SiteName.Winamax:
                    return new WinamaxFastParserImpl();
                case SiteName.WinningPoker:
                    var wpnMulti = new MultiVersionParser();
                    wpnMulti.Add(new WinningPokerNetworkFastParserImpl(), p => p.StartsWith("Game started at: "));
                    wpnMulti.Add(new WinningPokerNetworkV2LineCatParserImpl(), p => p.StartsWith("Hand #"));
                    //wpnMulti.Add(new WinningPokerNetworkV2LineCatParserImpl(), p => p.StartsWith("Game Hand #")); //Tournament
                    return wpnMulti;
                case SiteName.WinningPokerV1:
                    return new WinningPokerNetworkFastParserImpl();
                case SiteName.WinningPokerV2:
                    return new WinningPokerNetworkV2LineCatParserImpl();
                case SiteName.BossMedia:
                    return new BossMediaFastParserImpl();
                case SiteName.IGT:
                    return new IGTJSONParserImpl();
                case SiteName.AsianPokerClubs:
                    var splitRegex = new Regex("PokerMaster Hand #", RegexOptions.Compiled);
                    return new PokerStarsFastParserImpl(SiteName.AsianPokerClubs, splitRegex);
                case SiteName.GGPoker:
                    return new GGPokerFastParserImpl();
                default:
                    throw new NotImplementedException("GetHandHistorySummaryParser: No full regex parser for " + siteName);
            }
        }

        public IHandHistorySummaryParser GetHandHistorySummaryParser(SiteName siteName)
        {
            switch (siteName)
            {
                case SiteName.PartyPokerEs:
                case SiteName.PartyPokerFr:
                case SiteName.PartyPokerNJ:
                case SiteName.PartyPokerIt:
                case SiteName.PartyPoker:
                    return new PartyPokerLineCatParserImpl();
                case SiteName.PokerStars:
                case SiteName.PokerStarsFr:
                case SiteName.PokerStarsIt:
                case SiteName.PokerStarsEs:
                    return GetFullHandHistoryParser(siteName);
                case SiteName.Merge:
                    return new MergeFastParserImpl();
                case SiteName.IPoker:
                    return new IPokerFastParserImpl();
                case SiteName.IPoker2:
                    return new IPokerFastParserImpl(true);
                case SiteName.Pacific:
                    return new Poker888FastParserImpl();
                case SiteName.Entraction:
                    return new EntractionFastParserImpl();                    
                case SiteName.OnGame:
                    return new OnGameFastParserImpl();
                case SiteName.OnGameFr:
                    return new OnGameFastParserImpl(SiteName.OnGameFr);
                case SiteName.OnGameIt:
                    return new OnGameFastParserImpl(SiteName.OnGameIt);
                case SiteName.FullTilt:
                    return new FullTiltPokerFastParserImpl();
                case SiteName.MicroGaming:
                    return new MicroGamingFastParserImpl();
                case SiteName.Winamax:
                    return new WinamaxFastParserImpl();
                case SiteName.WinningPoker:
                    var wpnMulti = new MultiVersionParser();
                    wpnMulti.Add(new WinningPokerNetworkFastParserImpl(), p => p.StartsWith("Game started at: "));
                    wpnMulti.Add(new WinningPokerNetworkV2LineCatParserImpl(), p => p.StartsWith("Hand #"));
                    return wpnMulti;
                case SiteName.WinningPokerV1:
                    return new WinningPokerNetworkFastParserImpl();
                case SiteName.WinningPokerV2:
                    return new WinningPokerNetworkV2LineCatParserImpl();
                case SiteName.BossMedia:
                    return new BossMediaFastParserImpl();
                case SiteName.IGT:
                    return new IGTJSONParserImpl();
                case SiteName.AsianPokerClubs:
                    var splitRegex = new Regex("PokerMaster Hand #", RegexOptions.Compiled);
                    return new PokerStarsFastParserImpl(SiteName.AsianPokerClubs, splitRegex);
                case SiteName.GGPoker:
                    return new GGPokerFastParserImpl();
                default:
                    throw new NotImplementedException("GetHandHistorySummaryParser: No summary regex parser for " + siteName);
            }
        }
    }
}
