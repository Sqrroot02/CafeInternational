using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// Represents the storage for lobby-specific data and players in an online game setting.
    /// </summary>
    public class LobbyStorage : MonoBehaviour
    {
        public static LobbyStorage Instance { get; private set; }

        /// <summary>
        /// All active players in the current joined session
        /// </summary>
        public List<Player> ActivePlayers { get; set; } = new();

        /// <summary>
        /// The client associated player a.k.a. "you"
        /// </summary>
        public string ClientPlayerId { get; set; }

        /// <summary>
        /// The current player that is on turn
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// The Name of the Lobby
        /// </summary>
        public string LobbyName { get; set; }

        /// <summary>
        /// The Port of the game server
        /// </summary>
        public int LobbyPort { get; set; } = 57967;

        /// <summary>
        /// The IP Address of the game server 
        /// </summary>
        public string LobbyIp { get; set; }

        public string CardPath { get; set; } = "Normal/";

        private string globalLobbyName;
        private string globalLobbyIp;

        void Awake()
        {
            Debug.Log("[LobbyStorage] Awake called.");

            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[LobbyStorage] Another instance detected. Destroying this duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[LobbyStorage] Singleton instance assigned and marked to not destroy on load.");
        }

        public void InitializeLobby(string localPlayerName, string lobbyName)
        {
            Debug.Log($"[LobbyStorage] InitializeLobby called with localPlayerName: '{localPlayerName}', lobbyName: '{lobbyName}'");

            LobbyName = lobbyName;

            ActivePlayers.Clear();
            Debug.Log("[LobbyStorage] ActivePlayers list cleared.");

            var hostPlayer = new Player(localPlayerName, 0, false, true, false);
            ClientPlayerId = hostPlayer.PlayerId;
            Debug.Log($"[LobbyStorage] Host player created with PlayerId: {ClientPlayerId}");

            ActivePlayers.Add(hostPlayer);
            Debug.Log("[LobbyStorage] Host player added to ActivePlayers.");

            MainMenuHelper.AssignPlayerSprite(hostPlayer, ActivePlayers.IndexOf(hostPlayer));
            Debug.Log("[LobbyStorage] Assigned sprite to host player.");

            // Fill remaining slots with bots
            for (var i = ActivePlayers.Count; i < 4; i++)
            {
                var botName = MainMenuHelper.GenerateName(true);
                var botPlayer = new Player(botName, 0, true, false, false);
                ActivePlayers.Add(botPlayer);
                Debug.Log($"[LobbyStorage] Added bot player '{botName}' to ActivePlayers.");
                MainMenuHelper.AssignPlayerSprite(botPlayer, ActivePlayers.IndexOf(botPlayer));
                Debug.Log("[LobbyStorage] Assigned sprite to botPlayer.");
            }
        }

        /// <summary>
        /// Shuffles the players turn sequence  
        /// </summary>
        public void ShufflePlayers()
        {
            Debug.Log("[LobbyStorage] ShufflePlayers called.");

            var playersStack = new Stack<Player>(LobbyStorage.Instance.ActivePlayers);

            ActivePlayers.Clear();

            if (playersStack.Count == 0)
            {
                Debug.LogWarning("[LobbyStorage] ShufflePlayers: No players to shuffle.");
                return;
            }

            // Take one player out (top of the stack)
            ActivePlayers.Add(playersStack.Pop());
            Debug.Log("[LobbyStorage] Added one player to ActivePlayers as start of shuffle.");

            // Insert others randomly
            while (playersStack.Count > 0)
            {
                int insertIndex = Random.Range(0, ActivePlayers.Count + 1);
                var playerToInsert = playersStack.Pop();
                ActivePlayers.Insert(insertIndex, playerToInsert);
                Debug.Log($"[LobbyStorage] Inserted player '{playerToInsert.PlayerName}' at index {insertIndex}.");
            }

            Debug.Log("[LobbyStorage] ShufflePlayers completed.");
        }
    }
}
