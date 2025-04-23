using System.Collections.Generic;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    List<Player> players = new();
    public Player CurrentPlayer { get; set; }
    public int CurrentPlayerIndex { get; set; }

    public GameObject deckManager;

    private Deck _deck;
    
    public int CountCardsPlayed { get; private set; }

    private Button EndTurnButton;
    
    private bool _firstTurn = true;

    private void Awake()
    {
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        EndTurnButton.interactable = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _deck = deckManager.GetComponent<Deck>();
        ShufflePlayers();
        UpdatePlayerGameBars();
        DrawInitialCards();
        CountCardsPlayed = 0;
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
    /// Gives the players the initial 5 start cards
    /// </summary>
    private void DrawInitialCards()
    {
        foreach (var player in players)
        {
            FillPlayerHand(player);
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

    public void IncrementCountCardsPlayed(int increment)
    {
        CountCardsPlayed += increment;
        EndTurnButton.interactable = true;
    }
    
    /// <summary>
    /// Updates the current Player and changes the color of the current player to red. Is invoked by END TURN and when the max number of cards has been placed
    /// </summary>
    public void UpdatePlayer()
    {
        if (CountCardsPlayed != 0)
        {
            if (CurrentPlayer.IsMoveValid(_firstTurn))
            {
                CurrentPlayer.CountPlayerScore();
                FillPlayerHand(CurrentPlayer);

                CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % players.Count;
                CurrentPlayer = players[CurrentPlayerIndex];
                CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                CountCardsPlayed = 0;
                EndTurnButton.interactable = false;
                _firstTurn = false;
            }
            else
            {
                EndTurnButton.interactable = false;
                CountCardsPlayed = 0;
                CurrentPlayer.ResetCards();
            }
        }
    }

    private void FillPlayerHand(Player player)
    {
        int toCreate = player.MaxCardCount - player.PlayerHand.Count;
        for (int i = 0; i < toCreate; i++)
        {
            player.PlayerHand.Add(_deck.DrawCard(player));
        }
    }
}
