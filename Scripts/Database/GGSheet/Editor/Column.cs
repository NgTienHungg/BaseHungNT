using System.Collections;
using System.Collections.Generic;

namespace HungNT.Database.Editor
{
    public struct Column : IReadOnlyList<string>
    {
        public string FieldName => _fieldName;

        public string this[int index] =>
            index < 0 || index >= _values.Length
                ? throw new System.IndexOutOfRangeException()
                : _values[index];

        public int Count => _values.Length;

        private string _fieldName;
        private string[] _values;

        public Column(string fieldName, string[] values)
        {
            _fieldName = fieldName;
            _values = values;
        }

        public void Copy(out string[] destination)
        {
            destination = new string[_values.Length];
            for (int ix = 0; ix < _values.Length; ix++)
                destination[ix] = _values[ix];
        }

        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)_values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}
