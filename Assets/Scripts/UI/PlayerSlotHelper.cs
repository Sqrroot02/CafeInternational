using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Models;
using UnityEngine.UI;
using Player = Assets.Scripts.Models.Player;

/// <summary>
/// Unterstützt die Darstellung und Interaktion eines einzelnen Spieler-Slots im Lobby-Menü.
/// Handhabt Zuweisung, Bot-Status, Namen, Avatare und Interaktivität.
/// </summary>
public class PlayerSlotHelper : MonoBehaviour
{
    public TMP_InputField nameText;
    public Button actionButton;
    public TMP_Dropdown botStrengthDropdown;
    public LobbyPanelManager lobbyPanelManager;
    public GameObject placeHolder;
    public SceneMessageHandler sceneMessageHandler;
    public Image playerPicture;

    private Player assignedPlayer;

    /// <summary>
    /// Initialisiert diesen Slot mit einem gegebenen Spielerobjekt.
    /// </summary>
    public void SetUp(Player player)
    {
        Debug.Log($"[PlayerSlotHelper] SetUp: Initializing slot for player '{player.PlayerName}'");
        assignedPlayer = player;

        // Lade das Spielerbild
        Sprite sprite = Resources.Load<Sprite>(player.PlayerSpritePath);
        if (sprite == null)
        {
            Debug.LogWarning($"[PlayerSlotHelper] SetUp: Could not load sprite at path '{player.PlayerSpritePath}'");
        }
        playerPicture.sprite = sprite;

        // Zeige Spielernamen im Eingabefeld an
        nameText.text = player.PlayerName;
        Debug.Log($"[PlayerSlotHelper] SetUp: Assigned name '{assignedPlayer.PlayerName}'");

        // Wenn Spieler kein Host ist, Buttons und Dropdowns entsprechend setzen
        if (!assignedPlayer.LobbyHost && !LobbyStorage.Instance.IsMuliplayerLobby && botStrengthDropdown != null && actionButton != null)
        {
            string label = player.IsBot ? "Add Player" : "Add Bot";
            actionButton.GetComponentInChildren<TMP_Text>().text = label;
            Debug.Log($"[PlayerSlotHelper] SetUp: Action button set to '{label}'");

            botStrengthDropdown.gameObject.SetActive(player.IsBot);
            placeHolder.SetActive(!player.IsBot);
            Debug.Log($"[PlayerSlotHelper] SetUp: Bot dropdown active = {player.IsBot}, Placeholder active = {!player.IsBot}");
        }
    }

    /// <summary>
    /// Setzt den Bot-Stärkedropdown auf den Standardwert zurück.
    /// </summary>
    public void InitMultiplayerLobby()
    {
        Debug.Log("[PlayerSlotHelper] Init Multiplayer Lobby.");
        DeactivateInteractives();
        if (actionButton != null) actionButton.gameObject.SetActive(false);
        if (placeHolder != null) placeHolder.gameObject.SetActive(true);
        
    }

    /// <summary>
    /// Setzt den Bot-Stärkedropdown auf den Standardwert zurück.
    /// </summary>
    public void ResetBotStrengthDropDown()
    {
        Debug.Log("[PlayerSlotHelper] ResetBotStrengthDropDown: Resetting dropdown to default value.");
        botStrengthDropdown.value = 0;
        botStrengthDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Bot-Stärke geändert wurde.
    /// </summary>
    public void OnBotStrengthChanged()
    {
        Debug.Log("[PlayerSlotHelper] OnBotStrengthChanged: Called.");

        if (assignedPlayer != null && assignedPlayer.IsBot)
        {
            string selected = botStrengthDropdown.options[botStrengthDropdown.value].text;
            Debug.Log($"[PlayerSlotHelper] OnBotStrengthChanged: Selected value '{selected}'");

            // Weise neuen Bottyp zu
            assignedPlayer.BotType = (botStrengthDropdown.value == 0) ? BotType.IsWeakBot :
                                     (botStrengthDropdown.value == 1) ? BotType.IsMischiefBot :
                                     BotType.IsScoringBot;

            Debug.Log($"[PlayerSlotHelper] OnBotStrengthChanged: Bot type set to {assignedPlayer.BotType}");
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn der Add Player / Add Bot Button gedrückt wird.
    /// </summary>
    public void OnActionButtonClicked()
    {
        Debug.Log("[PlayerSlotHelper] OnActionButtonClicked: Called.");

        if (assignedPlayer != null)
        {
            // Botstatus toggeln
            assignedPlayer.IsBot = !assignedPlayer.IsBot;
            Debug.Log($"[PlayerSlotHelper] OnActionButtonClicked: Bot status toggled to {assignedPlayer.IsBot}");

            // Neuen Namen generieren
            string playerName = MainMenuHelper.GenerateName(assignedPlayer.IsBot);
            assignedPlayer.PlayerName = playerName;
            Debug.Log($"[PlayerSlotHelper] OnActionButtonClicked: New name assigned: '{assignedPlayer.PlayerName}'");

            // Neues Bild zuweisen
            MainMenuHelper.AssignPlayerSprite(assignedPlayer, LobbyStorage.Instance.ActivePlayers.IndexOf(assignedPlayer));
            playerPicture.sprite = Resources.Load<Sprite>(assignedPlayer.PlayerSpritePath);

            // Slot erneut setzen
            SetUp(assignedPlayer);
        }
        else
        {
            Debug.LogWarning("[PlayerSlotHelper] OnActionButtonClicked: No player assigned.");
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn der Name im Eingabefeld geändert wird.
    /// </summary>
    public void OnNameChanged()
    {
        Debug.Log("[PlayerSlotHelper] OnNameChanged: Called.");

        if (assignedPlayer != null)
        {
            string newName = nameText.text;
            Debug.Log($"[PlayerSlotHelper] OnNameChanged: Input received: '{newName}'");

            if (MainMenuHelper.IsValidNicknameOrLobbyName(newName))
            {
                if (assignedPlayer.IsBot)
                {
                    newName = "Bot " + newName;
                }

                assignedPlayer.PlayerName = newName;
                Debug.Log($"[PlayerSlotHelper] OnNameChanged: Valid name assigned: '{assignedPlayer.PlayerName}'");

                MainMenuHelper.AssignPlayerSprite(assignedPlayer, LobbyStorage.Instance.ActivePlayers.IndexOf(assignedPlayer));
                playerPicture.sprite = Resources.Load<Sprite>(assignedPlayer.PlayerSpritePath);

                Debug.Log($"[PlayerSlotHelper] OnNameChanged: Updated sprite and name to '{assignedPlayer.PlayerName}'");
            }
            else
            {
                Debug.LogWarning($"[PlayerSlotHelper] OnNameChanged: Invalid name entered: '{newName}'");
                sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
            }

            // Aktuellen Namen (ggf. korrigiert) zurück ins UI setzen
            nameText.text = assignedPlayer.PlayerName;
        }
        else
        {
            Debug.LogWarning("[PlayerSlotHelper] OnNameChanged: No player assigned.");
        }
    }

    /// <summary>
    /// Macht alle UI-Elemente dieses Slots nicht interaktiv.
    /// </summary>
    public void DeactivateInteractives()
    {
        Debug.Log("[PlayerSlotHelper] DeactivateInteractives: Disabling inputs");
        if (nameText != null) nameText.interactable = false;
        if (botStrengthDropdown != null) botStrengthDropdown.interactable = false;
        if (actionButton != null) actionButton.interactable = false;
    }

    /// <summary>
    /// Macht alle UI-Elemente dieses Slots wieder interaktiv.
    /// </summary>
    public void ActivateInteractives()
    {
        Debug.Log("[PlayerSlotHelper] ActivateInteractives: Enabling inputs");
        if (nameText != null) nameText.interactable = true;
        if (botStrengthDropdown != null) botStrengthDropdown.interactable = true;
        if (actionButton != null) actionButton.interactable = true;
    }
}
