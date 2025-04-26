using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Card: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardData cardData;
    
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private Transform _originalParent;
    private bool _isPlaced = false;
    public Player Player { get; set; }
    private PlayerManager _playerManager;

    public Transform PlayerBarSlot { get; set; }

    public Nationality JokerIdentity; // Only set if card is a joker and is placed on a Chair that is at two tables

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        PlayerBarSlot = transform.parent;
    }
    
    /// <summary>
    /// Updates the _isPlaced value and increments the cardcount for the current turn. Checks if the card is placeable -> No/One Card on a chair and none on a barstool
    /// </summary>
    /// <param name="increment">Increments by one when placed on a chair and by two for a barstool</param>
    public bool UpdateIsPlaced(int increment)
    {
        if (_playerManager.CountCardsPlayed + increment > 2)
        {
            return false;
        } 
        _isPlaced = true;
        _playerManager.IncrementCountCardsPlayed(increment);
        return true;
    }

    public bool GetIsPlaced()
    {
        return _isPlaced;
    }

    public PlayerManager GetPlayerManager()
    {
        return _playerManager;
    }
    
    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;
        GetComponent<Image>().sprite = cardData.cardSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isPlaced && _playerManager.CurrentPlayer == Player) {
            _originalParent = transform.parent;
            transform.SetParent(_canvas.transform);
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f; // Transparent while dragging
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isPlaced && _playerManager.CurrentPlayer == Player)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isPlaced && _playerManager.CurrentPlayer == Player)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;

            if (transform.parent == _canvas.transform)
            {
                transform.SetParent(_originalParent);
                _rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
    
    
    /// <summary>
    /// Reverts the position of the card back to the original spot in the playerbar
    /// </summary>
    public void ResetCardPosition()
    {
        _isPlaced = false;
        _canvasGroup.blocksRaycasts = true;
        transform.SetParent(PlayerBarSlot);
        _rectTransform.anchorMin = Vector2.zero;
        _rectTransform.anchorMax = Vector2.one;
        _rectTransform.sizeDelta = Vector2.zero;
        _rectTransform.anchoredPosition = Vector2.zero;
        _rectTransform.localScale = new Vector3(0.85f, 0.85f, 1);
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
    }
}
