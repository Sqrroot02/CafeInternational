using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyPanelManager : MonoBehaviour
{
    public TMP_InputField lobbyNameInput;

    public Button createLobbyButton;

    void Start()
    {
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

    public void ResetLobbyName()
    {
        Debug.Log("Reset Lobbyname");
        lobbyNameInput.text = "Enter Lobbyname...";
        DisableCreateLobbyButton();
    }

    private void DisableCreateLobbyButton()
    {
        createLobbyButton.interactable = false;
        Debug.Log("Disable Create Lobby Button");
    }
}
