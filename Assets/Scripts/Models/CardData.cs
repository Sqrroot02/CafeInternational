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
    public int cardID;
    public void Serialize(Message message)
    {
        message.AddString(gender.ToString());
        message.AddString(nationality.ToString());
        message.AddInt(cardID);
    }

    public void Deserialize(Message message)
    {
        gender = Enum.Parse<Gender>(message.GetString());
        nationality = Enum.Parse<Nationality>(message.GetString());
        cardID = message.GetInt();
    }

    public override string ToString()
    {
        return $"Id: {cardID}, Gender: {gender}, Nationality: {nationality}";
    }
}
