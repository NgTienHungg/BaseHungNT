#if HUNGNT_ADDRESSABLE
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WingsMob.HungNT
{
    public interface IAssetLoader
    {
        UniTask<TAsset> Load<TAsset>(string address) where TAsset : Object;

        void ReleaseAll();
    }
}
#endif