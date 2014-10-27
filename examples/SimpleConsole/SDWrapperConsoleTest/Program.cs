using System;
using System.Threading.Tasks;

// A slightly more complex api wrapper program

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
			var api = new MyLocalApi();
			var apiConsumer = new ApiConsumer(api);
			await apiConsumer.ShowReleaseCharts();
		}
	}
}
