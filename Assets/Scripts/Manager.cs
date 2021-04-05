using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager manager;

    public Level level;

    public bool pause;

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
    }

    // Level _ tao map_ tao quai

    public void CheckWinLevel()
    {
        if (!level.win)
        {
            level.leftEnemy--;

            if (level.leftEnemy <= 0)
            {
                level.win = true;
                OpenDoor();
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

    public void LoadNextRound()
    {
        pause = true;
        SceneManager.LoadScene(0);
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
