namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Enumerates all defined message-types for this game.
	/// (0xxx) -> Server Unicast (If you want to notify server)
	/// (1xxx) -> Broadcast (If you want to notify everyone)
	/// (2xxx) -> Client Unicast (If you want to notify a special participant ;D ) (!!!Will be realized if necessary!!!)
	/// </summary>
	public enum MessageType : ushort
	{
		/// <summary>
		/// New player salutation message
		/// </summary>
		PlayerSalutation = 1,
        
		/// <summary>
		/// Player property changed in a lobby
		/// </summary>
		PlayerLobbyAction = 1001,
		
		/// <summary>
		/// A user has started the game 
		/// </summary>
		StartGame = 1002 
	}
}