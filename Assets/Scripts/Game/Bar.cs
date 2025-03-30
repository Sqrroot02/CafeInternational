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
                    cardSlots.Add(slot);
                }
            }
        }
    }

    public void AddCard(GameObject card)
    {
        nextSlotIntex++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
