using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class DutyTodayTaskView : UserControl
    {
        private readonly ObservableCollection<DutyPostItem> todayTaskItems = new ObservableCollection<DutyPostItem>();
        private DutyPostItem selectedTodayTaskItem;
        private bool isNewTodayTaskMode;

        public DutyTodayTaskView()
        {
            InitializeComponent();

            TodayTaskListBox.ItemsSource = todayTaskItems;
            DutyDateTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " 기준";

            SeedTodayTasks();
            SelectTodayTask(todayTaskItems.FirstOrDefault());
        }

        private void NewTodayTaskButton_Click(object sender, RoutedEventArgs e)
        {
            isNewTodayTaskMode = true;
            selectedTodayTaskItem = null;
            TodayTaskListBox.SelectedItem = null;
            TodayTaskEditorTitleTextBlock.Text = "확인 업무 추가";
            TodayTaskTitleInput.Text = string.Empty;
            TodayTaskDetailInput.Text = string.Empty;
            SaveTodayTaskButton.Content = "저장";
            TodayTaskTitleInput.Focus();
        }

        private void TodayTaskTitleButton_Click(object sender, RoutedEventArgs e)
        {
            DutyPostItem item = (sender as Button)?.CommandParameter as DutyPostItem;
            SelectTodayTask(item);
        }

        private void SaveTodayTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TodayTaskTitleInput.Text.Trim();
            string detail = TodayTaskDetailInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(detail))
            {
                MessageBox.Show("확인 업무의 제목과 내용을 입력해주세요.", "EZHOWTOUSE", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (isNewTodayTaskMode || selectedTodayTaskItem == null)
            {
                DutyPostItem newItem = new DutyPostItem(title, detail, "오늘 확인 업무", Environment.UserName, DateTime.Now);
                todayTaskItems.Insert(0, newItem);
                SelectTodayTask(newItem);
                TodayTaskListBox.SelectedItem = newItem;
                TodayTaskListBox.Items.Refresh();
                return;
            }

            selectedTodayTaskItem.Update(title, detail, Environment.UserName, DateTime.Now);
            SelectTodayTask(selectedTodayTaskItem);
            TodayTaskListBox.Items.Refresh();
        }

        private void SelectTodayTask(DutyPostItem item)
        {
            isNewTodayTaskMode = false;
            selectedTodayTaskItem = item;
            TodayTaskEditorTitleTextBlock.Text = "상세 내용";
            SaveTodayTaskButton.Content = "수정";

            if (item == null)
            {
                TodayTaskTitleInput.Text = string.Empty;
                TodayTaskDetailInput.Text = string.Empty;
                return;
            }

            TodayTaskTitleInput.Text = item.Title;
            TodayTaskDetailInput.Text = item.Detail;
        }

        private void SeedTodayTasks()
        {
            todayTaskItems.Add(new DutyPostItem(
                "서버 접속 상태 확인",
                "당직 시작 전 서버, 네트워크, 주요 업무 시스템 접속 상태를 확인합니다.",
                "오늘 확인 업무",
                "system",
                DateTime.Now));

            todayTaskItems.Add(new DutyPostItem(
                "백업 및 배치 결과 확인",
                "백업 결과와 배치 작업 실패 여부를 확인합니다.",
                "오늘 확인 업무",
                "system",
                DateTime.Now));
        }
    }
}
