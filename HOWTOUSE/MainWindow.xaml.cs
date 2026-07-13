using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOWTOUSE
{
    public partial class MainWindow : Window
    {
        private DutyProgramView dutyProgramView;
        private PopupManualView popupManualView;
        private SuggestionView suggestionView;
        private SurveyView surveyView;
        private bool isLoggedIn;

        public MainWindow()
        {
            InitializeComponent();
            UpdateLoginArea();
            CreateDutyProgram();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            string target = (sender as Button)?.Tag?.ToString() ?? "Duty";

            if (target == "Duty") CreateDutyProgram();
            if (target == "Manual") CreatePopupManual();
            if (target == "Suggestion") CreateSuggestion();
            if (target == "Survey") CreateSurvey();
        }

        private void CreateDutyProgram()
        {
            if (dutyProgramView == null)
            {
                dutyProgramView = new DutyProgramView();
            }

            MainContentControl.Content = dutyProgramView;
            UpdateSelectedMenu("Duty");
        }

        private void CreatePopupManual()
        {
            if (popupManualView == null)
            {
                popupManualView = new PopupManualView();
            }

            MainContentControl.Content = popupManualView;
            UpdateSelectedMenu("Manual");
        }

        private void CreateSuggestion()
        {
            if (suggestionView == null)
            {
                suggestionView = new SuggestionView();
            }

            MainContentControl.Content = suggestionView;
            UpdateSelectedMenu("Suggestion");
        }

        private void CreateSurvey()
        {
            if (surveyView == null)
            {
                surveyView = new SurveyView();
            }

            MainContentControl.Content = surveyView;
            UpdateSelectedMenu("Survey");
        }

        private void UpdateSelectedMenu(string selectedMenu)
        {
            ApplyMenuState(DutyNavButton, selectedMenu == "Duty");
            ApplyMenuState(ManualNavButton, selectedMenu == "Manual");
            ApplyMenuState(SuggestionNavButton, selectedMenu == "Suggestion");
            ApplyMenuState(SurveyNavButton, selectedMenu == "Survey");
        }

        private static void ApplyMenuState(Button button, bool isSelected)
        {
            button.Background = isSelected ? new SolidColorBrush(Color.FromRgb(14, 145, 245)) : Brushes.Transparent;
            button.Foreground = isSelected ? Brushes.White : new SolidColorBrush(Color.FromRgb(100, 116, 139));
        }

        public void SetLoginUser(string employeeNo, string userName)
        {
            string displayName = string.IsNullOrWhiteSpace(userName) ? employeeNo : userName;

            isLoggedIn = true;
            LoginUserNameTextBlock.Text = displayName;
            LoginUserRoleTextBlock.Text = employeeNo;
            UserInitialTextBlock.Text = GetUserInitial(displayName);
            UpdateLoginArea();
        }

        private void UpdateLoginArea()
        {
            UserInfoPanel.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        }

        private static string GetUserInitial(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return "EZ";
            }

            return userName.Substring(0, 1).ToUpper();
        }
    }
}
