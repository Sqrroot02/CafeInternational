using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.UI;

public class LobbyPanelManager : MonoBehaviour
{
    public TMP_Text lobbyNameTMP;

    public TMP_Text lobbyPortTMP;

    public TMP_Text lobbyIPTMP;

    public List<TMP_Text> playerNameTexts;

    public void InitiateLobby()
    {
        SetLobbyIPLabel("Lobby Ip: 1.1.1.1");
        SetLobbyNameLabel("Lobbyname: " + LobbyStorage.Instance.GlobalLobbyName);
        SetLobbyPortLabel("Lobbyport: " + LobbyStorage.Instance.GlobalLobbyPort.ToString());
        SetPlayerNames();
    }

    public void ResetLobbyTMPs()
    {
        ResetLobbyIPLabel();
        ResetLobbyNameLabel();
        ResetLobbyPortLabel();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void AddLocalPlayer()
    {
        LobbyStorage.Instance.ReplaceBotWithHuman(MainMenuHelper.GenerateName());
    }

    public void SetPlayerNames()
    {
        var players = LobbyStorage.Instance.ActivePlayers;

        for (int i = 0; i < Mathf.Min(players.Count, playerNameTexts.Count); i++)
        {
            playerNameTexts[i].text = players[i].PlayerName;
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
