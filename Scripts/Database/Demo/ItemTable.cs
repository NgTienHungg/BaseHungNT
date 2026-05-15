using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Database
{
    // ── Enums ────────────────────────────────────────────────────────────────

    public enum ItemCategory
    {
        Armor,
        Weapon,
        Accessory,
    }

    public enum UnlockType
    {
        Free,
        Ads,
        Coin
    }

    // ── Data ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Dữ liệu của một item trong game.
    /// Mỗi field tương ứng một cột trên Google Sheet qua GGSheet.
    /// </summary>
    [Serializable]
    public class ItemData
    {
        [Content, ColumnName("id")]
        public string Id;

        [Content, ColumnName("name")]
        public string Name;

        [Content, ColumnName("category")]
        public ItemCategory Category;

        [Content, ColumnName("unlock_type")]
        public UnlockType UnlockType;

        /// <summary>Số lượt ads cần xem để unlock (chỉ áp dụng khi UnlockType = Ads).</summary>
        [Content, ColumnName("ads_required")]
        public int AdsRequired;

        /// <summary>Giá coin cần bỏ ra (chỉ áp dụng khi UnlockType = Coin).</summary>
        [Content, ColumnName("coin_price")]
        public long CoinPrice;

        /// <summary>Sprite icon path trong Resources (tùy chọn).</summary>
        [Content, ColumnName("icon")]
        public string Icon;

        // IDataModelWithId
        public string GetId() => Id;
    }

    // ── Table ────────────────────────────────────────────────────────────────

    /// <summary>
    /// ScriptableObject chứa toàn bộ <see cref="ItemData"/>.
    /// <para>Đặt file tại: <c>Assets/Resources/Database/ItemTable.asset</c></para>
    /// <para>Import từ Google Sheet qua GGSheet (worksheet tên "ItemTable").</para>
    /// </summary>
    [ContentAsset]
    [CreateAssetMenu(menuName = "Game/Database/ItemTable", fileName = "ItemTable")]
    public class ItemTable : BaseDataTable
    {
        [ArrayContent("ItemTable")]
        [TableList(ShowIndexLabels = true)]
        public ItemData[] Items = Array.Empty<ItemData>();

        // ── Lookup ────────────────────────────────────────────────────────────

        private Dictionary<string, ItemData> _lookup;

        public override void Initialize()
        {
            _lookup = new Dictionary<string, ItemData>(Items.Length);
            foreach (var item in Items)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    Debug.LogWarning($"[ItemTable] Item có Id rỗng — bỏ qua.");
                    continue;
                }
                _lookup[item.Id] = item;
            }
        }

        // ── API ───────────────────────────────────────────────────────────────

        public ItemData GetById(string id)
        {
            if (_lookup != null && _lookup.TryGetValue(id, out var item))
                return item;

            Debug.LogWarning($"[ItemTable] Không tìm thấy item id='{id}'");
            return null;
        }

        public IReadOnlyList<ItemData> GetByCategory(ItemCategory category)
        {
            var result = new List<ItemData>();
            foreach (var item in Items)
                if (item.Category == category)
                    result.Add(item);
            return result;
        }

        public IReadOnlyList<ItemData> GetByUnlockType(UnlockType type)
        {
            var result = new List<ItemData>();
            foreach (var item in Items)
                if (item.UnlockType == type)
                    result.Add(item);
            return result;
        }

        public IReadOnlyList<ItemData> GetAll() => Items;
    }
}