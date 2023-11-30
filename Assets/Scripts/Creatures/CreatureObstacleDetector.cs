using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureObstacleDetector : MonoBehaviour
{
    public enum Sides { Left, Right, Both };
    public Sides Side;

    GameObject ParentObj;
    CreatureAI ParentObjAI;

    private float jumpMagnitude;
    private float momentumFactor;

    Rigidbody2D ParentRB;
    //float initialSpeed;

    private void Start()
    {
        ParentObj = gameObject.transform.parent.gameObject;
        ParentObjAI = ParentObj.GetComponent<CreatureAI>();
        ParentRB = ParentObj.GetComponentInParent<Rigidbody2D>();

        jumpMagnitude = ParentObjAI.jumpMagnitude;
        momentumFactor = ParentObjAI.momentumFactor;
    }
    private void OnTriggerStay2D(Collider2D arg_collision)
    {
        JumpSystem(jumpMagnitude);
    }

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        JumpSystem(jumpMagnitude * momentumFactor);
    }

    private void JumpSystem(float arg_force)
    {
        if (!ParentObjAI.stuck && ParentObjAI.rb.velocity.y >= 0.0f && ParentObjAI.enabled)
            ParentObjAI.Jump(arg_force);
    }
}
