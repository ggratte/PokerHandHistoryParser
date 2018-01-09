using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Parser.Serializer.JSON.JSONObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Parser.Serializer.JSON
{
    public partial class JSONHandSerializer
    {
        public string Serialize(HandHistory hand)
        {
            JSON_hand jhand = new JSON_hand();
            jhand.gameinfo = GetGameInfo(hand);

            //JObject JSON = new JObject();
            //AddGameInfo(JSON, hand);
            //AddPlayerList(JSON, hand);
            //AddActions(JSON, hand);
            //AddBoard(JSON, hand.ComumnityCards);
            return JsonConvert.SerializeObject(jhand);// JSON.ToString(Formatting.Indented);
        }

        private JSON_gameinfo GetGameInfo(HandHistory hand)
        {
            return new JSON_gameinfo()
            {
                bigBlind = hand.GameDescription.Limit.BigBlind,
                smallBlind = hand.GameDescription.Limit.SmallBlind,
            };
        }

        private void AddBoard(JObject JSON, BoardCards communityCards)
        {
            JSON.Add("board", communityCards.ToString());
        }

        private void AddActions(JObject JSON, HandHistory hand)
        {
            var plist = hand.Players;
            var jactions = new JObject();
            foreach (var a in hand.HandActions)
            {
                var jaction = new JObject();
                jaction.Add("seat", plist[a.PlayerName].SeatNumber);
                jaction.Add("action", a.HandActionType.ToString());
                jaction.Add("amount", a.Amount);
                if (a.IsAllIn)
                {
                    jaction.Add("allin", a.IsAllIn);
                }
                jactions.Add(a.ActionNumber.ToString(), jaction);
            }

            JSON.Add("actions", jactions);
        }

        private void AddPlayerList(JObject JSON, HandHistory hand)
        {
            var plist = new JObject();
            foreach (var p in hand.Players)
            {
                var pitem = new JObject();
                pitem.Add("name", p.PlayerName);
                pitem.Add("balance", p.StartingStack);
                if (p.IsSittingOut)
                {
                    pitem.Add("sitout", p.IsSittingOut);
                }
                if (p.hasHoleCards)
                {
                    pitem.Add("cards", p.HoleCards.ToString());
                }

                plist.Add(p.SeatNumber.ToString(), pitem);
            }
            JSON.Add("players", plist);
        }

        private void AddGameInfo(JObject JSON, HandHistory hand)
        {
            var game = hand.GameDescription;
            JSON.Add("site", game.Site.ToString());
            JSON.Add("tablename", hand.TableName);
            JSON.Add("gameId", hand.HandId);
            JSON.Add("currency", GetJSONCurrency(game.Limit.Currency));
            JSON.Add("game", game.GameType.Game.ToString());
            JSON.Add("date", GetUnixTime(hand.DateOfHandUtc));
            JSON.Add("dealer", hand.DealerButtonPosition);
            JSON.Add("maxSeats", game.SeatType.MaxPlayers);
            JSON.Add("bigBlind", game.Limit.BigBlind);
            JSON.Add("smallBlind", game.Limit.SmallBlind);
            JSON.Add("ante", game.Limit.Ante);
        }

        static long GetUnixTime(DateTime time)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time.ToUniversalTime() - dtDateTime).TotalSeconds;
        }

        private JToken GetJSONCurrency(Currency currency)
        {
            switch (currency)
            {
                case Currency.USD:
                    return "USD";
                case Currency.GBP:
                    return "GBP";
                case Currency.EURO:
                    return "EUR";
                case Currency.CHIPS:
                    return "CHIPS";
                case Currency.SEK:
                    return "SEK";
                default:
                    throw new NotImplementedException("Writer Currency: " + currency);
            }
        }
    }
}
