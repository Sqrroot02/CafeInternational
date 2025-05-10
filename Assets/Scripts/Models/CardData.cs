using Assets.Scripts.Models;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public Gender gender;
    public Nationality nationality;
    public Sprite cardSprite;
    public int cardID;
}
