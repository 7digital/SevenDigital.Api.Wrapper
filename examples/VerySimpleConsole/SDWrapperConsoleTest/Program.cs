	/*
	 The simplest possible 7Digital wrapper program:
	 * 
	 1) "File|New Console application". Use .Net version 4.5 or later
	 2) "Manage Nuget packages", add a reference to the package "SevenDigital.Api.Wrapper", latest version
	 3) Code below to show a release chart.
	 */

	using System;
	using System.Threading.Tasks;
	using SevenDigital.Api.Schema.Releases;
	using SevenDigital.Api.Wrapper;

	namespace SDWrapperConsoleTest
	{
		class Program
		{
			private static void Main()
			{
				var task = Use7DigitalApi();
				task.Wait();
				Console.ReadLine();
			}

			private static async Task Use7DigitalApi()
			{
				var request = Api<ReleaseChart>.Create;
				var chart = await request.Please();

				Console.WriteLine("Retrieved a chart containing {0} releases", 
					chart.ChartItems.Count);
				foreach (var chartItem in chart.ChartItems)
				{
					Console.WriteLine("{0}: '{1}' by {2}", 
						chartItem.Position, chartItem.Release.Title, 
						chartItem.Release.Artist.Name);
				}
			}
		}
	
		public class ApiUri : IApiUri
		{
			public string Uri
			{
				get { return "http://api.7digital.com/1.2"; }
			}

			public string SecureUri
			{
				get { return "https://api.7digital.com/1.2"; }
			}
		}
	
		/// <summary>
		/// "your_key_here" is a valid key, but it has restrictions,
		/// including a low daily request limit which it often reaches 
		/// due to other people's actions.
		/// Please request your own key from https://api-signup.7digital.com/ 
		/// and put it here!
		/// </summary>
		public class Credentials : IOAuthCredentials
		{
			public string ConsumerKey
			{
				get { return "your_key_here"; }
			}

			public string ConsumerSecret
			{
				get { return "your_value_here"; }
			}
		}
	}
