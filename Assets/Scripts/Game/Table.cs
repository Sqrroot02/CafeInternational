using UnityEngine;
using UnityEngine.UI;
using static Nationality;

public class Table : MonoBehaviour
{
    public Nationality Nationality;
    private Image tableImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tableImage = GetComponent<Image>();
        if (Nationality != Nationality.None)
        {
            string spriteName = "Flags/" + Nationality.ToString() + " Flag";
            tableImage.sprite = Resources.Load<Sprite>(spriteName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
