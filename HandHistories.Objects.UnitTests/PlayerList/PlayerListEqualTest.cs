using HandHistories.Objects.Players;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandHistories.Objects.UnitTests.PlayerList
{
    [TestFixture]
    public class CardEqualityTests
    {
        [Test]
        public void Player_TestEquality_ReturnTrue()
        {
            Player player = new Player("name_1", 101.5m, 1);
            Assert.IsTrue(player.Equals(player));
        }

        [Test]
        public void Player_TestEquality_ReturnFalse()
        { 
            Player player1 = new Player("name_1", 101.5m, 1);
            Player player2 = new Player("name_1", 102.5m, 1);
            Player player3 = new Player("name_2", 101.5m, 1);
            Player player4 = new Player("name_1", 101.5m, 2);
            
            Assert.IsFalse(player1.Equals(player2));
            Assert.IsFalse(player1.Equals(player3));
            Assert.IsFalse(player1.Equals(player4));
            Assert.IsFalse(player2.Equals(player3));
            Assert.IsFalse(player2.Equals(player4));
            Assert.IsFalse(player3.Equals(player4));
        }

        [Test]
        public void PlayerList_TestEquality_ReturnTrue()
        {
            Player player1 = new Player("name_1", 101.5m, 0);
            Player player2 = new Player("name_1", 102.5m, 1);
            Player player3 = new Player("name_2", 101.5m, 2);
            Player player4 = new Player("name_1", 101.5m, 3);
            
            Players.PlayerList playerList1 = new Players.PlayerList()
            {
                player1, player2, player3, player4
            };
            Players.PlayerList playerList2 = new Players.PlayerList()
            {
                player4, player2, player3, player1
            };
            
            Assert.IsTrue(playerList1.Equals(playerList1));
            Assert.IsTrue(playerList1.Equals(playerList2));
        }

        [Test]
        public void PlayerList_TestEquality_ReturnFalse()
        {
            Player player1 = new Player("name_1", 101.5m, 0);
            Player player2 = new Player("name_1", 102.5m, 1);
            Player player3 = new Player("name_2", 101.5m, 2);
            Player player4 = new Player("name_1", 101.5m, 3);
            
            Players.PlayerList playerList1 = new Players.PlayerList()
            {
                player1, player2, player3, player4
            };
            Players.PlayerList playerList2 = new Players.PlayerList()
            {
                player1, player2, player4
            };

            Players.PlayerList playerList3 = new Players.PlayerList()
            {
                player1, player3, player4
            };
            
            Assert.IsFalse(playerList1.Equals(playerList2));
            Assert.IsFalse(playerList2.Equals(playerList3));
            Assert.IsFalse(playerList1.Equals(playerList3));
        }
    }
}
