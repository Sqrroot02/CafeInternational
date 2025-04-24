using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public GameObject FirstTableGO;
    public GameObject SecondTableGO; // Can be null
    private Table _firstTable;
    private Table _secondTable;
    public Card PlacedCard { private set; get; }

    private void Awake()
    {
        _firstTable = FirstTableGO.GetComponent<Table>();
        if (SecondTableGO != null)
        {
            _secondTable = SecondTableGO.GetComponent<Table>();
        }
    }

    public List<Table> GetTables()
    {
        var tables = new List<Table>();
        tables.Add(_firstTable);
        if (_secondTable != null)
        {
            tables.Add(_secondTable);
        }
        return tables;
    } 

    private bool HasPlaceableNationality(Nationality nationality)
    {
        return nationality == Nationality.Joker || _firstTable.nationality == nationality || (_secondTable != null && _secondTable.nationality == nationality);
    }

    private bool CheckPlaceCard(Card card)
    {
        // Check nationality matches the chair
        if (HasPlaceableNationality(card.cardData.nationality))
        {   // Check the first table is placeable
            if (_firstTable.CheckGenderPlaceable(card.cardData.gender))
            {
                // If the chair is only at one table the gendercheck of the first table is enough
                if (_secondTable == null)
                {
                    return true;
                }
                // Otherwise the gender has to be checked for the second gender
                return _secondTable.CheckGenderPlaceable(card.cardData.gender);
            }
            Debug.Log("Table 1 Gender Fail");
        }
        else 
            Debug.Log("No placable nationality");
        return false;
    }

    public bool PlaceCard(Card card)
    {
        if (!card.GetIsPlaced())
        {
            if (PlacedCard ==null)
            {
                if (CheckPlaceCard(card))
                {
                    if (card.UpdateIsPlaced(1))
                    {
                        PlacedCard = card;
                        card.Player.Chairs.Add(this);
                        GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);

                        _firstTable.AddPlacedCard(card.cardData);
                        if (_secondTable != null)
                        {
                            _secondTable.AddPlacedCard(card.cardData);
                        }

                        return true;
                    }
                }
            }
            else if (PlacedCard.cardData.nationality == Nationality.Joker 
                 && card.cardData.nationality != Nationality.Joker 
                 && PlacedCard.cardData.gender == card.cardData.gender 
                 && HasPlaceableNationality(card.cardData.nationality)
                 && card.UpdateIsPlaced(2))
            { // Placing a card that matches the field at the jokers spot and is not a joker
                Debug.Log("Replace by Joker");
                ReplaceJoker(card);
                return true;
            }
        }
        return false;
    }

    private void ReplaceJoker(Card card)
    {
        GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);
        _firstTable.placedCards.Remove(PlacedCard.cardData);
        _firstTable.AddPlacedCard(card.cardData);
        if (_secondTable != null)
        {
            _secondTable.placedCards.Remove(PlacedCard.cardData);
            _secondTable.AddPlacedCard(card.cardData);
        }

        PlacedCard.PlayerBarSlot = card.PlayerBarSlot;
        PlacedCard.ResetCardPosition();
        card.Player.PlayerHand.Add(PlacedCard);
        card.Player.PlayerHand.Remove(card);
        PlacedCard.Player = card.Player;
        

        PlacedCard = card;
    }

    /// <summary>
    /// Checks if the chair is the only that placed a card at either one or both tables.
    /// Only invoke after a card is placed on this 
    /// </summary>
    /// <returns>True if the card on the chair sits alone</returns>
    public bool OnlyCardAtTheTable()
    {
        bool onlyCard = _firstTable.placedCards.Count == 1;
        if (_secondTable != null && onlyCard && _secondTable.placedCards.Count != 1)
        {
            onlyCard = false;
        }
        
        return onlyCard;
    }

    public void RemovePlacedCard()
    {
        _firstTable.placedCards.Remove(PlacedCard.cardData);
        if (_secondTable != null)
        {
            _secondTable.placedCards.Remove(PlacedCard.cardData);
        }
        PlacedCard = null;
        GetComponent<Outline>().UpdateOutlineSprite(null); // TODO Needs to be changed if the actual images of the chairs are implemented -> Change to the original image of the chair
    }
}
