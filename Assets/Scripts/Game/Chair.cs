using Assets.Scripts.Models;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public GameObject FirstTableGO;
    public GameObject SecondTableGO; // Can be null
    private Table _firstTable;
    private Table _secondTable;

    private void Awake()
    {
        _firstTable = FirstTableGO.GetComponent<Table>();
        if (SecondTableGO != null)
        {
            _secondTable = SecondTableGO.GetComponent<Table>();
        }
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
        if (CheckPlaceCard(card) && !card.GetIsPlaced())
        {
            GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);
            _firstTable.AddPlacedCard(card.cardData);
            card.Player.Tables.Add(_firstTable);
            if (_secondTable != null)
            {
                _secondTable.AddPlacedCard(card.cardData);
                card.Player.Tables.Add(_secondTable);
            }
            card.UpdateIsPlaced(1);
            return true;
        }
        return false;
    }
}
