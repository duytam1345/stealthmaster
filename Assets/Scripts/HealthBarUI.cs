using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform original;

    Image img;

    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        img = transform.GetChild(1).GetComponent<Image>();
    }

    void LateUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(original.position+Vector3.forward);
    }

    public void SetImg(float amount)
    {
        img.fillAmount = amount;
    }
}
