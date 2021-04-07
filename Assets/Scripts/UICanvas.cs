using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public GameObject nextLevelButton;

    public GameObject panelTutorialComplete;

    public Text textMoney;

    public GameObject panelContract;

    public Transform processLevelContent;

    public GameObject panelSelectItem;

    public Text textSelectItem1;
    public Text textSelectItem2;
    public Text textSelectItem3;

    public void NextLevelButton()
    {
        Manager.manager.Fade();
    }

    public void SelectItemBtn(Text t)
    {
        if(Manager.manager.rolling)
        {
            return;
        }

        switch (t.text)
        {
            case "+100 Money":
                Manager.manager.TakeMoney(100);
                break;
            case "+25% Hp":
                FindObjectOfType<PlayerMovement>().TakeHp();
                break;
            case "+1 Arrmor":
                FindObjectOfType<PlayerMovement>().TakeArmor();
                break;
            case "Gun":
                FindObjectOfType<PlayerMovement>().weapon = PlayerMovement.Weapon.Gun;
                break;
            case "+10% Speed":
                FindObjectOfType<PlayerMovement>().TakeSpeed();
                break;
        }

        Manager.manager.HideSelectItem();
    }
}
