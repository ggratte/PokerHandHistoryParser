using System;
using System.Collections.Generic;
using System.Text;

namespace HandHistories.Parser.WPFTestApp.ViewModels
{
    public class ParsedEventViewModel : ViewModel
    {
        public string PlayerName { get; set; }
        public string EventName { get; set; }
        public double Amount { get; set; }
    }
}
