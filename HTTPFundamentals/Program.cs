using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTTPFundamentalsClient
{
	internal class Program
	{
		static readonly HttpClient client = new HttpClient();
		static async Task Main()
		{
			try
			{
				//TASK 1
				string MyName = "MyCoolName";
				var responseTask1 = await client.GetStringAsync($"http://localhost:8888/GetMyName/{MyName}");
				Console.WriteLine(responseTask1);

				//TASK 2
				var responseTask21 = await client.GetStringAsync($"http://localhost:8888/Information/");
				Console.WriteLine(responseTask21);
				var responseTask22 = await client.GetStringAsync($"http://localhost:8888/Success/");
				Console.WriteLine(responseTask22);
				var responseTask23 = await client.GetStringAsync($"http://localhost:8888/Redirection/");
				Console.WriteLine(responseTask23);
				var responseTask24 = await client.GetStringAsync($"http://localhost:8888/ClientError/");
				Console.WriteLine(responseTask24);
				var responseTask25 = await client.GetStringAsync($"http://localhost:8888/ServerError/");
				Console.WriteLine(responseTask25);
				
				
				//TASK 3
				var responseTask3 = await client.GetAsync($"http://localhost:8888/GetMyNameByHeader/");
				Console.WriteLine(responseTask3.Headers);

				//TASK 4
				var responseTask4 = await client.GetAsync($"http://localhost:8888/GetMyNameByCookies/");
				Console.WriteLine(responseTask4);

				// Close server
				var responseTask26 = await client.GetStringAsync($"http://localhost:8888/ServerStatus/false");
				Console.WriteLine(responseTask26);

				Console.ReadLine();
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error message :{e.Message}");
			}
		}
	}
}
