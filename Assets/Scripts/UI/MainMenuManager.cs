using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    public string lobbyName;

    public string lobbyPort;

    public string lobbyIP;

    public GameObject createLobbyPanel;

    public GameObject mainMenuContainer;

    public GameObject lobbyPanel;

    public GameObject joinLobbyPanel;

    public JoinLobbyPanelManager joinLobbyPanelManager;

    public CreateLobbyPanelManager createLobbyPanelManager;

    public LobbyPanelManager lobbyPanelManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        createLobbyPanelManager.ResetLobbyName();
        lobbyPanelManager.ResetLobbyTMPs();
        joinLobbyPanelManager.ResetJoinLobbyTMDs();
        Debug.Log("Show Main Menu");

    }

    public void ShowLobby()
    {
        createLobbyPanel.SetActive(false);
        mainMenuContainer.SetActive(false);
        lobbyPanel.SetActive(true);
        joinLobbyPanel.SetActive(false);
        lobbyPanelManager.initiateLobby();
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

    private void ResetGlobalVars()
    {
        lobbyName = "";
        lobbyPort = "";
        lobbyIP = "";
    }
}
