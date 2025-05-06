using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyPanelManager : MonoBehaviour
{
    public TMP_InputField lobbyNameInput;

    public TMP_InputField nicknameInputField;

    private string nickNamePlacerholderValue = "Enter Nickname...";

    private string lobbyNamePlaceholderValue = "Enter Lobby Name...";

    public Button createLobbyButton;

    void Start()
    {
        SetLobbyNamePlaceholder(lobbyNamePlaceholderValue);
        SetNicknamePlaceholder(nickNamePlacerholderValue);
        lobbyNameInput.onValueChanged.AddListener(LobbyNameInput_TMP_ValueChanged);
        DisableCreateLobbyButton();
    }

    void LobbyNameInput_TMP_ValueChanged(string newValue)
    {
        if (newValue != "Enter Lobby Name...".Trim() && !string.IsNullOrEmpty(newValue))
        {
            createLobbyButton.interactable = true;
            Debug.Log("Enable Create Lobby Button");
        }
    }

    public void CreateLobby()
    {
        string enteredName = lobbyNameInput.text;

        MainMenuManager.Instance.lobbyName = enteredName;
        Debug.Log("Saved Lobbyname: " + enteredName);
    }

    public void ResetCreateLobbyPanel()
    {
        Debug.Log("Reset Create Lobby Panel");
        ResetLobbyNameText();
        ResetNicknameText();
        DisableCreateLobbyButton();
    }

    private void DisableCreateLobbyButton()
    {
        createLobbyButton.interactable = false;
        Debug.Log("Disable Create Lobby Button");
    }

    public void SetLobbyNamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(lobbyNameInput, text);
    public void SetNicknamePlaceholder(string text) => MainMenuHelper.SetPlaceholder(nicknameInputField, text);

    public string GetLobbyNameText() => MainMenuHelper.GetInputText(lobbyNameInput);
    public string GetNicknameText() => MainMenuHelper.GetInputText(nicknameInputField);

    public void ResetLobbyNameText() => MainMenuHelper.ResetInputText(lobbyNameInput);
    public void ResetNicknameText() => MainMenuHelper.ResetInputText(nicknameInputField);
}
