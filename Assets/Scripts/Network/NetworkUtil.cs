using System.Net.Http;
using System.Threading.Tasks;

namespace Assets.Scripts.Network
{
	public class NetworkUtil
	{
		public static string PublicIpAddress()
		{
			var result = Task.Run(async () =>
			{
				using var client = new HttpClient();
				var response = await client.GetStringAsync("https://api.ipquery.io");
				return response.Trim();
			});
			return result.Result;
		}
	}
}