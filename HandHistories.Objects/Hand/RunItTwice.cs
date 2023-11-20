using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Objects.Hand
{
    public sealed class RunItTwice
    {
        /// <summary>
        /// The second board
        /// </summary>
        public BoardCards Board = BoardCards.FromCards(String.Empty);

        /// <summary>
        /// All actions that occur during the second showdown
        /// </summary>
        public List<HandAction> Actions = new List<HandAction>();

        public List<WinningsAction> Winners = new List<WinningsAction>();


        public override bool Equals(object obj)
        {
            RunItTwice runItTwice = obj as RunItTwice;

            if (runItTwice == null) return false;
            return CompareBoard(this.Board, runItTwice.Board) && CompareActions(this.Actions, runItTwice.Actions) 
                && CompareWinners(this.Winners, runItTwice.Winners); 
        }


        public bool CompareBoard(BoardCards b1, BoardCards b2)
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

        public bool CompareActions(List<HandAction> handActions1, List<HandAction> handActions2)
        {
            if (ReferenceEquals(handActions1, handActions2))
            {
                return true;
            }
            else if (ReferenceEquals(handActions1, null) || ReferenceEquals(handActions2, null))
            {
                return false;
            }

            if (handActions1.Count != handActions2.Count) { return false; }

            for (int i = 0; i < handActions1.Count; i++)
            {
                if (!handActions1[i].Equals(handActions2[i]))
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool CompareWinners(List<WinningsAction> w1, List<WinningsAction> w2)
        {
            if (ReferenceEquals(w1,w2))
            {
                return true;
            }
            else if (ReferenceEquals(w1, null) || ReferenceEquals(w2, null))
            {
                return false;
            }

            if (w1.Count != w2.Count) { return false; }

            for (int i = 0; i < w1.Count; i++)
            {

                if (!w1[i].Equals(w2[i]))
                {
                    Console.WriteLine(w1[i].Amount);
                    Console.WriteLine(w2[i].Amount);
                    return false;
                }
            }
            return true;
        }
        
        public static bool operator ==(RunItTwice r1, RunItTwice r2)  
        { 
            if (ReferenceEquals(r1, r2))
            {
                return true;
            }
            else if (ReferenceEquals(r1, null) || ReferenceEquals(r2, null))
            {
                return false;
            }

            return r1.Equals(r2); 
        }

        public static bool operator !=(RunItTwice r1, RunItTwice r2) { return !(r1 == r2); }

        public override int GetHashCode()
        {
            int res = 0x2D2816FE;
            foreach (var item in this.Board)
            {
                res = res * 31 + (item == null ? 0 : item.GetHashCode());
            }

            foreach (var item in this.Winners) 
            { 
                res = res * 31 + (item == null ? 0 : item.GetHashCode());
            }

            foreach (var item in this.Actions) 
            { 
                res = res * 31 + (item == null ? 0 : item.GetHashCode());
            }

            return res;
        }
    }
}
