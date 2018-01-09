using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Parser.Serializer.JSON.JSONObjects
{
    class JSON_gameinfo
    {
        public string site;
        public string tablename;
        public long gameId;
        public string currency;
        public string limit;
        public string game;
        public int dealer;
        public int maxSeats;
        public decimal smallBlind;
        public decimal bigBlind;
        public long date;
    }
}
