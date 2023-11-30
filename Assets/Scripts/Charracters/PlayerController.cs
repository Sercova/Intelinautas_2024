
using System;
using UnityEngine;
//using System.IO;

public class PlayerController : MonoBehaviour
{
// Public properties
    public FloatingJoystick joystick;
    public float charSpeedX = 1f;
    public float jumpSpeed = 1f;
    public bool isDragging = false;
    public groundDetect ground;
    public bool isHit = false;

    // Private properties
    private float inputX;
    private float inputY;
    private int charDir;
    private Rigidbody2D RB2D;
    [SerializeField]
    private InteractuableReference CurrentInteractuable;
    private int jumpCounter = 0; 
    private int maxJumps = 2; // first jump is zero

    private void Start()
    {
        charDir = 1;
        RB2D = gameObject.GetComponent<Rigidbody2D>();
    }
    private bool GRD;
    private void Update()
    {
        GRD = ground.isGrounded;

        if (Input.GetMouseButtonDown(0) 
            && !isDragging 
            && CurrentInteractuable.Variable.Value == "-" 
            && jumpCounter < maxJumps)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit_collider = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Intelli_Player"));
            isDragging = hit_collider.collider != null;
            if (isDragging)
            {
                jumpCounter++;
                CurrentInteractuable.Variable.Value = transform.name;
            }
        }

        if (GRD &&  jumpCounter > 0 && !isDragging)
        {
            jumpCounter = 0;
        }
            

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }

        if (isDragging && !isHit )
        {
            inputX = joystick.Horizontal;
            inputY = joystick.Vertical;
            float deltaY  = 0.0f;

            if (inputY > 0.3f && inputY < 1.0f && deltaY == 0.0f)
            {
                deltaY = inputY * jumpSpeed;
            }

            if (inputX < 0.0f)
                charDir = -1;
            else if (inputX > 0.0f)
                charDir = 1;

            gameObject.transform.localScale = new Vector3(charDir, 1.0f, 1.0f);

            float offSetY = Mathf.Lerp(0.0f, deltaY, Time.deltaTime);

            Vector3 newCharPos = gameObject.transform.position + new Vector3(inputX * Time.deltaTime * charSpeedX, offSetY, 0f);

            gameObject.transform.position = newCharPos;
        }
    }

    public void StopDragging()
    {
        isDragging = false;
        CurrentInteractuable.Variable.Value = "-";
        RB2D.velocity = new Vector2(0.0f, 0.0f);
    }
}