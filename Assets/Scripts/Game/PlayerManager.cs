using System.Collections.Generic;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    List<Player> players = new();
    public Player CurrentPlayer { get; set; }
    public int CurrentPlayerIndex { get; set; }

    public GameObject deckManager;

    private Deck _deck;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _deck = deckManager.GetComponent<Deck>();
        ShufflePlayers();
        UpdatePlayerGameBars();
        DrawInitialCards();
        CurrentPlayerIndex = 0;
        CurrentPlayer = players[CurrentPlayerIndex];
        CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;

    }

    private void ShufflePlayers()
    {
        Stack<Player> playersStack = new Stack<Player>(Players.ActivePlayers);
        players.Add(playersStack.Pop());

        while (playersStack.Count > 0)
        {
            players.Insert(Random.Range(0, players.Count + 1), playersStack.Pop());
        }
    }

    /// <summary>
    /// Updates the names of the player gamebars to the order of the players
    /// </summary>
    private void UpdatePlayerGameBars()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].PlayerGameBar = GameObject.Find("PlayerGameBarPlayer" + (i + 1)).transform.GetChild(0).gameObject;
            players[i].PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().text = players[i].PlayerName;
        }
    }

    private void DrawInitialCards()
    {
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _deck.DrawCard(players[i]);
            }
        }
    }

    /// <summary>
    /// Updates the current Player and changes the color of the current player to red. Is invoked by END TURN 
    /// </summary>
    public void UpdatePlayer()
    {
        CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % players.Count;
        CurrentPlayer = players[CurrentPlayerIndex];
        CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
    }
}
