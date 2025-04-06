using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Outline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _outline;
    public GameObject outlinePrefab;

    public void Awake()
    {
        _outline = Instantiate(outlinePrefab, transform);
        _outline.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        _outline.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_outline != null)
        {
            _outline.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_outline != null)
        {
            _outline.SetActive(false);
        }
    }
}
