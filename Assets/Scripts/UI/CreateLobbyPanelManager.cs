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

/// <summary>
/// Verantwortlich für die Erstellung einer neuen Lobby.
/// Verwaltet Eingaben, Validierung, Lobby-Initialisierung, Server-Start und UI-Umschaltung.
/// </summary>
public class CreateLobbyPanelManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public TMP_InputField lobbyNameInput;
    public TMP_InputField nicknameInputField;

    public SceneMessageHandler sceneMessageHandler;
    public TMP_Dropdown lobbyTypeDropdown;

    public Button createLobbyButton;

    private string nickNamePlacerholderValue = "Enter Nickname...";
    private string lobbyNamePlaceholderValue = "Enter Lobby Name...";

    /// <summary>
    /// Initialisiert Platzhaltertexte und die Button-Aktivierung abhängig von Eingaben.
    /// </summary>
    void Start()
    {
        SetLobbyNamePlaceholder(lobbyNamePlaceholderValue);
        SetNicknamePlaceholder(nickNamePlacerholderValue);

        MainMenuHelper.SetupButtonActivationValidation(createLobbyButton, nicknameInputField, lobbyNameInput);
    }

    /// <summary>
    /// Führt die Erstellung der Lobby durch – inklusive Validierung, Speichern in LobbyStorage und Serverstart.
    /// </summary>
    public void CreateLobby()
    {
        var enteredLobbyName = lobbyNameInput.text;
        var enteredNickname = nicknameInputField.text;

        // Validierung der Eingaben
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
            Debug.Log($"[CreateLobbyPanelManager] Creating lobby '{enteredLobbyName}' for host '{enteredNickname}'");

            // Lobby initialisieren: true = Multiplayer
            LobbyStorage.Instance.InitializeLobby(
                enteredNickname,
                enteredLobbyName,
                lobbyTypeDropdown.value != 0 // 0 = lokal, 1 = online/multiplayer
            );

            InitAndRunServerSession();
        }
    }

    /// <summary>
    /// Initialisiert das Session-Objekt, startet den Server, wartet auf die Verbindung und verbindet den lokalen Client.
    /// </summary>
    private void InitAndRunServerSession()
    {
        Debug.Log("[CreateLobbyPanelManager] InitAndRunServerSession: Initializing new game session...");

        var session = new Session(lobbyNameInput.text, new ObservableCollection<Player>(LobbyStorage.Instance.ActivePlayers));

        Debug.Log("[CreateLobbyPanelManager] Starting server...");
        NetworkServerAdapter.Instance.RunServer(session);

        // Warten bis der Server bereit ist oder ESC gedrückt wird
        while (!NetworkServerAdapter.Instance.Server.IsRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.LogWarning("[CreateLobbyPanelManager] Server initialization cancelled by host.");
                return;
            }
        }

        // Verbindung des lokalen Clients mit dem eigenen Server
        Debug.Log("[CreateLobbyPanelManager] Connecting local client to server...");
        NetworkClientAdapter.Instance.Port = Convert.ToUInt16(LobbyStorage.Instance.LobbyPort);
        NetworkClientAdapter.Instance.IpAddress = "127.0.0.1";
        NetworkClientAdapter.Instance.Connected += AfterClientConnect;
        NetworkClientAdapter.Instance.Connect();
    }

    /// <summary>
    /// Wird aufgerufen, wenn der lokale Client erfolgreich mit dem Server verbunden wurde.
    /// </summary>
    private void AfterClientConnect(object sender, EventArgs e)
    {
        NetworkClientAdapter.Instance.Connected -= AfterClientConnect;

        Debug.Log($"[CreateLobbyPanelManager] Local client connected. Client ID: {NetworkClientAdapter.Instance.Client.Id}");

        Debug.Log("[CreateLobbyPanelManager] Current session players:");
        foreach (var player in NetworkServerAdapter.Instance.Session?.Players ?? Enumerable.Empty<Player>())
        {
            Debug.Log($" - {player.PlayerName} [{player.PlayerId}]");
        }

        mainMenuManager.ShowLobby();
    }

    /// <summary>
    /// Setzt alle Eingabefelder und die Dropdown-Auswahl im UI zurück.
    /// </summary>
    public void ResetCreateLobbyPanel()
    {
        Debug.Log("[CreateLobbyPanelManager] ResetCreateLobbyPanel: Resetting all UI fields.");
        ResetLobbyNameText();
        ResetNicknameText();
        ResetLobbyTypeDropDown();
    }

    // --- Placeholder setzen ---
    public void SetLobbyNamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyNameInput, text);
    public void SetNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(nicknameInputField, text);

    // --- Texte abrufen ---
    public string GetLobbyNameText() => MainMenuHelper.GetInputText(lobbyNameInput);
    public string GetNicknameText() => MainMenuHelper.GetInputText(nicknameInputField);

    // --- Texte zurücksetzen ---
    public void ResetLobbyNameText() => MainMenuHelper.ResetInputText(lobbyNameInput);
    public void ResetNicknameText() => MainMenuHelper.ResetInputText(nicknameInputField);
    public void ResetLobbyTypeDropDown() => lobbyTypeDropdown.value = 0;
}
