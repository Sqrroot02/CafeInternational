using System;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Adapter;
using JetBrains.Annotations;
using Assets.Scripts.Network.Messages.PlayerLobbyAction;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Referenzen zu UI-Elementen
    [CanBeNull] public GameObject createLobbyPanel;
    [CanBeNull] public GameObject mainMenuContainer;
    [CanBeNull] public GameObject lobbyPanel;
    [CanBeNull] public GameObject joinLobbyPanel;
    [CanBeNull] public GameObject rulesPanel;

    // Referenzen zu Panel-Managern
    public JoinLobbyPanelManager joinLobbyPanelManager;
    public CreateLobbyPanelManager createLobbyPanelManager;
    public LobbyPanelManager lobbyPanelManager;

    // Nachrichtenanzeige-Handler f�r UI-Texte
    public SceneMessageHandler sceneMessageHandler;

    void Awake()
    {
        Debug.Log("[MainMenuManager] Awake - Subscribed to Disconnected event.");
        PlayerLobbyActionHandler.mainMenuManager = this;
    }

    // Event-Handler f�r Verbindungsabbruch
    private void OnDisconnected(object sender, EventArgs e)
    {
        Debug.Log("[MainMenuManager] Disconnected from server. Returning to main menu.");
        ShowMainMenu();
    }

    // Anwendung beenden
    public void QuitGame()
    {
        Debug.Log("[MainMenuManager] QuitGame - Exiting game.");
        Application.Quit();
    }

    // Erstellen-Lobby-Panel anzeigen
    public void ShowCreateLobbyPanel()
    {
        Debug.Log("[MainMenuManager] ShowCreateLobbyPanel - Displaying create lobby panel.");
        createLobbyPanel?.SetActive(true);
        mainMenuContainer?.SetActive(false);
        lobbyPanel?.SetActive(false);
        joinLobbyPanel?.SetActive(false);
    }

    /// <summary>
    /// Trennt von der aktuellen Lobby und stoppt ggf. den Server.
    /// </summary>
    public void DisconnectLobby()
    {
        Debug.Log("[MainMenuManager] DisconnectLobby - Disconnecting from lobby and shutting down server if running.");
        NetworkClientAdapter.Instance.Disconnect();

        if (NetworkServerAdapter.Instance.Server.IsRunning)
        {
            Debug.Log("[MainMenuManager] DisconnectLobby - Server is running. Tearing down.");
            NetworkServerAdapter.Instance.TearDown();
        }
    }

    // Hauptmen� anzeigen und Panels zur�cksetzen
    public void ShowMainMenu()
    {
        Debug.Log("[MainMenuManager] ShowMainMenu - Returning to main menu.");
        rulesPanel?.SetActive(false);
        createLobbyPanel?.SetActive(false);
        mainMenuContainer?.SetActive(true);
        lobbyPanel?.SetActive(false);
        joinLobbyPanel?.SetActive(false);

        joinLobbyPanelManager?.ResetJoinLobbyTMPs();
        createLobbyPanelManager?.ResetCreateLobbyPanel();
        lobbyPanelManager?.ResetLobbyTMPs();
        LobbyStorage.Instance.ResetLobbyStorage();
    }

    // Lobby-Panel anzeigen
    public void ShowLobby()
    {
        Debug.Log("[MainMenuManager] ShowLobby - Showing lobby panel.");
        createLobbyPanel?.SetActive(false);
        mainMenuContainer?.SetActive(false);
        lobbyPanel?.SetActive(true);
        joinLobbyPanel?.SetActive(false);

        lobbyPanelManager.InitiateLobby();
    }

    // Join-Lobby-Panel anzeigen
    public void ShowJoinLobbyPanel()
    {
        Debug.Log("[MainMenuManager] ShowJoinLobbyPanel - Displaying join lobby panel.");
        createLobbyPanel?.SetActive(false);
        mainMenuContainer?.SetActive(false);
        lobbyPanel?.SetActive(false);
        joinLobbyPanel?.SetActive(true);
    }

    // Regel-Panel anzeigen
    public void ShowRulesPanel()
    {
        Debug.Log("[MainMenuManager] ShowRulesPanel - Showing rules panel.");
        createLobbyPanel?.SetActive(false);
        mainMenuContainer?.SetActive(false);
        lobbyPanel?.SetActive(false);
        joinLobbyPanel?.SetActive(false);
        rulesPanel?.SetActive(true);
    }

    // Wechselt zwischen verschiedenen Karten-Designs
    public void SetCardPathOnButtonClick()
    {
        string cardPath = LobbyStorage.Instance.CardPath;

        Debug.Log("[MainMenuManager] SetCardPathOnButtonClick - Current card path: " + cardPath);

        if (cardPath == "Normal/")
        {
            cardPath = "StickmanCards/";
            Debug.Log("[MainMenuManager] SetCardPathOnButtonClick - Switching to StickmanCards.");
        }
        else if (cardPath == "StickmanCards/")
        {
            cardPath = "Normal/";
            Debug.Log("[MainMenuManager] SetCardPathOnButtonClick - Switching to Normal cards.");
        }

        LobbyStorage.Instance.CardPath = cardPath;
        Debug.Log("[MainMenuManager] SetCardPathOnButtonClick - New card path: " + LobbyStorage.Instance.CardPath);

        if (cardPath == "Normal/")
        {
            sceneMessageHandler.ShowScene("The DLC has been deactivated :)");
        }
        else
        {
            sceneMessageHandler.ShowScene("The DLC has been activated :)");
        }
    }
}
