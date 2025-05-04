namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Enumerates all defined message-types for this game.
	///		- (0xxx) -> Server Unicast (If you want to notify server)
	///		- (1xxx) -> Broadcast (If you want to notify everyone)
	///		- (2xxx) -> Client Unicast (If you want to notify a special participant ;D ) (!!!Will be realized if necessary!!!)
	///		- (3xxx) -> Multicast (Everyone except me)
	/// </summary>
	public enum MessageType : ushort
	{
		/// <summary>
		/// New player salutation message (UNICAST)
		/// </summary>
		PlayerSalutation = 1,
        
		/// <summary>
		/// Player property changed in a lobby (BROADCAST)
		/// </summary>
		PlayerLobbyAction = 1001,
		
		/// <summary>
		/// A user has started the game (BROADCAST)
		/// </summary>
		StartGame = 1002 ,
		
		/// <summary>
		/// The current player has committed his turn (MULTICAST)
		/// </summary>
		TurnCommit = 3001,
	}
}