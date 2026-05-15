using System;
using System.Collections.Generic;

namespace HungNT.Database.Editor
{
    /// <summary>Bỏ cột có header bắt đầu bằng <c>##</c>, và bỏ hàng có ô đầu là <c>##</c>.</summary>
    public static class SheetDataFilter
    {
        public static List<string[]> ApplyCommentConvention(List<string[]> raw)
        {
            if (raw == null || raw.Count == 0)
                return raw ?? new List<string[]>();

            var header = raw[0];
            if (header == null || header.Length == 0)
                return raw;

            var keepIndices = new List<int>();
            for (int c = 0; c < header.Length; c++)
            {
                var h = header[c]?.Trim() ?? "";
                if (h.StartsWith("##", StringComparison.Ordinal))
                   continue;
                keepIndices.Add(c);
            }

            var result = new List<string[]>();
            for (int r = 0; r < raw.Count; r++)
            {
                var row = raw[r];
                if (row == null || row.Length == 0)
                    continue;

                var first = row.Length > 0 ? (row[0]?.Trim() ?? "") : "";
                if (first.StartsWith("##", StringComparison.Ordinal))
                    continue;

                var newRow = new string[keepIndices.Count];
                for (int k = 0; k < keepIndices.Count; k++)
                {
                    int src = keepIndices[k];
                    newRow[k] = src < row.Length ? row[src] ?? "" : "";
                }

                result.Add(newRow);
            }

            return result;
        }
    }
}
