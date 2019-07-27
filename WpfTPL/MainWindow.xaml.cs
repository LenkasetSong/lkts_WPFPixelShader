using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTPL
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await TestAsync.DoSomethingAsync();

			BeginAnimation();

			CompositionTarget.Rendering += CompositionTarget_Rendering;
			CompositionTarget.Rendering += CompositionTarget_Rendering1;
		}

		private void CompositionTarget_Rendering1(object sender, EventArgs e)
		{
			Trace.WriteLine("1:" + DateTime.Now);
		}

		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			Trace.WriteLine(DateTime.Now);
			//Thread.Sleep(20);
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			string tag = (e.OriginalSource as Button).Tag.ToString();
			switch (tag)
			{
				case "0x0001":
					await TestAsync.DoSomethingAsync1();
					break;
				case "0x0002":
					ststxt.Text = await TestAsync.DelayResult("0x0002", TimeSpan.FromSeconds(2));
					break;
				case "0x0003":
					ststxt.Text = await TestAsync.DownloadStringWithRetries("https://localhost:44312/api/values");
					break;
				case "0x0004":
					ststxt.Text = await TestAsync.DowloadStringWithTimeout("https://localhost:44312/api/values/20");
					break;
				case "0x0005":
					var progress = new Progress<int>();
					progress.ProgressChanged += (sender, args) =>
					{
						ststxt.Text = args.ToString();
					};
					await TestAsync.ProgressAsync(progress);
					break;
				case "0x0006":
					ststxt.Text = await TestAsync.DowloadAllAsync(
						new string[] {
							"https://localhost:44312/api/values/20",
							"https://localhost:44312/api/math/sin/0.52358",
							"https://localhost:44312/api/math/cos/num=1.04716",
						}
					);
					break;
				default:
					break;
			}
		}


		public void BeginAnimation()
		{
			Storyboard sb = new Storyboard();
			EasingDoubleKeyFrame frame1 = new EasingDoubleKeyFrame(0.5, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5)));
			EasingDoubleKeyFrame frame2 = new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)));
			DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
			frames.KeyFrames.Add(frame1);
			frames.KeyFrames.Add(frame2);
			Storyboard.SetTarget(frames, this);
			Storyboard.SetTargetProperty(frames, new PropertyPath(OpacityProperty));
			sb.Children.Add(frames);
			sb.AutoReverse = true;
			sb.RepeatBehavior = RepeatBehavior.Forever;
			sb.Begin();
			//sb.Stop();
		}
	}
}
