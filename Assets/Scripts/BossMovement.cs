using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Transform[] waypoints1;
    public Transform[] waypoints2;

    public Transform model;

    public int indexWp;

    public int hp = 3;

    public bool running;

    public enum State
    {
        Idle,
        MoveToWp1,
        MoveToWp2,
        Attack
    }

    public State state;

    public bool isAlive;

    bool rotateToLeft;

    Vector3 vRotate;

    public float delayAttack;

    PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (FindPlayer())
                {
                    Attack();
                }
                else
                {
                    vRotate = model.eulerAngles;
                    if (rotateToLeft)
                    {
                        if (model.eulerAngles.y < 225)
                        {
                            vRotate.y++;
                        }
                        else
                        {
                            rotateToLeft = false;
                        }
                    }
                    else
                    {
                        if (model.eulerAngles.y > 135)
                        {
                            vRotate.y--;
                        }
                        else
                        {
                            rotateToLeft = true;
                        }
                    }
                    model.eulerAngles = vRotate;
                }
                break;
            case State.MoveToWp1:
                if (model.position != waypoints1[indexWp].position)
                {
                    LookAt(waypoints1[indexWp].position);
                    model.position = Vector3.MoveTowards(model.position, waypoints1[indexWp].position, 10 * Time.deltaTime);
                }
                else
                {
                    indexWp++;
                    if (indexWp >= waypoints1.Length)
                    {
                        model.GetChild(1).gameObject.SetActive(true);
                        state = State.Idle;
                        indexWp = 0;
                    }
                }
                break;
            case State.MoveToWp2:
                if (model.position != waypoints2[indexWp].position)
                {
                    LookAt(waypoints2[indexWp].position);
                    model.position = Vector3.MoveTowards(model.position, waypoints2[indexWp].position, 10 * Time.deltaTime);
                }
                else
                {
                    indexWp++;
                    if (indexWp >= waypoints2.Length)
                    {
                        model.GetChild(1).gameObject.SetActive(true);
                        state = State.Idle;
                        indexWp = 0;
                    }
                }
                break;
        }
    }

    bool FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(model.position, 7, 1 << 6);
        foreach (var item in colliders)
        {
            Vector3 dirToTarget = (item.transform.position - model.position).normalized;
            if (Vector3.Angle(model.forward, dirToTarget) < 90 / 2)
            {
                float distance = Vector3.Distance(model.position, item.transform.position);

                if (!Physics.Raycast(model.position, dirToTarget, distance, 1 << 9))
                {
                    LookAt(player.transform.position);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    void Attack()
    {
        delayAttack -= Time.deltaTime;
        if (delayAttack < 0)
        {
            delayAttack = 2;

            for (int i = -2; i <= 2; i++)
            {
                GameObject g = Instantiate(Resources.Load("Bullet Boss") as GameObject, model.transform.position, Quaternion.identity);
                g.transform.eulerAngles = model.eulerAngles + new Vector3(0, i * 15, 0);
            }
        }
    }

    void LookAt(Vector3 vector)
    {
        Quaternion quaternion = Quaternion.LookRotation(vector - model.position);
        Vector3 v = Manager.manager.QuaternionToEuler(quaternion);
        model.eulerAngles = new Vector3(0, v.y, v.z);
    }

    public Vector3 GetEuler(Vector3 angle)
    {
        return new Vector3(
            (angle.x > 180) ? angle.x - 360 : angle.x,
            (angle.y > 180) ? angle.y - 360 : angle.y,
            (angle.z > 180) ? angle.z - 360 : angle.z);
    }

    public void TakeDamage()
    {
        if (isAlive)
        {
            Manager.manager.ShakeCamera();
            GameObject e = Instantiate(Resources.Load("Blood Effect") as GameObject, model.position, Quaternion.identity);
            Destroy(e, 1f);

            hp--;

            if (hp == 2)
            {
                state = State.MoveToWp1;
                model.GetChild(1).gameObject.SetActive(false);
            }
            else if (hp == 1)
            {
                state = State.MoveToWp2;
                model.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                isAlive = false;
                Manager.manager.TakeMoney(100);
                Manager.manager.CheckWinLevel();
                Destroy(gameObject);
            }
        }
    }
}
