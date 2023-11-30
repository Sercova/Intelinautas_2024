using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuable : MonoBehaviour
{
    Animator ValuableAnim;

    private void Awake()
    {
        ValuableAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.layer == LayerMask.NameToLayer("Intelli_Player"))
        {
            ValuableAnim.SetTrigger("Grabbed");
            //Debug.Log("Gema " + gameObject.transform.name);
        }

    }
}
