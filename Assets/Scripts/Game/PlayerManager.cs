using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.TurnCommit;
using Game.BotBehaviour;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// Manages player-related operations and interactions in the game.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        
        /// <summary>
        /// ref. to active lobby players 
        /// </summary>
        private List<Player> Players => LobbyStorage.Instance.ActivePlayers;
        
        /// <summary>
        /// ref. to the current player
        /// </summary>
        public Player CurrentPlayer => LobbyStorage.Instance.CurrentPlayer;
    
        public GameObject deckManager;
        public int CountCardsPlayed { get; private set; }
        public GameObject JokerIdentitySelectionPrefab;
    
        private int CurrentPlayerIndex { get; set; }
        private Deck _deck;
        private Button _endTurnButton;
        private ScoreTable _scoreTable;
        private bool _firstTurn = true;
        private Chair[] _chairs;
        private EasyBotBehaviour _easyBotBehaviour;
        private Bar _bar;

        private void Awake()
        {
            // Assign TurnCommit Handler this Manager for operating after turns of other players
            TurnCommitHandler.Manager = this;
            
            _endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
            _endTurnButton.interactable = false;
            _scoreTable = GameObject.Find("ScoreTable").GetComponent<ScoreTable>();
            _chairs = GameObject.Find("Chairs").transform.GetComponentsInChildren<Chair>();
            _bar = GameObject.Find("Bar").GetComponent<Bar>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _easyBotBehaviour = EasyBotBehaviour.GetInstance();
            _deck = deckManager.GetComponent<Deck>();
            UpdatePlayerGameBars();
            DrawInitialCards();
            CountCardsPlayed = 0;
            CurrentPlayerIndex = 0;
            LobbyStorage.Instance.CurrentPlayer = Players[CurrentPlayerIndex];
            CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            CurrentPlayer.PlayerGameBar.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 3;

            _scoreTable.UpdateScores(Players);
        
            foreach (var player in Players)
            {
                player.SetPlayerManager(this);
            }
        
            if (CurrentPlayer.IsBot)
            {
                StartCoroutine(WaitForBotPlay(4));
                StartCoroutine(WaitForUpdate(5));
            }        
            Instance = this;
        }

        /// <summary>
        /// Gives the players the initial 5 start cards
        /// </summary>
        private void DrawInitialCards()
        {
            foreach (var player in Players)
            {
                FillPlayerHand(player);
            }
        }
    
        /// <summary>
        /// Updates the names of the player gamebars to the order of the players
        /// </summary>
        private void UpdatePlayerGameBars()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].PlayerGameBar = GameObject.Find("PlayerGameBarPlayer" + (i + 1)).transform.GetChild(0).gameObject;
                Players[i].PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().text = Players[i].PlayerName;
            }
        }

        /// <summary>
        /// Commit changes and send them to other players. Updates all players as well
        /// </summary>
        public void Commit()
        {
            UpdatePlayer();
            
            // Build Message for transferring turn-updates information
            var message = TurnHistory.PullMessage(LobbyStorage.Instance.ActivePlayers.Single(x => x.PlayerId == LobbyStorage.Instance.ClientPlayerId));
            NetworkRouter.SendToServer(message, MessageType.TurnCommit);
        }
    
        /// <summary>
        /// Updates the current Player and changes the color of the current player to red. Is invoked by END TURN and when the max number of cards has been placed
        /// </summary>
        public void UpdatePlayer()
        {
            Debug.Log($"Current Player: {CurrentPlayer.PlayerName} [{CurrentPlayer.ClientId}]");
            if (CountCardsPlayed != 0)
            {
                // Check if the move is valid by game-rule and current player is client player
                if (CurrentPlayer.IsMoveValid(_firstTurn)) 
                {
                    if (!CheckChairsHaveFreeSpots())
                    {
                        GameEnded();
                    }
                
                    CurrentPlayer.CountPlayerScore();
                    if (!CurrentPlayer.IsPlayerEliminated()) // Only fill the players cards if the player is not eliminated
                    {
                        FillPlayerHand(CurrentPlayer);
                    }

                    CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    CurrentPlayer.PlayerGameBar.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;

                    for (int i = 0; i < Players.Count; i++)
                    {
                        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
                        if (!Players[CurrentPlayerIndex].IsPlayerEliminated()) // Check if the player is eliminated and only continue if not
                        {
                            break;
                        }

                        if (i == Players.Count - 1) // If the 4th player is reached and also eliminated the game ends because no active players remain
                        {
                            GameEnded();
                        }
                    }
                    
                    var nextPlayer = Players[CurrentPlayerIndex];
                    LobbyStorage.Instance.CurrentPlayer = nextPlayer;
                    
                    CurrentPlayer.PlayerGameBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    CurrentPlayer.PlayerGameBar.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 3;
                    CountCardsPlayed = 0;
                
                    _endTurnButton.interactable = false;
                    _scoreTable.UpdateScores(Players);
                    _firstTurn = false;
                    
                    if (CurrentPlayer.IsBot)
                    {
                        StartCoroutine(WaitForBotPlay());
                        StartCoroutine(WaitForUpdate());
                    }
                }
                else
                {
                    _endTurnButton.interactable = false;
                    CountCardsPlayed = 0;
                    CurrentPlayer.ResetCards();
                }
            }
        }
    
        public void IncrementCountCardsPlayed(int increment)
        {
            CountCardsPlayed += increment;
            _endTurnButton.interactable = true;
        }
    
        private void FillPlayerHand(Player player)
        {
            int toCreate = player.MaxCardCount - player.PlayerHand.Count;
            for (int i = 0; i < toCreate; i++)
            {
                player.PlayerHand.Add(_deck.DrawCard(player));
            }
        }
    
        public void SetEndTurnButtonInteractable(bool interactable)
        {
            _endTurnButton.interactable = interactable;
        }
    
        public void GameEnded()
        {
            Debug.Log("Game ended");
        
            SubtractPlayerCardPoints();
            var x = GameObject.Find("EndScreenManager");
            EndScreenHelper endScreenHelper = GameObject.Find("EndScreenManager").GetComponent<EndScreenHelper>();
            endScreenHelper.PlayerScores.Clear(); // Clear any remaining scores from prior rounds
            foreach (Player player in Players)
            {
                endScreenHelper.PlayerScores.Add(new PlayerScore(player.PlayerName, player.PlayerScore));
            }
        
            SceneManager.LoadScene("EndScreen");
        }

        private void SubtractPlayerCardPoints()
        {
            foreach (Player player in Players)
            {
                player.SubtractPointsForRemainingCards();
            }
        }

        /// <summary>
        /// Checks if a there are empty chairs left
        /// </summary>
        /// <returns>True if empty chairs remain, otherwise false</returns>
        private bool CheckChairsHaveFreeSpots()
        {
            foreach (var chair in _chairs)
            {
                if (chair.PlacedCard == null)
                {
                    return true;
                }
            }
            return false;
        }
    
        IEnumerator WaitForBotPlay(int seconds = 2)
        {
            yield return new WaitForSeconds(seconds);
            _easyBotBehaviour.Play(CurrentPlayer);
        }
    
        IEnumerator WaitForUpdate(int seconds = 5)
        {
            yield return new WaitForSeconds(seconds);
            UpdatePlayer();
        }

        public void PlayCard(int cardId, int chairId, int barStoolId, string playerId)
        {
            var card = GetCardFromId(cardId);
            if (card == null)
                Debug.LogError($"Cannot find Card: {cardId}");
            
            var player = LobbyStorage.Instance.ActivePlayers.FirstOrDefault(x => x.PlayerId == playerId);
            if (player == null)
                Debug.LogError($"Cannot find player with ID: {playerId}");
            
            Debug.Log($"Play-Card: [CardID: {cardId}], [ChairID: {chairId}], [BarStoolID: {barStoolId}], " +
                      $"[PlayerId: {playerId}], [Card-Gender: {card.cardData.gender}], [Card-Nationality: {card.cardData.nationality}]," +
                      $"[PlayerName: {player.PlayerName}], [PlayerScore: {player.PlayerScore}]");
            
            card.Player = player;
            
            if (chairId >= 0)
            {
                var chair = GetChairFromId(chairId);
                chair.PlaceCard(card);
                EasyBotBehaviour.PlacePlayerCard(card.gameObject, chair.gameObject);
            }
            else if (barStoolId >= 0)
            {
                var barStool = GetBarStoolFromId(barStoolId);
                barStool.PlaceCard(card);
                EasyBotBehaviour.PlacePlayerCard(card.gameObject, barStool.gameObject);
            }
            else
            {
                Debug.Log($"The call of PlayCard was invalid with ChairId {chairId} and BarStoolId {barStoolId} for CardId {cardId}");
            }
        }

        /// <summary>
        /// Iterates through all players and finds the requested card
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Card GetCardFromId(int id)
        {
            foreach (var player in Players)
                foreach (var card in player.PlayerHand)
                    if (card.cardData.cardID == id)
                        return card;
            return null;
        }

        public Chair GetChairFromId(int id)
        {
            foreach (var chair in _chairs)
            {
                if (chair.ChairID == id)
                {
                    return chair;
                }
            }
            return null;
        }

        public BarStool GetBarStoolFromId(int id)
        {
            return _bar.BarStools[id];
        }
    }
}
