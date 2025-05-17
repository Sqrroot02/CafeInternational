using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.PlayerSalutation;
using Assets.Scripts.UI;


public class JoinLobbyPanelManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public TMP_InputField lobbyIPTMP;

    public TMP_InputField lobbyPortTMP;

    public TMP_InputField lobbyUserNickname;

    public Button joinLobbyButton;

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
        var enteredIp = lobbyIPTMP.text;
        var intLobbyPort = GetNumberFromLobbyPortTMP();
        var nickname = lobbyUserNickname.text;

        if (intLobbyPort == 0) { 
            intLobbyPort = lobbyPortTMPStandardValue;
        }

        if (ValidateIp(enteredIp) && MainMenuHelper.IsValidNicknameOrLobbyName(nickname) && MainMenuHelper.IsValidUserPort(intLobbyPort)) {
            Debug.Log("JoinLobby TMP input is valid");
            //LobbyStorage.Instance.InitializeLobby("Lobby Host", "lobby name");
            
            // Establish connection
            NetworkClientAdapter.Instance.IpAddress = enteredIp;
            NetworkClientAdapter.Instance.Port = Convert.ToUInt16(intLobbyPort);
		
            NetworkClientAdapter.Instance.Connected += OnConnected;    
            NetworkClientAdapter.Instance.Connect();
        }
    }
    
    /// <summary>
    /// Handles the event triggered when the client successfully connects to the server.
    /// Updates the lobby connection details within the `MainMenuManager`, logs the connection
    /// details, and displays the lobby menu UI.
    /// </summary>
    /// <param name="sender">The source of the event. Typically, this is the instance of the `NetworkClientAdapter` that triggered the event.</param>
    /// <param name="e">The event arguments containing details about the connection event.</param>
    private void OnConnected(object sender, EventArgs e)
    {
        var ip = NetworkClientAdapter.Instance.IpAddress;
        var port = NetworkClientAdapter.Instance.Port;
		
        // Switch to Lobby Menu when a connection has been established to the selected Game-Server
        Debug.Log($"Connection Established to Server {ip}:{port}");
        mainMenuManager.ShowLobby();
		
        // Unsubscribe on connected
        NetworkClientAdapter.Instance.Connected -= OnConnected;  
		
        // Send Salutation Message for updating Lobby on Server
        Debug.Log("Sending Salutation Message");
        var msg = new PlayerSalutationMessage
        {
            PlayerName = lobbyUserNickname.text,
            PlayerId = Guid.NewGuid().ToString(),
        };
        NetworkRouter.SendToServer(msg, MessageType.PlayerSalutation);
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

            if (address.AddressFamily == AddressFamily.InterNetworkV6)
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

        Debug.LogWarning("Invalid Port Input");
        return 0;
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
