using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum State{making, idle, attackingR, attackingL, blocking};
    public enum MoveState { left, right, idle}

    public State state;
    public MoveState moveState;

    GameObject armR;
    GameObject armL;

    Rigidbody rb;

    public Button startButton;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float timeToJumpApex = 1.0f;
    [SerializeField]
    private bool jumping;
    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        Button strtbtn = startButton.GetComponent<Button>();
        strtbtn.onClick.AddListener(GetArms);

        rb = gameObject.GetComponent<Rigidbody>();

        state = State.making;
        moveState = MoveState.idle;

        jumping = false;
        onGround = true;
    }

    private void Update()
    {
        if (state != State.making)
        {
            if(state != State.idle)
            {
                state = State.idle;
            }

            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    moveState = MoveState.right;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    moveState = MoveState.left;
                }

                if ((Input.GetKeyDown(KeyCode.W)) && onGround)
                {
                    jumping = true;
                }
            }
            else
            {
                moveState = MoveState.idle;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (state == State.idle)
                {
                    switch (armL.name)
                    {
                        case "Sword_Arm(Clone)":
                            state = State.attackingL;
                            break;
                        case "Shield_Arm(Clone)":
                            state = State.blocking;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (state == State.idle)
                {
                    switch (armR.name)
                    {
                        case "Sword_Arm(Clone)":
                            state = State.attackingR;
                            break;
                        case "Shield_Arm(Clone)":
                            state = State.blocking;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        

    }

    // Fixed update for physics interaction
    void FixedUpdate()
    {
        if (state != State.making)
        {
            Vector3 velocity = rb.velocity;
            Vector3 movementVector = gameObject.transform.rotation * Vector3.forward;

            switch(moveState)
            {
                case MoveState.right:
                    velocity.x = movementVector.x * speed;
                    break;

                case MoveState.left:
                    velocity.x = movementVector.x * -speed;
                    break;

                case MoveState.idle:
                    velocity.x = 0;
                    break;

                default:
                    break;
            }

            if(jumping)
            {
                onGround = false;
                velocity.y = Mathf.Abs(Physics.gravity.magnitude) * timeToJumpApex;
                jumping = false;
            }

            rb.velocity = velocity;
        }
    }

    void GetArms()
    {
        foreach (Transform child in gameObject.transform)
        {
            if(child.CompareTag("arm_R"))
            {
                armR = child.gameObject;
            }
            if (child.CompareTag("arm_L"))
            {
                armL = child.gameObject;
            }
        }

        state = State.idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumping = false;
            onGround = true;
        }
    }

}
