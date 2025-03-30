using System.Collections.Generic;
using Assets.Scripting.Models;

namespace Assets.Scripting.TestHelper
{
	public static class PlayerTestHelper
	{
		public static List<Player> Players = new()
		{
			new("Test-Player", 10),
			new("Alex", 3),
			new("Meef", 2)
		};
	}
}