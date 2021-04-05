using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public PlayerMovement player;

    public Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 v = offset + player.transform.position;
        transform.position = Vector3.Lerp(transform.position, v, 3* Time.deltaTime);
    }
}
