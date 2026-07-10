using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOWTOUSE
{
    public partial class DutyScheduleView : UserControl
    {
        private readonly ObservableCollection<DutyCalendarDay> calendarDays = new ObservableCollection<DutyCalendarDay>();
        private bool isCalendarInitializing;

        public DutyScheduleView()
        {
            InitializeComponent();
            CalendarItemsControl.ItemsSource = calendarDays;
            InitializeCalendar();
        }

        private void InitializeCalendar()
        {
            isCalendarInitializing = true;
            MonthComboBox.ItemsSource = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            YearComboBox.ItemsSource = Enumerable.Range(DateTime.Today.Year - 2, 6).ToList();
            MonthComboBox.SelectedIndex = DateTime.Today.Month - 1;
            YearComboBox.SelectedItem = DateTime.Today.Year;
            isCalendarInitializing = false;
            RenderCalendar();
        }

        private void CalendarSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isCalendarInitializing)
            {
                RenderCalendar();
            }
        }

        private void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            MoveCalendarMonth(-1);
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            MoveCalendarMonth(1);
        }

        private void MoveCalendarMonth(int offset)
        {
            DateTime current = new DateTime((int)YearComboBox.SelectedItem, MonthComboBox.SelectedIndex + 1, 1).AddMonths(offset);
            if (!YearComboBox.Items.Contains(current.Year))
            {
                YearComboBox.ItemsSource = Enumerable.Range(current.Year - 2, 6).ToList();
            }

            YearComboBox.SelectedItem = current.Year;
            MonthComboBox.SelectedIndex = current.Month - 1;
            RenderCalendar();
        }

        private void RenderCalendar()
        {
            if (MonthComboBox.SelectedIndex < 0 || YearComboBox.SelectedItem == null)
            {
                return;
            }

            calendarDays.Clear();
            int year = (int)YearComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex + 1;
            DateTime firstDay = new DateTime(year, month, 1);
            DateTime start = firstDay.AddDays(-(int)firstDay.DayOfWeek);
            DateTime today = DateTime.Today;

            for (int index = 0; index < 42; index++)
            {
                DateTime date = start.AddDays(index);
                bool isCurrentMonth = date.Month == month;
                bool isDutyDay = isCurrentMonth && (date.Day == 9 || date.Day == 13 || date.Day == 24);
                bool isToday = date.Date == today;
                calendarDays.Add(new DutyCalendarDay(date.Day.ToString(), isCurrentMonth, isDutyDay, isToday));
            }
        }
    }

    public class DutyCalendarDay
    {
        public DutyCalendarDay(string dayText, bool isCurrentMonth, bool isDutyDay, bool isToday)
        {
            DayText = dayText;
            Background = isDutyDay ? new SolidColorBrush(Color.FromRgb(49, 49, 49)) : isToday ? new SolidColorBrush(Color.FromRgb(219, 234, 254)) : isCurrentMonth ? new SolidColorBrush(Color.FromRgb(248, 250, 252)) : Brushes.Transparent;
            Foreground = isDutyDay ? Brushes.White : isCurrentMonth ? new SolidColorBrush(Color.FromRgb(31, 41, 55)) : new SolidColorBrush(Color.FromRgb(180, 185, 191));
        }

        public string DayText { get; private set; }
        public Brush Background { get; private set; }
        public Brush Foreground { get; private set; }
    }
}
