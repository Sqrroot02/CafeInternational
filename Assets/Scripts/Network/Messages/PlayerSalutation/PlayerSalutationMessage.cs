using Riptide;

namespace Assets.Scripts.Network.Messages.PlayerSalutation
{
	/// <summary>
	/// Represents a message that contains a salutation from a player, including their PlayerName.
	/// </summary>
	public class PlayerSalutationMessage : IMessageSerializable
	{
		public string PlayerName { get; set; }
		public string PlayerId { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddString(PlayerName);
			message.AddString(PlayerId);
		}

		public void Deserialize(Message message)
		{
			PlayerName = message.GetString();
			PlayerId = message.GetString();
		}
	}
}