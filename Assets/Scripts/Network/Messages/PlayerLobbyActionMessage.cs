using Assets.Scripts.Models;
using Riptide;

namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Represents a message for performing player-related actions in a lobby.
	/// Used to communicate actions like adding players to a lobby.
	/// </summary>
	public class PlayerLobbyActionMessage : IMessageSerializable
	{
		public Player[] Players { get; set; }
		public string LobbyName { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddSerializables(Players);
			message.AddString(LobbyName);
		}

		public void Deserialize(Message message)
		{
			Players = message.GetSerializables<Player>();
			LobbyName = message.GetString();
		}
	}
}