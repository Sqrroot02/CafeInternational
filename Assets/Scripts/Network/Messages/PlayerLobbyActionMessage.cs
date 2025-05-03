using Riptide;

namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Represents a message type used for player actions in a lobby.
	/// </summary>
	public class PlayerLobbyActionMessage : IMessageSerializable
	{
		public string PlayerId { get; set; }
		public string Action { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddString(PlayerId);
			message.AddString(Action);
		}

		public void Deserialize(Message message)
		{
			PlayerId = message.GetString();
			Action = message.GetString();
		}
	}
}