using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class DutySwapView : UserControl
    {
        private readonly ObservableCollection<string> swapRequests = new ObservableCollection<string>();

        public DutySwapView()
        {
            InitializeComponent();
            SwapListBox.ItemsSource = swapRequests;
        }

        private void SaveSwapButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = SwapDatePicker.SelectedDate;
            string reason = SwapReasonInput.Text.Trim();
            if (!selectedDate.HasValue || string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("교환 희망일과 요청 내용을 입력하세요.", "EZHOWTOUSE", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string date = selectedDate.Value.ToString("yyyy-MM-dd");
            swapRequests.Insert(0, string.Format("{0:yyyy-MM-dd HH:mm} · {1} · {2}", DateTime.Now, date, reason));
            SwapDatePicker.SelectedDate = null;
            SwapReasonInput.Text = string.Empty;
        }
    }
}
