using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfTPL
{
	public class TestAsync
	{
		public static async Task DoSomethingAsync()
		{
			int val = 10;
			Trace.WriteLine(val);
			await Task.Delay(TimeSpan.FromSeconds(2));
			val *= 2;
			await Task.Delay(TimeSpan.FromSeconds(1));
			Trace.WriteLine(val);
		}


		public static async Task DoSomethingAsync1()
		{
			int val = 10;
			Trace.WriteLine(val);
			await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
			val *= 2;
			await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
			Trace.WriteLine(val);
		}
		public static async Task<T> DelayResult<T>(T result, TimeSpan delay)
		{
			await Task.Delay(delay);
			return result;
		}
		public static async Task<string> DownloadStringWithRetries(string uri)
		{
			using (var client = new HttpClient())
			{
				var nextDelay = TimeSpan.FromSeconds(1);
				for (int i = 0; i != 3; ++i)
				{
					try
					{
						return await client.GetStringAsync(uri);
					}
					catch
					{

					}
					await Task.Delay(nextDelay);
					nextDelay = nextDelay + nextDelay;
				}
				return await client.GetStringAsync(uri);
			}
		}
		public static async Task<string> DowloadStringWithTimeout(string uri)
		{
			using (var client = new HttpClient())
			{
				var downloadTask = client.GetStringAsync(uri);
				var timeoutTask = Task.Delay(3000);
				var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
				if (completedTask == timeoutTask)
					return null;
				return await downloadTask;
			}
		}
		public static async Task ProgressAsync(IProgress<int> progress = null)
		{
			int percentComplete = 0;
			while (percentComplete != 10)
			{
				await Task.Delay(500);
				percentComplete += 1;
				if (progress != null)
					progress.Report(percentComplete);
			}
		}
		public static async Task<string> DowloadAllAsync(IEnumerable<string> urls)
		{
			var httpClient = new HttpClient();
			var downloads = urls.Select(url => httpClient.GetStringAsync(url));
			Task<string>[] downloadTasks = downloads.ToArray();
			string[] htmlPages = await Task.WhenAll(downloadTasks);
			return default(string).Concat(htmlPages, " - ");
		}
	}
}
