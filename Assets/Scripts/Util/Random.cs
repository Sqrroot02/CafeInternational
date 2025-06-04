using System;

namespace Assets.Scripts.Util
{
	/// <summary>
	/// Util class for working with random  
	/// </summary>
	public static class RandomUtil
	{
		static RandomUtil()
		{
			Random = new Random(31415);
		}
		
		/// <summary>
		/// Random generator with identical seed for all session participants
		/// </summary>
		public static Random Random;

		private static readonly Random FixRandom = new Random(31415);
		
		public static int Next(int min, int max) => Random.Next(min, max);
		public static int Next(int max) => Random.Next(max);
		public static double NextDouble() => Random.NextDouble();
		public static int NextDouble(int max) => Random.Next(max);
		public static int NextFix(int max) => FixRandom.Next(max);
		
	}
}