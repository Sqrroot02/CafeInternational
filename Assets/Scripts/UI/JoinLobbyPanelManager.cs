using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Adapter;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.PlayerSalutation;
using Assets.Scripts.UI;

/// <summary>
/// Verwaltet das Join-Lobby-Menü: Eingaben validieren, Verbindungsaufbau,
/// Kommunikation mit dem Server und UI-Verhalten.
/// </summary>
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

    /// <summary>
    /// Initialisiert Platzhaltertexte und deaktiviert Join-Button.
    /// </summary>
    void Start()
    {
        SetLobbyIPPlaceholder(lobbyIpTMPStandardValue);
        SetLobbyPortPlaceholder(lobbyPortTMPStandardValue.ToString());
        SetLobbyUserNicknamePlaceholder(lobbyUserNicknameValue);

        MainMenuHelper.SetupButtonActivationValidation(joinLobbyButton, lobbyIPTMP, lobbyUserNickname);
        DisableCreateLobbyButton();
    }

    /// <summary>
    /// Prüft Eingaben und baut Verbindung zum Server auf, wenn gültig.
    /// </summary>
    public void JoinLobby()
    {
        var enteredIp = lobbyIPTMP.text;
        var intLobbyPort = MainMenuHelper.GetNumberFromLobbyPortTMP(lobbyPortTMP);
        var nickname = lobbyUserNickname.text;

        if (intLobbyPort == 0)
        {
            intLobbyPort = lobbyPortTMPStandardValue;
        }

        // Eingabevalidierung
        if (!MainMenuHelper.IsValidNicknameOrLobbyName(nickname))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
        }
        else if (!MainMenuHelper.ValidateIp(enteredIp))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetIPErrorMsg());
        }
        else if (!MainMenuHelper.IsValidUserPort(intLobbyPort))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetPortErrorMsg());
        }
        else
        {
            Debug.Log("[JoinLobbyPanelManager] JoinLobby: Inputs are valid, attempting to connect...");

            // Verbindung vorbereiten
            NetworkClientAdapter.Instance.IpAddress = enteredIp;
            NetworkClientAdapter.Instance.Port = Convert.ToUInt16(intLobbyPort);

            NetworkClientAdapter.Instance.Connected += OnConnected;
            NetworkClientAdapter.Instance.Connect();
        }
    }

    /// <summary>
    /// Wird beim Verbindungsaufbau mit dem Server ausgelöst.
    /// Setzt Lobby-Ansicht und schickt Begrüßungsnachricht.
    /// </summary>
    private void OnConnected(object sender, EventArgs e)
    {
        var ip = NetworkClientAdapter.Instance.IpAddress;
        var port = NetworkClientAdapter.Instance.Port;

        Debug.Log($"[JoinLobbyPanelManager] OnConnected: Connection established to {ip}:{port}");

        // Ereignis abmelden
        NetworkClientAdapter.Instance.Connected -= OnConnected;

        // Begrüßungsnachricht an Server senden
        Debug.Log("[JoinLobbyPanelManager] OnConnected: Sending Salutation Message...");
        var msg = new PlayerSalutationMessage
        {
            PlayerName = lobbyUserNickname.text,
            PlayerId = Guid.NewGuid().ToString(),
        };

        NetworkRouter.SendToServer(msg, MessageType.PlayerSalutation);
        LobbyStorage.Instance.ClientPlayerId = msg.PlayerId;
    }

    /// <summary>
    /// Setzt alle Eingabefelder im Join-Menü zurück und deaktiviert den Button.
    /// </summary>
    public void ResetJoinLobbyTMPs()
    {
        Debug.Log("[JoinLobbyPanelManager] ResetJoinLobbyTMPs: Resetting all fields");
        ResetLobbyUserNicknameText();
        ResetLobbyIPText();
        ResetLobbyPortText();
        DisableCreateLobbyButton();
    }

    /// <summary>
    /// Deaktiviert den Join-Button.
    /// </summary>
    private void DisableCreateLobbyButton()
    {
        joinLobbyButton.interactable = false;
    }

    // (Veraltete/ungenutzte eigene IP-Validierung – MainMenuHelper wird genutzt)
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

    // (Veraltete/ungenutzte eigene Port-Validierung – MainMenuHelper wird genutzt)
    private int GetNumberFromLobbyPortTMP()
    {
        if (int.TryParse(lobbyPortTMP.text, out int result))
        {
            return result;
        }

        Debug.LogWarning("Invalid Port Input");
        return 0;
    }

    // --- Platzhalter setzen ---
    public void SetLobbyIPPlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyIPTMP, text);
    public void SetLobbyPortPlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyPortTMP, text);
    public void SetLobbyUserNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyUserNickname, text);

    // --- Texte abrufen ---
    public string GetLobbyIPText() => MainMenuHelper.GetInputText(lobbyIPTMP);
    public string GetLobbyPortText() => MainMenuHelper.GetInputText(lobbyPortTMP);
    public string GetLobbyUserNicknameText() => MainMenuHelper.GetInputText(lobbyUserNickname);

    // --- Texte zurücksetzen ---
    public void ResetLobbyIPText() => MainMenuHelper.ResetInputText(lobbyIPTMP);
    public void ResetLobbyPortText() => MainMenuHelper.ResetInputText(lobbyPortTMP);
    public void ResetLobbyUserNicknameText() => MainMenuHelper.ResetInputText(lobbyUserNickname);
}
