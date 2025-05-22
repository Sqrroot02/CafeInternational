using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;


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

        public static void ResetLabelText(TMP_Text field)
        {
            field.text = "";
        }

        public static bool IsValidNicknameOrLobbyName(string input)
        {
            if (!string.IsNullOrEmpty(input) || input.Length <= 10 && input.Length > 0)
            {
                if (Regex.IsMatch(input, "^[a-zA-Z]*$"))
                {
                    Debug.Log("Valid Nickname or LobbyName");
                    return true;
                }
            }

            return false;
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

        private static readonly string[] FunncyNameNouns = {
            "Candamir", "Hildegard", "Jean", "Franz", "LarsiHasi", "AlexPat�la", "Wolli"
        };

        private static readonly System.Random random = new();

        public static string GenerateName()
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
                name = FunncyNameNouns[random.Next(FunncyNameNouns.Length)];
            } while (usedNames.Contains(name));

            return name;
        }
    }
} 