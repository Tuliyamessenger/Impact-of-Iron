using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEstArea : MonoBehaviour
{
    [SerializeField] Animation anime;

    float time = 0.1f;

    private void Awake() {
        anime.Play();
    }

    private void Update() {
        if (time > 0) {
            time -= Time.deltaTime;
            if (time <= 0) {
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnEnable() {
        if (time <= 0) {
            //AnimationState state = anime[anime.clip.name];
            //state.time = state.length;
            //anime.Play();
        }
    }

    private void OnDisable() {
        
    }
}
