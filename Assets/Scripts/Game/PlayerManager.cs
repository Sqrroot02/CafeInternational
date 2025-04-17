using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    List<Player> players = new List<Player>();
    public Player CurrentPlayer { get; set; }
    public int CurrentPlayerIndex { get; set; }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShufflePlayers();
        CurrentPlayerIndex = 0;
        CurrentPlayer = players[CurrentPlayerIndex];
    }

    private void ShufflePlayers()
    {
        Stack<Player> playersStack = new Stack<Player>(Players.ActivePlayers);
        players.Add(playersStack.Pop());

        while (playersStack.Count > 0)
        {
            players.Insert(Random.Range(0, players.Count + 1), playersStack.Pop());
        }
    }

    public void UpdatePlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % players.Count;
        CurrentPlayer = players[CurrentPlayerIndex];
    }
}
