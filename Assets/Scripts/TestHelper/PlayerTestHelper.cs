using System.Collections.Generic;
using Assets.Scripts.Models;

namespace Assets.Scripts.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 0, true, false, BotType.IsWeakBot),
			new("Alex", 0, true, false, BotType.IsWeakBot),
			new("Joel", 0, true, false, BotType.IsWeakBot),
			new("Lars", 0, false, false, BotType.IsWeakBot)
		};
	}
}