using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public List<Transform> cardSlots; // List with all cards
    public GameObject cardPrefab;
    private int nextSlotIntex = 0;
    private PlayerManager _playerManager;
    public List<BarStool> BarStools = new ();

    private void Awake()
    {
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        foreach (var row in GameObject.FindGameObjectsWithTag("Row"))
        {
            BarStools.AddRange(row.GetComponentsInChildren<BarStool>().ToList());
        }
    }
    
    void Start()
    {
        InitializeSlots();
        BarStools = BarStools.OrderBy(barStool => barStool.GetIndex()).ToList();
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
        // If all 20 barslots are taken the game ends
        if (nextSlotIntex == 20)
        {
            _playerManager.GameEnded();
        }
    }

    public bool CheckAddCard(int stoolIndex)
    {
        if (stoolIndex == nextSlotIntex)
        {
            return true;
        }

        return false;
    }

    public int GetNextIndex()
    {
        return nextSlotIntex;
    }
}
