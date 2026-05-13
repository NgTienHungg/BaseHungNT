namespace HungNT
{
    /// <summary>
    /// Marker interface cho entity/model trong database table.
    /// <code>
    /// [Serializable]
    /// public class EnemyEntity : IDataModel
    /// {
    ///     public int Id;
    ///     public string Name;
    ///     public int Hp;
    /// }
    /// </code>
    /// </summary>
    public interface IDataModel
    {
    }

    /// <summary>
    /// Data model có ID — dùng cho lookup nhanh theo key.
    /// </summary>
    public interface IDataModelWithId<TKey> : IDataModel
    {
        TKey Id { get; }
    }
}
