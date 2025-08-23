using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace DriverApp.Views
{
    public partial class HomePage : ContentView
    {
        public ObservableCollection<TaskItem> TaskList { get; set; }

        public HomePage()
        {
            InitializeComponent();

            TaskList = new ObservableCollection<TaskItem>
            {
                new TaskItem { Task = "Morning Shift", Hours = "4", Earnings = "$50" },
                new TaskItem { Task = "Afternoon Delivery", Hours = "3", Earnings = "$40" },
                new TaskItem { Task = "Evening Shift", Hours = "5", Earnings = "$70" }
            };

            TaskCollection.ItemsSource = TaskList;
        }

        private void OnDutySwitchToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                DutyStatusLabel.Text = "Currently On Duty";
                DutyStatusLabel.TextColor = Colors.Green;
            }
            else
            {
                DutyStatusLabel.Text = "Currently Off Duty";
                DutyStatusLabel.TextColor = Colors.Red;
            }
        }
    }

    public class TaskItem
    {
        public string? Task { get; set; }
        public string? Hours { get; set; }
        public string? Earnings { get; set; }
    }
}
