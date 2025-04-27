using Riptide;

namespace Assets.Scripts.Network
{
	/// <summary>
	/// Defines a player within a networked environment.
	/// </summary>
	/// <remarks>
	/// Maintains player-specific properties such as name, network address, and communication port.
	/// </remarks>
	public class PlayerConnection
	{
		/// <summary>
		/// Gets or sets the name of the player.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the client associated with the player.
		/// </summary>
		public Connection ClientConnection { get; set; }

		public override string ToString()
		{
			return $"{Name} ({ClientConnection.Id})";
		}
	}
}