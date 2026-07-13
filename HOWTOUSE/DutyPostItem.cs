using System;

namespace HOWTOUSE
{
    public class DutyPostItem
    {
        private bool isCompleted;

        public DutyPostItem(string title, string detail, string category, string modifiedBy, DateTime modifiedAt)
        {
            Category = category;
            Update(title, detail, modifiedBy, modifiedAt);
        }

        public string Title { get; private set; }

        public string Detail { get; private set; }

        public string Category { get; private set; }

        public string LastModifiedLabel { get; private set; }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        public void Update(string newTitle, string newDetail, string modifiedBy, DateTime modifiedAt)
        {
            Title = newTitle;
            Detail = newDetail;
            LastModifiedLabel = string.Format("마지막 수정: {0:yyyy-MM-dd HH:mm} · {1}", modifiedAt, modifiedBy);
        }

        public bool Contains(string keyword)
        {
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;
            return Title.IndexOf(keyword, comparison) >= 0
                || Detail.IndexOf(keyword, comparison) >= 0
                || Category.IndexOf(keyword, comparison) >= 0;
        }
    }
}
