using HandHistories.Objects.GameDescription;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace HandHistories.Parser.WPFTestApp.ViewModels
{
    [ValueConversion(typeof(GameType), typeof(string))]
    public class GametypeShortNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var game =  (GameType)value;

            var limit = "";
            switch (game.Limit)
            {
                case GameLimitEnum.NoLimit:
                    limit = "NL";
                    break;
                case GameLimitEnum.FixedLimit:
                    limit = "FL";
                    break;
                case GameLimitEnum.PotLimit:
                    limit = "PL";
                    break;
                default:
                    throw new NotImplementedException();
            }

            var gametype = "";
            switch (game.Game)
            {
                case GameEnum.Holdem:
                    gametype = "H";
                    break;
                case GameEnum.Omaha:
                    gametype = "H";
                    break;
                case GameEnum.FiveCardOmaha:
                    gametype = "O5";
                    break;
                case GameEnum.OmahaHiLo:
                    gametype = "OHL";
                    break;
                case GameEnum.FiveCardOmahaHiLo:
                    gametype = "O5HL";
                    break;
                default:
                    gametype = gametype.ToString();
                    break;
            }

            return limit + gametype;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
