using UnityEngine;

[System.Serializable]
public class Card: MonoBehaviour
{
    public string country;
    public string gender; 
    public Sprite sprite;

    public Card(string country, string gender, Sprite sprite)
    {
        this.country = country;
        this.gender = gender;
        this.sprite = sprite;
    }
}
