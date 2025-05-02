using System.Collections.Generic;
using UnityEngine;

public class PlayerScore
{
    public string PlayerName { get; set; }
    public int Score { get; set; }

    public PlayerScore(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

/// <summary>
/// Saves the player scores for the scene swap at the end of the game
/// </summary>
public class EndScreenHelper : MonoBehaviour
{
    public static EndScreenHelper Instance;
    
    public List<PlayerScore> PlayerScores = new();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Survive Scene Change
        }
        else
        {
            Destroy(gameObject); // Singleton
        }
    }
}
