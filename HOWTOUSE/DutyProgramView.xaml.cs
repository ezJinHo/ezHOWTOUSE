using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class DutyProgramView : UserControl
    {
        private DutyManualView dutyManualView;
        private DutyTodayTaskView dutyTodayTaskView;
        private DutyScheduleView dutyScheduleView;
        private DutySwapView dutySwapView;

        public DutyProgramView()
        {
            InitializeComponent();
            SelectDutyTab("Manual");
        }

        public void SelectDutyTab(string target)
        {
            if (target == "Manual") CreateDutyManual();
            if (target == "Today") CreateDutyTodayTask();
            if (target == "Schedule") CreateDutySchedule();
            if (target == "Swap") CreateDutySwap();
        }

        private void DutyTabButton_Click(object sender, RoutedEventArgs e)
        {
            string target = (sender as Button)?.Tag?.ToString() ?? "Manual";
            SelectDutyTab(target);
        }

        private void CreateDutyManual()
        {
            if (dutyManualView == null)
            {
                dutyManualView = new DutyManualView();
            }

            DutyContentControl.Content = dutyManualView;
        }

        private void CreateDutyTodayTask()
        {
            if (dutyTodayTaskView == null)
            {
                dutyTodayTaskView = new DutyTodayTaskView();
            }

            DutyContentControl.Content = dutyTodayTaskView;
        }

        private void CreateDutySchedule()
        {
            if (dutyScheduleView == null)
            {
                dutyScheduleView = new DutyScheduleView();
            }

            DutyContentControl.Content = dutyScheduleView;
        }

        private void CreateDutySwap()
        {
            if (dutySwapView == null)
            {
                dutySwapView = new DutySwapView();
            }

            DutyContentControl.Content = dutySwapView;
        }
    }
}
