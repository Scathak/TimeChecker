using System.Windows;
using System.Windows.Threading;

namespace timeChecker
{
	public partial class MainWindow : Window
	{
		private readonly List<DispatcherTimer> timersList = [];
		public AffirmationContainer? affirmationsContainer;
		readonly int timersMaxAmount = 96;

		public void ReadStrings()
		{
			affirmationsContainer = new AffirmationContainer();

			var fileReader = new FileReader("affirmations.json");
			var affirmations = AffirmationContainer.FromJson(fileReader.ReadAll());

			affirmationsContainer.Affirmations = affirmations.Affirmations;
		}

		private void StartClock()
		{
			ClockUpdate();
			var dispatcherTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			dispatcherTimer.Tick += TimeTickEvent;
			dispatcherTimer.Start();
		}

		private void TimeTickEvent(object? sender, EventArgs e)
		{
			ClockUpdate();
		}
	}
}
