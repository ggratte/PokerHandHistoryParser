namespace HandHistories.Parser.WPFTestApp.ViewModels
{
    public class ParsedPlayerViewModel : ViewModel
    {
        string _playerName;
        double _stack;
        int _seat;
        string _holecards;

        public string PlayerName { get => _playerName; set => SetProperty(ref _playerName, value, nameof(PlayerName)); }
        public double Stack { get => _stack; set => SetProperty(ref _stack, value, nameof(Stack)); }
        public int Seat { get => _seat; set => SetProperty(ref _seat, value, nameof(Seat)); }
        public string Holecards { get => _holecards; set => SetProperty(ref _holecards, value, nameof(Holecards)); }
    }
}