using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class DutyManualView : UserControl
    {
        private readonly List<DutyPostItem> dutyManualItems = new List<DutyPostItem>();
        private readonly ObservableCollection<DutyPostItem> filteredDutyManualItems = new ObservableCollection<DutyPostItem>();

        public DutyManualView()
        {
            InitializeComponent();

            DutyListBox.ItemsSource = filteredDutyManualItems;
            SeedDutyManualItems();
            ApplyDutyManualFilter();
        }

        private void DutySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyDutyManualFilter();
        }

        private void ApplyDutyManualFilter()
        {
            string keyword = DutySearchTextBox == null ? string.Empty : DutySearchTextBox.Text.Trim();
            List<DutyPostItem> matches = dutyManualItems
                .Where(item => string.IsNullOrWhiteSpace(keyword) || item.Contains(keyword))
                .OrderBy(item => item.Title)
                .ToList();

            filteredDutyManualItems.Clear();
            foreach (DutyPostItem item in matches)
            {
                filteredDutyManualItems.Add(item);
            }
        }

        private void NewManualPostButton_Click(object sender, RoutedEventArgs e)
        {
            ManualPostInputResult result = ShowManualPostInputDialog();
            if (result == null)
            {
                return;
            }

            dutyManualItems.Add(new DutyPostItem(
                result.Title,
                result.Detail,
                result.Category,
                Environment.UserName,
                DateTime.Now));

            DutySearchTextBox.Text = string.Empty;
            ApplyDutyManualFilter();
        }

        private void ManualTitleButton_Click(object sender, RoutedEventArgs e)
        {
            DutyPostItem item = (sender as Button)?.CommandParameter as DutyPostItem;
            if (item == null)
            {
                return;
            }

            string message = item.Detail
                + Environment.NewLine
                + Environment.NewLine
                + item.Category
                + Environment.NewLine
                + item.LastModifiedLabel;

            MessageBox.Show(message, item.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private ManualPostInputResult ShowManualPostInputDialog()
        {
            Window dialog = new Window
            {
                Title = "당직 매뉴얼 게시글 작성",
                Width = 460,
                Height = 430,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };

            Grid layout = new Grid
            {
                Margin = new Thickness(18)
            };
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock titleLabel = CreateDialogLabel("제목");
            TextBox titleInput = CreateDialogTextBox();
            TextBlock categoryLabel = CreateDialogLabel("분류");
            TextBox categoryInput = CreateDialogTextBox();
            TextBlock detailLabel = CreateDialogLabel("내용");
            TextBox detailInput = CreateDialogTextBox();
            detailInput.AcceptsReturn = true;
            detailInput.TextWrapping = TextWrapping.Wrap;
            detailInput.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            detailInput.MinHeight = 150;

            StackPanel titlePanel = CreateDialogField(titleLabel, titleInput);
            StackPanel categoryPanel = CreateDialogField(categoryLabel, categoryInput);
            StackPanel detailPanel = CreateDialogField(detailLabel, detailInput);

            Grid.SetRow(titlePanel, 0);
            Grid.SetRow(categoryPanel, 1);
            Grid.SetRow(detailPanel, 2);
            layout.Children.Add(titlePanel);
            layout.Children.Add(categoryPanel);
            layout.Children.Add(detailPanel);

            StackPanel buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 16, 0, 0)
            };

            Button saveButton = new Button
            {
                Content = "저장",
                Width = 72,
                Height = 32,
                Margin = new Thickness(0, 0, 8, 0)
            };
            Button cancelButton = new Button
            {
                Content = "취소",
                Width = 72,
                Height = 32
            };

            saveButton.Click += delegate
            {
                if (string.IsNullOrWhiteSpace(titleInput.Text) || string.IsNullOrWhiteSpace(detailInput.Text))
                {
                    MessageBox.Show("제목과 내용을 입력해주세요.", "EZHOWTOUSE", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                dialog.Tag = new ManualPostInputResult
                {
                    Title = titleInput.Text.Trim(),
                    Detail = detailInput.Text.Trim(),
                    Category = string.IsNullOrWhiteSpace(categoryInput.Text) ? "당직 매뉴얼" : categoryInput.Text.Trim()
                };
                dialog.DialogResult = true;
            };

            cancelButton.Click += delegate
            {
                dialog.DialogResult = false;
            };

            buttons.Children.Add(saveButton);
            buttons.Children.Add(cancelButton);
            Grid.SetRow(buttons, 4);
            layout.Children.Add(buttons);

            dialog.Content = layout;
            bool? dialogResult = dialog.ShowDialog();
            return dialogResult == true ? dialog.Tag as ManualPostInputResult : null;
        }

        private static TextBlock CreateDialogLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 5)
            };
        }

        private static TextBox CreateDialogTextBox()
        {
            return new TextBox
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(8),
                FontSize = 14,
                MinHeight = 34
            };
        }

        private static StackPanel CreateDialogField(TextBlock label, TextBox input)
        {
            StackPanel panel = new StackPanel
            {
                Margin = new Thickness(0, 0, 0, 12)
            };

            panel.Children.Add(label);
            panel.Children.Add(input);
            return panel;
        }

        private void SeedDutyManualItems()
        {
            dutyManualItems.Add(new DutyPostItem(
                "평일 야간 당직 점검",
                "18:00 이후 서버 상태, 백업 결과, 주요 시스템 접속 가능 여부를 확인합니다.",
                "기본 업무",
                "system",
                DateTime.Now.AddHours(-3)));

            dutyManualItems.Add(new DutyPostItem(
                "주말 당직 인수인계",
                "전일 미처리 건, 장애 조치 이력, 예정 작업을 확인하고 당직 일지에 기록합니다.",
                "인수인계",
                "system",
                DateTime.Now.AddDays(-1)));

            dutyManualItems.Add(new DutyPostItem(
                "장애 발생 시 보고 절차",
                "장애 범위와 영향도를 파악한 뒤 담당자와 관리자에게 순서대로 보고합니다.",
                "장애 대응",
                "system",
                DateTime.Now.AddDays(-2)));

            dutyManualItems.Add(new DutyPostItem(
                "배치 작업 실패 확인",
                "배치 실패 알림이 있으면 로그와 재처리 가능 여부를 확인하고 담당자에게 공유합니다.",
                "배치",
                "system",
                DateTime.Now.AddDays(-4)));
        }

        private class ManualPostInputResult
        {
            public string Title { get; set; }

            public string Detail { get; set; }

            public string Category { get; set; }
        }
    }
}
