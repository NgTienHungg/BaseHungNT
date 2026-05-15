using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Database
{
    /// <summary>Một tab (worksheet) trên Google Sheet và khoảng cột tùy chọn.</summary>
    [Serializable]
    public class WorksheetInfo
    {
        [TableColumnWidth(140, Resizable = true)]
        [Tooltip("Tên tab hiển thị trên thanh dưới cùng của Google Sheets (phải khớp chính xác).")]
        public string SheetName;

        [LabelText("From column")]
        [TableColumnWidth(70, Resizable = true)]
        [Tooltip("Để trống = từ cột đầu. Ví dụ: A")]
        public string FromCol = "";

        [LabelText("To column")]
        [TableColumnWidth(70, Resizable = true)]
        [Tooltip("Để trống = đến cột cuối có dữ liệu. Ví dụ: Z")]
        public string ToCol = "";

        public WorksheetInfo()
        {
        }

        public WorksheetInfo(string sheetName)
        {
            SheetName = sheetName;
        }
    }
}
