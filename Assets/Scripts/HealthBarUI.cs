using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform original;

    Image img;

    Image imgArm;

    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        img = transform.GetChild(1).GetComponent<Image>();
        imgArm = transform.GetChild(2).GetComponent<Image>();
    }

    void LateUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(original.position+Vector3.forward);
    }

    public void SetImg(float amount)
    {
        img.fillAmount = amount;
    }

    public void SetImgArmor(float amount)
    {
        imgArm.fillAmount = amount;
    }
}
