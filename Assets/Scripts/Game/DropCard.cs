using UnityEngine;
using UnityEngine.EventSystems;

public class DropCard : MonoBehaviour, IDropHandler
{
    private PlayerManager _playerManager;
    
    private void Awake()
    {
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

    }
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null && droppedObject.GetComponent<Card>().Player == _playerManager.CurrentPlayer && Place(droppedObject.GetComponent<Card>()))
        {
            CanvasGroup canvasGroup = droppedObject.GetComponent<Card>().GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            
            RectTransform droppedRect = droppedObject.GetComponent<RectTransform>(); 
            droppedRect.SetParent(transform, false);

            droppedRect.anchoredPosition = Vector2.zero;
            
            droppedRect.anchorMin = new Vector2(0.5f, 0.5f);
            droppedRect.anchorMax = new Vector2(0.5f, 0.5f);
            droppedRect.pivot = new Vector2(0.5f, 0.5f);
            droppedRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
            droppedRect.localScale = new Vector3(1, 1, 1);

        }
    }

    private bool Place(Card card)
    {
        // Returns true if the card was placed otherwise false
        if (CompareTag("Chair"))
        {
            return PlaceChair(card);
        }
        if (CompareTag("BarStool"))
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
        return GetComponent<BarStool>().PlaceCard(card);
    }
}
