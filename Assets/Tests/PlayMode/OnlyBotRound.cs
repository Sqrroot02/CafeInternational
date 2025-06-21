using System.Collections;
using NUnit.Framework;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.ExploroationTests
{
    public class OnlyBotRound
    {
        [Test]
        public void TestPlayBotsVsBots()
        {
            new GameObject().AddComponent<LobbyStorage>(); // Instantiate LobbyStorage
            LobbyStorage.Instance.InitializeLobby("TestPlayer", "TestLobby", false); // Setup the lobby
            foreach (var player in LobbyStorage.Instance.ActivePlayers) // Override all automatically added players to bots
            {
                player.IsBot = true;
                player.BotType = BotType.IsScoringBot;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); // Launch the game
        }
    }
}