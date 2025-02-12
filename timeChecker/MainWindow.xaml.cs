using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace timeChecker
{
	public partial class MainWindow : Window
    {
        public ObservableCollection<int> NumberList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            StartClock();
            ReadStrings();

            InitializeComponent();
            NumberList = new ObservableCollection<int> { 5, 10, 30, 50, 55 }; // Initial values
            NumberComboBox.ItemsSource = NumberList;
        }
        private void NumberComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NumberComboBox.Text))
            {
                if (int.TryParse(NumberComboBox.Text, out int newValue))
                {
                    if (!NumberList.Contains(newValue))
                    {
                        NumberList.Add(newValue);
                        SortNumberList();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.");
                    NumberComboBox.Text = "";
                }
            }
        }
        private void SortNumberList()
        {
            var sortedList = NumberList.OrderBy(n => n).ToList();
            NumberList.Clear();
            foreach (var num in sortedList)
            {
                NumberList.Add(num);
            }
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
                if(number > 0 && number < 60) SetupNewTimer(number);
			}
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

        private void btnIntervalAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtonAction(NumberComboBox.Text);
        }
    }
}