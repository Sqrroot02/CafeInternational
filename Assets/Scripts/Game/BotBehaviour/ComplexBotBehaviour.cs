using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
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
        public Nationality JokerIdentity { get; set; }

        public Move(int cardId, int chairId, Nationality jokerIdentity = Nationality.Joker)
        {
            CardId = cardId;
            ChairId = chairId;
            JokerIdentity = jokerIdentity;
        }

        public override string ToString()
        {
            var s = JokerIdentity != Nationality.Joker ? $", JokerIdentity: {JokerIdentity}" : "";
            return $"CardId: {CardId}, ChairId: {ChairId}{s}";
        }
    }
    
    /// <summary>
    /// A complete move a player can make. So first placed card, followed by a second card
    /// </summary>
    public class Turn
    {
        public Move FirstMove { get; private set; }
        public Move SecondMove { get; private set; }

        public int Points { get; private set; }
        public int CompletedNationalities { get; private set; }
        
        public Card FirstCard { get; private set; } // TODO Remove
        public Card SecondCard { get; private set; } // TODO Remove
        public Chair FirstChair { get; private set; } // TODO Remove
        public Chair SecondChair { get; private set; } // TODO Remove

        /// <summary>
        /// Create a turn with two moves
        /// </summary>
        /// <param name="firstCard"></param>
        /// <param name="firstChair"></param>
        /// <param name="secondCard"></param>
        /// <param name="secondChair"></param>
        /// <param name="jokerIdentityFirstCard">The identity of the card if it is a joker</param>
        /// <param name="jokerIdentitySecondCard">The identity of the card if it is a joker</param>
        public Turn(Card firstCard,Chair firstChair, Card secondCard, Chair secondChair, Nationality jokerIdentityFirstCard = Nationality.Joker, Nationality jokerIdentitySecondCard = Nationality.Joker)
        {
            FirstMove = new Move(firstCard.cardData.cardID, firstChair.ChairID, jokerIdentityFirstCard);
            SecondMove = new Move(secondCard.cardData.cardID, secondChair.ChairID, jokerIdentitySecondCard);
            EvaluateTurn( firstChair, firstCard, secondChair, secondCard);
            
            FirstCard = firstCard;
            SecondCard = secondCard;
            FirstChair = firstChair;
            SecondChair = secondChair;
        }

        /// <summary>
        /// Create a turn with a single move
        /// </summary>
        /// <param name="firstCard"></param>
        /// <param name="firstChair"></param>
        /// <param name="jokerIdentityFirstCard">The identity of the card if it is a joker</param>
        public Turn(Card firstCard,Chair firstChair, Nationality jokerIdentityFirstCard = Nationality.Joker)
        {
            FirstMove = new Move(firstCard.cardData.cardID, firstChair.ChairID, jokerIdentityFirstCard);
            EvaluateTurn( firstChair, firstCard);
            
            FirstCard = firstCard;
            FirstChair = firstChair;
        }
        
        /// <summary>
        /// Calculates the points of the turn and the number of completed nationalities
        /// </summary>
        /// <param name="firstChair"></param>
        /// <param name="firstCard"></param>
        /// <param name="secondChair"></param>
        /// <param name="secondCard"></param>
        private void EvaluateTurn(Chair firstChair, Card firstCard, Chair secondChair = null, Card secondCard = null)
        {
            foreach (var table in firstChair.GetTables()) // First played card
            {
                if (table.placedCards.Count > 0) // Otherwise no points
                {
                    if (table.isOneNationality(table.placedCards.Count) &&
                        (firstCard.cardData.nationality == table.nationality || (firstCard.cardData.nationality == Nationality.Joker && FirstMove.JokerIdentity == table.nationality))) // All have the same nationality -> Double points
                    {
                        int p = (table.placedCards.Count + 1) * 2;
                        if (p == 8) // Only for a completed Nationality are 8 points rewarded
                        {
                            CompletedNationalities++;
                        }
                        Points += p;
                    }
                    else
                    {
                        Points += table.placedCards.Count + 1;
                    }
                }
            }

            if (secondChair is not null && secondCard is not null)
            {
                foreach (var table in secondChair.GetTables())
                {
                    if (firstChair.GetTables().Contains(table)) // The first and second card are placed at the same table
                    {
                        if (table.isOneNationality(table.placedCards.Count) 
                            && (firstCard.cardData.nationality == table.nationality || (firstCard.cardData.nationality == Nationality.Joker && FirstMove.JokerIdentity == table.nationality))
                            && (secondCard.cardData.nationality == table.nationality || (secondCard.cardData.nationality == Nationality.Joker && SecondMove.JokerIdentity == table.nationality))) // All have the same nationality or Jokers with the table nationality -> Double points
                        {
                            int p = (table.placedCards.Count + 2) * 2;
                            if (p == 8) // Only for a completed Nationality are 8 points rewarded
                            {
                                CompletedNationalities++;
                            }
                            Points += p;
                        }
                        else
                        {
                            Points += table.placedCards.Count + 2;
                        }
                    }
                    else // The tables of the cards are different
                    {
                        if (table.placedCards.Count > 0) // Otherwise no points
                        {
                            if (table.isOneNationality(table.placedCards.Count) 
                                && (secondCard.cardData.nationality == table.nationality || (secondCard.cardData.nationality == Nationality.Joker && SecondMove.JokerIdentity == table.nationality))) // All have the same nationality -> Double points
                            {
                                int p = (table.placedCards.Count + 1) * 2;
                                if (p == 8) // Only for a completed Nationality are 8 points rewarded
                                {
                                    CompletedNationalities++;
                                }
                                Points += p;
                            }
                            else
                            {
                                Points += table.placedCards.Count + 1;
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var s = SecondMove != null ? $", SecondMove {SecondMove}" : "";
            return $"SecondMove: {FirstMove}{s}, Points: {Points}, CompletedNationalities: {CompletedNationalities}";
        }
    }
    
    public class ComplexBotBehaviour : MonoBehaviour
    {
        private Bar _bar;
        private Chair[] _chairs;
        private Dictionary<Nationality, List<Chair>> _nationalityToChairListDictionary = new ();
        private PlayerManager _playerManager;

        public void Setup(PlayerManager playerManager)
        {
            _playerManager = playerManager;
            _bar = GameObject.Find("Bar").GetComponent<Bar>();
            _chairs = GameObject.Find("Chairs").transform.GetComponentsInChildren<Chair>();
            MapNationalityToChairListDictionary();
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
                            if (firstCard.cardData.nationality == Nationality.Joker) // Add a possible Turn for every possible Jokeridentity
                            {
                                foreach (var nationality in firstChair.GetUniqueNationalities())
                                {
                                    possibleTurns.Add(new Turn(firstCard, firstChair, nationality)); // Add the possibility, that only this card is placed
                                }
                            }
                            else
                            {
                                possibleTurns.Add(new Turn(firstCard, firstChair)); // Add the possibility, that only this card is placed
                            }
                        }
                        foreach (var secondCard in player.PlayerHand) // Check every card for the second placed card
                        {
                            if (firstCard != secondCard) // Same card cant be played twice
                            {
                                foreach (var secondChair in _nationalityToChairListDictionary[secondCard.cardData.nationality]) // Check every possible chair
                                {
                                    if (firstChair != secondChair // No two cards on the same chair
                                        && secondChair.PlacedCard is null // Only empty chairs
                                        && CheckPlaceCardForSecondPlacement(firstCard.cardData.gender, secondCard.cardData.gender, firstChair, secondChair) // Is placeable after the first card
                                        && ((!firstChair.NoCardAtTheTable() && !secondChair.NoCardAtTheTable()) || HasOverlappingTables(firstChair, secondChair))) // Both cards either have to have other cards on the table, or need to be placed on the same table
                                    {
                                        // First and Second cards are jokers
                                        if (firstCard.cardData.nationality == Nationality.Joker && secondCard.cardData.nationality == Nationality.Joker)
                                        {
                                            foreach (var firstNationality in firstChair.GetUniqueNationalities())
                                            {
                                                foreach (var secondNationality in secondChair.GetUniqueNationalities())
                                                {
                                                    possibleTurns.Add(new Turn(firstCard, firstChair, secondCard, secondChair, firstNationality, secondNationality));
                                                }
                                            }
                                        }
                                        else if (firstCard.cardData.nationality == Nationality.Joker) // First card is Joker
                                        {
                                            foreach (var firstNationality in firstChair.GetUniqueNationalities())
                                            {
                                                possibleTurns.Add(new Turn(firstCard, firstChair, secondCard, secondChair, firstNationality));
                                            }
                                        }
                                        else if (secondCard.cardData.nationality == Nationality.Joker) // Second card is Joker
                                        {
                                            foreach (var secondNationality in secondChair.GetUniqueNationalities())
                                            {
                                                possibleTurns.Add(new Turn(firstCard, firstChair, secondCard, secondChair, Nationality.Joker, secondNationality));
                                            }
                                        }
                                        else // No card is Joker
                                        {
                                            possibleTurns.Add(new Turn(firstCard, firstChair, secondCard, secondChair));
                                        }
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
        /// <param name="secondCardGender">The gender of the card that is placed secondly</param>
        /// <param name="firstChair">The first chair to place the card on</param>
        /// <param name="secondChair">The second chair a card was placed on</param>
        /// <returns>true if the card is placeable</returns>
        private bool CheckPlaceCardForSecondPlacement(Gender firstCardGender, Gender secondCardGender, Chair firstChair, Chair secondChair)
        {
            foreach (var table in secondChair.GetTables())
            {
                if (firstChair.GetTables().Contains(table)) // The cards are placed at the same table
                {
                    if (!table.CheckGenderPlaceable(secondCardGender, firstCardGender == Gender.Male ? 1 : 0, firstCardGender == Gender.Female ? 1 : 0))
                    {
                        return false;
                    }
                }
                else // The cards are placed at different tables, so the gender of the first card does not matter for the second card
                {
                    if (!table.CheckGenderPlaceable(secondCardGender))
                    {
                        return false;
                    }
                }
            }

            return true;
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
        
        /// <summary>
        /// Finds all possible jokers the given player can replace with the cards in their hand
        /// </summary>
        /// <param name="player"></param>
        /// <returns>A list of moves that describe the replace operations</returns>
        private List<Move> FindReplaceJokerMoves(Player player)
        {
            List<Move> moves = new List<Move>();
            foreach (var card in player.PlayerHand)
            {
                if (card.cardData.nationality != Nationality.Joker) // A joker can not be replaced with another joker
                {
                    foreach (var chair in _nationalityToChairListDictionary[card.cardData.nationality])
                    {
                        if (chair.PlacedCard != null // A card has to be placed
                            && chair.PlacedCard.cardData.nationality == Nationality.Joker // It needs to be a joker
                            && chair.PlacedCard.cardData.gender == card.cardData.gender // If the gender matches the other cards at the table are irrelevant
                            && chair.HasPlaceableNationality(card.cardData.nationality)) // The nationality must fit the chair -> The check if the card is a joker in HasPlaceableNationality is not relevant here
                        {
                            moves.Add(new Move(card.cardData.cardID, chair.ChairID));
                        }
                    }
                }
            }
            return moves;
        }


        /// <summary>
        /// Checks how many open chairs there are for a card that is not a joker
        /// </summary>
        /// <param name="player"></param>
        /// <returns>A list of tuples with the card and the number of chairs that are still free for the gender of the card</returns>
        private List<(Card, int)> EvaluateCardsPlaceability(Player player)
        {
            List<(Card, int)> cards = new List<(Card, int)>();
            foreach (var card in player.PlayerHand)
            {
                if (card.cardData.nationality != Nationality.Joker) // Jokers are irrelevant for the bar
                {
                    int chairCount = 0;
                    foreach (var chair in _nationalityToChairListDictionary[card.cardData.nationality])
                    {
                        if (chair.PlacedCard == null) // Is somewhat inaccurate if for example only a single card is placed and all three of the other chairs are counted -> No need for an exact count that could differ due to placement of cards outside the searched chairs 
                        {
                            if (chair.CheckPlaceCard(card))
                            {
                                chairCount++;
                            }
                            else if (chair.OnlyCardAtTheTable()) // If only one card is at the table three chairs are free. If the gender does not match that would mean none of the chairs would be counted
                            {
                                chairCount++;
                            }
                        }
                    }
                    cards.Add((card, chairCount));
                }
            }
            return cards;
        }

        /// <summary>
        /// Finds the card with the smallest amount of open chairs
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The card with the least open chairs</returns>
        private Card GetCardToPlaceAtBar(Player player)
        {
            var evaluatedCards = EvaluateCardsPlaceability(player); // Orders the cards ascending 
            var smallestValues = evaluatedCards.Where(i => i.Item2 == evaluatedCards.Min(j => j.Item2)).ToList();
            
            // TODO: Random an dieser Stelle einfügen
            
            return smallestValues[0].Item1;
        }

        /// <summary>
        /// Replaces a joker if possible or otherwise plays the least useful card to the bar
        /// </summary>
        /// <param name="player"></param>
        private void ReplaceJokerOrPlaceCardToTheBar(Player player)
        {
            var jokerMoves = FindReplaceJokerMoves(player);
            if (jokerMoves.Count > 0)
            {
                // TODO Random an dieser Stelle einfügen
                var move = jokerMoves[0];
                Debug.Log($"Replaced joker at {move.ChairId} with card {move.CardId}");
                _playerManager.PlayCard(move.CardId, move.ChairId, -1);
            }
            else
            {
                var card = GetCardToPlaceAtBar(player);
                Debug.Log($"Placed card {card.cardData} to bar at index {_bar.GetNextIndex()}");
                _playerManager.PlayCard(card.cardData.cardID, -1, _bar.GetNextIndex());
            }
        }
        
        
        /// <summary>
        /// Chooses the best turn from the list of turns
        /// </summary>
        /// <param name="turns"></param>
        /// <param name="prioritizePoints">If points are prioritized a move with fewer completed nationalities can be chosen, if the total number of points + 5 * completed Nationalities is greater</param>
        /// <returns>The best found turn for the given priority</returns>
        private Turn FindBestTurn(List<Turn> turns, bool prioritizePoints)
        {
            List<Turn> sortedTurns;
            if (prioritizePoints)
            {
                // Add 5 points for each completed nationality in the turn
                // Has the flaw, that joker fields cost more points at the end, but is ignored here
                int turnMax = turns.Max(move => move.Points + move.CompletedNationalities * 5);
                sortedTurns = turns.Where(move => move.Points + move.CompletedNationalities * 5 == turnMax).OrderByDescending(move => move.CompletedNationalities).ToList();
            }
            else
            {
                int turnMax = turns.Max(move => move.CompletedNationalities);
                sortedTurns = turns.Where(move => move.CompletedNationalities == turnMax).OrderByDescending(move => move.Points).ToList();
            }
            
            // TODO: Random an dieser Stelle einfügen
            return sortedTurns[0];
        }

        /// <summary>
        /// Finds all turns that block other turns
        /// </summary>
        /// <param name="botTurns">The possible turns for the bot</param>
        /// <param name="player">The player to block</param>
        /// <returns>A list of tuples with (Blocking turn of the bot, List with blocked turns, sum of points of blocked turns, sum of completed nationalities of blocked turns)</returns>
        private List<(Turn, List<Turn>, int, int)> FindBlockingTurns(List<Turn> botTurns, Player player)
        {
            List<Turn> turnsPlayer = GetAllPossibleTurns(player); // The firstTurn bool is not needed because it cant be the first turn at the point when its the next players turn

            List<(Turn, List<Turn>, int, int)> blockingTurns = new(); // Blocking turn of the bot, List with blocked turns, sum of points of blocked turns, sum of completed nationalities of blocked turns
            foreach (var botTurn in botTurns)
            {
                List<Turn> blockedTurns = new List<Turn>();
                foreach (var playerTurn in turnsPlayer)
                {
                    if (botTurn.FirstMove.ChairId == playerTurn.FirstMove.ChairId ||
                        (playerTurn.SecondMove != null && botTurn.FirstMove.ChairId == playerTurn.SecondMove.ChairId) ||
                        (botTurn.SecondMove != null && botTurn.SecondMove.ChairId == playerTurn.FirstMove.ChairId)||
                        (botTurn.SecondMove != null && playerTurn.SecondMove != null && botTurn.SecondMove.ChairId == playerTurn.SecondMove.ChairId))
                    {
                        blockedTurns.Add(playerTurn);
                    }
                }
                if (blockedTurns.Count > 0)
                {
                    blockingTurns.Add((
                        botTurn,
                        blockedTurns,
                        blockedTurns.Sum(turn => turn.Points),
                        blockedTurns.Sum(turn => turn.CompletedNationalities)));
                }
            }
            return blockingTurns;
        }

        /// <summary>
        /// Checks all turns that the bot can block and evaluates the turn that blocks the highest number of complete nationalities and the highest score over all the blocked turns
        /// </summary>
        /// <param name="botTurns">The possible turns for the bot</param>
        /// <param name="bot">The bot</param>
        /// <param name="players">All players</param>
        /// <returns>The tuple with the turn that blocks the highest cumulative number of completedNationalities, after that the highest cumulative score and after that the highest turn score</returns>
        private (Turn, List<Turn>, int, int)? FindMostMischievousBehaviour(List<Turn> botTurns, Player bot, List<Player> players)
        {
            //(Blocking turn of the bot, List with blocked turns, sum of points of blocked turns, sum of completed nationalities of blocked turns)
            List<List<(Turn, List<Turn>, int, int)>> playerBlockedTurns = new();
            foreach (var player in players)
            {
                if (player != bot)
                {
                    playerBlockedTurns.Add(FindBlockingTurns(botTurns, player));
                }
            }

            List<(Turn, List<Turn>, int, int)> bestTurns = new (); 
            foreach (var t in playerBlockedTurns)
            {
                if (t.Count > 0)
                {
                    bestTurns.Add(t.OrderByDescending(turn => turn.Item4).ThenByDescending(turn => turn.Item3)
                        .ThenByDescending(turn => turn.Item1.Points).First());
                }
            }

            if (bestTurns.Count > 0)
                return bestTurns.OrderByDescending(turn => turn.Item4).ThenByDescending(turn => turn.Item3)
                    .ThenByDescending(turn => turn.Item1.Points).First();
            else
                return null;
        }

        /// <summary>
        /// Decides if the options for mischief are evaluated, or if the best turn is selected from the possible turns and played instantly  
        /// </summary>
        /// <param name="turn">The turn that is chosen as best possible turn</param>
        /// <returns>true if the options for mischief should be evaluated</returns>
        private bool DecideFindMischievousBehaviour(Turn turn)
        {
            // TODO Implement individual bot behaviour with random value that decides to prank even if its not beneficial
            return turn.CompletedNationalities == 0 && turn.Points <= 15;
        }

        /// <summary>
        /// Decide if the mischievous turn is used
        /// </summary>
        /// <param name="bestTurn">The best turn</param>
        /// <param name="bestMischievousTurn">The best mischievous turn</param>
        /// <returns>true if the mischievous turn is used</returns>
        private bool DecideUseMischievousBehaviour(Turn bestTurn, (Turn, List<Turn>, int, int) bestMischievousTurn)
        {
            //bestMischievousTurn = (Blocking turn of the bot, List with blocked turns, sum of points of blocked turns, sum of completed nationalities of blocked turns)
            //if (bestTurn == bestMischievousTurn.Item1) // The decision does not make a difference if they are the same
            //    return false;
            //if (bestTurn.Points > 6 && bestTurn.Points >= bestMischievousTurn.Item1.Points + 2) // If the mischievous turn is close in Points to the best turn play it; Only if the best turn brings in a decent number of points
            //    return true;
            if (bestMischievousTurn.Item4 > 0)
                return true;
            // TODO Implement individual bot behaviour with random value that decides to prank even if its not beneficial
            Debug.Log("Decided against mischievous behaviour");
            return false;
        }

        /// <summary>
        /// Plays a turn in order 
        /// </summary>
        /// <param name="turn">The turn to play</param>
        private void PlayTurn(Turn turn)
        {
            Debug.Log($"Placed first card at {turn.FirstMove.ChairId} with card {turn.FirstMove.CardId}");
            _playerManager.PlayCard(turn.FirstMove.CardId, turn.FirstMove.ChairId, -1, turn.FirstMove.JokerIdentity);
            
            if (turn.SecondMove != null)
            {
                Debug.Log($"Placed second card at {turn.SecondMove.ChairId} with card {turn.SecondMove.CardId}");
                _playerManager.PlayCard(turn.SecondMove.CardId, turn.SecondMove.ChairId, -1,turn.SecondMove.JokerIdentity);
            }
        }

        /// <summary>
        /// Finds and plays a complex turn
        /// </summary>
        /// <param name="bot">The bot to play for</param>
        /// <param name="players">A list with all players</param>
        /// <param name="firstTurn">true if it is the first turn</param>
        /// <param name="mischiefBot">true if the bot should prioritize mischief over scoring</param>
        public void MakeComplexTurn(Player bot, List<Player> players, bool firstTurn, bool mischiefBot)
        {
            Debug.Log($"MakeComplexTurn for bot {bot.PlayerName}");
            var turnsBot = GetAllPossibleTurns(bot, firstTurn);
            if (turnsBot.Count > 0)
            {
                var bestTurn = FindBestTurn(turnsBot, false);
                Debug.Log($"Best turn: {bestTurn}");
                if (mischiefBot || DecideFindMischievousBehaviour(bestTurn))
                {
                    Debug.Log("Decided to find mischievous behaviour");
                    var mischiefBehaviour = FindMostMischievousBehaviour(turnsBot, bot, players);
                    if (mischiefBehaviour != null) // If no move is found the mischief cant be played
                    {
                        Debug.Log(
                            $"Found mischievous behaviour for {mischiefBehaviour.Value.Item1} with {mischiefBehaviour.Value.Item3} blocked points and {mischiefBehaviour.Value.Item4} blocked completedNationalities");
                        PlayTurn(mischiefBot || DecideUseMischievousBehaviour(bestTurn, mischiefBehaviour.Value) ? mischiefBehaviour.Value.Item1 : bestTurn);
                    }
                    else
                    {
                        PlayTurn(bestTurn);
                    }
                }
                else
                {
                    PlayTurn(bestTurn);
                }
            }
            else // No playable Cards -> Replace a Joker or set a card at the bar
            {
                ReplaceJokerOrPlaceCardToTheBar(bot);
            }
            Debug.Log($"End MakeComplexTurn for bot {bot.PlayerName}");
        }
    }
}