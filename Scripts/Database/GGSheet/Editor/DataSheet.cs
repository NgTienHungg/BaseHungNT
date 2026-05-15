using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HungNT.Database.Editor
{
    public class DataSheet
    {
        public WorksheetId Id { get; }

        public IEnumerable<string> FieldNames => _fieldNames;

        public int RowCount => _rows.Length;

        public int ColumnCount => _columns.Length;

        public string FromCol => Id.FromCol;
        public string ToCol => Id.ToCol;

        private readonly string[] _fieldNames;
        private readonly uint[] _fieldHashes;
        private readonly string[][] _rows;
        private readonly string[][] _columns;

        public const string DEFAULT_PRIMARY_KEY = "key";

        /// <param name="allRows">All rows: header at <paramref name="headerRowIndex"/>, data follows.</param>
        internal DataSheet(WorksheetId id, IReadOnlyList<string[]> allRows, int headerRowIndex = 0)
        {
            Id = id;

            if (allRows == null || allRows.Count == 0 || headerRowIndex >= allRows.Count)
            {
                _fieldNames = Array.Empty<string>();
                _fieldHashes = Array.Empty<uint>();
                _rows = Array.Empty<string[]>();
                _columns = Array.Empty<string[]>();
                return;
            }

            var sliced = ColumnRange.ApplyColumnSlice(allRows, id.FromCol, id.ToCol);

            int maxLength = 0;
            for (int ix = 0; ix < sliced.Count; ix++)
            {
                if ((sliced[ix]?.Length ?? 0) > maxLength)
                    maxLength = sliced[ix].Length;
            }

            string[] fields = sliced[headerRowIndex].Select(o => o ?? string.Empty).ToArray();
            MakeValidFieldArray(ref fields, maxLength, out int[] removed);
            _fieldNames = fields;

            _fieldHashes = new uint[_fieldNames.Length];
            for (int ix = 0; ix < _fieldHashes.Length; ix++)
                _fieldHashes[ix] = Util.FNVHash(_fieldNames[ix]);

            var rows = new List<string[]>();
            for (int ix = headerRowIndex + 1; ix < sliced.Count; ix++)
            {
                var e = sliced[ix].Select(o => o ?? string.Empty).ToArray();
                if (IsStringArrayEmpty(e))
                    continue;

                var row = new string[maxLength];
                CopyStringArray(e, row);
                MatchFieldArray(ref row, removed);
                rows.Add(row);
            }

            _rows = rows.ToArray();

            var columns = new List<string[]>();
            if (_rows.Length > 0)
            {
                for (int ix = 0; ix < _rows[0].Length; ix++)
                    columns.Add(new string[_rows.Length]);

                for (int ix = 0; ix < _rows.Length; ix++)
                {
                    for (int iy = 0; iy < _rows[ix].Length; iy++)
                        columns[iy][ix] = _rows[ix][iy];
                }
            }

            _columns = columns.ToArray();
        }

        public bool HasField(string name) => GetFieldIndex(name) != -1;

        public int GetFieldIndex(string name)
        {
            uint hash = Util.FNVHash(name);
            for (int ix = 0; ix < _fieldHashes.Length; ix++)
            {
                if (_fieldHashes[ix] == hash)
                    return ix;
            }

            return -1;
        }

        public Row GetRow(string primaryKey, int index)
        {
            if (index < 0 || index >= _rows.Length)
                throw new IndexOutOfRangeException();

            return new Row(primaryKey, _fieldNames, _fieldHashes, _rows[index]);
        }

        public IEnumerable<Row> GetRows(string primaryKey)
        {
            var enumerator = new Enumerator(this, primaryKey);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public Column GetColumn(string fieldName)
        {
            int fieldIndex = GetFieldIndex(fieldName);
            if (fieldIndex == -1)
                throw new KeyNotFoundException($"No field with name `{fieldName}' exists in the DataSheet");

            return new Column(fieldName, _columns[fieldIndex]);
        }

        public IEnumerable<Column> GetColumns()
        {
            for (int ix = 0; ix < _fieldNames.Length; ix++)
                yield return new Column(_fieldNames[ix], _columns[ix]);
        }

        private static void CopyStringArray(string[] source, string[] destination)
        {
            for (int ix = 0; ix < source.Length && ix < destination.Length; ix++)
                destination[ix] = source[ix];
        }

        private static void MakeValidFieldArray(ref string[] fields, int maxLength, out int[] removed)
        {
            Array.Resize(ref fields, maxLength);
            var fieldsList = new List<string>(fields);
            var removedList = new List<int>();
            for (int ix = fields.Length - 1; ix >= 0; ix--)
            {
                if (string.IsNullOrEmpty(fields[ix]))
                {
                    fieldsList.RemoveAt(ix);
                    removedList.Add(ix);
                }
            }

            fields = fieldsList.ToArray();
            removed = removedList.ToArray();
        }

        private static void MatchFieldArray(ref string[] row, int[] removed)
        {
            if (removed.Length != 0)
            {
                var rowList = new List<string>(row);
                foreach (int index in removed)
                {
                    if (index < rowList.Count)
                        rowList.RemoveAt(index);
                }

                row = rowList.ToArray();
            }
        }

        private static bool IsStringArrayEmpty(string[] array)
        {
            if (array == null || array.Length == 0)
                return true;

            for (int ix = 0; ix < array.Length; ix++)
            {
                if (!string.IsNullOrEmpty(array[ix]))
                    return false;
            }

            return true;
        }

        private class Enumerator : IEnumerator<Row>
        {
            public Row Current => new Row(
                _primaryKey,
                _source._fieldNames,
                _source._fieldHashes,
                _source._rows[_index]);

            object IEnumerator.Current => Current;

            private DataSheet _source;
            private string _primaryKey;
            private int _index;

            public Enumerator(DataSheet source, string primaryKey)
            {
                _source = source;
                _primaryKey = primaryKey;
                _index = -1;
            }

            public void Dispose()
            {
                _source = null;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _source._rows.Length;
            }

            public void Reset() => _index = -1;
        }
    }
}
