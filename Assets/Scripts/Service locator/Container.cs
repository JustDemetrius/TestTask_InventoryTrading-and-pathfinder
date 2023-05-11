using System;
using System.Collections.Generic;

public class Container
{
    private static Dictionary<Type, BaseController> _container = new Dictionary<Type, BaseController>();

    public static void Add<T>(T controller) where T : BaseController
    {
        _container.Add(controller.GetType(), controller);
    }

    public static T Get<T>() where T : BaseController
    {
        if (_container.ContainsKey(typeof(T)))
        {
            BaseController founded = _container[typeof(T)];
            return founded as T;
        }
        return null;
    }

    public static void Remove<T>() where T : BaseController
    {
        if (_container.ContainsKey(typeof(T)))
        {
            _container.Remove(typeof(T));
        }
    }
}
