using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class DutyManualView : UserControl
    {
        private readonly ObservableCollection<DutyManualItem> dutyItems = new ObservableCollection<DutyManualItem>();
        private readonly ObservableCollection<string> auditLogs = new ObservableCollection<string>();

        public DutyManualView()
        {
            InitializeComponent();
            DutyListBox.ItemsSource = dutyItems;
            DutyAuditListBox.ItemsSource = auditLogs;
            SeedDutyItems();
        }

        private void SaveDutyButton_Click(object sender, RoutedEventArgs e)
        {
            string title = DutyTitleInput.Text.Trim();
            string detail = DutyDetailInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(detail))
            {
                MessageBox.Show("당직 제목과 내용을 입력하세요.", "EZHOWTOUSE", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string user = Environment.UserName;
            DateTime now = DateTime.Now;
            dutyItems.Add(new DutyManualItem(title, detail, user, now));
            auditLogs.Insert(0, string.Format("{0:yyyy-MM-dd HH:mm} · {1} 수정 · {2}", now, user, title));
            DutyTitleInput.Text = string.Empty;
            DutyDetailInput.Text = string.Empty;
        }

        private void SeedDutyItems()
        {
            dutyItems.Add(new DutyManualItem("평일 야간 당직", "18:00 이후 서버실 점검, 백업 상태 확인, 긴급 연락망 유지", "system", DateTime.Now));
            dutyItems.Add(new DutyManualItem("주말 당직", "장애 접수 확인, 네트워크 장비 알림 확인, 처리 결과 기록", "system", DateTime.Now));
        }
    }

    public class DutyManualItem
    {
        public DutyManualItem(string title, string detail, string modifiedBy, DateTime modifiedAt)
        {
            Title = title;
            Detail = detail;
            LastModifiedLabel = string.Format("마지막 수정: {0:yyyy-MM-dd HH:mm} · {1}", modifiedAt, modifiedBy);
        }

        public string Title { get; private set; }
        public string Detail { get; private set; }
        public string LastModifiedLabel { get; private set; }
    }
}
