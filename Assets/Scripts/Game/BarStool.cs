using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

public class BarStool : MonoBehaviour
{
    public int Value; // The value of the chair that is added to the points of the one placing a card here // Negative scores are also added and not implemented separately
    public Image stoolSprite; // The image on the stool // the number of points added
    private int _index = 0;
    public Card PlacedCard;

    // Awake is called once even before Start
    private void Awake()
    {
        stoolSprite = GetComponent<Image>();
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public int GetIndex()
    {
        return _index; 
    }

    /// <summary>
    /// Placed the given Card on the stool if possible
    /// </summary>
    public bool PlaceCard(Card card)
    {
        if (card.cardData.nationality != Nationality.Joker && !card.GetIsPlaced() && transform.parent.transform.parent.gameObject.GetComponent<Bar>().CheckAddCard(_index))
        {
            if (card.UpdateIsPlaced(2))
            {
                transform.parent.transform.parent.gameObject.GetComponent<Bar>().AddCard();
                GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);
                card.Player.BarStool = this;
                PlacedCard = card;
                return true;
            }
        }
        return false;
    }
}
