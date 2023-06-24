using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class CommonExtension
{
    /// <summary>
    /// 直接修改颜色透明度。
    /// </summary>
    public static void SetAlpha(this Graphic img, float a) {
        Color col = img.color;
        col.a = a;
        img.color = col;
    }

    /// <summary>
    /// 直接修改颜色透明度。
    /// </summary>
    public static void SetAlpha(this SpriteRenderer img, float a) {
        Color col = img.color;
        col.a = a;
        img.color = col;
    }

    /// <summary>
    /// 判断有无后协程后停止。
    /// </summary>
    static public void StopCoroutineSingle(this MonoBehaviour mono, ref IEnumerator enumerator) {
        if(enumerator != null) {
            mono.StartCoroutine(enumerator);
        }
    }

    /// <summary>
    /// 播放协程，如果当前协程播放中则停在后再播放。
    /// </summary>
    static public void StartCoroutineSingle(this MonoBehaviour mono, ref IEnumerator enumerator, IEnumerator coroutine) {
        mono.StopCoroutineSingle(ref enumerator);
        enumerator = coroutine;
        mono.StartCoroutine(enumerator);
    }
}
