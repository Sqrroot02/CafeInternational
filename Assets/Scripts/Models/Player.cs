using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

namespace Assets.Scripts.Models
{
    public class Player
    {
        public int MaxCardCount = 5;
        public List<Card> PlayerHand = new();
        public List<Chair> Chairs = new();
        public BarStool BarStool;
        public bool IsHuman = false;
        public bool LobbyHost = false;
        private bool _playerBlockedByJokerIdentitySelection = false;
        private PlayerManager _playerManager;
        private bool _playerEliminated = false;
        
        public Player(string playerName, int playerScore, bool isHuman, bool lobbyHost)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
            IsHuman = isHuman;
            LobbyHost = lobbyHost;
        }
    
        public string PlayerName { get; set; }
        public int PlayerScore { get; private set; }
        public bool IsBot { get; private set; }
        public GameObject PlayerGameBar { get; set; }

        public void SetPlayerManager(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public bool IsPlayerEliminated()
        {
            return _playerEliminated;
        }

        public void SetPlayerBlockedByJokerIdentitySelection(bool state)
        {
            _playerBlockedByJokerIdentitySelection = state;
            _playerManager.SetEndTurnButtonInteractable(!_playerBlockedByJokerIdentitySelection);
        }

        public bool GetPlayerBlockedByJokerIdentitySelection()
        {
            return _playerBlockedByJokerIdentitySelection;
        }

        /// <summary>
        /// Checks if the combination of cards the player played match the rules of the Game.
        /// Checked rules are:
        /// 1. A card has to be placed with at least one other card on one of the tables -> Does not apply in the first turn for a single card
        /// </summary>
        /// <returns>True if the move is valid.</returns>
        public bool IsMoveValid(bool firstTurn)
        {
            // 1. Rule
            if (!(firstTurn && Chairs.Count == 1))
            {
                foreach (var chair in Chairs)
                {
                    if (chair.OnlyCardAtTheTable())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void ResetCards()
        {
            foreach (var chair in Chairs)
            {
                chair.PlacedCard.ResetCardPosition();
                chair.RemovePlacedCard();
            }

            Chairs = new List<Chair>();
        }

        /// <summary>
        /// Counts the tables the player has placed cards at and sums up the points.
        /// Clears the chairs collection afterward.
        /// Checks if a card is placed on a barstool and clears Barstool afterward.
        /// </summary>
        public void CountPlayerScore()
        {
            //Chairs
            Dictionary<Table, int> tables = new(); // Find all tables that have cards placed at them and the amount of cards at the table
            for (int i = 0; i < Chairs.Count; i++)
            {
                PlayerHand.Remove(Chairs[i].PlacedCard); // Remove the placed cards from the list of cards in the playerhand for the refilling
                foreach (var table in Chairs[i].GetTables())
                {
                    if (!tables.TryAdd(table, 0))
                    {
                        tables[table]++;
                    }
                }
            }

            for (int i = 0; i < Chairs.Count; i++)
            {
                PlayerHand.Remove(Chairs[i].PlacedCard); // Remove the placed cards from the list of cards in the playerhand for the refilling
                foreach (var table in Chairs[i].GetTables())
                {
                    // The upper bound for the check is decreased by the number of cards placed on the table for the first card. That ensures, that the list of the cards at the table is only checked up to the point of the card
                    // For the second placed card that is no longer necessary
                    int maxIndex = table.placedCards.Count - math.max(tables[table] - i, 0);
                    if (maxIndex > 1)
                    {
                        if (table.isOneNationality(maxIndex)) // Double points
                        {
                            UpdatePlayerScore(2 * maxIndex);
                        }
                        else
                        {
                            UpdatePlayerScore(maxIndex);
                        }
                    }
                }
            }
            Chairs.Clear();
            
            // Barstool
            if (BarStool != null)
            {
                PlayerHand.Remove(BarStool.PlacedCard); // Remove the placed cards from the list of cards in the playerhand for the refilling
                UpdatePlayerScore(BarStool.Value);
            }
            BarStool =  null;

            if (PlayerScore < 0)
            {
                _playerEliminated = true;
                PlayerGameBar.GetComponentInParent<CanvasGroup>().alpha = 0.6f;
            }
        }

        public void UpdatePlayerScore(int points)
        {
            PlayerScore += points;
            if (points == 8)
            {
                MaxCardCount--;
                // If a player has no cards left the game ends
                if (MaxCardCount == 0)
                {
                    _playerManager.GameEnded();
                }
            }
            Debug.Log(PlayerName + " " + PlayerScore);
        }

        public GameObject FindNextCardSlot()
        {
            for (int i = 0; i < PlayerGameBar.transform.GetChild(1).transform.childCount; i++)
            {
                if (PlayerGameBar.transform.GetChild(1).transform.GetChild(i).childCount == 0)
                {
                    return PlayerGameBar.transform.GetChild(1).transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Subtract 5 Points for every normal card and 10 Points for every Joker remaining in the players hand at the end of the game
        /// </summary>
        public void SubtractPointsForRemainingCards()
        {
            foreach (var card in PlayerHand)
            {
                UpdatePlayerScore(card.cardData.nationality == Nationality.Joker ? -10 : -5);
            }
        }
    }
}

