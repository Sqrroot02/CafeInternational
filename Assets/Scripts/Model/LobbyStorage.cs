using UnityEngine;
using Assets.Scripts.Models;
using System.Collections.Generic;

public class LobbyStorage : MonoBehaviour
{
    public static LobbyStorage Instance { get; private set; }

    private string globalLobbyName;

    private int globalLobbyPort;

    private string globalLobbyIp;

    public List<Player> ActivePlayers { get; private set; } = new();

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
        GlobalLobbyName = lobbyName;

        ActivePlayers.Clear();
        ActivePlayers.Add(new Player(localPlayerName, 0, true));

        for (int i = 1; i < 4; i++)
        {
            string botName = MainMenuHelper.GenerateName();
            ActivePlayers.Add(new Player($"Bot {botName}", 0, false));
        }
    }

    public void ReplaceBotWithHuman(string playerName)
    {
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            if (!ActivePlayers[i].IsHuman)
            {
                ActivePlayers[i] = new Player(playerName, 0, true);
                return;
            }
        }
    }

    public void ReplacePlayerWithBot(string playerName)
    {
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            if (ActivePlayers[i].PlayerName == playerName && ActivePlayers[i].IsHuman)
            {
                string botName = MainMenuHelper.GenerateName();
                ActivePlayers[i] = new Player($"Bot {botName}", 0, false);
                Debug.Log($"{playerName} has been replaced with a bot");
                return;
            }
        }

        Debug.Log($"No Player with naem:  {playerName} found.");
    }

    public string GlobalLobbyName
    {
        get => globalLobbyName;
        set => globalLobbyName = value;
    }

    public int GlobalLobbyPort
    {
        get => globalLobbyPort;
        set => globalLobbyPort = value;
    }

    public string GlobalLobbyIp
    {
        get => globalLobbyIp;
        set => globalLobbyIp = value;
    }
}
