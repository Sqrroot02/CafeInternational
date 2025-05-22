using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Models;
using Riptide;
using UnityEngine;

public class Chair : MonoBehaviour, IMessageSerializable
{
    public GameObject FirstTableGO;
    public GameObject SecondTableGO; // Can be null
    public int ChairID;
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

    public bool HasPlaceableNationality(Nationality nationality)
    {
        return nationality == Nationality.Joker || _firstTable.nationality == nationality || (_secondTable != null && _secondTable.nationality == nationality);
    }

    public bool CheckPlaceCard(Card card)
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

    public bool PlaceAndCommit(Card card)
    {
        var hasPlaced = PlaceCard(card);
        if (hasPlaced)
            TurnHistory.CurrentTurnHistory.AddChair(this);
        return hasPlaced;
    }
    
    public bool PlaceCard(Card card)
    {
        if (!card.GetIsPlaced() && !card.Player.GetPlayerBlockedByJokerIdentitySelection())
        {
            if (PlacedCard == null)
            {
                if (CheckPlaceCard(card))
                {
                    if (card.UpdateIsPlaced(1))
                    {
                        PlacedCard = card;
                        card.Player.Chairs.Add(this);
                        GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);

                        _firstTable.AddPlacedCard(card);
                        if (_secondTable != null)
                        {
                            _secondTable.AddPlacedCard(card);
                        }

                        if (card.cardData.nationality == Nationality.Joker)
                        {
                            if (_secondTable != null && _firstTable.nationality != _secondTable.nationality)
                            {
                                SelectJokerIndentity();
                            }
                            else
                            {
                                PlacedCard.JokerIdentity = _firstTable.nationality;
                            }
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

    /// <summary>
    /// Open the UI for the selection of the joker identity if necessary
    /// </summary>
    public void SelectJokerIndentity()
    {
        if (!PlacedCard.Player.IsBot)
        {
            PlacedCard.Player.SetPlayerBlockedByJokerIdentitySelection(true);
            GameObject identitySelection = Instantiate(PlacedCard.GetPlayerManager().JokerIdentitySelectionPrefab, transform);
            identitySelection.transform.GetChild(0).GetComponent<JokerIdentitySelection>().SetUp(PlacedCard, _firstTable.nationality);
            identitySelection.transform.GetChild(1).GetComponent<JokerIdentitySelection>().SetUp(PlacedCard, _secondTable.nationality);
        }
    }

    private void ReplaceJoker(Card card)
    {
        GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);
        _firstTable.placedCards.Remove(PlacedCard);
        _firstTable.AddPlacedCard(card);
        if (_secondTable != null)
        {
            _secondTable.placedCards.Remove(PlacedCard);
            _secondTable.AddPlacedCard(card);
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
    
    public bool NoCardAtTheTable()
    {
        bool noCard = _firstTable.placedCards.Count == 0;
        if (_secondTable != null &&_secondTable.placedCards.Count != 0)
        {
            noCard = false;
        }
        return noCard;
    }

    public void RemovePlacedCard()
    {
        _firstTable.placedCards.Remove(PlacedCard);
        if (_secondTable != null)
        {
            _secondTable.placedCards.Remove(PlacedCard);
        }
        
        TurnHistory.CurrentTurnHistory.RemoveChair(this);
        PlacedCard = null;
        GetComponent<Outline>().UpdateOutlineSprite(null); // TODO Needs to be changed if the actual images of the chairs are implemented -> Change to the original image of the chair
    }

    public (Nationality, Nationality?) GetNationalities()
    {
        return (_firstTable.nationality, _secondTable?.nationality);
    }

    public List<Nationality> GetUniqueNationalities()
    {
        List<Nationality> nationalities = new List<Nationality>();
        nationalities.Add(_firstTable.nationality);
        if (_secondTable != null && _firstTable.nationality != _secondTable.nationality)
        {
            nationalities.Add(_secondTable.nationality);
        }
        return nationalities;
    }

    public Nationality GetFirstTableNationality()
    {
        return _firstTable.nationality;
    }

    /// <summary>
    /// Checks if the given table is part of the tables the chair is placed at 
    /// </summary>
    /// <param name="table">The table to check</param>
    /// <returns>true if the tables overlap</returns>
    public bool IsInTables(Table table)
    {
        return _firstTable == table || _secondTable == table;
    }

    public void Serialize(Message message)
    {
        message.AddInt(ChairID);
        message.AddSerializable(_firstTable);
        
        message.AddBool(_secondTable != null);
        if (_secondTable != null)
            message.AddSerializable(_secondTable);
        
        message.AddBool(PlacedCard != null);
        if (PlacedCard != null)
            message.AddSerializable(PlacedCard);
    }

    public void Deserialize(Message message)
    {
        ChairID = message.GetInt();
        _firstTable = message.GetSerializable<Table>();

        if (message.GetBool())
            _secondTable = message.GetSerializable<Table>();
        
        if (message.GetBool())
            PlacedCard = message.GetSerializable<Card>();
    }
}
