using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class SuggestionView : UserControl
    {
        private readonly ObservableCollection<string> suggestions = new ObservableCollection<string>();

        public SuggestionView()
        {
            InitializeComponent();
            SuggestionListBox.ItemsSource = suggestions;
        }

        private void SaveSuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            string title = SuggestionTitleInput.Text.Trim();
            string body = SuggestionBodyInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                MessageBox.Show("제안 제목과 필요한 이유를 입력하세요.", "EZHOWTOUSE", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            suggestions.Insert(0, string.Format("{0:yyyy-MM-dd HH:mm} · {1}\n{2}", DateTime.Now, title, body));
            SuggestionTitleInput.Text = string.Empty;
            SuggestionBodyInput.Text = string.Empty;
        }
    }
}
