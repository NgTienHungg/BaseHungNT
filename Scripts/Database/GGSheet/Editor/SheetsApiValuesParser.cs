using System;
using System.Collections.Generic;
using System.Text;

namespace HungNT.Database.Editor
{
    /// <summary>Parses the <c>values</c> array from a Google Sheets v4 <c>spreadsheets.values.get</c> JSON body.</summary>
    public static class SheetsApiValuesParser
    {
        public static List<string[]> Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new List<string[]>();

            int i = json.IndexOf("\"values\"", StringComparison.OrdinalIgnoreCase);
            if (i < 0)
            {
                int len = Math.Min(400, json.Length);
                string snip = len < json.Length ? json.Substring(0, len) + "…" : json;
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"Sheets API JSON has no 'values'".Color("red")} — " +
                    "thường là sai tên tab/range hoặc sheet trống. Phản hồi:\n" +
                    $"{snip.Color("orange")}");
                throw new InvalidOperationException("GGSheet: API response has no 'values' field. Raw: " + json.Substring(0, Math.Min(200, json.Length)));
            }

            i = json.IndexOf('[', i);
            if (i < 0)
                throw new InvalidOperationException("GGSheet: Malformed API JSON (missing '[' after values).");

            return ParseRowList(json, ref i);
        }

        private static List<string[]> ParseRowList(string s, ref int i)
        {
            var rows = new List<string[]>();
            Expect(s, ref i, '[');
            SkipWs(s, ref i);
            while (i < s.Length && s[i] != ']')
            {
                rows.Add(ParseCellList(s, ref i));
                SkipWs(s, ref i);
                if (i < s.Length && s[i] == ',')
                {
                    i++;
                    SkipWs(s, ref i);
                }
            }

            Expect(s, ref i, ']');
            return rows;
        }

        private static string[] ParseCellList(string s, ref int i)
        {
            var cells = new List<string>();
            Expect(s, ref i, '[');
            SkipWs(s, ref i);
            while (i < s.Length && s[i] != ']')
            {
                cells.Add(ParseCell(s, ref i));
                SkipWs(s, ref i);
                if (i < s.Length && s[i] == ',')
                {
                    i++;
                    SkipWs(s, ref i);
                }
            }

            Expect(s, ref i, ']');
            return cells.ToArray();
        }

        private static string ParseCell(string s, ref int i)
        {
            SkipWs(s, ref i);
            if (i >= s.Length)
                return string.Empty;

            if (s[i] == '"')
                return ParseJsonString(s, ref i);

            if (s[i] == 'n' && i + 3 < s.Length && s.Substring(i, 4) == "null")
            {
                i += 4;
                return string.Empty;
            }

            int start = i;
            while (i < s.Length && s[i] != ',' && s[i] != ']' && s[i] != '}' && !char.IsWhiteSpace(s[i]))
                i++;

            return s.Substring(start, i - start).Trim();
        }

        private static string ParseJsonString(string s, ref int i)
        {
            i++; // opening "
            var sb = new StringBuilder();
            while (i < s.Length)
            {
                char c = s[i];
                if (c == '\\' && i + 1 < s.Length)
                {
                    i++;
                    sb.Append(s[i]);
                    i++;
                    continue;
                }

                if (c == '"')
                {
                    i++;
                    return sb.ToString();
                }

                sb.Append(c);
                i++;
            }

            return sb.ToString();
        }

        private static void Expect(string s, ref int i, char ch)
        {
            SkipWs(s, ref i);
            if (i >= s.Length || s[i] != ch)
                throw new InvalidOperationException($"GGSheet: Expected '{ch}' at position {i}.");
            i++;
            SkipWs(s, ref i);
        }

        private static void SkipWs(string s, ref int i)
        {
            while (i < s.Length && char.IsWhiteSpace(s[i]))
                i++;
        }
    }
}
