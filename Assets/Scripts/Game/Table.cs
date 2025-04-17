using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Models.Nationality;

public class Table : MonoBehaviour
{
    public Nationality nationality;
    private Image _tableImage;
    public List<CardData> placedCards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placedCards = new List<CardData>();
        _tableImage = GetComponent<Image>();
        string spriteName = "Flags/" + nationality.ToString() + " Flag";
        _tableImage.sprite = Resources.Load<Sprite>(spriteName);
    }

    public int AddPlacedCard(CardData card)
    {
        placedCards.Add(card);
        return GetTablePoints();
    }

    private int GetTablePoints()
    {
        bool nationalityMatchesTable = true;
        foreach (var card in placedCards)
        {
            if (card.nationality != nationality)
            {
                nationalityMatchesTable = false;
                break;
            } 
        }

        if (placedCards.Count == 4 && nationalityMatchesTable)
        {
            return 8;
        }
        return placedCards.Count;
    }

    public bool CheckGenderPlaceable(Gender gender)
    {
        if (placedCards.Count == 0) // If no one sits at the table the gender does not 
        {
            return true;
        }
        int countMale = 0, countFemale = 0;
        foreach (CardData card in placedCards)
        {
            if (card.gender == Gender.Male)
            {
                countMale++;
            }
            else
            {
                countFemale++;
            }
        }

        if (countMale == countFemale) // If one male and one female sit at the table either gender is accepted
        {
            return true;
        }
        
        // If 1x male and 0x female or 2x male and 1x female or 1x female and 0x male or 2x female and 1x male
        if ((countMale > countFemale && gender == Gender.Male) || (countFemale > countMale && gender == Gender.Female))
        {
            Debug.Log("Gender missmatch with (male, female, gender) " + countMale + " " + countFemale + " " + gender);
            return false;
        }
        return true; // If gender is the gender with the lower count
    }
}
