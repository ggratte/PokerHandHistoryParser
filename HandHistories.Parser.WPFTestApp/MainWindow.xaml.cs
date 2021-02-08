using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Parser.Parsers.Base;
using HandHistories.Parser.Parsers.Factory;
using HandHistories.Parser.Utils;
using HandHistories.Parser.WPFTestApp.ViewModels;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandHistories.Parser.WPFTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //var model = new ShellViewModel();
            //model.HandHistories = "";
            //model.CmdParse.Execute(null);

            var model = new ShellViewModel();

            model.AvailableSites.Add(SiteName.Unknown);
            model.AvailableSites.Add(SiteName.AsianPokerClubs);
            model.AvailableSites.Add(SiteName.BossMedia);
            model.AvailableSites.Add(SiteName.PokerStars);
            model.AvailableSites.Add(SiteName.PokerStarsFr);
            model.AvailableSites.Add(SiteName.PokerStarsIt);
            model.AvailableSites.Add(SiteName.PokerStarsEs);
            model.AvailableSites.Add(SiteName.FullTilt);
            model.AvailableSites.Add(SiteName.PartyPoker);
            model.AvailableSites.Add(SiteName.IPoker);
            model.AvailableSites.Add(SiteName.OnGame);
            model.AvailableSites.Add(SiteName.OnGameFr);
            model.AvailableSites.Add(SiteName.OnGameIt);
            model.AvailableSites.Add(SiteName.Pacific);
            model.AvailableSites.Add(SiteName.Entraction);
            model.AvailableSites.Add(SiteName.Merge);
            model.AvailableSites.Add(SiteName.WinningPoker);
            model.AvailableSites.Add(SiteName.WinningPokerV2);
            model.AvailableSites.Add(SiteName.MicroGaming);
            model.AvailableSites.Add(SiteName.Winamax);
            model.AvailableSites.Add(SiteName.IGT);

            DataContext = model;
        }
    }
}
