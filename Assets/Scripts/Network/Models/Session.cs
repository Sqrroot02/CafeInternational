using System.Collections.ObjectModel;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.PlayerLobbyAction;
using Assets.Scripts.UI;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Network.Models
{
	/// <summary>
	/// Represents a network session containing a list of connected players.
	/// </summary>
	/// <remarks>
	/// This class manages the collection of players participating in the session.
	/// It can be used to reference or manipulate player-specific data collectively.
	/// </remarks>
	public class Session
	{
		/// <summary>
		/// The name of the opened Session
		/// </summary>
		public string Name { get; set; }
		
		public Session(string name, ObservableCollection<Player> players)
		{
			Players = players;
			Name = name;
			Debug.Log("Session has been created");
		}

		/// <summary>
		/// Replaces the first bot in the session with the provided player, if more than one bot exists.
		/// </summary>
		/// <param name="player">The player to be added to the session.</param>
		public void AddPlayer(Player player)
		{
			var countBots = Players.Count(x => x.IsBot);
			if (countBots > 1)
			{
				var firstBot = Players.IndexOf(Players.First(x => x.IsBot));
				Players[firstBot] = player;	
			}
			SendUpdate();
		}

		public void SendUpdate()
		{
			Debug.Log($"The current Session:\n {string.Join("\n", Players.Select(x => $"{x.PlayerName} [{x.PlayerId}]"))}");
			var lobbyActionMessage = new PlayerLobbyActionMessage()
			{
				Players = Players.ToArray(),
				LobbyName = Name,
				LobbyPort = LobbyStorage.Instance.LobbyPort,
				LobbyIp = LobbyStorage.Instance.LobbyIp
			};
			NetworkRouter.Broadcast(lobbyActionMessage, MessageType.PlayerLobbyAction);
		}

		/// <summary>
		/// Replaces a player in the session with a bot, identified by the given client ID.
		/// </summary>
		/// <param name="clientId">The unique identifier of the player to be removed.</param>
		public void RemovePlayer(int clientId)
		{
			var indexPlayer = Players.IndexOf(Players.First(x => x.ClientId == clientId));
			if (indexPlayer > 0)
			{
				var botName = MainMenuHelper.GenerateName();
				var newBot = new Player($"Bot {botName}", 0, true, false);
				Players[indexPlayer] = newBot;
			}
			
			SendUpdate();
		}
		
		/// <summary>
		/// Gets or sets the collection of players in the current session.
		/// </summary>
		/// <remarks>
		/// Represents a list of <see cref="PlayerConnection"/> objects associated with the session.
		/// Each player contains information such as name, IP address, and port.
		/// </remarks>
		public ObservableCollection<Player> Players { get; }
	}
}