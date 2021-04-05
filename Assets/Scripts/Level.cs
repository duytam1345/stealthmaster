using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public enum TypeLevel
    {
        _1, //Fight_TakeItem X3
        _2, //Fight_Fight_TakeItem + Fight_TakeItem X2
        _3, //Fight_Fight_TakeItem  X2 + Fight_TakeItem
        _4, //Fight_Fight_TakeItem  X3
        _5, //Fight_Fight_TakeItem  X4
    }

    public TypeLevel typeLevel;

    public int startLeftEnemy;
    public int leftEnemy;

    public bool hasBoss;

    public int indexCurrentRound;

    public bool win;

    public void SetStart()
    {
        SetNewRound();
    }

    public void SetNewRound()
    {
        win = false;
        leftEnemy = Random.Range(1, 5);
        for (int i = 0; i < leftEnemy; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-6f, 6f), .5f, Random.Range(4.5f, -9f));
            GameObject e = Instantiate(Resources.Load("Enemy") as GameObject, pos, Quaternion.identity);
        }
    }

    void OnEnable()
    {
        indexCurrentRound = 0;
    }
}
