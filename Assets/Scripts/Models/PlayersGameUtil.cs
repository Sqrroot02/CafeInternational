using System.Collections.Generic;
using Assets.Scripts.TestHelper;

namespace Assets.Scripts.Models
{
	/// <summary>
	/// Provides utility functionalities for managing active players in the game.
	/// </summary>
	public class PlayersGameUtil
	{
		/// A static property that holds a list of currently active players in the game.
		/// The property is initialized with a predefined list of players from the `PlayerTestHelper` class,
		/// but can be dynamically updated at runtime, such as during game initialization or progression.
		/// This property is used across various game components to manage and retrieve player information,
		/// providing a centralized way of accessing the current pool of players.
		public static List<Player> ActivePlayers { get; set; } = PlayerTestHelper.Players;
	}
}