using UnityEngine;
using UnityEngine.EventSystems;

public class DropCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null && Place(droppedObject.GetComponent<Card>()))
        {
            RectTransform droppedRect = droppedObject.GetComponent<RectTransform>();
            droppedRect.SetParent(transform, false);

            droppedRect.anchoredPosition = Vector2.zero;
            
            droppedRect.anchorMin = new Vector2(0.5f, 0.5f);
            droppedRect.anchorMax = new Vector2(0.5f, 0.5f);
            droppedRect.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    private bool Place(Card card)
    {
        // Returns true if the card was placed otherwise false
        if (CompareTag("Chair"))
        {
            return PlaceChair(card);
        }
        else if (CompareTag("BarStool"))
        {
            return CheckPlaceableBarStool(card);
        }
        return false;
    }

    private bool PlaceChair(Card card)
    { 
        return GetComponent<Chair>().PlaceCard(card);
    }

    private bool CheckPlaceableBarStool(Card card)
    {
        return true;
    }
}
