using Assets.Scripts.Models;
using Riptide;

namespace Assets.Scripts.Network.Messages.StartGame
{
	/// <summary>
	/// Represents a network message to start a game, containing relevant game state information.
	/// </summary>
	public class StartGameMessage : IMessageSerializable
	{
		public Player[] Players { get; set; }
		public string LobbyName { get; set; }
		public Player Starter { get; set; }
		public int Seed { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddSerializables(Players);
			message.AddString(LobbyName);
			message.AddSerializable(Starter);
			message.AddInt(Seed);
		}

		public void Deserialize(Message message)
		{
			Players = message.GetSerializables<Player>();
			LobbyName = message.GetString();
			Starter = message.GetSerializable<Player>();
			Seed = message.GetInt();
		}
	}
}