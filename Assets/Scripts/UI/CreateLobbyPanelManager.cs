using System;
using System.Collections.ObjectModel;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Adapter;
using Assets.Scripts.Network.Models;
using Assets.Scripts.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyPanelManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public TMP_InputField lobbyNameInput;

    public TMP_InputField nicknameInputField;

    public SceneMessageHandler sceneMessageHandler;

    public TMP_Dropdown lobbyTypeDropdown;

    private string nickNamePlacerholderValue = "Enter Nickname...";

    private string lobbyNamePlaceholderValue = "Enter Lobby Name...";


    public Button createLobbyButton;

    void Start()
    {
        SetLobbyNamePlaceholder(lobbyNamePlaceholderValue);
        SetNicknamePlaceholder(nickNamePlacerholderValue);

        MainMenuHelper.SetupButtonActivationValidation(createLobbyButton, nicknameInputField, lobbyNameInput);
    }

    public void CreateLobby()
    {
        var enteredLobbyName = lobbyNameInput.text;
        var enteredNickname = nicknameInputField.text;

        if (!MainMenuHelper.IsValidNicknameOrLobbyName(enteredNickname))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
        }
        else if (!MainMenuHelper.IsValidNicknameOrLobbyName(enteredLobbyName))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Lobbyname"));
        }
        else
        {
            LobbyStorage.Instance.InitializeLobby(enteredNickname, enteredLobbyName, lobbyTypeDropdown.value == 0 ? false: true);
            InitAndRunServerSession();
        }
    }

    /// <summary>
    /// Initializes the server session and runs the server
    /// </summary>
    private void InitAndRunServerSession()
    {
        // Build session
        var session = new Session(lobbyNameInput.text, new ObservableCollection<Player>(LobbyStorage.Instance.ActivePlayers));
            
        // Run Server
        
        Debug.Log("Starting Session");
        
        NetworkServerAdapter.Instance.RunServer(session);
        
        while (!NetworkServerAdapter.Instance.Server.IsRunning)
        {
            // Host pressed Escape during the Server establishment 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Server initialization cancelled!!!");
                return;
            }
        }
        
        // Connect Local client to server
        
        Debug.Log("Connecting server client to server");
        NetworkClientAdapter.Instance.Port = Convert.ToUInt16(LobbyStorage.Instance.LobbyPort);
        NetworkClientAdapter.Instance.IpAddress = "127.0.0.1";
        NetworkClientAdapter.Instance.Connected += AfterClientConnect;
        NetworkClientAdapter.Instance.Connect();
    }

    private void AfterClientConnect(object sender, EventArgs e)
    {
        NetworkClientAdapter.Instance.Connected -= AfterClientConnect;
        
        Debug.Log($"Server ClientID is: {NetworkClientAdapter.Instance.Client.Id}");
        Debug.Log($"The current Session:\n {string.Join("\n", NetworkServerAdapter.Instance.Session?.Players.Select(x => $"{x.PlayerName} [{x.PlayerId}]"))}");
        
        // Show Lobby after the Server establishment
        mainMenuManager.ShowLobby();

    }

    public void ResetCreateLobbyPanel()
    {
        Debug.Log("Reset Create Lobby Panel");
        ResetLobbyNameText();
        ResetNicknameText();
        ResetLobbyTypeDropDown();
    }

    public void SetLobbyNamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyNameInput, text);
    public void SetNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(nicknameInputField, text);

    public string GetLobbyNameText() => MainMenuHelper.GetInputText(lobbyNameInput);
    public string GetNicknameText() => MainMenuHelper.GetInputText(nicknameInputField);


    public void ResetLobbyNameText() => MainMenuHelper.ResetInputText(lobbyNameInput);
    public void ResetNicknameText() => MainMenuHelper.ResetInputText(nicknameInputField);
    public void ResetLobbyTypeDropDown() => lobbyTypeDropdown.value = 0;

}
