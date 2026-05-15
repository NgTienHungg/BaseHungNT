using System.Collections;
using System.Collections.Generic;

namespace HungNT.Database.Editor
{
    public readonly struct Row : IReadOnlyDictionary<string, string>
    {
        public string PrimaryValue =>
            _primaryIndex == -1 ? string.Empty : _values[_primaryIndex];

        public IEnumerable<string> Keys => _fieldNames;

        public IEnumerable<string> Values => _values;

        public int Count => _values.Length;

        public string this[string key] =>
            TryGetValue(key, out string value) ? value : throw new KeyNotFoundException();

        private readonly int _primaryIndex;
        private readonly uint[] _fieldHashes;
        private readonly string[] _fieldNames;
        private readonly string[] _values;

        public Row(string primaryKey, string[] fieldNames, uint[] fieldHashes, string[] values)
        {
            _fieldHashes = fieldHashes;
            _fieldNames = fieldNames;
            _values = values;
            _primaryIndex = FindFieldIndex(fieldHashes, primaryKey);
        }

        public int FindFieldIndex(string key) => FindFieldIndex(_fieldHashes, key);

        public bool ContainsKey(string key) => FindFieldIndex(key) != -1;

        public bool TryGetValue(string key, out string value)
        {
            int fieldIndex = FindFieldIndex(key);
            if (fieldIndex == -1)
            {
                value = string.Empty;
                return false;
            }

            value = _values[fieldIndex];
            return true;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
            new Enumerator(_fieldNames, _values);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static int FindFieldIndex(uint[] fieldHashes, string key)
        {
            uint hash = Util.FNVHash(key);
            for (int ix = 0; ix < fieldHashes.Length; ix++)
            {
                if (fieldHashes[ix] == hash)
                    return ix;
            }

            return -1;
        }

        private class Enumerator : IEnumerator<KeyValuePair<string, string>>
        {
            public KeyValuePair<string, string> Current =>
                new KeyValuePair<string, string>(_keys[_index], _values[_index]);

            object IEnumerator.Current => Current;

            private string[] _keys;
            private string[] _values;
            private int _index;

            public Enumerator(string[] keys, string[] values)
            {
                _keys = keys;
                _values = values;
                _index = -1;
            }

            public void Dispose()
            {
                _keys = null;
                _values = null;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _keys.Length && _index < _values.Length;
            }

            public void Reset() => _index = -1;
        }
    }
}
