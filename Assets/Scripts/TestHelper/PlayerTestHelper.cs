using System.Collections.Generic;
using Assets.Scripts.Models;

namespace Assets.Scripts.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 10),
			new("Alex", 3),
			new("Meef", 2),
			new("Huso", 20000)
		};
	}
}