using Assets.Scripts.Game;
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
            
            droppedRect.anchorMin = Vector2.zero;
            droppedRect.anchorMax = Vector2.one;
            droppedRect.pivot = new Vector2(0.5f, 0.5f);
            droppedRect.sizeDelta = Vector2.zero;
            droppedRect.localScale = Vector3.one;

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
        return GetComponent<Chair>().PlaceAndCommit(card);
    }

    private bool CheckPlaceableBarStool(Card card)
    {
        return GetComponent<BarStool>().PlaceAndCommit(card);
    }
}
