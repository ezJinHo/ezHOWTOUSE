using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOWTOUSE
{
    public partial class DutyProgramView : UserControl
    {
        private DutyManualView dutyManualView;
        private DutyScheduleView dutyScheduleView;
        private DutySwapView dutySwapView;

        public DutyProgramView()
        {
            InitializeComponent();
            CreateDutyManual();
        }

        private void DutyTabButton_Click(object sender, RoutedEventArgs e)
        {
            string target = (sender as Button)?.Tag?.ToString() ?? "Manual";

            if (target == "Manual") CreateDutyManual();
            if (target == "Schedule") CreateDutySchedule();
            if (target == "Swap") CreateDutySwap();
        }

        private void CreateDutyManual()
        {
            if (dutyManualView == null)
            {
                dutyManualView = new DutyManualView();
            }

            DutyContentControl.Content = dutyManualView;
            UpdateSelectedTab("Manual");
        }

        private void CreateDutySchedule()
        {
            if (dutyScheduleView == null)
            {
                dutyScheduleView = new DutyScheduleView();
            }

            DutyContentControl.Content = dutyScheduleView;
            UpdateSelectedTab("Schedule");
        }

        private void CreateDutySwap()
        {
            if (dutySwapView == null)
            {
                dutySwapView = new DutySwapView();
            }

            DutyContentControl.Content = dutySwapView;
            UpdateSelectedTab("Swap");
        }

        private void UpdateSelectedTab(string selectedTab)
        {
            ApplyTabState(ManualTabButton, selectedTab == "Manual");
            ApplyTabState(ScheduleTabButton, selectedTab == "Schedule");
            ApplyTabState(SwapTabButton, selectedTab == "Swap");
        }

        private static void ApplyTabState(Button button, bool isSelected)
        {
            button.Background = isSelected ? new SolidColorBrush(Color.FromRgb(14, 145, 245)) : Brushes.Transparent;
            button.Foreground = isSelected ? Brushes.White : new SolidColorBrush(Color.FromRgb(100, 116, 139));
            button.BorderBrush = isSelected ? new SolidColorBrush(Color.FromRgb(14, 145, 245)) : Brushes.Transparent;
        }
    }
}
