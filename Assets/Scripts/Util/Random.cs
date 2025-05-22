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
		public static readonly Random Random;
	}
}