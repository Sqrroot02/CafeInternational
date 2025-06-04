using Assets.Scripts.Models;
using Riptide;

namespace Assets.Scripts.Network.Messages.PlayerLobbyAction
{
	/// <summary>
	/// Represents a message for performing player-related actions in a lobby.
	/// Used to communicate actions like adding players to a lobby.
	/// </summary>
	public class PlayerLobbyActionMessage : IMessageSerializable
	{
		public Player[] Players { get; set; }
		public string LobbyName { get; set; }
		public string LobbyIp { get; set; }
		public int LobbyPort { get; set; }
		public bool SessionStarted { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddSerializables(Players);
			message.AddString(LobbyName);
			message.AddString(LobbyIp);
			message.AddInt(LobbyPort);
			message.AddBool(SessionStarted);
		}

		public void Deserialize(Message message)
		{
			Players = message.GetSerializables<Player>();
			LobbyName = message.GetString();
			LobbyIp = message.GetString();
			LobbyPort = message.GetInt();
			SessionStarted = message.GetBool();
		}
	}
}