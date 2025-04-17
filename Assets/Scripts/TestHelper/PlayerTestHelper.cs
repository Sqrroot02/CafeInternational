using System.Collections.Generic;
using Assets.Scripts.Models;

namespace Assets.Scripts.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 0),
			new("Alex", 0),
			new("Meef", 0),
			new("Huso", 0)
		};
	}
}