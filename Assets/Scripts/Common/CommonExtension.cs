using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class CommonExtension
{
    /// <summary>
    /// ֱ���޸���ɫ͸���ȡ�
    /// </summary>
    public static void SetAlpha(this Graphic img, float a) {
        Color col = img.color;
        col.a = a;
        img.color = col;
    }

    /// <summary>
    /// ֱ���޸���ɫ͸���ȡ�
    /// </summary>
    public static void SetAlpha(this SpriteRenderer img, float a) {
        Color col = img.color;
        col.a = a;
        img.color = col;
    }

    /// <summary>
    /// �ж����޺�Э�̺�ֹͣ��
    /// </summary>
    static public void StopCoroutineSingle(this MonoBehaviour mono, ref IEnumerator enumerator) {
        if(enumerator != null) {
            mono.StartCoroutine(enumerator);
        }
    }

    /// <summary>
    /// ����Э�̣������ǰЭ�̲�������ͣ�ں��ٲ��š�
    /// </summary>
    static public void StartCoroutineSingle(this MonoBehaviour mono, ref IEnumerator enumerator, IEnumerator coroutine) {
        mono.StopCoroutineSingle(ref enumerator);
        enumerator = coroutine;
        mono.StartCoroutine(enumerator);
    }
}
