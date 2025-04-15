using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour of Score Table
/// </summary>
public class ScoreTable : MonoBehaviour
{
    private Transform scoreBodyContainer;
    private Transform scoreEntry;
    
    public GameObject scoreEntryPrefab;
    
    private List<GameObject> scoreEntryList = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create Entries
        var scoreTableContainer = transform.Find("ScoreTableContainer");
        scoreBodyContainer = scoreTableContainer.Find("ScoreTableBody");

        var countPlayers = Players.ActivePlayers.Count;
        var scaler = 1 / countPlayers;
        var pivotSteps = countPlayers <= 1 ? 1 : 1 / (countPlayers - 1);
        
        for (var i = 0; i < Players.ActivePlayers.Count; i++)
        {
            Debug.Log($"Visualize Player-Score: {Players.ActivePlayers[i].PlayerName}");
            var entry = Instantiate(scoreEntryPrefab, scoreBodyContainer);
            var rect = entry.GetComponent<RectTransform>();
            rect.localScale = new Vector2(1, scaler);
            rect.pivot = new Vector2(1, 1 - pivotSteps * i);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            
            scoreEntryList.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Updates all scores
        for (var i = 0; i < Players.ActivePlayers.Count; i++)
        {
            var player = Players.ActivePlayers[i];
            var entry = scoreEntryList[i];
            var container = entry.transform.Find("ScoreEntryBackground");
            
            var nameContainer = container.Find("ScoreEntryNameContainer").Find("ScoreEntryNameText");
            var scoreContainer = container.Find("ScoreEntryScoreContainer").Find("ScoreEntryScoreText");
            
            nameContainer.GetComponent<TextMeshProUGUI>().text = player.PlayerName;
            scoreContainer.GetComponent<TextMeshProUGUI>().text = player.PlayerScore.ToString();
        }
    }
}
