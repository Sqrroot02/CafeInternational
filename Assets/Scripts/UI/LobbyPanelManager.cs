using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.PlayerLobbyAction;
using Assets.Scripts.Network.Messages.StartGame;
using Assets.Scripts.UI;

public class LobbyPanelManager : MonoBehaviour
{
    public TMP_Text lobbyNameTMP;
    public TMP_Text lobbyPortTMP;
    public TMP_Text lobbyIPTMP;
    public List<PlayerSlotHelper> playerSlots;

    void Awake()
    {
        PlayerLobbyActionHandler.Manager = this;
    }
    
    public void InitiateLobby()
    {
        Debug.Log("[LobbyPanelManager] InitiateLobby called.");
        SetLobbyIPLabel($"Lobby Ip: {LobbyStorage.Instance.LobbyIp}");
        SetLobbyNameLabel("Lobbyname: " + LobbyStorage.Instance.LobbyName);
        SetLobbyPortLabel("Lobbyport: " + LobbyStorage.Instance.LobbyPort);
        SetPlayerSlot();
    }

    public void ResetLobbyTMPs()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyTMPs called.");
        ResetLobbyIPLabel();
        ResetLobbyNameLabel();
        ResetLobbyPortLabel();
    }
    
    /// <summary>
    /// Starts the Game
    /// </summary>
    public void StartGame()
    {
        // Shuffle players sequence before start
        LobbyStorage.Instance.ShufflePlayers();
        
        // Build and send Message for initializing game 
        var message = new StartGameMessage
        {
            LobbyName = LobbyStorage.Instance.LobbyName,
            Players = LobbyStorage.Instance.ActivePlayers.ToArray(),
            Starter = LobbyStorage.Instance.ActivePlayers[0]
        };
        NetworkRouter.SendToServer(message, MessageType.StartGame);
    }

    
    public void SetPlayerSlot()
    {
        Debug.Log("[LobbyPanelManager] SetPlayerSlot called.");
        LobbyStorage.Instance.ReplaceBotWithHuman(MainMenuHelper.GenerateName(false));
    }

    /// <summary>
    /// Will be invoked if a new user joins the session
    /// </summary>
    /// <param name="message"></param>
    public void LobbyUpdate(PlayerLobbyActionMessage message)
    {
        // Update player names
        LobbyStorage.Instance.ActivePlayers = message.Players.ToList();
        SetPlayerNames();
        
        // Update lobby name
        LobbyStorage.Instance.LobbyName = message.LobbyName;
        RefreshLobbyNameLabel();
        
        // Update Lobby port
        LobbyStorage.Instance.LobbyPort = message.LobbyPort;
        RefreshLobbyPortLabel();
        
        // Update Lobby IP
        LobbyStorage.Instance.LobbyIp = message.LobbyIp;
        RefreshLobbyIpLabel();
    }

    public void SetPlayerNames()
    {
        var players = LobbyStorage.Instance.ActivePlayers;

        for (int i = 0; i < Mathf.Min(players.Count, playerSlots.Count); i++)
        {
            playerSlots[i].SetUp(players[i]);
        }
    }

    public void SetLobbyNameLabel(string text) => MainMenuHelper.SetLabelText(lobbyNameTMP, text);
    public void SetLobbyPortLabel(string text) => MainMenuHelper.SetLabelText(lobbyPortTMP, text);
    public void SetLobbyIPLabel(string text) => MainMenuHelper.SetLabelText(lobbyIPTMP, text);

    public void RefreshLobbyNameLabel() => MainMenuHelper.SetLabelText(lobbyNameTMP, $"Lobby: {LobbyStorage.Instance.LobbyName}");
    public void RefreshLobbyPortLabel() => MainMenuHelper.SetLabelText(lobbyPortTMP, $"Port: {LobbyStorage.Instance.LobbyPort}");
    public void RefreshLobbyIpLabel() => MainMenuHelper.SetLabelText(lobbyIPTMP, $"IP: {LobbyStorage.Instance.LobbyIp}");
    
    public string GetLobbyNameLabel() => MainMenuHelper.GetLabelText(lobbyNameTMP);
    public string GetLobbyPortLabel() => MainMenuHelper.GetLabelText(lobbyPortTMP);
    public string GetLobbyIPLabel() => MainMenuHelper.GetLabelText(lobbyIPTMP);

    public void ResetLobbyNameLabel() => MainMenuHelper.ResetLabelText(lobbyNameTMP);
    public void ResetLobbyPortLabel() => MainMenuHelper.ResetLabelText(lobbyPortTMP);
    public void ResetLobbyIPLabel() => MainMenuHelper.ResetLabelText(lobbyIPTMP);
}
