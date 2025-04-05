using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Models.Nationality;

public class Table : MonoBehaviour
{
    public Nationality nationality;
    private Image _tableImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tableImage = GetComponent<Image>();
        string spriteName = "Flags/" + nationality.ToString() + " Flag";
        _tableImage.sprite = Resources.Load<Sprite>(spriteName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
