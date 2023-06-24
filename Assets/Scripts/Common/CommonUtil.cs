using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonUtil
{
    #region 未分类
    /// <summary>
    /// 获取屏幕坐标换算成Canvas上的位置。
    /// </summary>
    static public Vector2 GetPosAtCanvasScaler(Vector2 screenPos, RectTransform canvasRect, Vector2 anchor) {
        screenPos /= canvasRect.localScale;
        screenPos -= canvasRect.sizeDelta * anchor;
        return screenPos;
    }
    #endregion

    #region 委托监听
    /// <summary>
    /// 为动作添加监听，忽略相同监听。
    /// </summary>
    static public void AddListener(ref Action action, Action listener) {
        if(!ActionHasListener(action, listener)) {
            action += listener;
        }
    }

    /// <summary>
    /// 为动作添加监听，忽略相同监听。
    /// </summary>
    static public void AddListener<T>(ref Action<T> action, Action<T> listener) {
        if (!ActionHasListener(action, listener)) {
            action += listener;
        }
    }

    /// <summary>
    /// 为动作添加监听，忽略相同监听。
    /// </summary>
    static public void AddListener<T1, T2>(ref Action<T1, T2> action, Action<T1, T2> listener) {
        if (!ActionHasListener(action, listener)) {
            action += listener;
        }
    }

    /// <summary>
    /// 为动作添加监听，忽略相同监听。
    /// </summary>
    static public void AddListener<T1, T2, T3>(ref Action<T1, T2, T3> action, Action<T1, T2, T3> listener) {
        if (!ActionHasListener(action, listener)) {
            action += listener;
        }
    }

    /// <summary>
    /// 判断该动作是否存在指定监听。
    /// </summary>
    static public bool ActionHasListener(Delegate action, Delegate listener){
        if (action != null) {
            Delegate[] delegates = action.GetInvocationList();
            for (int i = 0; i < delegates.Length; i += 1) {
                if (delegates[i].Equals(listener)) {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region 计算
    /// <summary>
    /// 获得二维向量A到向量B的夹角，负则B在A逆时针。
    /// </summary>
    public static float Vector2Angle(Vector2 A, Vector2 B) {
        float magnitude = A.magnitude * B.magnitude;
        if (magnitude <= 0) {
            return 0;
        }
        //夹角
        float angle = Mathf.Acos(Vector2.Dot(A, B) / magnitude);
        return angle * Mathf.Rad2Deg * (Vector2Pos(A, B) ? -1 : 1);
    }

    /// <summary>
    /// 判断是否二维向量B在A的顺时针位置。
    /// </summary>
    public static bool Vector2Pos(Vector2 A, Vector2 B) {
        if (A == Vector2.zero || B == Vector2.zero) {
            return true;
        }
        return (A.x * B.y - A.y * B.x) < 0;
    }
    #endregion
}
