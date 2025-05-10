using System.Collections.Generic;
using Assets.Scripts.Models;

namespace Assets.Scripts.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 0, true, false),
			new("Alex", 0, true, false),
			new("Joel", 0, true, false),
			new("Lars", 0, false, false)
		};
	}
}