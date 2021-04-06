using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public GameObject nextLevelButton;

    public GameObject panelTutorialComplete;

    public Text textMoney;

    public void NextLevelButton()
    {
        Manager.manager.Fade();
    }
}
