using UnityEngine;

namespace WingsMob.HungNT
{
    public abstract class SOSingleton<T> : ScriptableObject where T : SOSingleton<T>
    {
        /// <summary>
        /// The path inside the Resources folder where the asset is located.
        /// </summary>
        public abstract string ResourcePath { get; }

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Create a temporary instance to retrieve the ResourcePath
                    var temp = CreateInstance<T>();
                    var path = temp.ResourcePath;

                    _instance = Resources.Load<T>(path);

                    if (_instance == null)
                    {
                        _instance = temp; // Use the temporary instance if loading fails
                        DebugEx.LogError($"Failed to load asset at Resources/{path}".Color("red"));
                    }
                }

                return _instance;
            }
        }
    }
}