using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxNext : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag =="Player")
        {
            Manager.manager.LoadNextRound();
        }
    }
}
