using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    float x, y;

    Vector2 vMouseDown;

    public RectTransform imageInput;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vMouseDown = Input.mousePosition;
            imageInput.position = Input.mousePosition;
            imageInput.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 dir = ((Vector2)Input.mousePosition - vMouseDown).normalized;
            rb.velocity = new Vector3(dir.x, 0, dir.y) * speed;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            imageInput.gameObject.SetActive(false);
            vMouseDown = Vector2.zero;
        }
    }
}
