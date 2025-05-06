using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyPanelManager : MonoBehaviour
{
    public TMP_Text lobbyNameTMP;

    public TMP_Text lobbyPortTMP;

    public TMP_Text lobbyIPTMP;

    public void initiateLobby()
    {
        SetLobbyName();
        SetLobbyPortIPTMP();
    }

    private void SetLobbyName()
    {
        string lobbyName = MainMenuManager.Instance.lobbyName;
        Debug.Log($"Set Lobbyname to {lobbyName}" );
        lobbyNameTMP.text = "Lobby: " + lobbyName;
    }

    private void SetLobbyPortIPTMP()
    {
        string lobbyIP = MainMenuManager.Instance.lobbyIP;
        string lobbyPort = MainMenuManager.Instance.lobbyPort;

        if (!string.IsNullOrEmpty(lobbyPort))
        {
            lobbyPortTMP.text = "Port: " + lobbyPort;

        }

        Debug.Log("Set Lobby Port: " + lobbyPort + " Lobby IP: " + lobbyIP);
        lobbyIPTMP.text = "IP: " + lobbyIP;
    }

    public void ResetLobbyTMPs()
    {
        lobbyNameTMP.text = string.Empty;
        lobbyPortTMP.text = string.Empty;
        lobbyIPTMP.text = string.Empty;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
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
