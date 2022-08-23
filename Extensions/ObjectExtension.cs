namespace readfile.model;

public static class ObjectExtension
{
    public static void SetValueObjectReflection(this object obj, string path, string valueFinded)
    {
        var properties = path.Split('.');

        if (properties.Length == 1)
            obj.GetType().GetProperty(path)?.SetValue(obj, valueFinded);

        for (var i = 0; i < properties.Count() - 1; i++)
        {
            var part = properties[i];
            if (obj == null)
                return;

            var prop = obj.GetType().GetProperty(part);

            if (prop == null)
                return;

            obj = prop.GetValue(obj, null);
        }
        
        obj?.GetType().GetProperty(properties.Last())?.SetValue(obj, valueFinded);
    }
}