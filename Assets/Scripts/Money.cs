using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public bool canTake;

    public PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (canTake)
            {
                player.TakeMoney();
                Destroy(gameObject);
            }
        }
    }
}
