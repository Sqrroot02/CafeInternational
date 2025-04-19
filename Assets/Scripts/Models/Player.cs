using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Player
    {
        private GameObject playerGameBar; // The Gamebar itself is of no use so we always take the inner wrapper layer
        public int MaxCardCount = 5;
        public List<Card> PlayerHand = new();
        public HashSet<Table> Tables = new(); // A collection of the tables that need to be counted for the points

        
        public Player(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }
    
        public string PlayerName { get; set; }
        public int PlayerScore { get; private set; }
        
        public GameObject PlayerGameBar { get; set; }

        /// <summary>
        /// Counts the tables the player has placed cards at and sums up the points.
        /// Clears the tables collection afterward.
        /// </summary>
        public void CountPlayerScore()
        {
            foreach (var table in Tables)
            {
                UpdatePlayerScore(table.GetTablePoints());
            }
            Tables.Clear();
        }

        public void UpdatePlayerScore(int points)
        {
            PlayerScore += points;
            if (points == 8)
            {
                MaxCardCount--;
            }
            // TODO Was passiert bei 0 Karten? Hat der Spieler gewonnen?
            Debug.Log(PlayerName + " " + PlayerScore);
        }

        public GameObject FindNextCardSlot()
        {
            for (int i = 0; i < PlayerGameBar.transform.GetChild(1).transform.childCount; i++)
            {
                if (PlayerGameBar.transform.GetChild(1).transform.GetChild(i).childCount == 0)
                {
                    return PlayerGameBar.transform.GetChild(1).transform.GetChild(i).gameObject;
                }
            }
            return null;
        }
    }
}

