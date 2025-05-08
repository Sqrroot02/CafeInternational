using System.Collections.Generic;
using Assets.Scripts.Models;

namespace Assets.Scripts.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 0, true),
			new("Alex", 0, true),
			new("Joel", 0, true),
			new("Lars", 0, false)
		};
	}
}