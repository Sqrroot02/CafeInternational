using UnityEngine;
using Assets.Scripts.Models;
using System.Collections.Generic;
using Assets.Scripts.UI;

public class LobbyStorage : MonoBehaviour
{
    public static LobbyStorage Instance { get; private set; }
    public List<Player> ActivePlayers { get; set; } = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeLobby(string localPlayerName, string lobbyName)
    {
        LobbyName = lobbyName;
        
        ActivePlayers.Clear();
        ActivePlayers.Add(new Player(localPlayerName, 0, false, true));

        for (var i = 1; i < 4; i++)
        {
            var botName = MainMenuHelper.GenerateName();
            ActivePlayers.Add(new Player($"Bot {botName}", 0, true, false));
        }
    }

    public void ReplaceBotWithHuman(string playerName)
    {
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            if (!ActivePlayers[i].IsBot)
            {
                ActivePlayers[i] = new Player(playerName, 0, false, false);
                return;
            }
        }
    }

    public void ReplacePlayerWithBot(string playerName)
    {
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            if (ActivePlayers[i].PlayerName == playerName && ActivePlayers[i].IsBot)
            {
                string botName = MainMenuHelper.GenerateName();
                ActivePlayers[i] = new Player($"Bot {botName}", 0, true, false);
                Debug.Log($"{playerName} has been replaced with a bot");
                return;
            }
        }

        Debug.Log($"No Player with naem:  {playerName} found.");
    }

    public string LobbyName { get; set; }
    public int LobbyPort { get; set; } = 57967;
    public string LobbyIp { get; set; }
}
