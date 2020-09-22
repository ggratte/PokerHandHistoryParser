using HandHistories.Objects.GameDescription;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace HandHistories.Parser.WPFTestApp.ViewModels
{
    public class ParsedHandViewModel : ViewModel
    {
        public string TableName { get; internal set; }
        public decimal BigBlind { get; internal set; }
        public decimal SmallBlind { get; internal set; }
        public string HandID { get; internal set; }
        public GameType Gametype { get; internal set; }
        public string Hero { get; internal set; }

        public ObservableCollection<ParsedPlayerViewModel> Players { get; set; }
        public ObservableCollection<ParsedEventViewModel> Actions { get; set; }
        public ObservableCollection<ParsedEventViewModel> Winners { get; set; }
    }
}
