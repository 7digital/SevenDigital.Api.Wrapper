using System;
using System.Threading.Tasks;

namespace SevenDigital.Api.Wrapper.Integration.Tests
{
	public static class TaskHelpers
	{
		public static T BusyAwait<T>(this Task<T> task)
		{
			try
			{
				task.Wait();
				return task.Result;
			}
			catch (AggregateException aggreggateEx)
			{
				if (aggreggateEx.InnerExceptions.Count == 1)
				{
					throw aggreggateEx.InnerExceptions[0];
				}

				throw;
			}
		}
	}
}