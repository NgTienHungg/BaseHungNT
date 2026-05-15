using System;
using System.Collections.Generic;

namespace HungNT.Database.Editor
{
    internal static class ColumnRange
    {
        /// <summary>0-based start index from A1 letters, or 0 if empty.</summary>
        public static int LettersToZeroBasedIndex(string letters)
        {
            if (string.IsNullOrWhiteSpace(letters))
                return 0;

            int n = 0;
            foreach (char c in letters.Trim().ToUpperInvariant())
            {
                if (c < 'A' || c > 'Z')
                    break;
                n = n * 26 + (c - 'A' + 1);
            }

            return Math.Max(0, n - 1);
        }

        public static List<string[]> ApplyColumnSlice(IReadOnlyList<string[]> rows, string fromCol, string toCol)
        {
            if (rows == null || rows.Count == 0)
                return new List<string[]>();

            bool hasFrom = !string.IsNullOrWhiteSpace(fromCol);
            bool hasTo = !string.IsNullOrWhiteSpace(toCol);

            if (!hasFrom && !hasTo)
            {
                var copy = new List<string[]>(rows.Count);
                foreach (var r in rows)
                    copy.Add((string[])r.Clone());
                return copy;
            }

            int i0 = hasFrom ? LettersToZeroBasedIndex(fromCol) : 0;
            int i1 = hasTo ? LettersToZeroBasedIndex(toCol) : int.MaxValue;

            var result = new List<string[]>(rows.Count);
            foreach (var row in rows)
            {
                if (row == null || row.Length == 0)
                {
                    result.Add(Array.Empty<string>());
                    continue;
                }

                int end = Math.Min(row.Length - 1, i1);
                if (end < i0)
                {
                    result.Add(Array.Empty<string>());
                    continue;
                }

                int len = end - i0 + 1;
                var slice = new string[len];
                Array.Copy(row, i0, slice, 0, len);
                result.Add(slice);
            }

            return result;
        }
    }
}
