using Assets.Scripting.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour of Score Table
/// </summary>
public class ScoreTable : MonoBehaviour
{
    private Transform scoreBodyContainer;
    private Transform scoreEntry;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreBodyContainer = transform.Find("ScoreTableBody");
        scoreEntry = scoreBodyContainer.Find("ScoreEntry");
        foreach (var player in Players.ActivePlayers)
        {
            var entry = Instantiate(scoreEntry, scoreBodyContainer);
            entry.Find("ScoreEntryNameText").GetComponent<Text>().text = player.PlayerName;
            entry.Find("ScoreEntryScoreText").GetComponent<Text>().text = player.PlayerScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreBodyContainer = transform.Find("ScoreTableBody");
        scoreEntry = scoreBodyContainer.Find("ScoreEntry");
        foreach (var player in Players.ActivePlayers)
        {
            var entry = Instantiate(scoreEntry, scoreBodyContainer);
            entry.Find("ScoreEntryNameText").GetComponent<Text>().text = player.PlayerName;
            entry.Find("ScoreEntryScoreText").GetComponent<Text>().text = player.PlayerScore.ToString();
        }
    }

    private void Awake()
    {
        scoreBodyContainer = transform.Find("ScoreTableBody");
        scoreEntry = scoreBodyContainer.Find("ScoreEntry");
        foreach (var player in Players.ActivePlayers)
        {
            var entry = Instantiate(scoreEntry, scoreBodyContainer);
            entry.Find("ScoreEntryNameText").GetComponent<Text>().text = player.PlayerName;
            entry.Find("ScoreEntryScoreText").GetComponent<Text>().text = player.PlayerScore.ToString();
        }
    }
}
