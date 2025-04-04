namespace Assets.Scripts.Models
{
    public class Player
    {
        public Player(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }
    
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
    }
}

