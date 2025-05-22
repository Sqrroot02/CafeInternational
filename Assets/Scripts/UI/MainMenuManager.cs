using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject createLobbyPanel;

    public GameObject mainMenuContainer;

    public GameObject lobbyPanel;

    public GameObject joinLobbyPanel;

    public JoinLobbyPanelManager joinLobbyPanelManager;

    public CreateLobbyPanelManager createLobbyPanelManager;

    public LobbyPanelManager lobbyPanelManager;

    public SceneMessageHandler sceneMessageHandler;

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

    public void ShowMainMenu()
    {
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
            sceneMessageHandler.ShowScene("The DLC has been deactivated. :)");
        }
        else
        {
            sceneMessageHandler.ShowScene("The DLC has been activated. :)");
        }


    }
}
