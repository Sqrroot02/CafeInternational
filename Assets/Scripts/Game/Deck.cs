using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck : MonoBehaviour
{


    public enum availableCountries { }//TO BE FILLED
    public enum genders { MAN, WOMAN }
    public List<Sprite> countrySprites; //TODO: add sprites with Country, Gender Name

    private Stack<Card> deckStack;

    public GameObject cardPrefab; 
    public Transform cardSpawnArea;
    private Vector3 spawnOffset = new Vector3(0.5f, 0, 0);

    private int cardCount = 0;

    void Start()
    {
        GenerateDeck();
        ShuffleDeck();
    }

    void GenerateDeck()
    {

        Debug.Log("Generate");
        //deckStack = new Stack<Card>();

        //foreach (availableCountries country in Enum.GetValues(typeof(availableCountries)))
        //{
        //    foreach (genders gender in Enum.GetValues(typeof(genders)))
        //    {
        //        Sprite sprite = GetSpriteForCountry(country.ToString(), gender.ToString());
        //        Card newCard = new Card(country.ToString(), gender.ToString(), sprite);
        //        deckStack.Push(newCard);
        //    }
        //}
    }

    Sprite GetSpriteForCountry(string country, string gender)
    {
        string targetName = gender + "_" + country;

        foreach (Sprite sprite in countrySprites)
        {
            if (sprite.name == targetName)
            {
                return sprite;
            }
        }
        return null;
    }

    void ShuffleDeck()
    {
        Debug.Log("Shuffle");
        //List<Card> tempDeck = new List<Card>(deckStack);
        //deckStack.Clear();

        //while (tempDeck.Count > 0)
        //{
        //    int index = UnityEngine.Random.Range(0, tempDeck.Count);
        //    deckStack.Push(tempDeck[index]);
        //    tempDeck.RemoveAt(index);
        //}
    }

    public void DrawCard()
    {
        Debug.Log("DrawCard :)");
        //Debug.Log("Card will be drawn");
        //if (deckStack.Count > 0)
        //{
        //    Card drawnCard = deckStack.Pop();

        //    Vector3 spawnPosition = cardSpawnArea.position + spawnOffset * cardCount;
        //    GameObject newCardObject = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
        //    newCardObject.GetComponent<SpriteRenderer>().sprite = drawnCard.sprite;

        //    cardCount++;

        //    Debug.Log("Gezogene Karte: " + drawnCard.country + " - " + drawnCard.gender);
        //}
        //else
        //{
        //    Debug.Log("Der Kartenstapel ist leer!");
        //}
    }
}

