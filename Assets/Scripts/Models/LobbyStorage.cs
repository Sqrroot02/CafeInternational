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
        public Player ClientPlayer { get; set; }

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
    
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void InitializeLobby(string localPlayerName, string lobbyName)
        {
            LobbyName = lobbyName;
        
            ActivePlayers.Clear();

            var hostPlayer = new Player(localPlayerName, 0, false, true);
            ActivePlayers.Add(hostPlayer);
            ClientPlayer = hostPlayer;

            for (var i = 1; i < 4; i++)
            {
                var botName = MainMenuHelper.GenerateName();
                ActivePlayers.Add(new Player($"Bot {botName}", 0, true, false));
            }
        }

        public void ReplaceBotWithHuman(string playerName)
        {
            for (var i = 0; i < ActivePlayers.Count; i++)
            {
                if (!ActivePlayers[i].IsBot)
                {
                    ActivePlayers[i] = new Player(playerName, 0, false, false);
                    return;
                }
            }
        }

        public void ReplacePlayerWithBot(string playerName)
        {
            for (var i = 0; i < ActivePlayers.Count; i++)
            {
                if (ActivePlayers[i].PlayerName == playerName && ActivePlayers[i].IsBot)
                {
                    string botName = MainMenuHelper.GenerateName();
                    ActivePlayers[i] = new Player($"Bot {botName}", 0, true, false);
                    Debug.Log($"{playerName} has been replaced with a bot");
                    return;
                }
            }

            Debug.Log($"No Player with naem:  {playerName} found.");
        }
    
        /// <summary>
        /// Shuffles the players turn sequence  
        /// </summary>
        public void ShufflePlayers()
        {
            var playersStack = new Stack<Player>(LobbyStorage.Instance.ActivePlayers);
        
            ActivePlayers.Clear();
            ActivePlayers.Add(playersStack.Pop());

            while (playersStack.Count > 0)
                ActivePlayers.Insert(Random.Range(0, ActivePlayers.Count + 1), playersStack.Pop());
        }
    }
}
