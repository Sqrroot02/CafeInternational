using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour of Score Table
/// </summary>
public class ScoreTable : MonoBehaviour
{
    public List<GameObject> ScoreEntries;

    public void UpdateScores(List<Player> players)
    {
        List<Player> sortedByScore = players
            .OrderByDescending(player => player.PlayerScore)
            .ToList();
        for (int i = 0; i < ScoreEntries.Count; i++)
        {
            UpdateScoreEntry(ScoreEntries[i], sortedByScore[i]);    
        }
    }

    private void UpdateScoreEntry(GameObject scoreEntry, Player player)
    {
        scoreEntry.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = player.PlayerName;
        scoreEntry.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = player.PlayerScore.ToString();;
    }
}
