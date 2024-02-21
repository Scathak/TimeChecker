using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace timeChecker
{
	public partial class MainWindow : Window
    {
		public interface IReader
		{
			public Affirmation[]? ReadAll();
		}
		public class FileReader(string fileName) : IReader
		{
			private readonly string _fileName = fileName;

			public Affirmation[]? ReadAll()
			{
				try
				{
					using var r = new StreamReader(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + _fileName);
				    var json = r.ReadToEnd();
				    return JsonConvert.DeserializeObject<AffirmationContainer>(json)?.Affirmations;
				}
				catch (FileNotFoundException e)
				{
					Console.WriteLine($"The affirmations file was not found: '{e}'");
				}
				catch (DirectoryNotFoundException e)
				{
					Console.WriteLine($"The directory was not found: '{e}'");
				}
				catch (IOException e)
				{
					Console.WriteLine($"The affirmations file could not be opened: '{e}'");
				}
                return null;
			}
		}
		public class StringService
		{
			public static Affirmation[]? GetStrings(IReader reader)
			{
				return reader.ReadAll();
			}
		}

		private readonly List<DispatcherTimer> timersList = [];
        public class Affirmation
		{
			public int Id { get; set; }
			public string? Text { get; set; }
		}
		public class AffirmationContainer
		{
			public Affirmation[]? Affirmations { get; set; }
            private int _affirmationNumber;
            public int currentAffirmation { 
                get { return _affirmationNumber; }
                set { if (value >= Affirmations?.Length) _affirmationNumber = 0; 
                    else _affirmationNumber = value;} 
            }
            public AffirmationContainer()
            {
                Affirmations = [];
				currentAffirmation = 0;
			}
		}

		public AffirmationContainer? affirmationsContainer;

        readonly int timersMaxAmount = 96;

		public MainWindow()
        {
            InitializeComponent();
            StartClock();
            ReadStrings();
        }
		public void ReadStrings()
		{
            affirmationsContainer = new AffirmationContainer();
            var fileReader = new FileReader("\\affirmations.json");
			affirmationsContainer.Affirmations = StringService.GetStrings(fileReader);
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

        private void TimerTickEvent(object? sender, EventArgs e)
        {
            if (timersList.Count > 0)
            {
                var uri = new Uri(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\wav\\alarm1.wav");
                var player = new MediaPlayer();
                player.Open(uri);
                player.Play();

                timersList[0].Stop();
                timersList.RemoveAt(0);
                lstTimers.Items.RemoveAt(0);
                if (timersList.Count > 0)
                {
                    timersList[0].Start();
                    lstTimers.Items[0]+= DateTime.Now.AddMinutes(timersList[0].Interval.TotalMinutes).ToString(@"hh:mm:ss");
                }
            }
        }

        private void ClockUpdate()
        {
            timerLabel.Text = DateTime.Now.ToString(@"hh:mm:ss");
        }
		private void BtnInterval05_Click(object sender, RoutedEventArgs e)
		{
            ButtonAction(sender.ToString());
		}
		private void BtnInterval10_Click(object sender, RoutedEventArgs e)
		{
			ButtonAction(sender.ToString());
		}
		private void BtnInterval30_Click(object sender, RoutedEventArgs e)
		{
			ButtonAction(sender.ToString());
		}
		private void BtnInterval50_Click(object sender, RoutedEventArgs e)
        {
			ButtonAction(sender.ToString());
		}
		private void btnInterval55_Click(object sender, RoutedEventArgs e)
		{
			ButtonAction(sender.ToString());
		}
		private void ButtonAction(string input)
        {
			if (CheckTimersAmount())
			{
				int number = 0;
				Match match = Regex.Match(input, @"\d+");
				if (match.Success)
				{
					int.TryParse(match.Value, out number);
				}
				SetupNewTimer(number);
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
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = lstTimers.SelectedIndex;
            if (lstTimers.Items.Count > 0 && selectedIndex >= 0)
            {
                if (timersList.Count > 0 && lstTimers.Items.Count > 0)
                {
                    timersList[selectedIndex].Stop();
                    timersList.RemoveAt(selectedIndex);
					lstTimers.Items.RemoveAt(selectedIndex);
					if (selectedIndex == 0 && timersList.Count >= 1)
                    {
                        timersList[0].Start();
						lstTimers.Items[0] += DateTime.Now.AddMinutes(timersList[0].Interval.TotalMinutes).ToString(@"hh:mm:ss");
					}
                }
            }
        }
        private void LstTimers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstTimers.SelectedIndex >= 0) {
                btnDelete.Visibility =  Visibility.Visible;
            } else btnDelete.Visibility = Visibility.Collapsed;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
	}
}