using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class JoinLobbyPanelManager : MonoBehaviour
{
    public TMP_InputField lobbyIPTMP;

    public TMP_InputField lobbyPortTMP;

    public Button joinLobbyButton;

    private string lobbyIpTMPStandardValue = "Enter Lobby IP...";

    private string lobbyPortTMPStandardValue = "57967";

    void Start()
    {
        lobbyIPTMP.onValueChanged.AddListener(LobbyNameInput_TMP_ValueChanged);
        DisableCreateLobbyButton();
    }

    void LobbyNameInput_TMP_ValueChanged(string newValue)
    {
        if (newValue != lobbyIpTMPStandardValue && !string.IsNullOrEmpty(newValue))
        {
            joinLobbyButton.interactable = true;
        }
    }

    public void JoinLobby()
    {
        string enteredIp = lobbyIPTMP.text;
        string enteredPort = lobbyPortTMP.text;

        Debug.Log("Entered ip: " + enteredIp);

        if (enteredPort != lobbyPortTMPStandardValue)
        {
            Debug.Log("Entered port: " + enteredPort);
        }

        MainMenuManager.Instance.lobbyPort = enteredPort;
        MainMenuManager.Instance.lobbyIP = enteredIp;
        MainMenuManager.Instance.lobbyName = "Dummy Lobby Name from Join";
    }

    public void ResetJoinLobbyTMDs()
    {
        Debug.Log("Reset Join Lobby Panel");
        lobbyIPTMP.text = lobbyIpTMPStandardValue;
        lobbyPortTMP.text = lobbyPortTMPStandardValue;
        DisableCreateLobbyButton();
    }

    private void DisableCreateLobbyButton()
    {
        joinLobbyButton.interactable = false;
    }
}
