using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPFundamentalsServer
{
	internal class Program
	{
		static bool ServerOn = true;
		static void Main(string[] args)
		{
			using (var listener = new HttpListener()) 
			{
				listener.Prefixes.Add("http://localhost:8888/GetMyName/");
				listener.Prefixes.Add("http://localhost:8888/Information/");
				listener.Prefixes.Add("http://localhost:8888/Success/");
				listener.Prefixes.Add("http://localhost:8888/Redirection/");
				listener.Prefixes.Add("http://localhost:8888/ClientError/");
				listener.Prefixes.Add("http://localhost:8888/ServerError/");
				listener.Prefixes.Add("http://localhost:8888/ServerStatus/");
				listener.Prefixes.Add("http://localhost:8888/GetMyNameByHeader/");
				listener.Prefixes.Add("http://localhost:8888/GetMyNameByCookies/");
				listener.Start();

				while (ServerOn)
				{
					Console.WriteLine("Listening on port 8888...");
					HttpListenerContext context = listener.GetContext();

					// REDIRECT TO METHOD
					string methodName = context.Request.Url.Segments[1].Replace("/", "");
					string[] strParams = context.Request.Url.Segments
									.Skip(2)
									.Select(s => s.Replace("/", ""))
									.ToArray();
					var method = typeof(Program).GetMethod(methodName);
					object[] @params = method.GetParameters()
								.Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
								.ToArray();
					var methodResponse = method.Invoke(null, @params).ToString();

					//BUILD RESPONSE
					HttpListenerRequest request = context.Request;
					HttpListenerResponse response = context.Response;

					if (methodName == "GetMyNameByHeader")
					{
						response.Headers.Add($"X-MyName:{methodResponse}");
					}

					if (methodName == "GetMyNameByCookies")
					{
						Cookie cook = new Cookie("MyName", methodResponse);
						response.AppendCookie(cook);
					}

					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(methodResponse);
					response.ContentLength64 = buffer.Length;
					System.IO.Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				listener.Stop();
				Console.WriteLine("Disconnected.");
			}
		}
		public static string GetMyName(string myName) 
		{
			return "Server says: Hi " + myName;
		}
		public static string GetMyNameByHeader()
		{
			return "Server says: Hi MyCoolName from headers";
		}
		public static string GetMyNameByCookies()
		{
			return "Server says: Hi MyCoolName from cookies";
		}
		public static string Information()
		{
			return "1xx – Information";
		}
		public static string Success()
		{
			return "2xx – Success";
		}
		public static string Redirection()
		{
			return "3xx – Redirectio";
		}
		public static string ClientError()
		{
			return "4xx – Client error";
		}
		public static string ServerError()
		{
			return "5xx – Server error";
		}
		public static string ServerStatus(bool serverStatus)
		{
			ServerOn = serverStatus;
			return "Server says: bye.";
		}
	}
}
