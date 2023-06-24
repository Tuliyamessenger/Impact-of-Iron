using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                if (!IsQuiting.isQuiting) {
                    _instance = new GameObject("Manager").AddComponent<T>();
                    IsQuiting.RegisterListener();
                }
            }
            return _instance;
        }
    }

    static Transform _transform;
    public Transform selfTransform {
        get {
            if (_transform == null) {
                _transform = transform;
            }
            return _transform;
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(_instance);
        }
        _instance = GetSelf();
        selfTransform.position = Vector3.zero;
        selfTransform.rotation = Quaternion.identity;
    }

    protected abstract T GetSelf();
}

public class IsQuiting{
    static public bool isQuiting = false;
    static bool addListener;

    static public void Quiting() {
        isQuiting = true;
    }

    static public void RegisterListener() {
        if (!addListener) {
            addListener = true;
            Application.quitting += Quiting;
        }
    }
}
