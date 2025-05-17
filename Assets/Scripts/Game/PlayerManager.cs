using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.TurnCommit;
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
        Stack<Player> playersStack = new Stack<Player>(PlayersGameUtil.ActivePlayers);
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
                // Send Commit Message to other players
                SendEndTurnMessage();
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

    public static void HandleEndTurnMessage()
    {
        
    }

    /// <summary>
    /// Sends a message to the server to commit the current player's turn, including all actions performed,
    /// such as placing cards on chairs and the bar stool.
    /// </summary>
    public void SendEndTurnMessage()
    {
        var changes = new List<TurnCommitChangeMessage>();
        
        // Build Chair Change Messages
        foreach (var chair in CurrentPlayer.Chairs)
        {
            var chairChangeMessage = new TurnCommitChangeMessage()
            {
                Action = TurnCommitAction.PlaceCardOnChair,
                CardContext = chair.PlacedCard,
                ChairContext = chair,
            };
            changes.Add(chairChangeMessage);
        }

        // Build Bar Stool Change Message
        if (CurrentPlayer.BarStool != null)
        {
            var barStoolMessage = new TurnCommitChangeMessage
            {
                Action = TurnCommitAction.PlaceCardOnBar,
                BarStoolContext = CurrentPlayer.BarStool,
                CardContext = CurrentPlayer.BarStool.PlacedCard,
            };
            changes.Add(barStoolMessage);
        }

        var message = new TurnCommitMessage()
        {
            Changes = changes.ToArray(),
            Player = CurrentPlayer,
        };
        NetworkRouter.SendToServer(message, MessageType.TurnCommit);
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
