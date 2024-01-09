using System.Reflection;
using DebugMenu;
using UnityEngine;

public static class Utils
{
    public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        return component != null;
    }
    
    public static bool TryGetComponent<T>(this Transform gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        return component != null;
    }
    
    public static T GetPrivateField<T>(this object type, string fieldName)
    {
        FieldInfo fieldInfo = type.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
            return (T)fieldInfo.GetValue(type);
        
        Plugin.Log.LogError($"Could not get field info for {fieldName} on {type.GetType().Name}!");
        return default;
    }
    
    public static void InvokePrivateMethod(this object type, string methodName, params object[] args)
    {
        MethodInfo fieldInfo = type.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
            fieldInfo.Invoke(type, args);
        else
            Plugin.Log.LogError($"Could not get method info for {methodName} on {type.GetType().Name}!");
    }
    
    public static T InvokePrivateMethod<T>(this object type, string methodName, params object[] args)
    {
        MethodInfo fieldInfo = type.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
            return (T)fieldInfo.Invoke(type, args);
        
        Plugin.Log.LogError($"Could not get method info for {methodName} on {type.GetType().Name}!");
        return default;
    }
}