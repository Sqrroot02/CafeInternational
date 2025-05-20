using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Models;

public class LobbyPanelManager : MonoBehaviour
{
    public TMP_Text lobbyNameTMP;
    public TMP_Text lobbyPortTMP;
    public TMP_Text lobbyIPTMP;
    public List<PlayerSlotHelper> playerSlots;

    public void InitiateLobby()
    {
        Debug.Log("[LobbyPanelManager] InitiateLobby called.");
        SetLobbyIPLabel("Lobby Ip: 1.1.1.1");
        SetLobbyNameLabel("Lobbyname: " + LobbyStorage.Instance.GlobalLobbyName);
        SetLobbyPortLabel("Lobbyport: " + LobbyStorage.Instance.GlobalLobbyPort.ToString());
        SetPlayerSlot();
    }

    public void ResetLobbyTMPs()
    {
        Debug.Log("[LobbyPanelManager] ResetLobbyTMPs called.");
        ResetLobbyIPLabel();
        ResetLobbyNameLabel();
        ResetLobbyPortLabel();
    }

    public void StartGame()
    {
        Debug.Log("[LobbyPanelManager] StartGame called.");
        SceneManager.LoadScene("Game");
    }

    public void SetPlayerSlot()
    {
        Debug.Log("[LobbyPanelManager] SetPlayerSlot called.");
        var players = LobbyStorage.Instance.ActivePlayers;

        for (int i = 0; i < Mathf.Min(players.Count, playerSlots.Count); i++)
        {
            playerSlots[i].SetUp(players[i]);
        }
    }

    public void SetLobbyNameLabel(string text) => MainMenuHelper.SetLabelText(lobbyNameTMP, text);
    public void SetLobbyPortLabel(string text) => MainMenuHelper.SetLabelText(lobbyPortTMP, text);
    public void SetLobbyIPLabel(string text) => MainMenuHelper.SetLabelText(lobbyIPTMP, text);

    public string GetLobbyNameLabel() => MainMenuHelper.GetLabelText(lobbyNameTMP);
    public string GetLobbyPortLabel() => MainMenuHelper.GetLabelText(lobbyPortTMP);
    public string GetLobbyIPLabel() => MainMenuHelper.GetLabelText(lobbyIPTMP);

    public void ResetLobbyNameLabel() => MainMenuHelper.ResetLabelText(lobbyNameTMP);
    public void ResetLobbyPortLabel() => MainMenuHelper.ResetLabelText(lobbyPortTMP);
    public void ResetLobbyIPLabel() => MainMenuHelper.ResetLabelText(lobbyIPTMP);
}
