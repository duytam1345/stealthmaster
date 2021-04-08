using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfEnemy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, 4 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag =="Player")
        {
            collider.GetComponent<PlayerMovement>().TakeDamage(20);
            Destroy(gameObject);
        }
        else if(collider.tag=="Wall")
        {
            Destroy(gameObject);
        }
    }
}
