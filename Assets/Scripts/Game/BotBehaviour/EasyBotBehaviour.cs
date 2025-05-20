using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Game.BotBehaviour
{
    /// <summary>
    /// A class to use for easy completely random bot behaviour. Is a singleton
    /// </summary>
    public class EasyBotBehaviour
    {
        private static EasyBotBehaviour _instance;
        private Bar _bar;
        private Chair[] _chairs;
        private Dictionary<Nationality, List<Chair>> _nationalityToChairListDictionary = new ();
        
        private EasyBotBehaviour()
        {
            _bar = GameObject.Find("Bar").GetComponent<Bar>();
            _chairs = GameObject.Find("Chairs").transform.GetComponentsInChildren<Chair>();
            MapNationalityToChairListDictionary();
        }

        /// <summary>
        /// Get Singleton Instance of the class
        /// </summary>
        /// <returns>Singleton Instance</returns>
        public static EasyBotBehaviour GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EasyBotBehaviour();
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
                if (nat2 != null && nat1 != nat2)
                {
                    if (!_nationalityToChairListDictionary.ContainsKey(nat2.Value))
                    {
                        _nationalityToChairListDictionary.Add(nat2.Value, new List<Chair>());
                    }
                    _nationalityToChairListDictionary[nat2.Value].Add(chair);
                }
            }
        }
        
        /// <summary>
        /// Finds a random move to play
        /// </summary>
        /// <param name="player">The bot that plays the move</param>
        /// <returns>True if a card was played</returns>
        private (Card, Chair) FindMove(Player player, bool firstMove = false)
        {
            var cards = new List<Card>(player.PlayerHand);
            for (var _ = 0; _ < player.PlayerHand.Count; _++) // Check all cards in a random order
            {
                var randomCardIndex = Random.Range(0, cards.Count);
                var chairs =
                    new List<Chair>(_nationalityToChairListDictionary[cards[randomCardIndex].cardData.nationality]);
                for (var _2 = 0;
                     _2 < _nationalityToChairListDictionary[cards[randomCardIndex].cardData.nationality].Count;
                     _2++) // Check all chairs in a random order
                {
                    var randomChairIndex = Random.Range(0, chairs.Count);
                    if ((!chairs[randomChairIndex].NoCardAtTheTable() || firstMove) && chairs[randomChairIndex].PlaceCard(cards[randomCardIndex])) // If the card is placeable simply place it
                    {
                        if (cards[randomCardIndex].cardData.nationality == Nationality.Joker)
                        {
                            cards[randomCardIndex].JokerIdentity = chairs[randomChairIndex].GetFirstTableNationality();
                        }
                        return (cards[randomCardIndex], chairs[randomChairIndex]); // A random card was placed so the search is over
                    }

                    chairs.RemoveAt(
                        randomChairIndex); // The chair is not placeable for the card so it is removed from the list of possible chairs
                }
                cards.RemoveAt(randomCardIndex); // The card is not placeable at the tables
            }
            // No card is placeable at any table
            return (null, null);
        }

        private (Card, BarStool) PlayCardToTheBar(Player player)
        {
            var cards = new List<Card>(player.PlayerHand);
            for (var _ = 0; _ < player.PlayerHand.Count; _++) // Check all cards in a random order
            {
                var randomCardIndex = Random.Range(0, cards.Count);
                if (_bar.BarStools[_bar.GetNextIndex()].PlaceCard(cards[randomCardIndex]))
                {
                    return (cards[randomCardIndex], _bar.BarStools[_bar.GetNextIndex() - 1]);
                }
                cards.RemoveAt(randomCardIndex); // The card is not placeable at the bar -> Should only happen to Jokers
            }
            return (null, null);
        }

        public void Play(Player player, bool firstMove = false)
        {
            (Card card, Chair chair) = FindMove(player, firstMove);
            if (card != null)
            {
                PlacePlayerCard(card.gameObject, chair.gameObject);
                if (Random.value > 0.5f && !firstMove)
                {
                    (card, chair) = FindMove(player);
                    if (card != null)
                    {
                        PlacePlayerCard(card.gameObject, chair.gameObject);
                    }                
                }
            }
            else
            {
                (Card cardBar, BarStool barStool) = PlayCardToTheBar(player);
                PlacePlayerCard(cardBar.gameObject, barStool.gameObject);
            }
        }

        public static void PlacePlayerCard(GameObject card, GameObject placeGameObject)
        {
            CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;

            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.SetParent(placeGameObject.transform, false);

            rectTransform.anchoredPosition = Vector2.zero;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }
}