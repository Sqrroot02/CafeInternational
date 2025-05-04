namespace Assets.Scripts.Network.Messages.TurnCommit
{
	/// <summary>
	/// Represents the actions a player can commit during their turn.
	/// </summary>
	public enum TurnCommitAction
	{
		/// <summary>
		/// Player placed a card on the bar
		/// </summary>
		PlaceCardOnBar,
		
		/// <summary>
		/// Player placed a card on a chair
		/// </summary>
		PlaceCardOnChair,
	}
}