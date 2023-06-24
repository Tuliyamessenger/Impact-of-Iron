using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI text;

    public void SetFill(float fill) {
        fillImage.fillAmount = Mathf.Clamp01(fill);
    }

    public void SetText(string txt) {
        text.SetText(txt);
    }
}
