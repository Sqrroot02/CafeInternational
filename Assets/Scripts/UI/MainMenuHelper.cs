using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Models;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Helferklasse f�r Men�funktionen: Eingaben, Validierung, Labels, Spieler-Namen/Sprites, u.v.m.
    /// </summary>
    public static class MainMenuHelper
    {
        public static readonly string[] RandomPlayerNames = {
            "Candamir", "Hildegard", "Jean", "Franz", "LarsiHasi", "AlexPatoli", "Wolli"
        };

        public static readonly string[] FunnyBotNames = {
            "Botzilla", "KaffeeKarl", "LatteLarry", "Espressina", "Toastinator",
            "SchnitzelBot", "BroetchenBob", "WurstWilli", "Botfried", "Kekskruemel", "MokkaManni"
        };
        
        private static string SymbolSprites = "MainMenu/PlayerSymbols";

        /// <summary>
        /// Setzt den Placeholder-Text eines InputFields.
        /// </summary>
        public static void SetPlaceholder(TMP_InputField field, string text)
        {
            if (field.placeholder is TextMeshProUGUI placeholder)
            {
                placeholder.text = text;
                Debug.Log($"[MainMenuHelper] Placeholder set to: '{text}'");
            }
        }

        /// <summary>
        /// Gibt den aktuellen Text eines InputFields zur�ck.
        /// </summary>
        public static string GetInputText(TMP_InputField field)
        {
            Debug.Log($"[MainMenuHelper] Retrieved input text: '{field.text}'");
            return field.text;
        }

        /// <summary>
        /// Setzt ein InputField auf einen leeren Text zur�ck.
        /// </summary>
        public static void ResetInputText(TMP_InputField field)
        {
            Debug.Log("[MainMenuHelper] Reset input field.");
            field.text = "";
        }

        /// <summary>
        /// Setzt den Text eines Labels.
        /// </summary>
        public static void SetLabelText(TMP_Text field, string text)
        {
            Debug.Log($"[MainMenuHelper] Set label text to: '{text}'");
            field.text = text;
        }

        /// <summary>
        /// Gibt den Text eines Labels zur�ck.
        /// </summary>
        public static string GetLabelText(TMP_Text field)
        {
            Debug.Log($"[MainMenuHelper] Retrieved label text: '{field.text}'");
            return field.text;
        }

        /// <summary>
        /// Extrahiert eine Portnummer aus einem InputField (Text).
        /// </summary>
        public static int GetNumberFromLobbyPortTMP(TMP_InputField lobbyPortTmp)
        {
            if (int.TryParse(lobbyPortTmp.text, out int result))
            {
                Debug.Log($"[MainMenuHelper] Parsed lobby port: {result}");
                return result;
            }
            else
            {
                Debug.LogWarning("[MainMenuHelper] Invalid lobby port input.");
                return 0;
            }
        }

        /// <summary>
        /// Pr�ft, ob die angegebene IP-Adresse g�ltig ist (IPv4 oder IPv6).
        /// </summary>
        public static bool ValidateIp(string input)
        {
            if (input.Length < 7)
            {
                Debug.LogWarning("[MainMenuHelper] IP input too short.");
                return false;
            }

            if (IPAddress.TryParse(input, out IPAddress address))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log("[MainMenuHelper] Valid IPv4 address.");
                    return true;
                }
                else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Debug.Log("[MainMenuHelper] Valid IPv6 address.");
                    return true;
                }
            }

            Debug.LogWarning("[MainMenuHelper] Invalid IP input.");
            return false;
        }

        /// <summary>
        /// Setzt ein Label zur�ck (Text = "").
        /// </summary>
        public static void ResetLabelText(TMP_Text field)
        {
            Debug.Log("[MainMenuHelper] Label text reset.");
            field.text = "";
        }

        /// <summary>
        /// Gibt eine Fehlermeldung bei ung�ltigem Nicknamen zur�ck.
        /// </summary>
        public static string CreateNicknameLobbyErrorMsg(string invalid)
        {
            string msg = $"An invalid {invalid} has been entered. Try to use a {invalid} that has at least 1 and maximum 15 characters and only contains letters.";
            Debug.Log($"[MainMenuHelper] Created error message: {msg}");
            return msg;
        }

        /// <summary>
        /// Gibt eine Fehlermeldung bei ung�ltiger IP zur�ck.
        /// </summary>
        public static string GetIPErrorMsg()
        {
            const string msg = "An semantic invalid IP has been entered.";
            Debug.Log($"[MainMenuHelper] IP error message: {msg}");
            return msg;
        }

        /// <summary>
        /// Gibt eine Fehlermeldung bei ung�ltigem Port zur�ck.
        /// </summary>
        public static string GetPortErrorMsg()
        {
            const string msg = "An invalid Port has been entered. Enter a Port between 1024 and 65535.";
            Debug.Log($"[MainMenuHelper] Port error message: {msg}");
            return msg;
        }

        /// <summary>
        /// Validiert, ob ein Nickname oder Lobbyname g�ltig ist (nur Buchstaben, max. 15 Zeichen).
        /// </summary>
        public static bool IsValidNicknameOrLobbyName(string input)
        {
            Debug.Log("[MainMenuHelper] Validating nickname or lobby name.");

            if (string.IsNullOrEmpty(input) || input.Length > 15)
            {
                Debug.LogWarning("[MainMenuHelper] Input is null, empty or too long.");
                return false;
            }

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    Debug.LogWarning("[MainMenuHelper] Input contains invalid characters (only letters and spaces are allowed).");
                    return false;
                }
            }

            Debug.Log("[MainMenuHelper] Valid nickname or lobby name.");
            return true;
        }

        /// <summary>
        /// Pr�ft, ob ein Port im g�ltigen Bereich liegt (1024�65535).
        /// </summary>
        public static bool IsValidUserPort(int port)
        {
            bool isValid = port >= 1024 && port <= 65535;
            Debug.Log($"[MainMenuHelper] Port validation result: {isValid} for port {port}");
            return isValid;
        }

        /// <summary>
        /// Interne Methode zur Validierung von Eingabefeldern f�r Buttons.
        /// </summary>
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
            Debug.Log($"[MainMenuHelper] Button interactivity set to: {allFilled}");
        }

        /// <summary>
        /// Aktiviert/Deaktiviert einen Button basierend auf InputField-Inhalten.
        /// </summary>
        public static void SetupButtonActivationValidation(Button button, params TMP_InputField[] fields)
        {
            foreach (var field in fields)
            {
                field.onValueChanged.AddListener((_) =>
                {
                    Debug.Log($"[MainMenuHelper] Input field changed, revalidating...");
                    ValidateFieldsAndToggleButton(button, fields);
                });
            }

            ValidateFieldsAndToggleButton(button, fields);
        }

        /// <summary>
        /// Weist einem Spieler das passende Bild zu (Bot oder Spielername).
        /// </summary>
        public static void AssignPlayerSprite(Player player, int playerIndex)
        {
            if (player.IsBot)
            {
                player.PlayerSpritePath = $"{SymbolSprites}/Bot_{playerIndex + 1}";
                Debug.Log($"[MainMenuHelper] Assigned Bot sprite path: {player.PlayerSpritePath}");
            }
            else
            {
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
                Debug.Log($"[MainMenuHelper] Assigned Player sprite path: {player.PlayerSpritePath} for name {player.PlayerName}");
            }
        }

        /// <summary>
        /// Generiert einen einzigartigen Spielernamen, abh�ngig davon ob es ein Bot ist oder nicht.
        /// </summary>
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
                name = isBot
                    ? "Bot " + FunnyBotNames[RandomUtil.NextFix(FunnyBotNames.Length)]
                    : RandomPlayerNames[RandomUtil.NextFix(RandomPlayerNames.Length)];
            } while (usedNames.Contains(name));

            Debug.Log($"[MainMenuHelper] Generated {(isBot ? "bot" : "player")} name: {name}");
            return name;
        }
    }
}
