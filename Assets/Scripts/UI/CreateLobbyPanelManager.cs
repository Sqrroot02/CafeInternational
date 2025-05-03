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

    /// <summary>
    /// Creates a new lobby using the name entered the input field.
    /// </summary>
    /// <remarks>
    /// This method retrieves the text entered in the <see cref="TMP_InputField"/> associated with the create lobby panel
    /// and assigns it as the session name. The name is also stored in the main menu manager for further reference.
    /// Debug messages are logged to confirm the entered lobby name.
    /// </remarks>
    public void CreateLobby()
    {
        var enteredName = lobbyNameInput.text;
        
        NetworkServerAdapter.Instance.Session.Name = enteredName;
        MainMenuManager.Instance.lobbyName = enteredName;
        Debug.Log($"Saved Lobbyname: {enteredName}");
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
