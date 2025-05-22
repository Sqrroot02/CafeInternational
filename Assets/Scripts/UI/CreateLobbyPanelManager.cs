using Assets.Scripts.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyPanelManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public TMP_InputField lobbyNameInput;

    public TMP_InputField nicknameInputField;

    public TMP_InputField ipInputField;

    public TMP_InputField portInputField;

    public SceneMessageHandler sceneMessageHandler;

    private string nickNamePlacerholderValue = "Enter Nickname...";

    private string lobbyNamePlaceholderValue = "Enter Lobby Name...";

    private string ipPlaceHolderValue = "Enter IP...";

    private string portPlayholderValue = "Enter Port...";

    public Button createLobbyButton;

    void Start()
    {
        SetLobbyNamePlaceholder(lobbyNamePlaceholderValue);
        SetNicknamePlaceholder(nickNamePlacerholderValue);
        SetIPPlaceholder(ipPlaceHolderValue);
        SetPortPlaceholder(portPlayholderValue);  

        MainMenuHelper.SetupButtonActivationValidation(createLobbyButton, nicknameInputField, lobbyNameInput);
    }

    public void CreateLobby()
    {
        string enteredLobbyName = lobbyNameInput.text;
        string enteredNickname = nicknameInputField.text;
        string enteredIp = ipInputField.text;
        int intLobbyPort = MainMenuHelper.GetNumberFromLobbyPortTMP(portInputField);

        string invalid = null;


        if (!MainMenuHelper.IsValidNicknameOrLobbyName(enteredNickname))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
        } else if (!MainMenuHelper.IsValidNicknameOrLobbyName(enteredLobbyName))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Lobbyname"));
        } else if (!MainMenuHelper.ValidateIp(enteredIp))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetIPErrorMsg());
        } else if (!MainMenuHelper.IsValidUserPort(intLobbyPort))
        {
            sceneMessageHandler.ShowScene(MainMenuHelper.GetPortErrorMsg());
        } else
        {
            LobbyStorage.Instance.InitializeLobby(enteredNickname, enteredLobbyName, enteredIp, intLobbyPort);

            mainMenuManager.ShowLobby();
        }

    }

    public void ResetCreateLobbyPanel()
    {
        Debug.Log("Reset Create Lobby Panel");
        ResetLobbyNameText();
        ResetNicknameText();
        ResetIPText();
        ResetPortText();
    }

    public void SetLobbyNamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyNameInput, text);
    public void SetNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(nicknameInputField, text);
    public void SetIPPlaceholder(string text) => MainMenuHelper.SetPlaceholder(ipInputField, text);
    public void SetPortPlaceholder(string text) => MainMenuHelper.SetPlaceholder(portInputField, text);

    public string GetLobbyNameText() => MainMenuHelper.GetInputText(lobbyNameInput);
    public string GetNicknameText() => MainMenuHelper.GetInputText(nicknameInputField);
    public string GetIPText() => MainMenuHelper.GetInputText(ipInputField);
    public string GetPortText() => MainMenuHelper.GetInputText(portInputField);

    public void ResetLobbyNameText() => MainMenuHelper.ResetInputText(lobbyNameInput);
    public void ResetNicknameText() => MainMenuHelper.ResetInputText(nicknameInputField);
    public void ResetIPText() => MainMenuHelper.ResetInputText(ipInputField);
    public void ResetPortText() => MainMenuHelper.ResetInputText(portInputField);



}
