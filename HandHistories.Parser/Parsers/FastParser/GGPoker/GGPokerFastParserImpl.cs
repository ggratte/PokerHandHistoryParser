using HandHistories.Parser.Parsers.Base;
using HandHistories.Parser.Parsers.FastParser.Base;
using System;
using System.Collections.Generic;
using System.Text;
using HandHistories.Objects;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Parser.Parsers.Exceptions;
using HandHistories.Parser.Utils.Extensions;
using HandHistories.Parser.Utils.FastParsing;
using HandHistories.Parser.Utils.Uncalled;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.ComponentModel;
using System.Net;

namespace HandHistories.Parser.Parsers.FastParser.GGPoker
{

    public partial class GGPokerFastParserImpl : HandHistoryParserFastImpl, IThreeStateParser
    {
        static readonly TimeZoneInfo PokerStarsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public override bool RequiresAdjustedRaiseSizes => true;

        public override bool SupportRunItTwice => true;

        public override bool RequiresActionSorting => true;

        public override SiteName SiteName => SiteName.GGPoker;

        private readonly NumberFormatInfo _numberFormatInfo;

        private readonly Regex _handSplitRegex;

        private readonly String _summarySeperator = " | ";

        public GGPokerFastParserImpl()
        {
            _numberFormatInfo = new NumberFormatInfo
            {
                NegativeSign = "-",
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = ",",
                CurrencySymbol = "$"
            };
            _handSplitRegex = new Regex("Poker Hand #", RegexOptions.Compiled);
        }

        public override IEnumerable<string> SplitUpMultipleHands(string rawHandHistories)
        {
            return Utils.HandSplitter.RegexHandSplitter.Split(rawHandHistories, _handSplitRegex);
        }

        protected override int ParseDealerPosition(string[] handLines)
        {
            // Expect the 2nd line to look like the below two, one for regular table, one for rush&cash:
            // Table 'RushAndCash143219' 6-max Seat #1 is the button

            // or

            // Table 'NLHSilver9' 6-max Seat #2 is the button
            string line = handLines[1];
            return FastInt.Parse(line.Substring(line.Length-15,1));
        }

        protected override DateTime ParseDateUtc(string[] handLines)
        {
            // Expect the first line to look like this:
            // Poker Hand #RC2345481320: Hold'em No Limit  ($0.05/$0.1) - 2018/03/31 14:43:23

            // or

            // Poker Hand #HD138495: Hold'em No Limit  ($0.5/$1) - 2019/10/12 01:43:27
            string line = handLines[0];
            return DateTime.ParseExact(line.Substring(line.Length - 19), "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
        }

        protected override void ParseExtraHandInformation(string[] handLines, HandHistorySummary handHistorySummary)
        {
            if (handHistorySummary.Cancelled) { return; }

            for (int i = handLines.Length - 1; i >= 0; i--)
            {
                string line = handLines[i];

                // *** SUMMARY ***
                if (line.StartsWithFast("*** SUMMARY"))
                {
                    break;
                }

                // Total pot $1 | Rake $0.05 | Jackpot $0 | Bingo $0
                if (line.StartsWithFast("Total pot"))
                {
                    string[] breakdown = line.Split(_summarySeperator);
                    handHistorySummary.TotalPot = breakdown[0].Substring(10).ParseAmountWS();
                    handHistorySummary.Rake = breakdown[1].Substring(5).ParseAmountWS();
                    handHistorySummary.Jackpot = breakdown[2].Substring(8).ParseAmountWS();
                    handHistorySummary.Bingo = breakdown[3].Substring(6).ParseAmountWS();
                }
            }
        }

        public int ParseBlindActions(string[] handLines, List<HandAction> handActions, int firstActionIndex)
        {
            var bigBlind = false;

            for (int i = 2; i < handLines.Length; i++)
            {
                var line = handLines[i];

                switch (line.Last())
                {
                    //*** HOLE CARDS ***
                    case '*':
                        return i + 1;
                    //*** FLOP *** [6d 7c 6h]
                    //*** TURN *** [6d 7c 6h] [2s]
                    //*** RIVER *** [6d 7c 6h 2s] [Qc]
                    case ']':
                        throw new HandActionException(string.Join(Environment.NewLine, handLines), "Unexpected Line: " + line);

                    // Seat 1: 12345678 ($100 in chips)
                    case ')':
                        continue;

                    default:
                        break;
                }


                int colonIndex = line.LastIndexOf(':');

                if (colonIndex != -1)
                {
                    var action = ParsePostingActionLine(line, colonIndex, ref bigBlind);
                    handActions.Add(action);
                }
            }
            throw new HandActionException(string.Join(Environment.NewLine, handLines), "No end of posting actions");
        }

        public static HandAction ParsePostingActionLine(string actionLine, int colonIndex, ref bool bigBlindPosted)
        {
            string playerName = actionLine.Substring(0, colonIndex);
            bool isAllIn = false;

            // Expect lines to look like:
            // ac12334d: posts small blind $0.5
            // ac12334d: posts big blind $1
            // 62dfdfaf: straddle $2
            char identifierChar = actionLine[colonIndex + 8];

            // xyz: posts big blind $1 and is all-in
            if (actionLine.EndsWithFast("is all-in"))
            {
                isAllIn = true;
                actionLine = actionLine.Substring(0, actionLine.Length - 14);
            }

            int firstDigitIndex;
            HandActionType handActionType;

            switch (identifierChar)
            {
                // ac12334d: posts small blind $0.5
                case 's':
                    firstDigitIndex = colonIndex + 20;
                    handActionType = HandActionType.SMALL_BLIND;
                    break;

                // ac12334d: posts big blind $0.5
                case 'b':
                    firstDigitIndex = colonIndex + 18;
                    handActionType = bigBlindPosted ? HandActionType.POSTS : HandActionType.BIG_BLIND;
                    bigBlindPosted = true;
                    break;

                // xyz: straddle $2
                case 'l':
                    firstDigitIndex = colonIndex + 11;
                    handActionType = HandActionType.STRADDLE;
                    break;

                // xyz: posts missed blind $0.5
                case 'm':
                    firstDigitIndex = colonIndex + 21;
                    handActionType = HandActionType.POSTS_DEAD;
                    break;

                default:
                    throw new HandActionException(actionLine, "ParsePostingActionLine: Unregonized line " + actionLine);
            }

            decimal amount = actionLine.Substring(firstDigitIndex).ParseAmount();
            return new HandAction(playerName, handActionType, amount, Street.Preflop, isAllIn);
        }

        public int ParseGameActions(string[] handLines, List<HandAction> handActions, List<WinningsAction> winners, int firstActionIndex, out Street street)
        {
            street = Street.Preflop;

            for (int lineNumber = firstActionIndex; lineNumber < handLines.Length; lineNumber++)
            {
                string handLine = handLines[lineNumber];

                try
                {
                    bool isFinished = ParseLine(handLine, ref street, handActions, winners);

                    if (isFinished)
                    {
                        return lineNumber + 1;
                    }
                }
                catch (Exception ex)
                {
                    throw new HandActionException(handLine, "Couldn't parse line '" + handLine + " with ex: " + ex.Message);
                }
            }
            throw new InvalidHandException(string.Join(Environment.NewLine, handActions));
        }

        public void ParseShowDown(string[] handLines, List<HandAction> handActions, List<WinningsAction> winners, int firstActionIndex, GameType gameType)
        {
            for (int i = firstActionIndex; i < handLines.Length; i++)
            {
                var line = handLines[i];

                switch (line.Last())
                {
                    // xyz collected $7.50 from pot.
                    case 't':
                        if (line.EndsWithFast(" pot"))
                        {
                            winners.Add(ParseWinnings(line));
                        }
                        continue;

                    //*** FLOP *** [6d 7c 6h]
                    //*** TURN *** [6d 7c 6h] [2s]
                    //*** RIVER *** [6d 7c 6h 2s] [Qc]
                    case ']':
                        continue;

                    //*** SUMMARY ***
                    //*** SHOWDOWN ***
                    //*** THIRD RIVER ***
                    //*** SECOND RIVER ***
                    //*** THIRD RIVER ***
                    //*** FIRST SHOWDOWN ***
                    //*** SECOND SHOWDOWN ***
                    //*** THIRD SHOWDOWN ***
                    case '*':
                        // *** SUMMARY
                        if (line.StartsWithFast("*** SUM"))
                        {
                            return;
                        }
                        else if (line.EndsWithFast("DOWN ***"))
                        {
                            continue;
                        }
                        else
                        {
                            throw new ArgumentException("Unhandled line: " + line);
                        }
                    // 1234abcd: shows[4d 3d] (Pair of Fours)
                    case ')':
                        break;


                    default:
                        continue;

                }

                int colonIndex = line.LastIndexOf(':');   // do backwards as players can : in their name
                var action = ParseMiscShowdownLine(line, colonIndex);
                handActions.Add(action);
            }
        }

        public HandAction ParseMiscShowdownLine(string actionLine, int colonIndex) 
        { 
            string playerName = actionLine.Substring(0, colonIndex);

            char actionIdentifier = actionLine[colonIndex + 2];

            switch (actionIdentifier)
            {
                case 's': // RECHUK: shows [Ac Qh] (a full house, Aces full of Queens)
                    return new HandAction(playerName, HandActionType.SHOW, Street.Showdown);
                default:
                    throw new HandActionException(actionLine, "ParseMiscShowdownLine: Unrecognized line '" + actionLine + "'");
            }
        }

        protected override string ParseHeroName(string[] handlines)
        {
            return "Hero";  // In GGPoker hand history, the player's name is always labelled as Hero.
        }

        protected override long[] ParseHandId(string[] handLines)
        {
            // Expect the first line to look like this:
            // Poker Hand #RC2345481320: Hold'em No Limit  ($0.05/$0.1) - 2018/03/31 14:43:23

            // or

            // Poker Hand #HD138495: Hold'em No Limit  ($0.5/$1) - 2019/10/12 01:43:27
            string line = handLines[0];
            string id = line.Split(':')[0].Split(' ')[2].Substring(3);
            return HandID.Parse(id);
        }

        protected override long ParseTournamentId(string[] handLines)
        {
            throw new NotImplementedException();
        }

        protected override string ParseTableName(string[] handLines)
        {
            // Table 'NLHSilver5' 6-max Seat #2 is the button
            string tableNameWithQuote = handLines[1].Split(' ')[1];
            return tableNameWithQuote.Substring(1, tableNameWithQuote.Length - 2);
        }

        protected override PokerFormat ParsePokerFormat(string[] handLines)
        {
            // only support cash game for now
            return PokerFormat.CashGame;
        }

        protected override SeatType ParseSeatType(string[] handLines)
        {
            // Table 'NLHSilver5' 6-max Seat #2 is the button
            string seatType = handLines[1].Split(" ")[2];
            if (seatType[0] == '6')
            {
                return SeatType.FromMaxPlayers(6);
            }
            else if (seatType[0] == '9')
            {
                return SeatType.FromMaxPlayers(9);
            }
            throw new Exception("Invalid seatType " + handLines[1]);
        }

        protected override GameType ParseGameType(string[] handLines)
        {
            var line = handLines[0];
            var colonIndex = line.IndexOf(':', 12);

            // can be either 1 or 2 spaces after the colon
            var startIndex = colonIndex + 1;
            var endIndex = line.IndexOf('(', colonIndex) - 2;

            switch (line.SubstringBetween(startIndex, endIndex).Trim())
            {
                case "Hold'em No Limit": return GameType.NoLimitHoldem;
            }
            // Only Holdem is supported for now.
            return GameType.Unknown;
        }

        protected override TableType ParseTableType(string[] handLines)
        {
            // Expect the first line to look like this:
            // Poker Hand #RC2345481320: Hold'em No Limit  ($0.05/$0.1) - 2018/03/31 14:43:23

            // or

            // Poker Hand #HD138495: Hold'em No Limit  ($0.5/$1) - 2019/10/12 01:43:27
            string type = handLines[0].Substring(12, 2);
            if (type == "HD")
            {
                return TableType.FromTableTypeDescriptions(TableTypeDescription.Regular);
            }
            else if (type == "RC")
            {
                return TableType.FromTableTypeDescriptions(TableTypeDescription.Zoom);
            }
            else
            {
                throw new Exception("Unsupported TableType: " + handLines[0]);
            }
        }

        protected override Limit ParseLimit(string[] handLines)
        {
            // Expect the first line to look like this:
            // Poker Hand #RC2345481320: Hold'em No Limit  ($0.05/$0.1) - 2018/03/31 14:43:23

            // or

            // Poker Hand #HD138495: Hold'em No Limit  ($0.5/$1) - 2019/10/12 01:43:27
            string stake = handLines[0].Split(' ')[7];
            stake = stake.Substring(1, stake.Length - 2);
            Currency currency = ParseCurrency(handLines[0], stake[0]);

            int slashIndex = stake.IndexOf('/');
            string smallBlind = stake.Substring(0, slashIndex);
            string bigBlind = stake.Substring(slashIndex + 1);

            decimal small = smallBlind.ParseAmount();
            decimal big = bigBlind.ParseAmount();

            return Limit.FromSmallBlindBigBlind(small, big, currency);
        }

        protected override Buyin ParseBuyin(string[] handLines)
        {
            // not support tournament yet
            throw new NotImplementedException();
        }

        public override bool IsValidHand(string[] handLines)
        {
            // Does not seem to apply to gg.
            return true;
        }

        public override bool IsValidOrCancelledHand(string[] handLines, out bool isCancelled)
        {
            // Does not seem to apply to gg.
            isCancelled = false;
            return true;
        }

        protected override List<HandAction> ParseHandActions(string[] handLines, GameType gameType, out List<WinningsAction> winners)
        {
            int actionIndex = GetFirstActionIndex(handLines);

            List<HandAction> handActions = new List<HandAction>(handLines.Length - actionIndex);
            winners = new List<WinningsAction>();

            actionIndex = ParseBlindActions(handLines, handActions, actionIndex);

            Street currentStreet;
            actionIndex = ParseGameActions(handLines, handActions, winners, actionIndex, out currentStreet);

            ParseShowDown(handLines, handActions, winners, actionIndex, gameType);

            return handActions;
        }

        protected override PlayerList ParsePlayers(string[] handLines)
        {
            PlayerList playerList = new PlayerList();

            int lineNumber = 2;
            for (; lineNumber < handLines.Length; lineNumber++)
            {
                // Expecting Seat 1: Hero ($124.64 in chips)
                string line = handLines[lineNumber];

                if (!line.StartsWithFast("Seat "))
                {
                    break;
                }

                const int seatNumberStartIndex = 4;
                int spaceIndex = line.IndexOf(' ', seatNumberStartIndex);
                int colonIndex = line.IndexOf(':', spaceIndex + 1);
                int seatNumber = FastInt.Parse(line, spaceIndex + 1);

                // we need to find the ( before the number. players can have ( in their name so we need to go backwards and skip the last one
                int openParenIndex = line.LastIndexOf('(');
                int spaceAfterOpenParen = line.IndexOf(' ', openParenIndex);

                string playerName = line.Substring(colonIndex + 2, (openParenIndex - 1) - (colonIndex + 2));

                string stackString = line.Substring(openParenIndex + 1, spaceAfterOpenParen - (openParenIndex + 1));
                decimal stack = stackString.ParseAmount();

                playerList.Add(new Player(playerName, stack, seatNumber));
            }

            for (; lineNumber < handLines.Length; lineNumber++)
            {
                string line = handLines[lineNumber];

                // Dealt to Hero [3h 4s]
                if (line.StartsWithFast("Dealt to") && line.Last() == ']')
                {
                    const int nameStartIndex = 9;//"Dealt to ".Length
                    int cardsStartIndex = line.LastIndexOf('[') + 1;
                    int nameEndIndex = cardsStartIndex - 2;

                    string name = line.Substring(nameStartIndex, nameEndIndex - nameStartIndex);
                    string cards = line.Substring(cardsStartIndex, line.Length - cardsStartIndex - 1);

                    playerList[name].HoleCards = HoleCards.FromCards(cards);
                }

                // 689fsff: shows [4d 3d](Pair of Fours)
                // Hero: shows [Qs 4s](Pair of Fours)
                if (line.Contains("shows ["))
                {
                    int closeSquareBrackedIndex = line.LastIndexOf(']', line.Length - 1);
                    int openSquareBracketIndex = line.LastIndexOf('[', closeSquareBrackedIndex);
                    int colonIndex = line.LastIndexOf(':', openSquareBracketIndex);

                    string playerName = line.Substring(0, colonIndex);
                    string cards = line.Substring(openSquareBracketIndex + 1, closeSquareBrackedIndex - (openSquareBracketIndex + 1));

                    playerList[playerName].HoleCards = HoleCards.FromCards(cards);
                }
            }

            return playerList;
        }

        public HandAction ParseUncalledBetLine(string actionLine, Street currentStreet)
        {
            // Uncalled bet lines look like:
            // Uncalled bet ($6) returned to woezelenpip

            // position 14 is after the open paren
            int closeParenIndex = actionLine.IndexOf(')', 14);
            var amount = actionLine.Substring(14, closeParenIndex - 14);

            int firstLetterOfName = closeParenIndex + 14; // ' returned to ' is length 14

            string playerName = actionLine.Substring(firstLetterOfName, actionLine.Length - firstLetterOfName);

            return new HandAction(playerName, HandActionType.UNCALLED_BET, amount.ParseAmount(), currentStreet);
        }

        protected override BoardCards ParseCommunityCards(string[] handLines)
        {
            for (int i = handLines.Length - 1; i >= 0; i--)
            {
                string line = handLines[i];
                // FIRST Board [3c 9d 3d 8d 7s]
                // Board [3c 9d 3d 8d 7s]
                if (line.StartsWithFast("Board") || line.StartsWithFast("FIRST Board"))
                {
                    return ParseBoard(line);
                }
            }
            return BoardCards.ForPreflop();
        }

        public override RunItTwice[] ParseMultipleRuns(string[] handLines)
        {
            RunItTwice[] multipleRuns = new RunItTwice[3];
            for (int i = 0; i < 3; i++)
            {
                multipleRuns[i] = new RunItTwice();
            }

            int currentBoard = -1;
            for (int i = 0; i < handLines.Length; i++)
            {
                string line = handLines[i];
                // FIRST Board [3c 9d 3d 8d 7s]
                // Board [3c 9d 3d 8d 7s]
                // SECOND Board [8h]
                // THIRD Board [8s]
                // THIRD Board [4c Qc 6c Ac 9c]
                if (line.EndsWithFast("]") && (line.First() == 'F' || line.First() == 'B' || line.First() == 'S' || line.First() == 'T'))
                {
                    switch (currentBoard)
                    {
                        case 0:
                            multipleRuns[currentBoard].Board = ParseBoard(line);
                            break;
                        case 1:
                        case 2:
                            multipleRuns[currentBoard].Board = ParseBoard(line, multipleRuns[currentBoard - 1].Board);
                            break;
                        default:
                            throw new Exception("Exception in parsing multiple runs: " +  currentBoard.ToString() + handLines[i]);

                    }
                    currentBoard++;
                }

                // expect to look like this:
                // 125b4606 collected $2.5 from pot
                if (line.EndsWithFast(" pot"))
                {
                    line = handLines[i];
                    multipleRuns[currentBoard].Winners.Add(ParseWinnings(line));
                }

                // expect to look like this:
                // xyz: Receives Cashout ($9.98)
                //
                // Player who chooses to cashout will not have multiple runs.
                // Store the info on the first board.
                if (line.Contains("Receives Cashout"))
                {
                    line = handLines[i];
                    multipleRuns[0].Winners.Add(ParseWinnings(line));
                }

                // *** SHOWDOWN ***
                // *** FIRST SHOWDOWN ***
                // *** SECOND SHOWDOWN ***
                // *** THIRD SHOWDOWN ***
                if (line.EndsWithFast("SHOWDOWN ***")) 
                {
                    currentBoard++;
                }

                // *** SUMMARY ***
                if (line.StartsWithFast("*** SUMMARY"))
                {
                    // we need reset currentBoard for BOARD runout parsing.
                    currentBoard = 0;
                }
            }

            return multipleRuns;
        }

        private bool ParseLine(string line, ref Street currentStreet, List<HandAction> handActions, List<WinningsAction> winners)
        {
            //We filter out only possible line endings we want
            char lastChar = line[line.Length - 1];

            // Uncalled bet lines look like:
            // Uncalled bet ($6) returned to fadfac2sd 
            if (line.Length > 29 && line[13] == '(')
            {
                handActions.Add(ParseUncalledBetLine(line, currentStreet));
                currentStreet = Street.Showdown;
                return true;
            }

            if (line.Contains(" shows ")) 
            {
                handActions.Add(ParseMiscShowdownLine(line, line.LastIndexOf(':')));
                currentStreet = Street.Showdown;
                return false;
            }

            if (line.StartsWithFast("Dealt to"))
            {
                return false;
            }

            switch (lastChar)
            {
                //All actions with an amount(BET, CALL, RAISE)
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    break;

                //matze1987: raises $8.94 to $10.94 and is all-in
                case 'n':
                    break;


                //*** SUMMARY ***
                //*** SHOW DOWN ***
                case '*':
                //*** FLOP *** [Qs Js 3h]
                //Dealt to PS_Hero [4s 7h]
                case ']':
                    char firstChar = line[0];

                    if (firstChar == '*')
                    {
                        return ParseCurrentStreet(line, ref currentStreet);
                    }
                    return false;

                // xyz: folds
                // xyz: checks
                case 's':
                    if (line.EndsWithFast(" folds") || line.EndsWithFast(" checks")) { break; }
                    goto default;

                // xyz: Receives Cashout ($9.98)
                case ')':
                    if (line.Contains("Cashout"))
                    {
                        break;
                    }
                    goto default;

                default:
                    return false;
            }

            int colonIndex = line.LastIndexOf(':'); // do backwards as players can have : in their name

            switch (currentStreet)
            {
                case Street.Showdown:
                case Street.Null:
                    throw new HandActionException("", "Invalid State: Street");

                default:
                    if (colonIndex > -1 && !line.Contains("Receives Cashout"))
                    {
                        handActions.Add(ParseRegularActionLine(line, colonIndex, currentStreet));
                    }
                    else
                    {
                        winners.Add(ParseWinnings(line));
                    }
                    break;
            }

            return false;
        }

        public HandAction ParseRegularActionLine(string actionLine, int colonIndex, Street currentStreet)
        {

            string playerName = actionLine.Substring(0, colonIndex);

            // all-in likes look like: 'Piotr280688: raises $8.32 to $12.88 and is all-in'
            bool isAllIn = actionLine[actionLine.Length - 1] == 'n';
            if (isAllIn)// Remove the  ' and is all in' and just proceed like normal
            {
                actionLine = actionLine.Remove(actionLine.Length - 14);
            }

            // lines that reach the cap look like tzuiop23: calls $62 and has reached the $80 cap
            bool hasReachedCap = actionLine[actionLine.Length - 1] == 'p';
            if (hasReachedCap)// Remove the  ' and has reached the $80 cap' and just proceed like normal
            {
                int lastNonCapCharacter = actionLine.LastIndexOf('n') - 2;  // find the n in the and
                actionLine = actionLine.Remove(lastNonCapCharacter);
            }

            char actionIdentifier = actionLine[colonIndex + 2];

            HandActionType actionType;
            decimal amount;
            int firstDigitIndex;

            switch (actionIdentifier)
            {
                //gaydaddy: folds
                case 'f':
                    return new HandAction(playerName, HandActionType.FOLD, currentStreet);

                case 'c':
                    //Piotr280688: checks
                    if (actionLine[colonIndex + 3] == 'h')
                    {
                        return new HandAction(playerName, HandActionType.CHECK, currentStreet);
                    }
                    //MECO-LEO: calls $1.23
                    firstDigitIndex = actionLine.LastIndexOf(' ') + 1;
                    amount = actionLine.Substring(firstDigitIndex, actionLine.Length - firstDigitIndex).ParseAmount();
                    actionType = HandActionType.CALL;
                    break;

                //MS13ZEN: bets $1.76
                case 'b':
                    firstDigitIndex = actionLine.LastIndexOf(' ') + 1;
                    amount = actionLine.Substring(firstDigitIndex, actionLine.Length - firstDigitIndex).ParseAmount();
                    actionType = HandActionType.BET;
                    break;

                //Zypherin: raises $6400 to $8300              
                case 'r':
                    firstDigitIndex = actionLine.LastIndexOf(' ') + 1;
                    amount = actionLine.Substring(firstDigitIndex, actionLine.Length - firstDigitIndex).ParseAmount();
                    actionType = HandActionType.RAISE;
                    break;
                
                //xyz: Pays Cashout Risk ($22.68)
                case 'P':
                    firstDigitIndex = actionLine.LastIndexOf(' ') + 2;
                    amount = actionLine.Substring(firstDigitIndex, actionLine.Length - firstDigitIndex-1).ParseAmount();
                    actionType = HandActionType.PAYS_INSURANCE_FEE;
                    currentStreet = Street.Showdown;
                    break;

                default:
                    throw new HandActionException(actionLine, "ParseRegularActionLine: Unrecognized line:" + actionLine);
            }

            return new HandAction(playerName, actionType, amount, currentStreet, isAllIn);
        }


        private int GetFirstActionIndex(string[] handLines)
        {
            for (int lineNumber = 2; lineNumber < handLines.Length; lineNumber++)
            {
                // Seat 8: 4fgjkscjfds ($100.51 in chips)
                // fasjeif21: posts small blind $5
                string line = handLines[lineNumber];
                if (line[0] != 'S' || line[line.Length - 1] != ')')
                {
                    return lineNumber;
                }
            }

            throw new HandActionException(string.Empty, "GetFirstActionIndex: Couldn't find it.");
        }

        private static bool ParseCurrentStreet(string line, ref Street currentStreet)
        {
            // *** SHOWDOWN
            // *** FLOP
            // *** TURN
            // *** RIVER
            char typeOfEventChar = line[7];

            // this way we implement the collected lines in the regular showdown for the hand
            // both showdowns will be included in the regular hand actions, so the regular hand actions can be used for betting/pot/rake verification
            // might be readjusted so that only the first one is the regular handactions, and the second one goes to runittwice

            // *** FIRST FLOP
            // *** FIRST TURN
            // *** FIRST RIVER 
            // *** FIRST SHOWNDOWN 
            if (typeOfEventChar == 'S')
                typeOfEventChar = line[13];

            // *** SECOND FLOP
            // *** SECOND TURN
            // *** SECOND RIVER
            // *** SECOND SHOWDOWN
            if (typeOfEventChar == 'O')
                typeOfEventChar = line[14];


            // *** THIRD FLOP
            // *** THIRD TURN
            // *** THIRD RIVER
            // *** THIRD SHOWDOWN
            if (typeOfEventChar == 'R')
                typeOfEventChar = line[13];

            switch (typeOfEventChar)
            {
                case 'P':
                    currentStreet = Street.Flop;
                    return false;
                case 'N':
                    currentStreet = Street.Turn;
                    return false;
                case 'E':
                    currentStreet = Street.River;
                    return false;
                case 'W':
                    currentStreet = Street.Showdown;
                    return true;
                // *** SUMMARY
                case 'M':
                    return true;
                default:
                    throw new HandActionException(line, "Unrecognized line w/ a *:" + line);
            }
        }

        private BoardCards ParseBoard(string line)
        {
            int openBracketIndex = line.IndexOf('[');
            int closeBracketIndex = line.IndexOf(']', openBracketIndex);

            return BoardCards.FromCards(line.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1));
        }

        private BoardCards ParseBoard(string line, BoardCards baseBoard)
        {
            BoardCards newRunBoard = ParseBoard(line);
            switch (newRunBoard.Count)
            {
                case 1:
                    return BoardCards.FromCards(baseBoard.GetCards().GetRange(0, 4).Concat(newRunBoard.GetCards()).ToArray());

                case 2:
                    return BoardCards.FromCards(baseBoard.GetCards().GetRange(0, 3).Concat(newRunBoard.GetCards()).ToArray());

                case 5:
                    return newRunBoard;

                default:
                    throw new Exception("Parse board failed: " + line);
            }
        }

        public WinningsAction ParseWinnings(string line)
        {
            if (line.Contains("collected"))
            {
                // 12b64606 collected $2.5 from pot
                int firstSpaceIndex = line.IndexOf(" collected");
                int dollarSignIndex = line.LastIndexOf('$');
                int amountEndingIndex = line.Length - 8;

                string playerName = line.Substring(0, firstSpaceIndex);
                string amount = line.Substring(dollarSignIndex, amountEndingIndex - dollarSignIndex - 1);

                // 0 for main pot. In gg hand history, it does not show side pot information.
                return new WinningsAction(playerName, WinningsActionType.WINS, amount.ParseAmount(), 0);
            }
            else if (line.Contains("Receives Cashout"))
            {
                // xyz: Receives Cashout ($9.98)
                int lastOpenBracketIndex = line.LastIndexOf('(');
                int colonIndex = line.LastIndexOf(":");

                string playerName = line.Substring(0, colonIndex);
                string amount = line.Substring(lastOpenBracketIndex + 1, line.Length - lastOpenBracketIndex - 2);

                return new WinningsAction(playerName, WinningsActionType.INSURANCE, amount.ParseAmount(), 0);
            }
            else
            {
                throw new Exception("Unknown winning line: " + line);
            }
        }

        private Currency ParseCurrency(string handLine, char currencySymbol)
        {
            switch (currencySymbol)
            {
                case '$':
                    _numberFormatInfo.CurrencySymbol = "$";
                    return Currency.USD;
                case '€':
                    _numberFormatInfo.CurrencySymbol = "€";
                    return Currency.EURO;
                case '£':
                    _numberFormatInfo.CurrencySymbol = "£";
                    return Currency.GBP;
                case '¥':
                    _numberFormatInfo.CurrencySymbol = "¥";
                    return Currency.CNY;

                default:
                    throw new CurrencyException(handLine, "Unrecognized currency symbol " + currencySymbol);
            }
        }

    }
}
