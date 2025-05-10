using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Models;

public class Deck : MonoBehaviour
{
    private Stack<CardData> deckStack;

    public GameObject cardPrefab;
    
    private PlayerManager _playerManager; 

    private void Awake()
    {
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        GenerateDeck();
        ShuffleDeck();
    }

    void GenerateDeck()
    {
        deckStack = new Stack<CardData>();
        foreach (Nationality nationality in Enum.GetValues(typeof(Nationality)))
        {
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                // Every card is 4x in the deck, except for the jokers with 2 instances for male and female
                int repeatCount = nationality != Nationality.Joker ? 4 : 2;
                for (int i = 0; i < repeatCount; i++)
                {
                    CardData newCard = ScriptableObject.CreateInstance<CardData>();
                    newCard.gender = gender;
                    newCard.nationality = nationality;
                    newCard.cardSprite = GetSpriteForCountry(nationality, gender);
                    newCard.cardID = deckStack.Count;
                    deckStack.Push(newCard);
                }
            }
        }
    }

    public Sprite GetSpriteForCountry(Nationality nationality, Gender gender)
    {
        string spriteName = "Cards/" + gender + "_" + nationality;
        return Resources.Load<Sprite>(spriteName);
    }

    void ShuffleDeck()
    {
        List<CardData> tempDeck = new List<CardData>(deckStack);
        deckStack.Clear();

        while (tempDeck.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, tempDeck.Count);
            deckStack.Push(tempDeck[index]);
            tempDeck.RemoveAt(index);
        }
    }

    public Card DrawCard(Player player)
    {
        if (deckStack.Count > 4)
        {
            CardData cardData = deckStack.Pop();
            
            GameObject newCardObject = Instantiate(cardPrefab, player.FindNextCardSlot().transform);
            newCardObject.GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 1);
            newCardObject.GetComponent<Card>().SetCardData(cardData);
            newCardObject.GetComponent<Card>().Player = player;
            return newCardObject.GetComponent<Card>();
        }
        // The Game Ends if the 4th last card in the stack is drawn
        _playerManager.GameEnded();
        return null;
    }
}

