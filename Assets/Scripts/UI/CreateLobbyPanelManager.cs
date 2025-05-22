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
        string enteredLobbyName = lobbyNameInput.text;
        string enteredNickname = nicknameInputField.text;

        string invalid = null;

        if (!MainMenuHelper.IsValidNicknameOrLobbyName(enteredLobbyName))
        {
            invalid = "Lobbyname";
        } else if (MainMenuHelper.IsValidNicknameOrLobbyName(enteredNickname))
        {
            invalid = "Nickname";
        }

        if (invalid == null)
        {
            LobbyStorage.Instance.InitializeLobby(enteredNickname, enteredLobbyName);

            mainMenuManager.ShowLobby();
        }
        else {
            sceneMessageHandler.ShowScene($"An invalid {invalid} has been entered. Try to use a Nickname that has at least 1 and maximum 10 characters and only contains letters.");
        }

    }

    public void ResetCreateLobbyPanel()
    {
        Debug.Log("Reset Create Lobby Panel");
        ResetLobbyNameText();
        ResetNicknameText();
    }

    public void SetLobbyNamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyNameInput, text);
    public void SetNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(nicknameInputField, text);

    public string GetLobbyNameText() => MainMenuHelper.GetInputText(lobbyNameInput);
    public string GetNicknameText() => MainMenuHelper.GetInputText(nicknameInputField);

    public void ResetLobbyNameText() => MainMenuHelper.ResetInputText(lobbyNameInput);
    public void ResetNicknameText() => MainMenuHelper.ResetInputText(nicknameInputField);
}
