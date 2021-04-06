using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int currentIWayPoint;

    public PlayerMovement player;

    public Transform model;

    public int speedMove;

    public int sight;

    public SpriteRenderer spriteSight;

    public enum TypeMoveEnemy
    {
        MoveAround,
        MoveToLastPosPlayer,
        MoveToPlayer,
    }
    public TypeMoveEnemy typeMoveEnemy;

    public float timeToFindPlayer;

    public Vector3 lastPosPlayerFinded;

    public float delayAttack;

    public GameObject exclmationMark;

    void Start()
    {
        model = transform.Find("Model");

        wayPoints = new List<Transform>();
        foreach (Transform item in transform.Find("Way Points"))
        {
            wayPoints.Add(item);
        }

        spriteSight = model.transform.Find("Sprite Sight").GetComponentInChildren<SpriteRenderer>();

        player = FindObjectOfType<PlayerMovement>();

        if (!exclmationMark)
        {
            exclmationMark = Instantiate(Resources.Load("exclmation mark") as GameObject, GameObject.Find("Canvas").transform);
        }
        exclmationMark.SetActive(false);
    }

    void Update()
    {
        if (exclmationMark)
        {
            exclmationMark.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(model.position + Vector3.up);
        }

        timeToFindPlayer -= Time.deltaTime;
        if (timeToFindPlayer <= 0)
        {
            FindPlayer();
            timeToFindPlayer = .1f;
        }

        switch (typeMoveEnemy)
        {
            case TypeMoveEnemy.MoveAround:
                exclmationMark.SetActive(false);
                Move();
                break;
            case TypeMoveEnemy.MoveToLastPosPlayer:
                exclmationMark.SetActive(true);
                MoveToLastPosPlayer();
                break;
            case TypeMoveEnemy.MoveToPlayer:
                exclmationMark.SetActive(true);
                MoveToPlayer();
                break;
        }
    }

    void Move()
    {
        if(wayPoints.Count<=0)
        {
            return;
        }

        if (Vector3.Distance(model.position, wayPoints[currentIWayPoint].position) > .1f)
        {
            if (LookAt(wayPoints[currentIWayPoint].position))
            {
                model.position = Vector3.MoveTowards(model.position, wayPoints[currentIWayPoint].position, speedMove * .5f * Time.deltaTime);
            }
        }
        else
        {
            currentIWayPoint++;
            if (currentIWayPoint > wayPoints.Count - 1)
            {
                currentIWayPoint = 0;
            }
        }
    }

    bool LookAt(Vector3 vector)
    {
        Quaternion quaternion = Quaternion.LookRotation(vector - model.position);
        Vector3 v = Manager.manager.QuaternionToEuler(quaternion);

        model.eulerAngles = new Vector3(
            Mathf.Clamp(0, 0, 0),
            Mathf.Clamp(v.y, GetEuler(model.eulerAngles).y - 1, GetEuler(model.eulerAngles).y + 1),
            Mathf.Clamp(v.z, model.eulerAngles.z - 1, model.eulerAngles.z + 1));

        if (Vector3.Distance(GetEuler(model.eulerAngles), v) > 5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Vector3 GetEuler(Vector3 angle)
    {
        return new Vector3(
            (angle.x > 180) ? angle.x - 360 : angle.x,
            (angle.y > 180) ? angle.y - 360 : angle.y,
            (angle.z > 180) ? angle.z - 360 : angle.z);
    }



    void FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(model.position, 5, 1 << 6);
        foreach (var item in colliders)
        {
            Vector3 dirToTarget = (item.transform.position - model.position).normalized;
            if (Vector3.Angle(model.forward, dirToTarget) < 90 / 2)
            {
                if (spriteSight.color != Color.red)
                {
                    spriteSight.color = Color.red;
                }

                typeMoveEnemy = TypeMoveEnemy.MoveToLastPosPlayer;

                lastPosPlayerFinded = item.transform.position;

                return;
            }
        }
        if (spriteSight.color != Color.white)
        {
            spriteSight.color = Color.white;
        }

        typeMoveEnemy = TypeMoveEnemy.MoveAround;
    }

    void MoveToLastPosPlayer()
    {
        if (Vector3.Distance(model.position, player.transform.position) > 5)
        {
            LookAt(lastPosPlayerFinded);
            model.position = Vector3.MoveTowards(model.position, lastPosPlayerFinded, speedMove * 1.5f * Time.deltaTime);
        }
        else
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (Vector3.Distance(model.position, player.transform.position) < 3)
        {
            Attack();
        }
        else if (Vector2.Distance(model.position, player.transform.position) > 5)
        {
            MoveToLastPosPlayer();
        }
        else
        {
            LookAt(player.transform.position);
            model.position = Vector3.MoveTowards(model.position, player.transform.position, speedMove * 1.5f * Time.deltaTime);
        }
    }

    void Attack()
    {
        delayAttack -= Time.deltaTime;

        if (delayAttack <= 0)
        {
            delayAttack = .5f;
            player.TakeDamage(10);
        }
    }

    public void TakeDamage()
    {
        Manager.manager.CheckWinLevel();
        Destroy(gameObject);
    }
}
