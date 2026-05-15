using System;
using Sirenix.OdinInspector;

namespace HungNT.Database
{
    /// <summary>Một bảng (SO) ánh xạ tới tab trên spreadsheet. Type dữ liệu lấy từ <see cref="TableAsset"/>.</summary>
    [Serializable]
    public class SheetProfile
    {
        [TableColumnWidth(140, Resizable = true)]
        public string TableName = "ItemTable";

        [ListDrawerSettings(ListElementLabelName = "SheetName")]
        public WorksheetInfo[] Worksheets = { new WorksheetInfo("Sheet1") };

        [TableColumnWidth(220, Resizable = true)]
        [AssetsOnly]
        [AssetSelector(Paths = "Assets/")]
        public BaseDataTable TableAsset;
    }
}