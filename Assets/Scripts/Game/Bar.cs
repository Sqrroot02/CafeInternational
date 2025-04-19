using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public List<Transform> cardSlots; // List with all cards
    public GameObject cardPrefab;
    private int nextSlotIntex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeSlots();
    }

    /// <summary>
    /// Fills the slots list with all bar slots 
    /// </summary>
    void InitializeSlots()
    {
        cardSlots = new List<Transform>();
        foreach (Transform row in transform)
        {
            if (row.CompareTag("Row"))
            {
                foreach (Transform slot in row)
                {
                    slot.gameObject.GetComponent<BarStool>().SetIndex(cardSlots.Count);
                    cardSlots.Add(slot);
                }
            }
        }
    }

/// <summary>
/// Checks if the stool is the next in line and if so increases the nextSlotIndex
/// </summary>
/// <param name="stoolIndex">The index of the stool that is checked</param>
/// <returns>True if the stool is the next in line</returns>
    public void AddCard()
    {
        nextSlotIntex++;
    }

    public bool CheckAddCard(int stoolIndex)
    {
        if (stoolIndex == nextSlotIntex)
        {
            return true;
        }

        return false;
    }
}
