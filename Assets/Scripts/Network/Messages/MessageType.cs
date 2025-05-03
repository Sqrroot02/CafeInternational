namespace Assets.Scripts.Network.Messages
{
    /// <summary>
    /// Enumerates all defined message-types for this game
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
        PlayerLobbyAction = 1001
    }
}
