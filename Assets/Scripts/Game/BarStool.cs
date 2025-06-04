using Assets.Scripts.Game;
using Assets.Scripts.Models;
using Riptide;
using UnityEngine;
using UnityEngine.UI;

public class BarStool : MonoBehaviour, IMessageSerializable
{
    public int Value; // The value of the chair that is added to the points of the one placing a card here // Negative scores are also added and not implemented separately
    public Image stoolSprite; // The image on the stool // the number of points added
    private int _index = 0;
    public Card PlacedCard;
    
    private SceneMessageHandler _sceneMessageHandler;

    // Awake is called once even before Start
    private void Awake()
    {
        stoolSprite = GetComponent<Image>();
        _sceneMessageHandler = GameObject.Find("Overlay").GetComponentInChildren<SceneMessageHandler>(true);

    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public int GetIndex()
    {
        return _index; 
    }

    public bool PlaceAndCommit(Card card)
    {
        var hasPlaced = PlaceCard(card, true);
        if (hasPlaced)
            TurnHistory.CurrentTurnHistory.AddBarStool(this);
        return hasPlaced;
    }
    
    /// <summary>
    /// Placed the given Card on the stool if possible
    /// </summary>
    public bool PlaceCard(Card card, bool displayErrorMessages = false)
    {
        if (card.cardData.nationality != Nationality.Joker && !card.GetIsPlaced())
        {
            if (transform.parent.transform.parent.gameObject.GetComponent<Bar>().CheckAddCard(_index))
            {
                if (card.UpdateIsPlaced(2))
                {
                    transform.parent.transform.parent.gameObject.GetComponent<Bar>().AddCard();
                    GetComponent<Outline>().UpdateOutlineSprite(card.cardData.cardSprite);
                    card.Player.BarStool = this;
                    PlacedCard = card;
                    PlaySound.Instance.PlaySoundPlaceCard();

                    return true;
                }
                if (displayErrorMessages)
                    _sceneMessageHandler.ShowScene("Can´t play card because to many cards have already been played this turn");
            }
            else if (displayErrorMessages)
                _sceneMessageHandler.ShowScene("Can´t play card because this barstool is not the next in line");
        }
        else if (displayErrorMessages)
            _sceneMessageHandler.ShowScene("Can´t play card because jokers can´t be placed at the bar");
            
        return false;
    }

    public void Serialize(Message message)
    {
        message.AddInt(Value);
        message.AddInt(_index);
        message.AddSerializable(PlacedCard);
    }

    public void Deserialize(Message message)
    {
        Value = message.GetInt();
        _index = message.GetInt();
        PlacedCard = message.GetSerializable<Card>();
    }
}
