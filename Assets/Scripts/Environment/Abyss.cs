using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    public Transform playerTransf;

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.transform.tag == "Player")
        {
            playerTransf.position = CheckPoint.ChP_LastReachedPosition();
        }
    }
}
