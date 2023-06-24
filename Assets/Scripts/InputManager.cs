using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    static public readonly string Horizontal = "Horizontal";
    static public readonly string Vertical = "Vertical";
    static public readonly string Fire1 = "Fire1";
    static public readonly string Cancel = "Cancel";

    public static Vector2 GetHVAxis() {
        return new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }

    static Camera _mainCamera;
    static public Camera mainCamera {
        get {
            if(!_mainCamera) {
                _mainCamera = Camera.main;
                if (!_mainCamera) {
                    _mainCamera = GameObject.FindObjectOfType<Camera>();
                }
                if (!_mainCamera) {
                    Debug.LogError("�Ҳ�������ͷ");
                }
            }
            return _mainCamera;
        }
    }

    /// <summary>
    /// ��ȡ�������������µ��������ꡣ
    /// </summary>
    public static Vector3 GetMouseWorldPosition(float posZ = 0) {
        if (mainCamera) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = posZ;
            return mainCamera.ScreenToWorldPoint(mousePos);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// ͨ�û�ð�ť״̬��
    /// </summary>
    public static bool GetButton(string key, bool hold = false) {
        if (hold) {
            return Input.GetButton(key);
        }
        return Input.GetButtonDown(key);
    }
}
