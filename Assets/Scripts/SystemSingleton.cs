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
    /// �Ƴ�������ǰ��ʵ����
    /// </summary>
    public static void ResetInstance() {
        _instance = null;
    }

    /// <summary>
    /// ���õ�����ǰ��ʵ����
    /// </summary>
    public static void SetInstance(T ins) {
        _instance = ins;
    }
}
