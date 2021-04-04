using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    float x, y;

    Vector2 vMouseDown;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vMouseDown = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 dir = (Vector2)Input.mousePosition - vMouseDown;
            rb.velocity = new Vector3(dir.x, 0, dir.y) * speed;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            vMouseDown = Vector2.zero;
        }
    }
}
