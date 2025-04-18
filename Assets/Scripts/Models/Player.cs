using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Player
    {
        private GameObject playerGameBar; // The Gamebar itself is of no use so we always take the inner wrapper layer
        public Player(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }
    
        public string PlayerName { get; set; }
        public int PlayerScore { get; private set; }
        
        public GameObject PlayerGameBar { get; set; }

        public void UpdatePlayerScore(int points)
        {
            PlayerScore += points;
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

