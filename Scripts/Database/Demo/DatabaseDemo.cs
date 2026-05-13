using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo IDatabaseService với ItemTable.
    /// <para>Điều kiện:</para>
    /// <list type="bullet">
    ///   <item>Tạo ScriptableObject ItemTable tại <c>Assets/Resources/Database/ItemTable.asset</c></item>
    ///   <item>Có GameObject trong scene với DatabaseService + ServiceRegister đã register IDatabaseService</item>
    /// </list>
    /// </summary>
    public class DatabaseDemo : MonoBehaviour
    {
        [ShowInInspector, ReadOnly, TableList]
        private IReadOnlyList<ItemEntity> _displayedItems;

        private IDatabaseService _db;

        private void Start()
        {
            _db = this.GetService<IDatabaseService>();
            ShowAll();
        }

        // ── Buttons ───────────────────────────────────────────────────────────

        [Button("Show All Items"), FoldoutGroup("Query")]
        private void ShowAll()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            _displayedItems = table.GetAll();
        }

        [Button("Filter: Free Items"), FoldoutGroup("Query")]
        private void ShowFree()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            _displayedItems = table.GetByUnlockType(UnlockType.Free);
        }

        [Button("Filter: Coin Items"), FoldoutGroup("Query")]
        private void ShowCoin()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            _displayedItems = table.GetByUnlockType(UnlockType.Coin);
        }

        [Button("Filter: Ads Items"), FoldoutGroup("Query")]
        private void ShowAds()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            _displayedItems = table.GetByUnlockType(UnlockType.Ads);
        }

        [Button("Filter: Category Ao"), FoldoutGroup("Query")]
        private void ShowAo()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            _displayedItems = table.GetByCategory(ItemCategory.Ao);
        }

        [SerializeField, FoldoutGroup("Query")] private string _findId = "item_001";

        [Button("Find By Id"), FoldoutGroup("Query")]
        private void FindById()
        {
            if (!_db.TryGetTable<ItemTable>(out var table)) return;
            var item = table.GetById(_findId);
            if (item != null)
                Debug.Log($"[Demo] Found: {item.Name} | {item.Category} | {item.UnlockType}");
        }
    }
}
