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
        private readonly List<ManualItem> popupManualItems;
        private readonly List<ManualItem> inquiryItems;
        private readonly ObservableCollection<ManualItem> filteredPopupManualItems = new ObservableCollection<ManualItem>();
        private readonly ObservableCollection<ManualItem> filteredInquiryItems = new ObservableCollection<ManualItem>();
        private readonly ObservableCollection<string> popupComments = new ObservableCollection<string>();

        public PopupManualView()
        {
            InitializeComponent();

            popupManualItems = CreatePopupManualItems();
            inquiryItems = CreateInquiryItems();

            PopupGuideListBox.ItemsSource = filteredPopupManualItems;
            InquiryListBox.ItemsSource = filteredInquiryItems;
            PopupCommentListBox.ItemsSource = popupComments;

            ApplyPopupManualFilter();
            ApplyInquiryFilter();
            SelectManualTab("Popup");
        }

        public void SelectManualTab(string tabName)
        {
            bool isPopup = tabName == "Popup";

            PopupManualPanel.Visibility = isPopup ? Visibility.Visible : Visibility.Collapsed;
            InquiryPanel.Visibility = isPopup ? Visibility.Collapsed : Visibility.Visible;

            if (isPopup && PopupGuideListBox.SelectedItem == null && filteredPopupManualItems.Count > 0)
            {
                PopupGuideListBox.SelectedIndex = 0;
            }

            if (!isPopup && InquiryListBox.SelectedItem == null && filteredInquiryItems.Count > 0)
            {
                InquiryListBox.SelectedIndex = 0;
            }
        }

        private void PopupSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyPopupManualFilter();
        }

        private void ApplyPopupManualFilter()
        {
            string keyword = PopupSearchTextBox == null ? string.Empty : PopupSearchTextBox.Text.Trim();
            List<ManualItem> matches = popupManualItems
                .Where(item => string.IsNullOrWhiteSpace(keyword) || item.Contains(keyword))
                .OrderBy(item => item.Title)
                .ToList();

            filteredPopupManualItems.Clear();
            foreach (ManualItem item in matches)
            {
                filteredPopupManualItems.Add(item);
            }

            if (filteredPopupManualItems.Count > 0)
            {
                PopupGuideListBox.SelectedIndex = 0;
            }
        }

        private void PopupGuideListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManualItem item = PopupGuideListBox.SelectedItem as ManualItem;
            if (item == null) return;

            PopupTitleTextBlock.Text = item.Title;
            PopupSummaryTextBlock.Text = item.Summary;
            PopupKeywordsTextBlock.Text = "키워드: " + item.KeywordsLabel;
            PopupOwnerTextBlock.Text = item.Owner + " 담당자만 수정 가능합니다.";
            PopupStepsItemsControl.ItemsSource = CreateGuideSteps(item.Steps);
        }

        private void AddPopupCommentButton_Click(object sender, RoutedEventArgs e)
        {
            string text = PopupCommentInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            popupComments.Insert(0, string.Format("{0:yyyy-MM-dd HH:mm} · {1}: {2}", DateTime.Now, Environment.UserName, text));
            PopupCommentInput.Text = string.Empty;
        }

        private void InquirySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyInquiryFilter();
        }

        private void ApplyInquiryFilter()
        {
            string keyword = InquirySearchTextBox == null ? string.Empty : InquirySearchTextBox.Text.Trim();
            List<ManualItem> matches = inquiryItems
                .Where(item => string.IsNullOrWhiteSpace(keyword) || item.Contains(keyword))
                .OrderBy(item => item.Title)
                .ToList();

            filteredInquiryItems.Clear();
            foreach (ManualItem item in matches)
            {
                filteredInquiryItems.Add(item);
            }

            if (filteredInquiryItems.Count > 0)
            {
                InquiryListBox.SelectedIndex = 0;
            }
        }

        private void InquiryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManualItem item = InquiryListBox.SelectedItem as ManualItem;
            if (item == null) return;

            InquiryTitleTextBlock.Text = item.Title;
            InquirySummaryTextBlock.Text = item.Summary;
            InquiryKeywordsTextBlock.Text = "키워드: " + item.KeywordsLabel;
            InquiryStepsItemsControl.ItemsSource = CreateGuideSteps(item.Steps);
        }

        private void AddInquiryButton_Click(object sender, RoutedEventArgs e)
        {
            string text = NewInquiryInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            ManualItem item = new ManualItem(
                "신규 문의사항",
                "신규, 문의, 접수",
                text,
                "전산실",
                new[] { "문의 내용을 확인합니다.", "담당자가 처리 가능 여부를 검토합니다.", "처리 결과를 사용자에게 안내합니다." });

            inquiryItems.Insert(0, item);
            NewInquiryInput.Text = string.Empty;
            ApplyInquiryFilter();
            InquiryListBox.SelectedItem = item;
        }

        private static List<GuideStep> CreateGuideSteps(IEnumerable<string> steps)
        {
            return steps.Select((step, index) => new GuideStep { Number = index + 1, Text = step }).ToList();
        }

        private static List<ManualItem> CreatePopupManualItems()
        {
            return new List<ManualItem>
            {
                new ManualItem(
                    "로그인 오류 팝업",
                    "로그인, 비밀번호, 계정, 접근",
                    "로그인 실패 또는 계정 잠금 팝업이 뜰 때 확인하는 절차입니다.",
                    "계정 담당자",
                    new[] {
                        "팝업 메시지에 표시된 사용자 ID와 발생 시간을 확인합니다.",
                        "계정 상태와 잠금 여부를 관리자 화면에서 조회합니다.",
                        "필요 시 비밀번호 초기화 또는 잠금 해제를 진행합니다.",
                        "동일 오류가 반복되면 처리 이력을 댓글로 남깁니다."
                    }),
                new ManualItem(
                    "프린터 오류 메시지",
                    "프린터, 출력, 용지, 연결",
                    "출력 실패, 프린터 연결 안 됨, 용지 없음 메시지가 뜰 때 확인합니다.",
                    "장비 담당자",
                    new[] {
                        "오류 메시지에 표시된 프린터 이름을 확인합니다.",
                        "프린터 전원, 네트워크, 용지 상태를 점검합니다.",
                        "사용자 PC의 기본 프린터와 드라이버 상태를 확인합니다.",
                        "테스트 출력 후 결과를 댓글로 기록합니다."
                    }),
                new ManualItem(
                    "시스템 접속 불가",
                    "접속, 서버, 네트워크, 페이지",
                    "시스템 접속 실패 또는 페이지가 열리지 않을 때 확인하는 절차입니다.",
                    "시스템 담당자",
                    new[] {
                        "사용자가 접속한 URL과 발생 시간을 확인합니다.",
                        "내부망에서 같은 주소 접속이 가능한지 확인합니다.",
                        "서버 상태와 최근 배포 여부를 점검합니다.",
                        "장애가 맞으면 공지 후 조치 이력을 남깁니다."
                    })
            };
        }

        private static List<ManualItem> CreateInquiryItems()
        {
            return new List<ManualItem>
            {
                new ManualItem(
                    "비밀번호 초기화 요청",
                    "비밀번호, 초기화, 로그인",
                    "사용자가 비밀번호를 잊어버렸을 때 접수되는 대표 문의입니다.",
                    "계정 담당자",
                    new[] {
                        "사용자 사번과 이름을 확인합니다.",
                        "본인 확인 후 비밀번호 초기화 가능 여부를 판단합니다.",
                        "초기화 완료 후 사용자에게 임시 비밀번호 또는 재설정 절차를 안내합니다."
                    }),
                new ManualItem(
                    "메뉴 권한 요청",
                    "권한, 메뉴, 접근, 사용자",
                    "특정 메뉴가 보이지 않거나 접근할 수 없을 때 접수되는 문의입니다.",
                    "권한 담당자",
                    new[] {
                        "요청 메뉴명과 필요한 사유를 확인합니다.",
                        "사용자 부서와 직무 기준으로 권한 부여 가능 여부를 검토합니다.",
                        "승인 후 권한을 반영하고 재로그인을 안내합니다."
                    }),
                new ManualItem(
                    "자료 조회 결과가 다름",
                    "조회, 데이터, 결과, 오류",
                    "사용자가 조회한 결과가 예상과 다르다고 문의할 때 확인합니다.",
                    "시스템 담당자",
                    new[] {
                        "조회 조건과 화면 캡처를 요청합니다.",
                        "동일 조건으로 재현 가능한지 확인합니다.",
                        "데이터 기준 또는 프로그램 오류 여부를 구분해 안내합니다."
                    })
            };
        }
    }

    public class ManualItem
    {
        public ManualItem(string title, string keywords, string summary, string owner, IEnumerable<string> steps)
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
            return Title.IndexOf(keyword, comparison) >= 0
                || Keywords.IndexOf(keyword, comparison) >= 0
                || Summary.IndexOf(keyword, comparison) >= 0
                || Steps.Any(step => step.IndexOf(keyword, comparison) >= 0);
        }
    }

    public class GuideStep
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
