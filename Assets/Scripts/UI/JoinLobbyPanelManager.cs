using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        int intLobbyPort = MainMenuHelper.GetNumberFromLobbyPortTMP(lobbyPortTMP);
        string nickname = lobbyUserNickname.text;

        if (intLobbyPort == 0) { 
            intLobbyPort = lobbyPortTMPStandardValue;
        }

        if (!MainMenuHelper.IsValidNicknameOrLobbyName(nickname))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
        } else if (!MainMenuHelper.ValidateIp(enteredIp))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetIPErrorMsg());
        } else if (!MainMenuHelper.IsValidUserPort(intLobbyPort))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetPortErrorMsg());
        } else {
            Debug.Log("JoinLobby TMP input is valid");
            LobbyStorage.Instance.InitializeLobby("DummyHost", "DummyLobby", enteredIp, intLobbyPort);
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
