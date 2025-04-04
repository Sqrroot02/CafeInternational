using System.Collections.Generic;
using Assets.Scripts.TestHelper;

namespace Assets.Scripts.Models
{
	public class Players
	{
		public static List<Player> ActivePlayers => PlayerTestHelper.Players;
	}
}