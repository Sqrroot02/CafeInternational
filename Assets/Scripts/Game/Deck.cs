using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<string> availableCountries = new List<string> { }; //TOBE filled
    public List<string> genders = new List<string> { "M", "W" };
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
        deckStack = new Stack<Card>();

        foreach (string country in availableCountries)
        {
            foreach (string gender in genders)
            {
                Sprite sprite = GetSpriteForCountry(country, gender);
                Card newCard = new Card(country, gender, sprite);
                deckStack.Push(newCard);
            }
        }
    }

    Sprite GetSpriteForCountry(string country, string gender)
    {
        foreach (Sprite sprite in countrySprites)
        {
            if (sprite.name.Contains(country) && sprite.name.Contains(gender))
            {
                return sprite;
            }
        }
        return null;
    }

    void ShuffleDeck()
    {
        List<Card> tempDeck = new List<Card>(deckStack);
        deckStack.Clear();

        while (tempDeck.Count > 0)
        {
            int index = Random.Range(0, tempDeck.Count);
            deckStack.Push(tempDeck[index]);
            tempDeck.RemoveAt(index);
        }
    }

    public void DrawCard()
    {
        Debug.Log("Card will be drawn");
        if (deckStack.Count > 0)
        {
            Card drawnCard = deckStack.Pop();

            Vector3 spawnPosition = cardSpawnArea.position + spawnOffset * cardCount;
            GameObject newCardObject = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
            newCardObject.GetComponent<SpriteRenderer>().sprite = drawnCard.sprite;

            cardCount++;

            Debug.Log("Gezogene Karte: " + drawnCard.country + " - " + drawnCard.gender);
        }
        else
        {
            Debug.Log("Der Kartenstapel ist leer!");
        }
    }
}

