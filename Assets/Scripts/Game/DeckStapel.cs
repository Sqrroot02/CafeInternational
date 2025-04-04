using UnityEngine;
using UnityEngine.EventSystems;

public class DeckStapel : MonoBehaviour, IPointerClickHandler
{

    public Deck deck;

    public void OnPointerClick(PointerEventData eventData) 
    {
        Debug.Log("Onclicked");
        deck.DrawCard();
    }
}
