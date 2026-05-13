using System;
using System.Collections.Generic;
using DataLabs.GoogleSheets.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    // ── Enums ────────────────────────────────────────────────────────────────

    public enum ItemCategory
    {
        Quan,       // quần
        Ao,         // áo
        Mu,         // mũ
        Toc,        // tóc
        PhuKien     // phụ kiện
    }

    public enum UnlockType
    {
        Free,
        Ads,
        Coin
    }

    // ── Entity ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Dữ liệu của một item trong game.
    /// Mỗi field tương ứng một cột trên Google Sheet qua DataLab.
    /// </summary>
    [Serializable]
    public class ItemEntity : IDataModelWithId<string>
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
    /// ScriptableObject chứa toàn bộ <see cref="ItemEntity"/>.
    /// <para>Đặt file tại: <c>Assets/Resources/Database/ItemTable.asset</c></para>
    /// <para>Import từ Google Sheet qua DataLab (worksheet tên "Items").</para>
    /// </summary>
    [ContentAsset(ImportType.Automatic, primaryKey: "id")]
    [CreateAssetMenu(menuName = "HungNT/Database/ItemTable", fileName = "ItemTable")]
    public class ItemTable : BaseDataTable
    {
        [ArrayContent("Items")]
        [TableList(ShowIndexLabels = true)]
        public ItemEntity[] Items = Array.Empty<ItemEntity>();

        // ── Lookup ────────────────────────────────────────────────────────────

        private Dictionary<string, ItemEntity> _lookup;

        public override void Initialize()
        {
            _lookup = new Dictionary<string, ItemEntity>(Items.Length);
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

        public ItemEntity GetById(string id)
        {
            if (_lookup != null && _lookup.TryGetValue(id, out var item))
                return item;

            Debug.LogWarning($"[ItemTable] Không tìm thấy item id='{id}'");
            return null;
        }

        public IReadOnlyList<ItemEntity> GetByCategory(ItemCategory category)
        {
            var result = new List<ItemEntity>();
            foreach (var item in Items)
                if (item.Category == category)
                    result.Add(item);
            return result;
        }

        public IReadOnlyList<ItemEntity> GetByUnlockType(UnlockType type)
        {
            var result = new List<ItemEntity>();
            foreach (var item in Items)
                if (item.UnlockType == type)
                    result.Add(item);
            return result;
        }

        public IReadOnlyList<ItemEntity> GetAll() => Items;
    }
}
