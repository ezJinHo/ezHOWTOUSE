using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class SurveyView : UserControl
    {
        private readonly ObservableCollection<SurveyItem> surveys = new ObservableCollection<SurveyItem>();

        public SurveyView()
        {
            InitializeComponent();
            SurveyListBox.ItemsSource = surveys;
            UpdateSurveyAverage();
        }

        private void SaveSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            surveys.Insert(0, new SurveyItem((int)SatisfactionSlider.Value, SurveyCommentInput.Text.Trim(), DateTime.Now));
            SurveyCommentInput.Text = string.Empty;
            SatisfactionSlider.Value = 4;
            UpdateSurveyAverage();
        }

        private void UpdateSurveyAverage()
        {
            SurveyAverageText.Text = surveys.Count == 0 ? "아직 제출된 만족도 조사가 없습니다." : string.Format("평균 {0:0.0}점 · {1}건 제출", surveys.Average(item => item.Score), surveys.Count);
        }
    }

    public class SurveyItem
    {
        public SurveyItem(int score, string comment, DateTime createdAt)
        {
            Score = score;
            Comment = string.IsNullOrWhiteSpace(comment) ? "의견 없음" : comment;
            CreatedAt = createdAt;
        }

        public int Score { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm} · {1}점 · {2}", CreatedAt, Score, Comment);
        }
    }
}
