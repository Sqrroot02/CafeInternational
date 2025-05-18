using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Game.BotBehaviour
{
    /// <summary>
    /// A Singular card that is placed
    /// </summary>
    public class Move
    {
        public int CardId { get; set; }
        public int ChairId { get; set; }

        public Move(int cardId, int chairId)
        {
            CardId = cardId;
            ChairId = chairId;
        }
    }
    
    /// <summary>
    /// A complete move a player can make. So first placed card, followed by a second card
    /// </summary>
    public class Turn
    {
        public Move FirstMove { get; set; }
        public Move SecondMove { get; set; }

        /// <summary>
        /// Create a turn with two moves
        /// </summary>
        /// <param name="firstCardId">The id of the card that is placed first</param>
        /// <param name="firstChairId">The id of the chair that the first card is placed on</param>
        /// <param name="secondCardId">The id of the card that is placed secondly</param>
        /// <param name="secondChairId">The id of the chair the second card is placed on</param>
        public Turn(int firstCardId, int firstChairId, int secondCardId, int secondChairId)
        {
            FirstMove = new Move(firstCardId, firstChairId);
            SecondMove = new Move(secondCardId, secondChairId);
        }

        /// <summary>
        /// Create a turn with a single move
        /// </summary>
        /// <param name="cardId">The id of the placed card</param>
        /// <param name="chairId">The id of the chair the card is placed on</param>
        public Turn(int cardId, int chairId)
        {
            FirstMove = new Move(cardId, chairId);
        }

        public bool CompletesNationality()
        {
            return false;
        }
        public int GetPoints()
        {
            return 0;
        }
        
        public void PlayMove()
        {
            
        }
    }
    
    public class ComplexBotBehaviour
    {
        private static ComplexBotBehaviour _instance;
        private Bar _bar;
        private Chair[] _chairs;
        private Dictionary<Nationality, List<Chair>> _nationalityToChairListDictionary = new ();

        private ComplexBotBehaviour()
        {
            _bar = GameObject.Find("Bar").GetComponent<Bar>();
            _chairs = GameObject.Find("Chairs").transform.GetComponentsInChildren<Chair>();
            MapNationalityToChairListDictionary();
        }
        
        /// <summary>
        /// Get Singleton Instance of the class
        /// </summary>
        /// <returns>Singleton Instance</returns>
        public static ComplexBotBehaviour GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ComplexBotBehaviour();
            }
            return _instance;
        }

        /// <summary>
        /// Fills the dictionary _nationalityToChairListDictionary with nationalities as keys and List of Chairs as values
        /// </summary>
        private void MapNationalityToChairListDictionary()
        {
            _nationalityToChairListDictionary.Add(Nationality.Joker, new List<Chair>(_chairs)); // The Joker Nationality is placeable at every chair, so every chair is saved in the list
            foreach (Chair chair in _chairs)
            {
                var (nat1, nat2) = chair.GetNationalities();
                if (!_nationalityToChairListDictionary.ContainsKey(nat1))
                {
                    _nationalityToChairListDictionary.Add(nat1, new List<Chair>());
                }
                _nationalityToChairListDictionary[nat1].Add(chair);
                if (nat2 != null && nat1 != nat2.Value)
                {
                    if (!_nationalityToChairListDictionary.ContainsKey(nat2.Value))
                    {
                        _nationalityToChairListDictionary.Add(nat2.Value, new List<Chair>());
                    }
                    _nationalityToChairListDictionary[nat2.Value].Add(chair);
                }
            }
        }

        private List<Turn> GetAllPossibleTurns(Player player, bool firstTurn = false)
        {
            List<Turn> possibleTurns = new ();
            foreach (var firstCard in player.PlayerHand) // Check every card for the first placed card
            {
                foreach (var firstChair in _nationalityToChairListDictionary[firstCard.cardData.nationality]) // Check every possible Chair
                {
                    if (firstChair.PlacedCard is null && firstChair.CheckPlaceCard(firstCard)) // The card is placeable at the position
                    {
                        if (!firstChair.NoCardAtTheTable() || firstTurn) // A single card has to have neighbours, or it needs to be the first turn
                        {
                            possibleTurns.Add(new Turn(firstCard.cardData.cardID, firstChair.ChairID)); // Add the possibility, that only this card is placed
                        }
                        foreach (var secondCard in player.PlayerHand) // Check every card for the second placed card
                        {
                            if (firstCard != secondCard) // Same card cant be played twice
                            {
                                foreach (var secondChair in _nationalityToChairListDictionary[secondCard.cardData.nationality]) // Check every possible chair
                                {
                                    bool e = ((!firstChair.NoCardAtTheTable() && !secondChair.NoCardAtTheTable()) ||
                                              HasOverlappingTables(firstChair, secondChair));
                                    if (firstChair != secondChair // No two cards on the same chair
                                        && secondChair.PlacedCard is null // Only empty chairs
                                        && CheckPlaceCardForSecondPlacement(firstCard.cardData.gender, secondCard, secondChair) // Is placeable after the first card
                                        && ((!firstChair.NoCardAtTheTable() && !secondChair.NoCardAtTheTable()) || HasOverlappingTables(firstChair, secondChair))) // Both cards either have to have other cards on the table, or need to be placed on the same table
                                    {
                                        possibleTurns.Add(new Turn(firstCard.cardData.cardID, firstChair.ChairID, secondCard.cardData.cardID, secondChair.ChairID));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return possibleTurns;
        }

        /// <summary>
        /// Checks if the second card is placeable, if the first card was played before
        /// </summary>
        /// <param name="firstCardGender">The gender of the card that was placed prior</param>
        /// <param name="secondCard">The card that is placed secondly</param>
        /// <param name="chair">The chair to place the card on</param>
        /// <returns>true if the card is placeable</returns>
        private bool CheckPlaceCardForSecondPlacement(Gender firstCardGender, Card secondCard, Chair chair)
        {
            return chair.CheckPlaceCard(secondCard, firstCardGender == Gender.Male ? 1:0, firstCardGender == Gender.Female ? 1:0);
        }

        /// <summary>
        /// Checks if the chairs have a common table
        /// </summary>
        /// <param name="firstChair">The first chair</param>
        /// <param name="secondChair">The second chair</param>
        /// <returns>true if they share a common table</returns>
        private bool HasOverlappingTables(Chair firstChair, Chair secondChair)
        {
            foreach (var table in firstChair.GetTables())
            {
                if (secondChair.IsInTables(table))
                {
                    return true;
                }
            }
            return false;
        }

        public void MakeComplexMove(Player bot, List<Player> players, bool firstTurn = false)
        {
            var turns = GetAllPossibleTurns(bot, firstTurn);
            Debug.Log("E");
        }
    }
}