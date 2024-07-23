using System.Windows;
using System.Windows.Threading;

namespace timeChecker
{
	public partial class MainWindow : Window
	{
		private void TimerTickEvent(object? sender, EventArgs e)
		{
			if (timersList.Count > 0)
			{
				this.Topmost = true;
                var alarm = new AlarmPlayer("alarm1.wav");
				alarm.StartAlarm();

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
