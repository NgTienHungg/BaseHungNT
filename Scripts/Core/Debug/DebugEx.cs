using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DebugEx
{
    #region === NORMAL ===
    [Conditional("DEBUG")]
    public static void Log(object message, Object context = null)
    {
        Debug.Log(message, context);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(string message, Object context = null)
    {
        Debug.LogWarning(message, context);
    }

    [Conditional("DEBUG")]
    public static void LogError(string message, Object context = null)
    {
        Debug.LogError(message, context);
    }
    #endregion

    /// <summary>Always logged (not stripped by <c>DEBUG</c>). Use for Editor tooling / import diagnostics.</summary>
    public static void LogEditor(object message, Object context = null)
    {
        Debug.Log(message, context);
    }

    public static void LogEditorWarning(object message, Object context = null)
    {
        Debug.LogWarning(message, context);
    }

    public static void LogEditorError(object message, Object context = null)
    {
        Debug.LogError(message, context);
    }

    #region === MONOBEHAVIOUR ===
    [Conditional("DEBUG")]
    public static void Log(this MonoBehaviour mono, object message)
    {
        Debug.Log($"[{mono.GetType().Name.Bold()}] {message}", mono.gameObject);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(this MonoBehaviour mono, object message)
    {
        Debug.LogWarning($"[{mono.GetType().Name.Bold()}] {message}", mono.gameObject);
    }

    [Conditional("DEBUG")]
    public static void LogError(this MonoBehaviour mono, object message)
    {
        Debug.LogError($"[{mono.GetType().Name.Bold()}] {message}", mono.gameObject);
    }
    #endregion

    #region === NONE MONOBEHAVIOR ===
    [Conditional("DEBUG")]
    public static void Log(this object obj, object message)
    {
        Debug.Log($"[{obj.GetType().Name.Bold()}] {message}");
    }

    [Conditional("DEBUG")]
    public static void LogWarning(this object obj, object message)
    {
        Debug.LogWarning($"[{obj.GetType().Name.Bold()}] {message}");
    }

    [Conditional("DEBUG")]
    public static void LogError(this object obj, object message)
    {
        Debug.LogError($"[{obj.GetType().Name.Bold()}] {message}");
    }
    #endregion

    #region === SCRIPTABLEOBJECT ===
    [Conditional("DEBUG")]
    public static void Log(this ScriptableObject so, object message)
    {
        Debug.Log($"[{so.GetType().Name.Bold()}] {message}", so);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(this ScriptableObject so, object message)
    {
        Debug.LogWarning($"[{so.GetType().Name.Bold()}] {message}", so);
    }

    [Conditional("DEBUG")]
    public static void LogError(this ScriptableObject so, object message)
    {
        Debug.LogError($"[{so.GetType().Name.Bold()}] {message}", so);
    }
    #endregion
}