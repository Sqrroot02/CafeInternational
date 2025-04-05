using System;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card: MonoBehaviour
{
    public CardData cardData;
    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;
        GetComponent<Image>().sprite = cardData.cardSprite;
    }
}
