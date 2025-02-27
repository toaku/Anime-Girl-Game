using System;
using System.Reflection;
using UnityEngine;

public static class PrivateMemberAccessor
{
    private static BindingFlags privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

    public static object GetField(object target, string fieldName)
    {
        object field = target.GetType().GetField(fieldName, privateInstance).GetValue(target);

        return field;
    }

    public static void SetField(object target, string fieldName, object value) 
    {
        target.GetType().GetField(fieldName, privateInstance).SetValue(target, value);
    }

    private static PropertyInfo GetProperty(Type target, string propertyName, bool isPropertyPrivate)
    {
        PropertyInfo targetProperty;
        if (isPropertyPrivate)
        {
            targetProperty = target.GetProperty(propertyName, privateInstance);
        }
        else
        {
            targetProperty = target.GetProperty(propertyName);
        }

        return targetProperty;
    }

    public static object GetFieldByPropertyGetter(object target, string propertyName, bool isPropertyPrivate)
    {
        object field = GetProperty(target.GetType(), propertyName, isPropertyPrivate).GetGetMethod(true).Invoke(target, null);

        return field;
    }

    public static void SetFieldByPropertySetter(object target, string propertyName, object value, bool isPropertyPrivate)
    {
        object field = GetProperty(target.GetType(), propertyName, isPropertyPrivate).GetSetMethod(true).Invoke(target, new object[] { value });
    }

    public static object InvokeMethod(object target, string methodName, params object[] param)
    {
        return target.GetType().GetMethod(methodName, privateInstance).Invoke(target, param);
    }
}
