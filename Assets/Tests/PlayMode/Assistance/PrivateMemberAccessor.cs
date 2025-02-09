using System.Reflection;

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

    public static object InvokeMethod(object target, string methodName, params object[] param)
    {
        return target.GetType().GetMethod(methodName, privateInstance).Invoke(target, param);
    }
}
