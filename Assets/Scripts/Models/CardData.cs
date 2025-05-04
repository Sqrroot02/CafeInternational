using System;
using Assets.Scripts.Models;
using Riptide;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject, IMessageSerializable
{
    public Gender gender;
    public Nationality nationality;
    public Sprite cardSprite;

    public void Serialize(Message message)
    {
        message.AddString(gender.ToString());
        message.AddString(nationality.ToString());
    }

    public void Deserialize(Message message)
    {
        gender = Enum.Parse<Gender>(message.GetString());
        nationality = Enum.Parse<Nationality>(message.GetString());
    }
}
