using System;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Adapter;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject createLobbyPanel;

    public GameObject mainMenuContainer;

    public GameObject lobbyPanel;

    public GameObject joinLobbyPanel;

    public GameObject rulesPanel;

    public JoinLobbyPanelManager joinLobbyPanelManager;

    public CreateLobbyPanelManager createLobbyPanelManager;

    public LobbyPanelManager lobbyPanelManager;

    public SceneMessageHandler sceneMessageHandler;


    void Awake()
    {
        NetworkClientAdapter.Instance.Disconnected += InstanceOnDisconnected;
    }

    private void InstanceOnDisconnected(object sender, EventArgs e) => ShowMainMenu();

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit game"); 
    }

    public void ShowCreateLobbyPanel()
    {
        createLobbyPanel.SetActive(true);
        mainMenuContainer.SetActive(false);
        lobbyPanel.SetActive(false);
        joinLobbyPanel.SetActive(false);
        Debug.Log("Show Create Lobby Panel");
    }

    /// <summary>
    /// Disconnects from the current lobby
    /// </summary>
    public void DisconnectLobby()
    {
        Debug.Log("Disconnect Lobby");
        NetworkClientAdapter.Instance.Disconnect();
        if (NetworkServerAdapter.Instance.Server.IsRunning)
            NetworkServerAdapter.Instance.TearDown();
    }
    
    public void ShowMainMenu()
    {
        rulesPanel.SetActive(false);
        createLobbyPanel.SetActive(false);
        mainMenuContainer.SetActive(true);
        lobbyPanel.SetActive(false);
        joinLobbyPanel.SetActive(false);
        joinLobbyPanelManager.ResetJoinLobbyTMPs();
        createLobbyPanelManager.ResetCreateLobbyPanel();
        lobbyPanelManager.ResetLobbyTMPs();
        Debug.Log("Show Main Menu");

    }

    public void ShowLobby()
    {
        createLobbyPanel.SetActive(false);
        mainMenuContainer.SetActive(false);
        lobbyPanel.SetActive(true);
        joinLobbyPanel.SetActive(false);
        lobbyPanelManager.InitiateLobby();
        Debug.Log("Show Lobby Panel");
    }

    public void ShowJoinLobbyPanel()
    {
        createLobbyPanel.SetActive(false);
        mainMenuContainer.SetActive(false);
        lobbyPanel.SetActive(false);
        joinLobbyPanel.SetActive(true);
        Debug.Log("Show Join Lobby Panel");
    }

    public void ShowRulesPanel()
    {
        createLobbyPanel.SetActive(false);
        mainMenuContainer.SetActive(false);
        lobbyPanel.SetActive(false);
        joinLobbyPanel.SetActive(false);
        rulesPanel.SetActive(true);
        Debug.Log("Show Rules Panel");
    }

    public void SetCardPathOnButtonClick()
    {
        string cardPath = LobbyStorage.Instance.CardPath;

        if (cardPath == "Normal/")
        {
            cardPath = "StickmanCards/";
        }
        else if (cardPath == "StickmanCards/")
        {
            cardPath = "Normal/";
        }

        LobbyStorage.Instance.CardPath = cardPath;

        Debug.Log("Card Path: " + LobbyStorage.Instance.CardPath);

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
