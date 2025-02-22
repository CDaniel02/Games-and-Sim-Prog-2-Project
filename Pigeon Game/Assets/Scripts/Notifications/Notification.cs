using System;
using System.Collections.Generic;

public class Notification
{
    private readonly object _object;
    public object Object { get { return _object; } }

    private readonly string _name;
    public string Name { get { return _name; } }

    public Dictionary<String, object> UserInfo;

    public Notification(string p_name, object p_object) : this(p_name, p_object, new Dictionary<string, object>()) { }

    public Notification(string p_name, object p_object, Dictionary<string, object> p_userInfo)
    {
        _object = p_object;
        _name = p_name;
        UserInfo = p_userInfo;
    }

    public static Notification Empty
    {
        get
        {
            return new Notification(null, null);
        }
    }
}

