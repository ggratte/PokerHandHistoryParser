using System;
using System.Runtime.Serialization;

namespace HandHistories.Objects.Cards
{
    [DataContract]
    public class BoardCards : CardGroup
    {
        public Street Street
        {
            get
            {
                switch (Cards.Count)
                {
                    case 0:
                        return Street.Preflop;
                    case 3:
                        return Street.Flop;
                    case 4:
                        return Street.Turn;
                    case 5:
                        return Street.River;
                    default:
                        throw new ArgumentException("Unknown number of board cards " + Cards.Count);
                }
            }
        }

        private BoardCards(params Card[] cards)
            : base(cards)
        {

        }

        public static BoardCards ForPreflop()
        {
            return new BoardCards();
        }

        public static BoardCards ForFlop(Card card1, Card card2, Card card3)
        {
            return new BoardCards(card1, card2, card3);
        }

        public static BoardCards ForTurn(Card card1, Card card2, Card card3, Card card4)
        {
            return new BoardCards(card1, card2, card3, card4);
        }

        public static BoardCards ForRiver(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            return new BoardCards(card1, card2, card3, card4, card5);
        }

        public static BoardCards FromCards(string cards)
        {
            return FromCards(Parse(cards));
        }

        public static BoardCards FromCards(Card[] cards)
        {
            return new BoardCards(cards);
        }

        public BoardCards GetBoardOnStreet(Street streetAllIn)
        {
            switch (streetAllIn)
            {
                case Street.Preflop:
                    return BoardCards.ForPreflop();
                case Street.Flop:
                    return BoardCards.ForFlop(this[0], this[1], this[2]);
                case Street.Turn:
                    return BoardCards.ForTurn(this[0], this[1], this[2], this[3]);
                case Street.River:
                case Street.Showdown:
                    return BoardCards.ForRiver(this[0], this[1], this[2], this[3], this[4]);
                default:
                    throw new ArgumentException("Can't get board in for null street");
            }
        }

        public Card GetTurnCard()
        {
            switch (this.Count)
            {
                case 0:
                case 3:
                    throw new Exception("Turn card does not exist on this board!");
                case 4:
                case 5:
                    return this[3];
                default:
                    throw new ArgumentException("Unknown number of board cards " + Cards.Count);

            }
        }

        public Card GetRiverCard()
        {
            switch (this.Count)
            {
                case 0:
                case 3:
                case 4:
                    throw new Exception("River card does not exist on this board!");
                case 5:
                    return this[4];
                default:
                    throw new ArgumentException("Unknown number of board cards " + Cards.Count);
            }
        }

        /**
         * When comparing two boards, order matters. The same 5 cards, if come to the board with different order, should be considerred as different
         * boards. Therefore, when comparing two boards, we should firstly compare Flop as a group, and then compare Turn and River respecticely.
         */
        public override bool Equals(object obj)
        {
            bool stringEquality = obj.ToString().Equals(ToString());
            if (stringEquality) return true;

            BoardCards boardGroup = obj as BoardCards;

            if (boardGroup == null) return false;

            if (this.Count != boardGroup.Count) return false;
            if (this.Count == 0) return true;

            // if this is just comparing flops, we can use the base compartor.
            if (this.Count == 3)
            {
                return base.Equals(boardGroup);
            }

            // because the order matters, we need to compare flop first, and then compare turn and river.
            if (this.Count == 4)
            {
                return this.GetBoardOnStreet(Street.Flop).Equals(boardGroup.GetBoardOnStreet(Street.Flop)) && this.GetTurnCard().Equals(boardGroup.GetTurnCard());
            }

            if (this.Count == 5)
            {
                return this.GetBoardOnStreet(Street.Turn).Equals(boardGroup.GetBoardOnStreet(Street.Turn)) && this.GetRiverCard().Equals(boardGroup.GetRiverCard());
            }

            throw new ArgumentException("Unknown number of board cards " + Cards.Count);
        }

        public static bool operator ==(BoardCards b1, BoardCards b2)
        {
            if (ReferenceEquals(b1, b2))
            {
                return true;
            }
            else if (ReferenceEquals(b1, null) || ReferenceEquals(b2, null))
            {
                return false;
            }

            return b1.Equals(b2);
        }

        public static bool operator !=(BoardCards b1, BoardCards b2) { return !(b1 == b2); }

        public override int GetHashCode()
        {
            return (Cards != null ? Cards.GetHashCode() : 0);
        }
    }
}

