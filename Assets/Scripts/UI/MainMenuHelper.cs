using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using System.Net;
using System.Net.Sockets;

namespace Assets.Scripts.UI

{
    public static class MainMenuHelper
    {
        public static void SetPlaceholder(TMP_InputField field, string text)
        {
            if (field.placeholder is TextMeshProUGUI placeholder)
                placeholder.text = text;
        }

        public static string GetInputText(TMP_InputField field)
        {
            return field.text;
        }

        public static void ResetInputText(TMP_InputField field)
        {
            field.text = "";
        }

        public static void SetLabelText(TMP_Text field, string text)
        {
            field.text = text;
        }

        public static string GetLabelText(TMP_Text field)
        {
            return field.text;
        }

        public static int GetNumberFromLobbyPortTMP(TMP_InputField lobbyPortTMP)
        {
            if (int.TryParse(lobbyPortTMP.text, out int result))
            {
                return result;
            }
            else
            {
                Debug.LogWarning("Invalid Port Input");
                return 0;
            }
        }

        public static bool ValidateIp(string input)
        {
            if (input.Length < 7)
            {
                Debug.LogWarning("Invalid ip join lobby input: Too short.");
                return false;
            }

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log("Valid ipv4 as join lobby input");
                    return true;
                }
                else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Debug.Log("Valid ipv6 as join lobby input");
                    return true;
                }
            }

            Debug.LogWarning("Invalid ip join lobby input.");
            return false;
        }

        public static void ResetLabelText(TMP_Text field)
        {
            field.text = "";
        }

        public static string CreateNicknameLobbyErrorMsg(string invalid)
        {
            return $"An invalid {invalid} has been entered. Try to use a {invalid} that has at least 1 and maximum 10 characters and only contains letters.";
        }

        public static string GetIPErrorMsg()
        {
            return "An semantic invalid IP has beend entered.";
        }

        public static string GetPortErrorMsg()
        {
            return "An invalid Port has beend entered. Enter a Port between 1024 and 65535.";
        }

        public static bool IsValidNicknameOrLobbyName(string input)
        {
            Debug.Log("Validate Nickname oder Lobbyname.");
            if (string.IsNullOrEmpty(input) || input.Length > 10)
            {
                Debug.Log("An invalid input has been entered.");
                return false;
            }

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            Debug.Log("Valid input.");
            return true;
        }

        public static bool IsValidUserPort(int port)
        {
            if (port >= 1024 && port <= 65535)
            {
                Debug.Log("Valid port");
                return true;
            }

            Debug.Log("invalid Port");
            return false;
        }

        private static void ValidateFieldsAndToggleButton(Button button, params TMP_InputField[] fields)
        {
            bool allFilled = true;
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.text))
                {
                    allFilled = false;
                    break;
                }
            }
            button.interactable = allFilled;
        }

        public static void SetupButtonActivationValidation(Button button, params TMP_InputField[] fields)
        {
            foreach (var field in fields)
            {
                field.onValueChanged.AddListener((_) => ValidateFieldsAndToggleButton(button, fields));
            }
            // Initial check
            ValidateFieldsAndToggleButton(button, fields);
        }

        public static readonly string[] RandomPlayerNames = {
            "Candamir", "Hildegard", "Jean", "Franz", "LarsiHasi", "AlexPatoli", "Wolli"
        };

        public static readonly string[] FunnyBotNames = {
            "Botzilla",
            "KaffeeKarl",
            "LatteLarry",
            "Espressina",
            "Toastinator",
            "SchnitzelBot",
            "BrötchenBob",
            "WurstWilli",
            "Botfried",
            "Kekskrümel",
            "MokkaManni"
        };


        public static void AssignPlayerSprite(Player player, int playerIndex)
        {
            if (player.IsBot)
            {
                // Feste Bilder für Bot 0 bis Bot 3
                player.PlayerSpritePath = $"{SymbolSprites}/Bot_{playerIndex + 1}";
            }
            else
            {
                // Spezielle Spielernamen
                switch (player.PlayerName)
                {
                    case "LarsiHasi":
                        player.PlayerSpritePath = $"{SymbolSprites}/LarsiHasi";
                        break;
                    case "AlexPatoli":
                        player.PlayerSpritePath = $"{SymbolSprites}/AlexPatoli";
                        break;
                    case "Wolli":
                        player.PlayerSpritePath = $"{SymbolSprites}/Wolli";
                        break;
                    default:
                        player.PlayerSpritePath = $"{SymbolSprites}/Player_{playerIndex + 1}";
                        break;
                }
            }
        }

        private static string SymbolSprites = "Assets/Ressources/MainMenu/PlayerSymbols";

        private static readonly System.Random random = new(31415);

        public static string GenerateName(bool isBot)
        {
            List<Player> activePlayers = LobbyStorage.Instance.ActivePlayers;
            HashSet<string> usedNames = new();

            if (activePlayers != null)
            {
                foreach (var player in activePlayers)
                {
                    if (!string.IsNullOrEmpty(player.PlayerName))
                    {
                        usedNames.Add(player.PlayerName);
                    }
                }
            }

            string name;
            do
            {
                name = isBot ? "Bot " + FunnyBotNames[random.Next(FunnyBotNames.Length)] : RandomPlayerNames[random.Next(RandomPlayerNames.Length)];
            } while (usedNames.Contains(name));

            return name;
        }
    }
} 