using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace HandHistories.Objects.Cards
{
    /// <summary>
    /// Represents a card.
    /// </summary>
    [DataContract]
    public partial struct Card
    {
        [DataMember]
        public readonly CardEnum CardEnum;

        #region Properties
        public int RankNumericValue
        {
            get
            {
                return (int)Rank + 2;
            }
        }
        public int SuitNumericValue
        {
            get
            {
                return (int)Suit;
            }
        }

        public SuitEnum Suit => (SuitEnum)((int)CardEnum / 13);

        public CardValueEnum Rank => (CardValueEnum)((int)CardEnum % 13);

        /// <summary>
        /// 2c = 0, 3c = 1, ..., Ac = 12, ..., As = 51.
        /// </summary>
        public int CardIntValue => (int)CardEnum;
        #endregion

        #region Constructors
        public Card(char rank, char suit)
        {
            var r = ParseRank(rank);
            var s = ParseSuit(suit);

            if (r == null || s == null)
            {
                throw new ArgumentException("Hand is not correctly formatted. Value: " + rank + " Suit: " + suit);
            }

            CardEnum = GetCardEnum(r.Value, s.Value);
        }

        /// <summary>
        /// </summary>
        /// <param name="rank">Rank should be 2-9,T,J,Q,K,A.</param>
        /// <param name="suit">Suit should be c,d,h,s.</param>
        public Card(string rank, string suit) : this(rank[0], suit[0])
        {
        }

        private Card(CardValueEnum rank, SuitEnum suit)
        {
            CardEnum = GetCardEnum(rank, suit);
        }

        private Card(CardEnum cardCode)
        {
            CardEnum = cardCode;
        }
        #endregion

        #region Operators
        public static bool operator ==(Card c1, Card c2)
        {
            return c1.CardEnum == c2.CardEnum;
        }

        public static bool operator !=(Card c1, Card c2)
        {
            return c1.CardEnum != c2.CardEnum;
        }
        #endregion

        #region Functions
        public override string ToString()
        {
            return AllCardStrings[(int)CardEnum];
        }

        public override bool Equals(object obj)
        {
            if (obj is Card)
            {
                Card other = (Card)obj;
                return Rank == other.Rank && Suit == other.Suit;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CardEnum.GetHashCode();
        } 
        #endregion
    }
}
