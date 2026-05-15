using System;
using System.Reflection;
using UnityEngine;

namespace HungNT.Database.Editor
{
    internal static class GGCallbackBindings
    {
        public static void InvokeOnImportComplete()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }

                foreach (var type in types)
                {
                    if (type.GetCustomAttribute<ContentCallbacksAttribute>() == null)
                        continue;

                    var m = type.GetMethod("OnImportComplete", BindingFlags.Public | BindingFlags.Static);
                    if (m == null || m.GetParameters().Length != 0)
                        continue;

                    try
                    {
                        m.Invoke(null, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
        }
    }
}
