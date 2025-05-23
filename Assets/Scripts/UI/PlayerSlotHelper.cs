using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Models;
using UnityEngine.UI;

public class PlayerSlotHelper : MonoBehaviour
{
    public TMP_InputField nameText;
    public Button actionButton;
    public TMP_Dropdown botStrengthDropdown;
    public LobbyPanelManager lobbyPanelManager;
    public GameObject placeHolder;
    public SceneMessageHandler sceneMessageHandler;

    private Player assignedPlayer;

    public void SetUp(Player player)
    {
        Debug.Log($"[PlayerSlotHelper] SetUp called for player: {player.PlayerName}");
        assignedPlayer = player;
        Debug.Log("Assgined Playername:" + assignedPlayer.PlayerName);

        nameText.text = player.PlayerName;
        if (!assignedPlayer.LobbyHost)
        {
            actionButton.GetComponentInChildren<TMP_Text>().text = player.IsBot ? "Add Player" : "Add Bot";
            Debug.Log("Player Bot: " + player.IsBot);
            botStrengthDropdown.gameObject.SetActive(player.IsBot);
            placeHolder.SetActive(!player.IsBot);
        }
        
    }

    public void ResetBotStrengthDropDown()
    {
        Debug.Log("Reset Bot Strength");
        botStrengthDropdown.value = 0;
        botStrengthDropdown.RefreshShownValue();
    }

    public void OnBotStrengthChanged()
    {
        Debug.Log($"[PlayerSlotHelper] OnBotStrengthChanged called.");

        if (assignedPlayer != null && assignedPlayer.IsBot)
        {
            string selected = botStrengthDropdown.options[botStrengthDropdown.value].text;
            assignedPlayer.IsStrongBot = selected == "Strong";
            Debug.Log($"[PlayerSlotHelper] Bot strength set to: {assignedPlayer.IsStrongBot}");
        }
    }

    public void OnActionButtonClicked()
    {
        Debug.Log("[PlayerSlotHelper] OnActionButtonClicked called.");

        if (assignedPlayer.IsBot) {
            Debug.Log("Player ist set to non bot.");
            assignedPlayer.IsBot = false;
        } else {
            assignedPlayer.IsBot = true;
        }

        if (assignedPlayer != null)
        {
            Debug.Log(assignedPlayer.IsBot);
            string namePrefix = assignedPlayer.IsBot ? "Bot " : "";
            
            string playerName = assignedPlayer.IsBot ? MainMenuHelper.GenerateName(true) : MainMenuHelper.GenerateName(false);
            assignedPlayer.PlayerName = playerName;
            Debug.Log($"[PlayerSlotHelper] Player name set to: {assignedPlayer.PlayerName}");
        }

        SetUp(assignedPlayer);
    }

    public void OnNameChanged()
    {
        Debug.Log("[PlayerSlotHelper] OnNameChanged called.");

        if (assignedPlayer != null)
        {
            string newName = nameText.text;

            if (MainMenuHelper.IsValidNicknameOrLobbyName(newName)) {
                if (assignedPlayer.IsBot)
                {
                    newName = "Bot " + newName;
                }
                
                Debug.Log($"[PlayerSlotHelper] Name changed to: {assignedPlayer.PlayerName}");
                assignedPlayer.PlayerName = newName;
            }
            else
            {
                sceneMessageHandler.ShowScene(MainMenuHelper.CreateNicknameLobbyErrorMsg("Nickname"));
            }
        }

        nameText.text = assignedPlayer.PlayerName;
    }
}