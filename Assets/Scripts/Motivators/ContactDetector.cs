using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour
{
    public enum Sides { Left, Right, Both };
    public Sides Side;
    public float magnitude;

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player"
            && arg_collision is CapsuleCollider2D)
        {
            float angle = 0.0f;

            Rigidbody2D RB = arg_collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController PCtrll = arg_collision.gameObject.GetComponent<PlayerController>();

            PCtrll.StopDragging();

            float xcomponent = 0.0f;
            float ycomponent = 0.0f;

            if (Side == Sides.Left)
            {
                angle = 45;
                xcomponent = -Mathf.Cos(angle * Mathf.PI / 180) * magnitude;
            }
            else if (Side == Sides.Right)
            {
                angle = 45.0f;
                xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * magnitude;
            }

            ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * magnitude;

            RB.AddRelativeForce(new Vector2(xcomponent, ycomponent));
        }
    }
}
