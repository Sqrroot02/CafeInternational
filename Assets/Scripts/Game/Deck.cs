using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Models;

public class Deck : MonoBehaviour
{
    public List<Sprite> countrySprites; //TODO: add sprites with Country, Gender Name

    private Stack<CardData> deckStack;

    public GameObject cardPrefab; 
    public Transform cardSpawnArea;
    private Vector3 spawnOffset = new Vector3(1.25f, 0, 0);


    void Start()
    {
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
                CardData newCard = ScriptableObject.CreateInstance<CardData>();
                newCard.gender = gender;
                newCard.nationality = nationality;
                newCard.cardSprite = GetSpriteForCountry(nationality, gender);
                deckStack.Push(newCard);
            }
        }
    }

    Sprite GetSpriteForCountry(Nationality nationality, Gender gender)
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

    public void DrawCard()
    {
        if (deckStack.Count > 0)
        {
            CardData cardData = deckStack.Pop();

            GameObject newCardObject = Instantiate(cardPrefab, cardSpawnArea);
            newCardObject.GetComponent<Card>().SetCardData(cardData);
        }
        else
        {
            Debug.Log("Der Kartenstapel ist leer!");
        }
    }
}

