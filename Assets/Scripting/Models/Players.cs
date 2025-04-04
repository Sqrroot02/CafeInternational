using System.Collections.Generic;
using Assets.Scripting.TestHelper;

namespace Assets.Scripting.Models
{
	public class Players
	{
		public static List<Player> ActivePlayers => PlayerTestHelper.Players;
	}
}