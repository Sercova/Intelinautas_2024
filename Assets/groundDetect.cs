using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundDetect : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public bool isGrounded;

    private void OnCollisionEnter2D(Collision2D arg_collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D arg_collision)
    {
        isGrounded = false;
    }
}
