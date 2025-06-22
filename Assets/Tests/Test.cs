using System.Collections;
using Assets.Scripts.Game;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Tests
{
    public class Test
    {
        [UnityTest]
        public IEnumerator TestPlayBotsVsBots()
        {
            new GameObject().AddComponent<LobbyStorage>(); // Instantiate LobbyStorage
            LobbyStorage.Instance.InitializeLobby("TestPlayer", "TestLobby", false); // Setup the lobby
            foreach (var player in LobbyStorage.Instance.ActivePlayers) // Override all automatically added players to bots
            {
                player.IsBot = true;
                player.BotType = BotType.IsScoringBot;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); // Launch the game
            
            PlayerManager.DefaultWaitForBotPlay = 0.1f;
            PlayerManager.DefaultWaitForUpdate = 0.2f;
            
            yield return new WaitUntil(() => PlayerManager.BoolGameEnded);
        }
    }
}