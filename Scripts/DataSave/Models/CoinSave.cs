// using System;
// using HungNT.Datasave;
//
// namespace HungNT
// {
//     /// <summary>Lưu số coin (soft currency) trong một domain file riêng.</summary>
//     [Serializable]
//     public class CoinSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_coin.es3";
//
//         public override string RootStorageKey => "coin";
//
//         public long Amount;
//
//         public override void OnAfterLoad()
//         {
//             if (Amount < 0)
//                 Amount = 0;
//         }
//     }
//
//     /// <summary>Lưu số gem (hard currency) trong một domain file riêng.</summary>
//     [Serializable]
//     public class GemSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_gem.es3";
//
//         public override string RootStorageKey => "gem";
//
//         public long Amount;
//
//         public override void OnAfterLoad()
//         {
//             if (Amount < 0)
//                 Amount = 0;
//         }
//     }
// }
