using UnityEngine;
using Assets.Scripts.Models;
using System.Collections.Generic;
using Assets.Scripts.UI;

public class LobbyStorage : MonoBehaviour
{
    public static LobbyStorage Instance { get; private set; }

    private string globalLobbyName;
    private int globalLobbyPort = 57967;
    private string globalLobbyIp;
    private string cardPath = "Normal/";

    public List<Player> ActivePlayers { get; private set; } = new();

    void Awake()
    {
        Debug.Log("[LobbyStorage] Awake called.");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeLobby(string localPlayerName, string lobbyName, string lobbyIp, int lobbyPort)
    {
        Debug.Log($"[LobbyStorage] InitializeLobby called with localPlayerName: {localPlayerName}, lobbyName: {lobbyName}");

        GlobalLobbyName = lobbyName;
        GlobalLobbyIp = lobbyIp;
        GlobalLobbyPort = lobbyPort;

        ActivePlayers.Clear();
        ActivePlayers.Add(new Player(localPlayerName, 0, false, true, false));

        for (int i = ActivePlayers.Count; i < 4; i++)
        {
            string botName = MainMenuHelper.GenerateName(true);
            ActivePlayers.Add(new Player(botName, 0, true, false, false));
            Debug.Log("Active Players: " + ActivePlayers[i]);
        }
        
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

    public string CardPath
    {
        get => cardPath;
        set => cardPath = value;
    }
}
