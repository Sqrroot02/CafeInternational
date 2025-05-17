using Assets.Scripts.Models;
using Riptide;

namespace Assets.Scripts.Network.Messages.TurnCommit
{
	/// <summary>
	/// Represents a message used to commit the actions of the current player's turn and
	/// transitions the game state to the next player. This message contains information about the
	/// current player, the next player, and the set of changes or actions performed during the turn.
	/// </summary>
	public class TurnCommitMessage : IMessageSerializable
	{
		/// <summary>
		/// The player that has committed the turn
		/// </summary>
		public Player Player { get; set; }

		/// <summary>
		/// The player designated to take the next turn in the game.
		/// </summary>
		public Player NextPlayer { get; set; }

		/// <summary>
		/// Represents the collection of changes or actions performed during a player's turn that are being committed
		/// as part of the turn transition within the game state.
		/// </summary>
		public TurnCommitChangeMessage[] Changes { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddSerializable(Player);
			//message.AddSerializable(NextPlayer);
			message.AddSerializables(Changes);
		}

		public void Deserialize(Message message)
		{
			Player = message.GetSerializable<Player>();
			//NextPlayer = message.GetSerializable<Player>();
			Changes = message.GetSerializables<TurnCommitChangeMessage>();
		}
	}
}