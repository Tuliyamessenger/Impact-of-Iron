using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSingleton<T> where T : SystemSingleton<T> , new(){
    static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = new T();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 移除单例当前的实例。
    /// </summary>
    public static void ResetInstance() {
        _instance = null;
    }

    /// <summary>
    /// 设置单例当前的实例。
    /// </summary>
    public static void SetInstance(T ins) {
        _instance = ins;
    }
}
