using HungNT;
#if HUNGNT_ADDRESSABLE
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace WingsMob.HungNT
{
    public partial class AssetLoader
    {
        /// <summary>
        /// Loads a sprite from a specified atlas.
        /// </summary>
        /// <param name="atlasAddress">The address of the sprite atlas.</param>
        /// <param name="spriteName">The name of the sprite.</param>
        /// <returns>The loaded sprite, or null if not found.</returns>
        public async UniTask<Sprite> LoadSprite(string atlasAddress, string spriteName)
        {
            var atlas = await Load<SpriteAtlas>(atlasAddress);
            if (atlas == null)
            {
                WMLog.LogError($"Cannot find atlas {atlasAddress.Color("red")}");
                return null;
            }

            var sprite = atlas.GetSprite(spriteName);
            if (sprite == null)
            {
                WMLog.LogError($"Cannot find sprite {spriteName.Color("red")} in atlas {atlasAddress.Color("red")}");
            }
            return sprite;
        }

        /// <summary>
        /// Load sprite with texture address
        /// </summary>
        /// <param name="textureAddress"></param>
        /// <returns></returns>
        public async UniTask<Sprite> LoadSprite(string textureAddress)
        {
            var texture = await Load<Texture2D>(textureAddress);
            if (texture == null)
            {
                WMLog.LogError($"Cannot find texture {textureAddress.Color("red")}");
                return null;
            }

            string textureName = texture.name;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = textureName;
            return sprite;
        }

        /// <summary>
        /// Load text asset from addressable
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async UniTask<TextAsset> LoadTextAsset(string address)
        {
            var textAsset = await Load<TextAsset>(address);
            if (textAsset == null)
            {
                WMLog.LogError($"Cannot find texture {address.Color("red")}");
                return null;
            }
            return textAsset;
        }

        /// <summary>
        /// Load ScriptableObject from addressable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <returns></returns>
        public async UniTask<ScriptableObject> LoadScriptableObject(string address)
        {
            var scriptableObject = await Load<ScriptableObject>(address);
            if (scriptableObject == null)
            {
                WMLog.LogError($"Cannot find scriptableObject {address.Color("red")}");
                return null;
            }
            return scriptableObject;
        }

        /// <summary>
        /// Loads a prefab from the specified path.
        /// </summary>
        /// <param name="path">The path to the prefab.</param>
        /// <returns>The loaded prefab.</returns>
        public async UniTask<GameObject> LoadPrefab(string path) => await Load<GameObject>(path);

        /// <summary>
        /// Loads a prefab and gets a component of type T.
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour component to retrieve.</typeparam>
        /// <param name="path">The path to the prefab.</param>
        /// <returns>The component of type T from the prefab.</returns>
        public async UniTask<T> LoadPrefab<T>(string path) where T : MonoBehaviour => (await Load<GameObject>(path)).GetComponent<T>();

        /// <summary>
        /// Instantiates a prefab from the specified path.
        /// </summary>
        /// <param name="path">The path to the prefab.</param>
        /// <param name="parent">The optional parent transform.</param>
        /// <returns>The instantiated GameObject.</returns>
        public async UniTask<GameObject> LoadAndInstantiate(string path, Transform parent = null) => Instantiate(await LoadPrefab(path), parent);

        /// <summary>
        /// Instantiates a prefab and gets a component of type T.
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour component to retrieve.</typeparam>
        /// <param name="path">The path to the prefab.</param>
        /// <param name="parent">The optional parent transform.</param>
        /// <returns>The component of type T from the instantiated object.</returns>
        public async UniTask<T> LoadAndInstantiate<T>(string path, Transform parent = null) where T : MonoBehaviour => Instantiate(await LoadPrefab<T>(path), parent);
    }
}
#endif