using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Abstract ScriptableObject base cho database table.
    /// <para>Kế thừa và thêm các array/list entity data.</para>
    /// <para>Dùng kết hợp với DataLab attributes (<c>[ContentAsset]</c>, <c>[ArrayContent]</c>,
    /// <c>[ColumnName]</c>) để auto-import từ Google Sheets.</para>
    /// <code>
    /// [ContentAsset(ImportType.Automatic)]
    /// [CreateAssetMenu(menuName = "Database/EnemyTable")]
    /// public class EnemyTable : BaseDataTable
    /// {
    ///     [ArrayContent("EnemySheet")] public EnemyEntity[] Enemies;
    /// }
    /// </code>
    /// </summary>
    public abstract class BaseDataTable : ScriptableObject, IDataTable
    {
        public virtual string TableName => GetType().Name;

        public virtual void Initialize() { }
    }
}
