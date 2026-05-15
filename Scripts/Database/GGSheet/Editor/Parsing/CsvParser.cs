using System.Collections.Generic;
using System.Text;

namespace HungNT.Database.Editor
{
    /// <summary>Parses CSV text (e.g. Google gviz export) into rows of string cells.</summary>
    public static class CsvParser
    {
        public static List<string[]> Parse(string csv)
        {
            var rows = new List<string[]>();
            if (string.IsNullOrEmpty(csv))
                return rows;

            var currentRow = new List<string>();
            var currentCell = new StringBuilder();
            bool inQuotes = false;
            int i = 0;

            while (i < csv.Length)
            {
                char c = csv[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < csv.Length && csv[i + 1] == '"')
                        {
                            currentCell.Append('"');
                            i += 2;
                            continue;
                        }

                        inQuotes = false;
                        i++;
                        continue;
                    }

                    currentCell.Append(c);
                    i++;
                    continue;
                }

                switch (c)
                {
                    case '"':
                        inQuotes = true;
                        i++;
                        break;
                    case ',':
                        currentRow.Add(currentCell.ToString());
                        currentCell.Clear();
                        i++;
                        break;
                    case '\r':
                        i++;
                        break;
                    case '\n':
                        currentRow.Add(currentCell.ToString());
                        currentCell.Clear();
                        rows.Add(currentRow.ToArray());
                        currentRow.Clear();
                        i++;
                        break;
                    default:
                        currentCell.Append(c);
                        i++;
                        break;
                }
            }

            currentRow.Add(currentCell.ToString());
            if (currentRow.Count > 1 || (currentRow.Count == 1 && currentRow[0].Length > 0))
                rows.Add(currentRow.ToArray());

            return rows;
        }
    }
}
