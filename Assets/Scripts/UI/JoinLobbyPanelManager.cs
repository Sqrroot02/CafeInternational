using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;


public class JoinLobbyPanelManager : MonoBehaviour
{
    public TMP_InputField lobbyIPTMP;

    public TMP_InputField lobbyPortTMP;

    public TMP_InputField lobbyUserNickname;

    public Button joinLobbyButton;

    private string lobbyIpTMPStandardValue = "Enter Lobby IP...";

    private string lobbyPortTMPStandardValue = "57967";

    private string lobbyUserNicknameValue = "Enter Nickname...";

    void Start()
    {
        SetLobbyIPPlaceholder();
        SetLobbyPortPlaceholder();
        SetLobbyUserNicknamePlaceholder();

        lobbyIPTMP.onValueChanged.AddListener(LobbyIPInput_TMP_ValueChanged);
        DisableCreateLobbyButton();
    }

    void LobbyIPInput_TMP_ValueChanged(string newValue)
    {
        if (newValue != lobbyIpTMPStandardValue && !string.IsNullOrEmpty(newValue) && ValidateIp(newValue))
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

    public void ResetJoinLobbyTMPs()
    {
        Debug.Log("Reset Join Lobby Panel");
        SetLobbyIPPlaceholder();
        SetLobbyPortPlaceholder();
        SetLobbyUserNicknamePlaceholder();
        DisableCreateLobbyButton();
    }

    private void DisableCreateLobbyButton()
    {
        joinLobbyButton.interactable = false;
    }

    private bool ValidateIp(string input)
    {
        if (IPAddress.TryParse(input, out IPAddress address))
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                Debug.Log("Valid ipv4 as join lobby input");
                return true;
            }
            else if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                Debug.Log("Valid ipv6 as join lobby input");
                return true;
            }
        }

        Debug.LogWarning("Invalid ip join lobby input.");
        return false;
    }


    private void SetLobbyIPPlaceholder()
    {
        if (lobbyIPTMP.placeholder is TextMeshProUGUI placeholder)
            placeholder.text = lobbyIpTMPStandardValue;
    }

    private void SetLobbyPortPlaceholder()
    {
        if (lobbyPortTMP.placeholder is TextMeshProUGUI placeholder)
            placeholder.text = lobbyPortTMPStandardValue;
    }

    private void SetLobbyUserNicknamePlaceholder()
    {
        if (lobbyUserNickname.placeholder is TextMeshProUGUI placeholder)
            placeholder.text = lobbyUserNicknameValue;
    }
}
