#if HUNGNT_ADDRESSABLE
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace WingsMob.HungNT
{
    /// <summary>
    /// Manages loading and caching of assets using Unity Addressables.
    /// Automatically releases all loaded assets when the active scene changes.
    /// </summary>
    public partial class AssetLoader : MonoSingleton<AssetLoader>, IAssetLoader
    {
        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<string, object>> _assetsCached = new();

        private readonly List<AsyncOperationHandle> _activeRequests = new();

        protected override void OnAwake()
        {
            base.OnAwake();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        /// <summary>
        /// Loads an asset of type TAsset asynchronously from Addressables.
        /// Caches the asset for future use.
        /// </summary>
        /// <typeparam name="TAsset">Type of the asset to load.</typeparam>
        /// <param name="address">The Addressables key of the asset.</param>
        /// <returns>The loaded asset.</returns>
        public async UniTask<TAsset> Load<TAsset>(string address) where TAsset : Object
        {
            var type = typeof(TAsset);
            if (!_assetsCached.TryGetValue(type, out var assetsOfType))
            {
                WMLog.Log($"Registering new asset type: {type.Name.Color("cyan")}");
                assetsOfType = new Dictionary<string, object>();
                _assetsCached[type] = assetsOfType;
            }

            if (assetsOfType.TryGetValue(address, out var cachedAsset))
            {
                return (TAsset)cachedAsset;
            }

            var requestHandle = Addressables.LoadAssetAsync<TAsset>(address);
            _activeRequests.Add(requestHandle);

            await requestHandle.Task;
            assetsOfType[address] = requestHandle.Result;
            return requestHandle.Result;
        }

        /// <summary>
        /// Releases all loaded assets and clears the cache.
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var request in _activeRequests)
            {
                Addressables.Release(request);
            }
            _activeRequests.Clear();
            _assetsCached.Clear();
            WMLog.Log("All assets have been released.");
        }

        /// <summary>
        /// Handles automatic asset cleanup when the active scene changes.
        /// </summary>
        private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            ReleaseAll();
        }
    }
}
#endif