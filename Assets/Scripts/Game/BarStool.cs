using UnityEngine;
using UnityEngine.UI;

public class BarStool : MonoBehaviour
{
    public int Value; // The value of the chair that is added to the points of the one placing a card here // Negative scores are also added and not implemented seperately
    public Image stoolSprite; // The image on the stool // Either the number of points added or the placed card
    private bool stoolTaken; // stoolTaken is a flag to mark if the stool is available or not // Standard is false, so a card can be placed
                             // This is a placeholder for when the Card is completed // public Card card;

    // Awake is called once even before Start
    private void Awake()
    {
        stoolTaken = false;
        stoolSprite = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// <summary>
    /// Placed the given Card on the stool if possible
    /// </summary>
    /// <returns>Returns the points given to the player or 0 if placing the card is not possible</returns>
    public int placeCard()
    {
        if (stoolTaken)
        {
            return 0;
        }
        else
        {
            // TODO: stoolSprite.sprite = newCard.cardSprite; 
            stoolTaken = true;
            return Value;
        }
    }
}
