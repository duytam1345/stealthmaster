using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public string nameLevel;
    public enum TypeRound
    {
        Fight,
        Item
    }

    public List<TypeRound> typeRounds;

    public int[] startRemainEnemy;
    public int remainEnemy;

    public bool hasBoss;

    public int maxRound;
    public int indexCurrentRound;

    public bool win;

    public void SetStart()
    {
        if (indexCurrentRound == 0)
        {
            Transform t = Manager.manager.ui.processLevelContent;

            foreach (Transform item in t)
            {
                Destroy(item.gameObject);
            }

            Manager.manager.ui.textContract.text = nameLevel;

            foreach (var item in typeRounds)
            {
                if (item == TypeRound.Fight)
                {
                    GameObject g = Instantiate(Resources.Load("UI/Image Process Level Fight") as GameObject, t);
                }
                else
                {
                    GameObject g = Instantiate(Resources.Load("UI/Image Process Level Take Item") as GameObject, t);
                }
            }
        }

        SetNewRound();
    }

    public void SetNewRound()
    {
        indexCurrentRound++;

        if (indexCurrentRound > maxRound)
        {
            Manager.manager.LoadNextLevel();
            return;
        }

        win = false;

        remainEnemy = startRemainEnemy[indexCurrentRound - 1];

        Manager.manager.SetTextRemainEnemy();

        for (int i = 0; i < GameObject.Find("Map").transform.childCount; i++)
        {
            Destroy(GameObject.Find("Map").transform.GetChild(i).gameObject);
        }

        //if (GameObject.Find("Map").transform.childCount > 0)
        //{
        //    Destroy(GameObject.Find("Map").transform.GetChild(0).gameObject);
        //}

        if (typeRounds[indexCurrentRound-1] == TypeRound.Fight)
        {
            GameObject g = Instantiate(Resources.Load("Level/" + nameLevel + indexCurrentRound) as GameObject, GameObject.Find("Map").transform);
        }
        else
        {
            Manager.manager.selecNewItem = true;
        }

        Manager.manager.pause = false;
    }

    void OnEnable()
    {
        indexCurrentRound = 0;
    }
}
