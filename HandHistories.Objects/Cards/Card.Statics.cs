using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Objects.Cards
{
    partial struct Card
    {
        #region Lookups
        public static readonly Card[] AllCards = GetAllCardStrings().Select(p => Card.Parse(p)).ToArray();

        static readonly string[] AllCardStrings = GetAllCardStrings();

        private static string[] GetAllCardStrings()
        {
            List<string> cards = new List<string>(52);
            foreach (var c in (CardEnum[])Enum.GetValues(typeof(CardEnum)))
            {
                var cardName = c.ToString();
                cards.Add(string.Concat(cardName[5], char.ToLower(cardName[6])));
            }
            return cards.ToArray();
        }
        #endregion

        public static Card GetCardFromIntValue(int value)
        {
            //Sanity check
            if (value < 0 || 51 < value)
            {
                throw new ArgumentOutOfRangeException(string.Format("Value: {0} is not valid supply a value between 0 and 51", value));
            }

            return new Card((CardEnum)value);
        }

        /// <summary>
        /// Parses a card with format <RANK><SUIT>. Must be 2 characters long
        /// Examples: "2s" or "Ac"
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Card Parse(string card)
        {
            if (card.Length != 2)
            {
                throw new ArgumentException("Cards must be length 2. Format Rs where R is rank and s is suit.");
            }

            var rank = ParseRank(card[0]);
            var suit = ParseSuit(card[1]);

            return new Card(rank.Value, suit.Value);
        }

        internal static CardEnum GetCardEnum(CardValueEnum rank, SuitEnum suit)
        {
            return (CardEnum)((int)suit * 13 + (int)rank);
        }

        internal static SuitEnum? ParseSuit(char suit)
        {
            switch (suit)
            {
                case 'C':
                case 'c': return SuitEnum.Clubs;
                case 'D':
                case 'd': return SuitEnum.Diamonds;
                case 'H':
                case 'h': return SuitEnum.Hearts;
                case 'S':
                case 's': return SuitEnum.Spades;
                default: return null;
            }
        }

        internal static CardValueEnum? ParseRank(char rank)
        {
            switch (rank)
            {
                case '2': return CardValueEnum._2;
                case '3': return CardValueEnum._3;
                case '4': return CardValueEnum._4;
                case '5': return CardValueEnum._5;
                case '6': return CardValueEnum._6;
                case '7': return CardValueEnum._7;
                case '8': return CardValueEnum._8;
                case '9': return CardValueEnum._9;
                case 't':
                case 'T': return CardValueEnum._T;
                case 'j':
                case 'J': return CardValueEnum._J;
                case 'q':
                case 'Q': return CardValueEnum._Q;
                case 'k':
                case 'K': return CardValueEnum._K;
                case 'a':
                case 'A': return CardValueEnum._A;
                default: return null;
            }
        }
    }
}
