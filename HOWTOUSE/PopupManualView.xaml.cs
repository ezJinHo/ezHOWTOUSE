using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HOWTOUSE
{
    public partial class PopupManualView : UserControl
    {
        private readonly List<GuideItem> guideItems;
        private readonly ObservableCollection<GuideItem> filteredGuideItems = new ObservableCollection<GuideItem>();
        private readonly ObservableCollection<string> comments = new ObservableCollection<string>();

        public PopupManualView()
        {
            InitializeComponent();
            guideItems = CreateGuideItems();
            GuideListBox.ItemsSource = filteredGuideItems;
            CommentListBox.ItemsSource = comments;
            ApplyManualFilter();
            GuideListBox.SelectedIndex = 0;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyManualFilter();
        }

        private void ApplyManualFilter()
        {
            string keyword = SearchTextBox == null ? string.Empty : SearchTextBox.Text.Trim();
            List<GuideItem> matches = guideItems.Where(item => string.IsNullOrWhiteSpace(keyword) || item.Contains(keyword)).OrderBy(item => item.Title).ToList();
            filteredGuideItems.Clear();
            foreach (GuideItem item in matches)
            {
                filteredGuideItems.Add(item);
            }

            if (filteredGuideItems.Count > 0)
            {
                GuideListBox.SelectedIndex = 0;
            }
        }

        private void GuideListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GuideItem item = GuideListBox.SelectedItem as GuideItem;
            if (item == null) return;

            TitleTextBlock.Text = item.Title;
            SummaryTextBlock.Text = item.Summary;
            KeywordsTextBlock.Text = "키워드: " + item.KeywordsLabel;
            OwnerTextBlock.Text = item.Owner + " 담당자만 수정 가능";
            StepsItemsControl.ItemsSource = item.Steps.Select((step, index) => new GuideStep { Number = index + 1, Text = step }).ToList();
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            string text = CommentInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;
            comments.Insert(0, string.Format("{0:yyyy-MM-dd HH:mm} · {1}: {2}", DateTime.Now, Environment.UserName, text));
            CommentInput.Text = string.Empty;
        }

        private static List<GuideItem> CreateGuideItems()
        {
            return new List<GuideItem>
            {
                new GuideItem("로그인 오류 팝업", "로그인 실패, 비밀번호 오류, 계정 잠김", "계정 상태를 확인하고 비밀번호 초기화 또는 잠금 해제를 진행합니다.", "계정 담당자", new[] { "팝업 메시지의 사용자 ID를 확인합니다.", "계정 잠김 여부를 관리자 화면에서 조회합니다.", "비밀번호 초기화 또는 잠금 해제 후 사용자에게 재시도 안내를 합니다.", "동일 오류가 반복되면 처리 내역을 댓글로 남깁니다." }),
                new GuideItem("인쇄 오류 메시지", "프린터 연결 안됨, 출력 실패, 용지 없음", "프린터 상태와 사용자 PC의 기본 프린터 설정을 확인합니다.", "장비 담당자", new[] { "오류 메시지와 프린터 이름을 확인합니다.", "프린터 전원, 네트워크, 용지 상태를 점검합니다.", "사용자 PC의 기본 프린터와 드라이버 상태를 확인합니다.", "처리 후 테스트 출력 결과를 댓글로 남깁니다." }),
                new GuideItem("시스템 접속 불가", "접속 실패, 서버 오류, 페이지 열리지 않음", "서버 상태와 네트워크 연결을 순서대로 확인합니다.", "시스템 담당자", new[] { "사용자가 접속한 URL과 발생 시간을 확인합니다.", "내부망에서 같은 주소 접속이 가능한지 확인합니다.", "서버 상태와 최근 배포 여부를 점검합니다.", "장애가 맞으면 공지 후 조치 이력을 기록합니다." })
            };
        }
    }

    public class GuideItem
    {
        public GuideItem(string title, string keywords, string summary, string owner, IEnumerable<string> steps)
        {
            Title = title;
            Keywords = keywords;
            Summary = summary;
            Owner = owner;
            Steps = steps.ToList();
        }

        public string Title { get; private set; }
        public string Keywords { get; private set; }
        public string Summary { get; private set; }
        public string Owner { get; private set; }
        public List<string> Steps { get; private set; }
        public string KeywordsLabel { get { return Keywords; } }

        public bool Contains(string keyword)
        {
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;
            return Title.IndexOf(keyword, comparison) >= 0 || Keywords.IndexOf(keyword, comparison) >= 0 || Summary.IndexOf(keyword, comparison) >= 0 || Steps.Any(step => step.IndexOf(keyword, comparison) >= 0);
        }
    }

    public class GuideStep
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
