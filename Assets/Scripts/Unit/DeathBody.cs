using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBody : RecycleObject<DeathBody>
{
    [SerializeField] Transform bodyTrans;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float disappearTime = 4.8f;
    [SerializeField] float fadeTime = 1.2f;
    [SerializeField] Animation deathAnime;

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    private void OnEnable() {
        if (deathAnime) {
            deathAnime.Play();
        }
        curTime = 0;
        sprite.SetAlpha(1);
    }

    private void OnDisable() {
        UnitManager.EnqueueDeathBody(this, relatePrefab);
    }

    float curTime = -1;
    private void Update() {
        if (curTime >= 0) {
            curTime += Time.deltaTime;
            if (curTime > disappearTime) {
                curTime = -1;
                FadeBody();
            }
        }
    }

    public void ShowBody(Sprite bodySprite, Vector3 pos, Quaternion rot, Vector3 scale) {
        sprite.sprite = bodySprite;
        selfTransform.position = pos;
        bodyTrans.rotation = rot;
        bodyTrans.localScale = scale;
        gameObject.SetActive(true);
        if (deathAnime) {
            deathAnime.Play();
        }
    }

    void FadeBody() {
        StartCoroutine(FadeBodyProgress());
    }

    IEnumerator FadeBodyProgress() {
        float curFade = 0;
        while (curFade < fadeTime) {
            curFade += Time.deltaTime;
            sprite.SetAlpha(1 - curFade / fadeTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
