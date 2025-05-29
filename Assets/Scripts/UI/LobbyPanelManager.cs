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

/// <summary>
/// Verwalter-Logik für das LobbyPanel. Zuständig für das Setzen von UI-Elementen,
/// Synchronisation mit dem Lobby-Zustand und das Starten des Spiels.
/// </summary>
public class LobbyPanelManager : MonoBehaviour
{
    // Referenzen zu den UI-Feldern
    public TMP_Text lobbyNameTMP;
    public TMP_Text lobbyPortTMP;
    public TMP_Text lobbyIPTMP;

    // Liste der Spieler-Slots (UI-Elemente)
    public List<PlayerSlotHelper> playerSlots;

    void Awake()
    {
        Debug.Log("[LobbyPanelManager] Awake: Assigning self to PlayerLobbyActionHandler");
        // Registrierung bei PlayerLobbyActionHandler
        PlayerLobbyActionHandler.Manager = this;
    }

    /// <summary>
    /// Initialisiert das Lobby-UI beim Betreten der Lobby.
    /// </summary>
    public void InitiateLobby()
    {
        Debug.Log("[LobbyPanelManager] InitiateLobby: Initializing lobby display");

        LobbyStorage.Instance.LobbyIp = NetworkUtil.PublicIpAddress();

        SetLobbyIPLabel($"Lobby Ip: {LobbyStorage.Instance.LobbyIp}");
        SetLobbyNameLabel("Lobbyname: " + LobbyStorage.Instance.LobbyName);
        SetLobbyPortLabel("Lobbyport: " + LobbyStorage.Instance.LobbyPort);

        SetPlayerNames();
        if(LobbyStorage.Instance.IsMuliplayerLobby) initMultiplayerLobby();
    }

    private void initMultiplayerLobby()
    {
        Debug.Log("[LobbyPanelManager] InitiateMultiplayerLobby");
        lobbyIPTMP.gameObject.SetActive(true);
        lobbyPortTMP.gameObject.SetActive(true);

        foreach (var slot in playerSlots) {
            slot.InitMultiplayerLobby();
        }

        if (LobbyStorage.Instance.ActivePlayers.Any(p => p.PlayerId == LobbyStorage.Instance.ClientPlayerId && p.LobbyHost))
        {
            foreach (var slot in playerSlots)
            {
                slot.ActivateInteractives();
            }
        }
    }

    /// <summary>
    /// Setzt UI-Elemente zurück, wenn Lobby verlassen wird.
    /// </summary>
    public void ResetLobbyTMPs()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyTMPs: Resetting all lobby UI fields");

        ResetLobbyIPLabel();
        ResetLobbyNameLabel();
        ResetLobbyPortLabel();

        for (int i = 1; i < playerSlots.Count; i++)
        {
            var slot = playerSlots[i];
            if (slot.botStrengthDropdown != null)
            {
                Debug.Log($"[LobbyPanelManager] ResetLobbyTMPs: Resetting bot strength dropdown for slot {i}");
                slot.ResetBotStrengthDropDown();
            }

            if (slot.actionButton != null) slot.actionButton.gameObject.SetActive(true);

            Debug.Log($"[LobbyPanelManager] ResetLobbyTMPs: Reactivating slot {i}");
            slot.ActivateInteractives();
        }
    }

    /// <summary>
    /// Startet das Spiel und sendet die Spiel-Startnachricht an den Server.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("[LobbyPanelManager] StartGame: Preparing and sending start game message");

        LobbyStorage.Instance.ShufflePlayers();
        Debug.Log("[LobbyPanelManager] StartGame: Players shuffled");

        var message = new StartGameMessage
        {
            LobbyName = LobbyStorage.Instance.LobbyName,
            Players = LobbyStorage.Instance.ActivePlayers.ToArray(),
            Starter = LobbyStorage.Instance.ActivePlayers[0]
        };

        Debug.Log($"[LobbyPanelManager] StartGame: Sending StartGameMessage for lobby '{message.LobbyName}' with {message.Players.Length} players.");
        NetworkRouter.SendToServer(message, MessageType.StartGame);
    }

    /// <summary>
    /// Wird vom Netzwerk-Handler aufgerufen, um das UI mit neuen Lobbydaten zu aktualisieren.
    /// </summary>
    public void LobbyUpdate(PlayerLobbyActionMessage message)
    {
        Debug.Log("[LobbyPanelManager] LobbyUpdate: Received new lobby state from server");

        // Update player list
        LobbyStorage.Instance.ActivePlayers = message.Players.ToList();
        Debug.Log($"[LobbyPanelManager] LobbyUpdate: Updated active players list, count = {message.Players.Length}");

        SetPlayerNames();

        // Update lobby metadata
        LobbyStorage.Instance.LobbyName = message.LobbyName;
        LobbyStorage.Instance.LobbyPort = message.LobbyPort;
        LobbyStorage.Instance.LobbyIp = message.LobbyIp;

        Debug.Log($"[LobbyPanelManager] LobbyUpdate: LobbyName='{message.LobbyName}', Port='{message.LobbyPort}', IP='{message.LobbyIp}'");

        RefreshLobbyNameLabel();
        RefreshLobbyPortLabel();
        RefreshLobbyIpLabel(); 
    }

    /// <summary>
    /// Weist den PlayerSlots die Namen und Daten der Spieler zu.
    /// </summary>
    public void SetPlayerNames()
    {
        LobbyStorage.Instance.SetPlayerSprites();

        Debug.Log("[LobbyPanelManager] SetPlayerNames: Updating UI with player data");

        var players = LobbyStorage.Instance.ActivePlayers;
        int limit = Mathf.Min(players.Count, playerSlots.Count);
        Debug.Log($"[LobbyPanelManager] SetPlayerNames: Mapping {limit} players to UI slots");

        for (int i = 0; i < limit; i++)
        {
            if (players[i] != null)
            {
                Debug.Log($"[LobbyPanelManager] SetPlayerNames: Setting up slot {i} for player '{players[i].PlayerName}'");
                playerSlots[i].SetUp(players[i]);
            }
            else
            {
                Debug.LogWarning($"[LobbyPanelManager] SetPlayerNames: Player at index {i} is null");
            }
        }
    }

    // ------------------------------
    // UI Label Helper
    // ------------------------------

    public void SetLobbyNameLabel(string text)
    {
        Debug.Log($"[LobbyPanelManager] SetLobbyNameLabel: '{text}'");
        MainMenuHelper.SetLabelText(lobbyNameTMP, text);
    }

    public void SetLobbyPortLabel(string text)
    {
        Debug.Log($"[LobbyPanelManager] SetLobbyPortLabel: '{text}'");
        MainMenuHelper.SetLabelText(lobbyPortTMP, text);
    }

    public void SetLobbyIPLabel(string text)
    {
        Debug.Log($"[LobbyPanelManager] SetLobbyIPLabel: '{text}'");
        MainMenuHelper.SetLabelText(lobbyIPTMP, text);
    }

    public void RefreshLobbyNameLabel()
    {
        string label = $"Lobby: {LobbyStorage.Instance.LobbyName}";
        Debug.Log($"[LobbyPanelManager] RefreshLobbyNameLabel: '{label}'");
        MainMenuHelper.SetLabelText(lobbyNameTMP, label);
    }

    public void RefreshLobbyPortLabel()
    {
        string label = $"Port: {LobbyStorage.Instance.LobbyPort}";
        Debug.Log($"[LobbyPanelManager] RefreshLobbyPortLabel: '{label}'");
        MainMenuHelper.SetLabelText(lobbyPortTMP, label);
    }

    public void RefreshLobbyIpLabel()
    {
        string label = $"IP: {LobbyStorage.Instance.LobbyIp}";
        Debug.Log($"[LobbyPanelManager] RefreshLobbyIpLabel: '{label}'");
        MainMenuHelper.SetLabelText(lobbyIPTMP, label);
    }

    public string GetLobbyNameLabel() => MainMenuHelper.GetLabelText(lobbyNameTMP);
    public string GetLobbyPortLabel() => MainMenuHelper.GetLabelText(lobbyPortTMP);
    public string GetLobbyIPLabel() => MainMenuHelper.GetLabelText(lobbyIPTMP);

    public void ResetLobbyNameLabel()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyNameLabel: Resetting lobby name field");
        MainMenuHelper.ResetLabelText(lobbyNameTMP);
    }

    public void ResetLobbyPortLabel()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyPortLabel: Resetting lobby port field");
        MainMenuHelper.ResetLabelText(lobbyPortTMP);
        lobbyPortTMP.gameObject.SetActive(false);
    }

    public void ResetLobbyIPLabel()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyIPLabel: Resetting lobby IP field");
        MainMenuHelper.ResetLabelText(lobbyIPTMP);
        lobbyIPTMP.gameObject.SetActive(false);
    }
}
