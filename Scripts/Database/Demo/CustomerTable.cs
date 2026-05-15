using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Database
{
    [ContentAsset]
    [CreateAssetMenu(fileName = "CustomerTable", menuName = "Game/Database/CustomerTable")]
    public class CustomerTable : BaseDataTable
    {
        [ArrayContent("CustomerTable")]
        [TableList(ShowIndexLabels = true)]
        public CustomerData[] Customers = { };
    }

    [Serializable]
    public class CustomerData
    {
        [ColumnName("name")]
        public string Name;

        [ColumnName("avatar")]
        public string AvatarSprite;
    }
}