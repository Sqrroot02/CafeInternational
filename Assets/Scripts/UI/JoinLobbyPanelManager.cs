using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.UI;


public class JoinLobbyPanelManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public TMP_InputField lobbyIPTMP;

    public TMP_InputField lobbyPortTMP;

    public TMP_InputField lobbyUserNickname;

    public Button joinLobbyButton;

    public SceneMessageHandler sceneMessageHandler;

    private string lobbyIpTMPStandardValue = "Enter Lobby IP...";

    private int lobbyPortTMPStandardValue = 57967;

    private string lobbyUserNicknameValue = "Enter Nickname...";

    void Start()
    {
        SetLobbyIPPlaceholder(lobbyIpTMPStandardValue);
        SetLobbyPortPlaceholder(lobbyPortTMPStandardValue.ToString());
        SetLobbyUserNicknamePlaceholder(lobbyUserNicknameValue);

        MainMenuHelper.SetupButtonActivationValidation(joinLobbyButton, lobbyIPTMP, lobbyUserNickname);
        DisableCreateLobbyButton();
    }


    public void JoinLobby()
    {
        string enteredIp = lobbyIPTMP.text;
        int intLobbyPort = GetNumberFromLobbyPortTMP();
        string nickname = lobbyUserNickname.text;

        if (intLobbyPort == 0) { 
            intLobbyPort = lobbyPortTMPStandardValue;
        }

        if (!ValidateIp(enteredIp))
        {
            sceneMessageHandler.ShowScene("An semantic invalid IP has beend entered.");
        } else if (!MainMenuHelper.IsValidNicknameOrLobbyName(nickname))
        {
            sceneMessageHandler.ShowScene("An invalid Nickname has been entered. Try to use a Nickname that has at least 1 and maximum 10 characters and only contains letters.");
        } else if (!MainMenuHelper.IsValidUserPort(intLobbyPort))
        {
            sceneMessageHandler.ShowScene("An invalid Port has beend entered. Enter a Port between 1024 and 65535.");
        } else {
            Debug.Log("JoinLobby TMP input is valid");
            LobbyStorage.Instance.InitializeLobby("Lobby Host", "lobby name");
            mainMenuManager.ShowLobby();
        }
    }

    public void ResetJoinLobbyTMPs()
    {
        Debug.Log("Reset Join Lobby Panel");
        ResetLobbyUserNicknameText();
        ResetLobbyIPText();
        ResetLobbyPortText();
        DisableCreateLobbyButton();
    }

    private void DisableCreateLobbyButton()
    {
        joinLobbyButton.interactable = false;
    }

    private bool ValidateIp(string input)
    {
        if (input.Length < 7)
        {
            Debug.LogWarning("Invalid ip join lobby input: Too short.");
            return false;
        }

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

    private int GetNumberFromLobbyPortTMP()
    {
        if (int.TryParse(lobbyPortTMP.text, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning("Invalid Port Input");
            return 0;
        }
    }

    public void SetLobbyIPPlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyIPTMP, text);
    public void SetLobbyPortPlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyPortTMP, text);
    public void SetLobbyUserNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyUserNickname, text);

    public string GetLobbyIPText() => MainMenuHelper.GetInputText(lobbyIPTMP);
    public string GetLobbyPortText() => MainMenuHelper.GetInputText(lobbyPortTMP);
    public string GetLobbyUserNicknameText() => MainMenuHelper.GetInputText(lobbyUserNickname);

    public void ResetLobbyIPText() => MainMenuHelper.ResetInputText(lobbyIPTMP);
    public void ResetLobbyPortText() => MainMenuHelper.ResetInputText(lobbyPortTMP);
    public void ResetLobbyUserNicknameText() => MainMenuHelper.ResetInputText(lobbyUserNickname);
}
