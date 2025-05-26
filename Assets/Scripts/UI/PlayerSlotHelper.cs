using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Models;
using UnityEngine.UI;
using Player = Assets.Scripts.Models.Player;

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

    public void SetUp(Player player)
    {
        Debug.Log($"[PlayerSlotHelper] SetUp: Initializing slot for player '{player.PlayerName}'");
        assignedPlayer = player;

        Sprite sprite = Resources.Load<Sprite>(player.PlayerSpritePath);
        if (sprite == null)
        {
            Debug.LogWarning($"[PlayerSlotHelper] SetUp: Could not load sprite at path '{player.PlayerSpritePath}'");
        }
        playerPicture.sprite = sprite;

        nameText.text = player.PlayerName;
        Debug.Log($"[PlayerSlotHelper] SetUp: Assigned name '{assignedPlayer.PlayerName}'");

        if (!assignedPlayer.LobbyHost)
        {
            actionButton.GetComponentInChildren<TMP_Text>().text = player.IsBot ? "Add Player" : "Add Bot";
        Debug.Log($"[PlayerSlotHelper] SetUp: Action button set to '{actionButton.GetComponentInChildren<TMP_Text>().text}'");
            botStrengthDropdown.gameObject.SetActive(player.IsBot);
            placeHolder.SetActive(!player.IsBot);
        Debug.Log($"[PlayerSlotHelper] SetUp: Bot dropdown active = {player.IsBot}, Placeholder active = {!player.IsBot}");
        }
    }

    public void ResetBotStrengthDropDown()
    {
        Debug.Log("[PlayerSlotHelper] ResetBotStrengthDropDown: Resetting dropdown to default value.");
        botStrengthDropdown.value = 0;
        botStrengthDropdown.RefreshShownValue();
    }

    public void OnBotStrengthChanged()
    {
        Debug.Log("[PlayerSlotHelper] OnBotStrengthChanged: Called.");

        if (assignedPlayer != null && assignedPlayer.IsBot)
        {
            string selected = botStrengthDropdown.options[botStrengthDropdown.value].text;
            assignedPlayer.BotType = (botStrengthDropdown.value == 0) ? BotType.IsWeakBot : (botStrengthDropdown.value == 1) ? BotType.IsMischiefBot : BotType.IsScoringBot;
        }
    }

    public void OnActionButtonClicked()
    {
        Debug.Log("[PlayerSlotHelper] OnActionButtonClicked: Called.");

        if (assignedPlayer != null)
        {
            assignedPlayer.IsBot = !assignedPlayer.IsBot;
            Debug.Log($"[PlayerSlotHelper] OnActionButtonClicked: Bot status toggled to {assignedPlayer.IsBot}");

            string playerName = MainMenuHelper.GenerateName(assignedPlayer.IsBot);
            assignedPlayer.PlayerName = playerName;
            Debug.Log($"[PlayerSlotHelper] OnActionButtonClicked: New name assigned: '{assignedPlayer.PlayerName}'");

            MainMenuHelper.AssignPlayerSprite(assignedPlayer, LobbyStorage.Instance.ActivePlayers.IndexOf(assignedPlayer));
            playerPicture.sprite = Resources.Load<Sprite>(assignedPlayer.PlayerSpritePath);

            SetUp(assignedPlayer);
        }
        else
        {
            Debug.LogWarning("[PlayerSlotHelper] OnActionButtonClicked: No player assigned.");
        }
    }

    public void OnNameChanged()
    {
        Debug.Log("[PlayerSlotHelper] OnNameChanged: Called.");

        if (assignedPlayer != null)
        {
            string newName = nameText.text;

            if (MainMenuHelper.IsValidNicknameOrLobbyName(newName))
            {
                if (assignedPlayer.IsBot)
                {
                    newName = "Bot " + newName;
                }

                Debug.Log($"[PlayerSlotHelper] OnNameChanged: Valid name entered: '{newName}'");
                assignedPlayer.PlayerName = newName;

                MainMenuHelper.AssignPlayerSprite(assignedPlayer, LobbyStorage.Instance.ActivePlayers.IndexOf(assignedPlayer));
                playerPicture.sprite = Resources.Load<Sprite>(assignedPlayer.PlayerSpritePath);

                Debug.Log($"[PlayerSlotHelper] OnNameChanged: Updated sprite and name to '{assignedPlayer.PlayerName}'");
            }
            else
            {
                Debug.LogWarning($"[PlayerSlotHelper] OnNameChanged: Invalid name entered: '{newName}'");
                sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
            }

            // Update field visually to current name (in case of rejection)
            nameText.text = assignedPlayer.PlayerName;
        }
        else
        {
            Debug.LogWarning("[PlayerSlotHelper] OnNameChanged: No player assigned.");
        }
    }

    public void DeactivateInteractives()
    {
        nameText.interactable = false;
        botStrengthDropdown.interactable = false;
        actionButton.interactable = false;
    }

    public void ActivateInteractives()
    {
        nameText.interactable = true;
        botStrengthDropdown.interactable = true;
        actionButton.interactable = true;
    }
    
}
