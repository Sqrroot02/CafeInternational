using System.Collections.Generic;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour of Score Table
/// </summary>
public class ScoreTable : MonoBehaviour
{
    private VerticalLayoutGroup scoreBodyContainer;
    private Transform scoreEntry;
    
    public GameObject scoreEntryPrefab;
    
    private List<GameObject> scoreEntryList = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create Entries
        var scoreTableContainer = transform.Find("ScoreTableContainer");
        scoreBodyContainer = scoreTableContainer.GetComponent<VerticalLayoutGroup>();
        for (var i = 0; i < PlayersGameUtil.ActivePlayers.Count; i++)
        {
            Debug.Log($"Visualize Player-Score: {PlayersGameUtil.ActivePlayers[i].PlayerName}");
            var entry = Instantiate(scoreEntryPrefab, scoreBodyContainer.transform);
            scoreEntryList.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Updates all scores
        for (var i = 0; i < PlayersGameUtil.ActivePlayers.Count; i++)
        {
            var player = PlayersGameUtil.ActivePlayers[i];
            var entry = scoreEntryList[i];
            var container = entry.transform.Find("ScoreEntryBackground");
            
            var nameContainer = container.Find("ScoreEntryNameContainer").Find("ScoreEntryNameText");
            var scoreContainer = container.Find("ScoreEntryScoreContainer").Find("ScoreEntryScoreText");
            
            nameContainer.GetComponent<TextMeshProUGUI>().text = player.PlayerName;
            scoreContainer.GetComponent<TextMeshProUGUI>().text = player.PlayerScore.ToString();
        }
    }
}
