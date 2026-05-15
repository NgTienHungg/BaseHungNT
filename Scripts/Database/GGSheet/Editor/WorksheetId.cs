using System;

namespace HungNT.Database.Editor
{
    /// <summary>Identifies a worksheet slice inside a spreadsheet (for <see cref="DataSheet"/>).</summary>
    public readonly struct WorksheetId : IEquatable<WorksheetId>
    {
        public string SheetId { get; }
        public string WorksheetName { get; }
        public string FromCol { get; }
        public string ToCol { get; }

        public WorksheetId(string sheetId, string worksheetName, string fromCol = null, string toCol = null)
        {
            SheetId = sheetId ?? "";
            WorksheetName = worksheetName ?? "";
            FromCol = fromCol ?? "";
            ToCol = toCol ?? "";
        }

        public bool Equals(WorksheetId other) =>
            SheetId == other.SheetId
            && WorksheetName == other.WorksheetName
            && FromCol == other.FromCol
            && ToCol == other.ToCol;

        public override bool Equals(object obj) => obj is WorksheetId other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int h = 17;
                h = h * 23 + (SheetId?.GetHashCode() ?? 0);
                h = h * 23 + (WorksheetName?.GetHashCode() ?? 0);
                h = h * 23 + (FromCol?.GetHashCode() ?? 0);
                h = h * 23 + (ToCol?.GetHashCode() ?? 0);
                return h;
            }
        }

        public override string ToString() => $"{WorksheetName} ({SheetId})";

        public static bool operator ==(WorksheetId a, WorksheetId b) => a.Equals(b);

        public static bool operator !=(WorksheetId a, WorksheetId b) => !a.Equals(b);
    }
}
