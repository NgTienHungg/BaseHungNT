using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Database
{
    /// <summary>
    /// Abstract ScriptableObject base cho database table.
    /// </summary>
    [InlineEditor]
    public class BaseDataTable : ScriptableObject, IDataTable
    {
        public virtual string TableName => GetType().Name;

        public virtual void Initialize()
        {
        }
    }
}