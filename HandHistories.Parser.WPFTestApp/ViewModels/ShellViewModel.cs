using HandHistories.Objects.Actions;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Parser.Parsers.Base;
using HandHistories.Parser.Parsers.Factory;
using HandHistories.Parser.Utils;
using HandHistories.Parser.WPFTestApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace HandHistories.Parser.WPFTestApp.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        string _handHistories;

        ICommand _cmdParse;

        bool _validateHands;

        ObservableCollection<ParsedHandViewModel> _parsedHands = new ObservableCollection<ParsedHandViewModel>();

        string _statusText;

        ObservableCollection<SiteName> _availableSites = new ObservableCollection<SiteName>();

        SiteName? _selectedSite;

        public ShellViewModel()
        {
            CmdParse = new RelayCommand((arg) => ParseHands());
        }

        public string HandHistories { get => _handHistories; set => SetProperty(ref _handHistories, value, nameof(HandHistories)); }

        public ICommand CmdParse { get => _cmdParse; set => SetProperty(ref _cmdParse, value, nameof(CmdParse)); }

        public bool ValidateHands { get => _validateHands; set => SetProperty(ref _validateHands, value, nameof(ValidateHands)); }

        public  ObservableCollection<ParsedHandViewModel> ParsedHands { get => _parsedHands; set => SetProperty(ref _parsedHands, value, nameof(ParsedHands)); }

        public string StatusText { get => _statusText; set => SetProperty(ref _statusText, value, nameof(StatusText)); }

        public ObservableCollection<SiteName> AvailableSites { get => _availableSites; set => SetProperty(ref _availableSites, value, nameof(AvailableSites)); }

        public SiteName? SelectedSite { get => _selectedSite; set => SetProperty(ref _selectedSite, value, nameof(SelectedSite)); }

        void ParseHands()
        {
            if (SelectedSite == null)
            {
                StatusText = "Please pick a site";
                return;
            }

            ParsedHands.Clear();

            IHandHistoryParserFactory factory = new HandHistoryParserFactoryImpl();
            var handParser = factory.GetFullHandHistoryParser(SelectedSite.Value);

            try
            {
                int parsedHands = 0;
                Stopwatch SW = new Stopwatch();
                SW.Start();

                IHandHistoryParser fastParser = handParser as IHandHistoryParser;

                var result = new List<HandHistory>();
                var hands = fastParser.SplitUpMultipleHands(HandHistories.Trim());
                foreach (var hand in hands)
                {
                    try
                    {
                        var parsedHand = fastParser.ParseFullHandHistory(hand, true);
                        result.Add(parsedHand);
                        if (ValidateHands)
                        {
                            HandIntegrity.Assert(parsedHand);
                        }
                        parsedHands++;
                    }
                    catch (Exception ex)
                    {
                        //treeviewHands.Items.Add(new TreeViewItem()
                        //{
                        //    Header = "Error: " + ex.Message,
                        //});
                    }
                }

                SW.Stop();

                foreach (var hand in result.Select(CreateParsedHandViewModel))
                {
                    ParsedHands.Add(hand);
                }

                StatusText = $"Parsed {parsedHands} hands. {SW.Elapsed.TotalMilliseconds:0.00}ms";
            }
            catch (Exception ex)
            {
                StatusText = ex.Message;
            }
        }

        static ParsedHandViewModel CreateParsedHandViewModel(HandHistory hand)
        {
            var model = new ParsedHandViewModel()
            {
                HandID = hand.HandIdString,
                TableName = hand.TableName,
                BigBlind = hand.GameDescription.Limit.BigBlind,
                SmallBlind = hand.GameDescription.Limit.SmallBlind,
                Gametype = hand.GameDescription.GameType,
                Hero = hand.Hero?.PlayerName,
            };

            model.Players = new ObservableCollection<ParsedPlayerViewModel>(hand.Players.Select(CreateParsedPlayerViewModel));
            model.Actions = new ObservableCollection<ParsedEventViewModel>(hand.HandActions.Select(CreateParsedActionViewModel));
            model.Winners = new ObservableCollection<ParsedEventViewModel>(hand.Winners.Select(CreateParsedWinnerViewModel));

            return model;
        }

        private static ParsedEventViewModel CreateParsedActionViewModel(HandAction action)
        {
            return new ParsedEventViewModel()
            {
                PlayerName = action.PlayerName,
                Amount = (double)action.Amount,
                EventName = action.HandActionType.ToString(),
            };
        }

        private static ParsedEventViewModel CreateParsedWinnerViewModel(WinningsAction action)
        {
            return new ParsedEventViewModel()
            {
                PlayerName = action.PlayerName,
                Amount = (double)action.Amount,
                EventName = action.ActionType.ToString(),
            };
        }

        private static ParsedPlayerViewModel CreateParsedPlayerViewModel(Player player)
        {
            var model = new ParsedPlayerViewModel() {
                PlayerName = player.PlayerName,
                Stack = (double)player.StartingStack,
                Seat = player.SeatNumber,
                Holecards = player.HoleCards?.ToString() ?? "",
            };
            return model;
        }
    }
}
