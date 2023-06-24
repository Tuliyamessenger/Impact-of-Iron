using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorOverride : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    [Header("不是原图，是罩住的图")]
    [SerializeField] bool extra;

    Material _material;
    Material material {
        get {
            if(_material == null) {
                _material = sprite.material;
            }
            return _material;
        }
    }

    IEnumerator flashEffectCoroutine;

    private void OnEnable() {
        if (extra) {
            sprite.enabled = false;
        }
    }

    public void SetFator(float factor) {
        material.SetFloat("_OverrideFactor", Mathf.Clamp01(factor));
    }

    public void SetColor(Color color) {
        material.SetColor("_OverrideColor", color);
    }

    /// <summary>
    /// 直接启动闪烁协程动画。power = (0, 1)
    /// </summary>
    public void FlashEffect(float power, float time = 0.2f) {
        if (!gameObject.activeSelf) {
            return;
        }
        if (extra) {
            SetFator(1);
            this.StartCoroutineSingle(ref flashEffectCoroutine, FlashEffectCoverProgress(Mathf.Clamp01(power), time));
        }
        else {
            this.StartCoroutineSingle(ref flashEffectCoroutine, FlashEffectProgress(Mathf.Clamp01(power), time));
        }
    }

    IEnumerator FlashEffectProgress(float power, float time) {
        float curTime = 0;
        time /= 2;
        while (curTime < time) {
            curTime += Time.deltaTime;
            SetFator(curTime / time * power);
            yield return null;
        }
        curTime = 0;
        while (curTime < time) {
            curTime += Time.deltaTime;
            SetFator((1 - curTime / time) * power);
            yield return null;
        }
        SetFator(0);
    }

    IEnumerator FlashEffectCoverProgress(float power, float time) {
        float curTime = 0;
        time /= 2;
        sprite.enabled = true;
        while (curTime < time) {
            curTime += Time.deltaTime;
            sprite.SetAlpha(curTime / time * power);
            yield return null;
        }
        curTime = 0;
        while (curTime < time) {
            curTime += Time.deltaTime;
            sprite.SetAlpha((1 - curTime / time) * power);
            yield return null;
        }
        sprite.enabled = false;
    }


#if UNITY_EDITOR
    private void OnValidate() {
        if (sprite == null) {
            sprite = GetComponent<SpriteRenderer>();
        }
        if (sprite != null) {
            if (extra) {
                sprite.enabled = false;
            }
            sprite.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/ColorOVerride.mat");
        }
    }
#endif
}
