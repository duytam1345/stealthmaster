using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager manager;

    public Level[] levels;
    public Level level;

    public bool pause;

    public bool rolling;

    public UICanvas ui;

    public int money;

    public bool selecNewItem;

    public enum Item
    {
        Money,
        Hp,
        Arrmor,
        Weapon,
        Speed
    }

    void Awake()
    {
        if (!manager)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        ui = FindObjectOfType<UICanvas>();
    }

    public void CheckWinLevel()
    {
        if (!level.win)
        {
            level.remainEnemy--;

            SetTextRemainEnemy();

            if (level.remainEnemy <= 0)
            {
                if (level != levels[0])
                {
                    if (level.indexCurrentRound == level.maxRound)
                    {
                        ui.panelBossWasKilled.SetActive(true);
                        ui.nextLevelButton.SetActive(true);
                    }
                    else
                    {
                        OpenDoor();
                    }
                }
                else
                {
                    OpenDoor();
                }
            }
        }
    }

    void OpenDoor()
    {
        StartCoroutine(OpenDoorCo());
    }

    IEnumerator OpenDoorCo()
    {
        Transform t = GameObject.Find("Door").transform;
        Vector3 v = t.position;
        while (t.position.x < 4)
        {
            v.x += .1f;
            t.position = v;
            yield return null;
        }
    }


    public void SetStart()
    {
        level.SetStart();
        ShowContract();
    }
    public void LoadNext()
    {
        if (level.indexCurrentRound == level.maxRound)
        {
            StartCoroutine(LoadNextLevelCo());
        }
        else
        {
            StartCoroutine(LoadNextRoundCo());
        }
    }

    IEnumerator LoadNextRoundCo()
    {
        pause = true;

        FocusContract();

        yield return new WaitForSeconds(1);

        RectTransform rect = ui.processLevelContent.GetChild(level.indexCurrentRound - 1).GetComponent<RectTransform>();

        rect.DOShakeScale(.5f);

        yield return new WaitForSeconds(.25f);

        rect.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(.25f);

        UnFocusContract();

        yield return new WaitForSeconds(1.5f);

        yield return FadeCo();

        ShowContract();
        HideContractCo(2);

        pause = false;
    }

    IEnumerator LoadNextLevelCo()
    {
        if (level.nameLevel == "Tutorial")
        {
            ui.panelTutorialComplete.SetActive(true);
        }

        pause = true;

        yield return new WaitForSeconds(.5f);

        ui.nextLevelButton.SetActive(true);
    }

    public void Fade()
    {
        StartCoroutine(FadeCo());
    }

    IEnumerator FadeCo()
    {
        Image img = GameObject.Find("Fade Image").GetComponent<Image>();

        img.DOFade(1, 1);
        yield return new WaitForSeconds(1); //fade out

        for (int i = 0; i < GameObject.Find("Trash").transform.childCount; i++)
        {
            Destroy(GameObject.Find("Trash").transform.GetChild(i).gameObject);
        }

        level.SetNewRound();

        FindObjectOfType<PlayerMovement>().transform.position = new Vector3(0, .5f, -8.5f);

        yield return new WaitForSeconds(.5f);//reset map time

        ui.panelTutorialComplete.SetActive(false);
        ui.nextLevelButton.SetActive(false);
        ui.panelBossWasKilled.SetActive(false);
        ui.panelPlayAgain.SetActive(false);

        img.DOFade(0, 1);
        yield return new WaitForSeconds(1);//fade in

        if (selecNewItem)
        {
            RandomItem();
        }
    }

    public void SetTextRemainEnemy()
    {
        GameObject.Find("TextRemainEnemy").GetComponent<Text>().text = level.remainEnemy.ToString();
    }

    public void ShowContract()
    {
        ui.processLevelContent.GetChild(level.indexCurrentRound - 1).GetComponent<Image>().color = Color.yellow;
        ui.panelContract.GetComponent<RectTransform>().DOAnchorPos3DY(0, 1, false);
    }

    public void HideContract()
    {
        ui.panelContract.GetComponent<RectTransform>().DOAnchorPos3DY(250, 1, false);
    }
    public void HideContract(float t)
    {
        StartCoroutine(HideContractCo(t));
    }
    IEnumerator HideContractCo(float t)
    {
        yield return new WaitForSeconds(t);
        ui.panelContract.GetComponent<RectTransform>().DOAnchorPos3DY(250, 1, false);
    }

    public void FocusContract()
    {
        ui.panelContract.GetComponent<RectTransform>().DOAnchorPos3DY(-200, 1, false);
        ui.panelContract.GetComponent<RectTransform>().DOScaleX(1.5f, 1);
        ui.panelContract.GetComponent<RectTransform>().DOScaleY(1.5f, 1);
    }

    public void UnFocusContract()
    {
        ui.panelContract.GetComponent<RectTransform>().DOAnchorPos3DY(-0, 1, false);
        ui.panelContract.GetComponent<RectTransform>().DOScaleX(1, 1);
        ui.panelContract.GetComponent<RectTransform>().DOScaleY(1, 1);
    }

    public void LoadNextLevel()
    {
        bool b = false;
        foreach (Level item in levels)
        {
            if (b)
            {
                level = item;
                level.SetStart();
                return;
            }

            if (level == item)
            {
                b = true;
            }
        }
    }

    public void SetTextMoney()
    {
        ui.textMoney.text = money.ToString();
    }

    public void TakeMoney(int i)
    {
        money += i;
        SetTextMoney();
    }

    public void RandomItem()
    {
        //selecNewItem = false;
        //ShowSelectItem();
        //StartCoroutine(RandomItemCo());
    }

    IEnumerator RandomItemCo()
    {
        pause = true;
        rolling = true;

        float t = .5f;

        Item item1 = Item.Arrmor;
        Item item2 = Item.Arrmor;
        Item item3 = Item.Arrmor;

        int r = 0;

        while (t > 0)
        {
            t -= .1f;

            r = Random.Range(0, 5);
            item1 = (Item)r;

            r = Random.Range(0, 5);
            item2 = (Item)r;

            r = Random.Range(0, 5);
            item3 = (Item)r;

            switch (item1)
            {
                case Item.Money:
                    ui.textSelectItem1.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem1.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem1.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem1.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem1.text = "+10% Speed";
                    break;
            }

            switch (item2)
            {
                case Item.Money:
                    ui.textSelectItem2.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem2.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem2.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem2.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem2.text = "+10% Speed";
                    break;
            }

            switch (item3)
            {
                case Item.Money:
                    ui.textSelectItem3.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem3.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem3.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem3.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem3.text = "+10% Speed";
                    break;
            }

            yield return new WaitForSeconds(.1f);
        }

        t = .5f;

        while (t > 0)
        {
            t -= .1f;

            r = Random.Range(0, 5);
            item2 = (Item)r;

            r = Random.Range(0, 5);
            item3 = (Item)r;

            switch (item2)
            {
                case Item.Money:
                    ui.textSelectItem2.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem2.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem2.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem2.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem2.text = "+10% Speed";
                    break;
            }

            switch (item3)
            {
                case Item.Money:
                    ui.textSelectItem3.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem3.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem3.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem3.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem3.text = "+10% Speed";
                    break;
            }

            yield return new WaitForSeconds(.1f);
        }

        t = .5f;

        while (t > 0)
        {
            t -= .1f;

            r = Random.Range(0, 5);
            item3 = (Item)r;

            switch (item3)
            {
                case Item.Money:
                    ui.textSelectItem3.text = "+100 Money";
                    break;
                case Item.Hp:
                    ui.textSelectItem3.text = "+25% Hp";
                    break;
                case Item.Arrmor:
                    ui.textSelectItem3.text = "+1 Arrmor";
                    break;
                case Item.Weapon:
                    ui.textSelectItem3.text = "Gun";
                    break;
                case Item.Speed:
                    ui.textSelectItem3.text = "+10% Speed";
                    break;
            }

            yield return new WaitForSeconds(.1f);
        }

        rolling = false;
    }
    public void ShowSelectItem()
    {
        ui.panelSelectItem.SetActive(true);
    }
    public void HideSelectItem()
    {
        if (level.indexCurrentRound == 1)
        {
        }
        else
        {
            Fade();
        }
        pause = false;
        ui.panelSelectItem.SetActive(false);
    }

    public void PlayerDeath()
    {
        PlayerMovement p = FindObjectOfType<PlayerMovement>();

        p.hp = 100;
        p.healthUI.SetImg(p.hp / p.maxHp);

        pause = true;
        ui.panelPlayAgain.SetActive(true);
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCamCo());
    }

    IEnumerator ShakeCamCo()
    {
        Vector3 p = Camera.main.transform.position;

        float t = .5f;

        while (t > 0)
        {
            t -= .1f;

            Camera.main.transform.position = p + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));

            yield return new WaitForSeconds(Time.deltaTime);
        }

        Camera.main.transform.position = p;
    }

    public Vector3 QuaternionToEuler(Quaternion q)
    {
        Vector3 euler;

        // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
        float unit = (q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w);

        // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
        float test = q.x * q.w - q.y * q.z;

        if (test > 0.4995f * unit) // singularity at north pole
        {
            euler.x = Mathf.PI / 2;
            euler.y = 2f * Mathf.Atan2(q.y, q.x);
            euler.z = 0;
        }
        else if (test < -0.4995f * unit) // singularity at south pole
        {
            euler.x = -Mathf.PI / 2;
            euler.y = -2f * Mathf.Atan2(q.y, q.x);
            euler.z = 0;
        }
        else // no singularity - this is the majority of cases
        {
            euler.x = Mathf.Asin(2f * (q.w * q.x - q.y * q.z));
            euler.y = Mathf.Atan2(2f * q.w * q.y + 2f * q.z * q.x, 1 - 2f * (q.x * q.x + q.y * q.y));
            euler.z = Mathf.Atan2(2f * q.w * q.z + 2f * q.x * q.y, 1 - 2f * (q.z * q.z + q.x * q.x));
        }

        // all the math so far has been done in radians. Before returning, we convert to degrees...
        euler *= Mathf.Rad2Deg;

        //...and then ensure the degree values are between 0 and 360
        euler.x %= 360;
        euler.y %= 360;
        euler.z %= 360;

        return euler;
    }
}
