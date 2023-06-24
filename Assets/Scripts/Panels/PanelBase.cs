using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public bool init { get; protected set; }
    public bool isOpen { get; protected set; }

    //正打开着的窗口。
    static protected HashSet<PanelBase> openingPanel = new HashSet<PanelBase>();

    public virtual void ShowPanel() {
        if (!init) {
            init = true;
            canvasGroup.alpha = 0;
        }
        gameObject.SetActive(true);
        this.StartCoroutineSingle(ref showHidePanelCoroutine, ShowHidePanel(true, 0.2f));
        isOpen = true;
        Time.timeScale = 0;
        openingPanel.Add(this);
    }

    public virtual void HidePanel() {
        this.StartCoroutineSingle(ref showHidePanelCoroutine, ShowHidePanel(false, 0.2f));
        isOpen = false;
        openingPanel.Remove(this);
        if(openingPanel.Count <= 0) { //全部窗口都关了再恢复。
            Time.timeScale = 1;
        }
    }

    IEnumerator ShowHidePanel(bool show, float time) {
        float curTime = 0;
        float alpha = canvasGroup.alpha;
        while (curTime < time) {
            curTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(alpha, show ? 1 : 0, curTime / time);
            yield return null;
        }
        canvasGroup.alpha = show ? 1 : 0;
        if (!show) {
            gameObject.SetActive(false);
        }
    }
    IEnumerator showHidePanelCoroutine;

#if UNITY_EDITOR
    private void OnValidate() {
        if(canvasGroup == null) {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
#endif
}
