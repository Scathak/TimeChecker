using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;

namespace timeChecker
{
	public partial class MainWindow : Window
	{
		private void TimerTickEvent(object? sender, EventArgs e)
		{
			if (timersList.Count > 0)
			{
				var uri = new Uri(
									Path.Combine(
										   AppDomain.CurrentDomain.BaseDirectory
										, "wav"
										, "alarm1.wav")
								 );
				var player = new MediaPlayer();
				player.Open(uri);
				player.Play();

				timersList[0].Stop();
				timersList.RemoveAt(0);
				lstTimers.Items.RemoveAt(0);
				if (timersList.Count > 0)
				{
					timersList[0].Start();
					lstTimers.Items[0] += DateTime.Now.AddMinutes(timersList[0].Interval.TotalMinutes).ToString(@"hh:mm:ss");
				}
			}
		}
		private void SetupNewTimer(int timerDuration)
		{
			timersList.Add(NewTimerStart(timerDuration));
			lstTimers.Items.Add($"The alarm {timerDuration} minutes will go off at: ");
			if (timersList.Count == 1)
			{
				timersList[0].Start();
				lstTimers.Items[0] += DateTime.Now.AddMinutes(timersList[0].Interval.TotalMinutes).ToString(@"hh:mm:ss");
			}
		}
		private bool CheckTimersAmount()
		{
			return (timersList.Count < timersMaxAmount);
		}
		private DispatcherTimer NewTimerStart(int interval)
		{
			var newTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMinutes(interval)
			};
			newTimer.Tick += TimerTickEvent;
			AffirmationTextbox.Text = affirmationsContainer?.Affirmations?[affirmationsContainer.currentAffirmation++].Text;

			return newTimer;
		}
	}
}
