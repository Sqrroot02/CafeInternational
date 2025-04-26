using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JokerIdentitySelection : MonoBehaviour, IPointerClickHandler
{
    public Card jokerCard;
    public Nationality optionNationality; // The nationality of this

    public void SetUp(Card card, Nationality nationality)
    {
        jokerCard = card;
        optionNationality = nationality;
        GetComponent<Image>().sprite = GameObject.Find("DeckManager").GetComponent<Deck>().GetSpriteForCountry(nationality, card.cardData.gender);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        jokerCard.JokerIdentity = optionNationality;
        jokerCard.Player.SetPlayerBlockedByJokerIdentitySelection(false);
        Destroy(transform.parent.gameObject);
    }
}
