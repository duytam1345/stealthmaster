using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int currentIWayPoint;

    public Transform model;

    public int speedMove;

    public int sight;

    public SpriteRenderer spriteSight;

    void Start()
    {
        model = transform.Find("Model");

        wayPoints = new List<Transform>();
        foreach (Transform item in transform.Find("Way Points"))
        {
            wayPoints.Add(item);
        }

        spriteSight = model.transform.Find("Sprite Sight").GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {

        if (FindPlayer())
        {
            MoveToPlayer();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        if (spriteSight.color != Color.white)
        {
            spriteSight.color = Color.white;
        }

        if (Vector3.Distance(model.position, wayPoints[currentIWayPoint].position) > .1f)
        {
            if (LookAt(wayPoints[currentIWayPoint].position))
            {
                model.position = Vector3.MoveTowards(model.position, wayPoints[currentIWayPoint].position, speedMove * Time.deltaTime);
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
        Vector3 v = QuaternionToEuler(quaternion);

        model.eulerAngles = new Vector3(
            Mathf.Clamp(v.x, model.eulerAngles.x - 1, model.eulerAngles.x + 1),
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

    bool FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(model.position, 5, 1 << 6);
        foreach (var item in colliders)
        {
            Vector3 dirToTarget = (item.transform.position - model.position).normalized;
            if (Vector3.Angle(model.forward, dirToTarget) < 90 / 2)
            {
                return true;
            }
        }
        return false;
    }

    void MoveToPlayer()
    {
        if (spriteSight.color != Color.red)
        {
            spriteSight.color = Color.red;
        }
    }
}
