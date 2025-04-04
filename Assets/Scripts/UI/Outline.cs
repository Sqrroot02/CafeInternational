using UnityEngine;
using UnityEngine.EventSystems;

public class Outline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject outline;

    public void Awake()
    {
        outline = this.gameObject.transform.GetChild(0).gameObject;
        if (outline != null)
        {
            outline.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.SetActive(false);
        }
    }
}
