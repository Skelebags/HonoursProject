using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBasedSystem : MonoBehaviour
{
    [Tooltip("Player Movement Speed")]
    public float speed = 1f;
    private float currentSpeed;

    [Tooltip("Player Max Health")]
    public const int healthMax = 100;

    // Also track current health
    public int healthCurrent { get; private set; }

    public int wins;

    // The enemy
    public GameObject enemy;

    // Current Action
    public Coroutine action;

    // This object's rigidbody
    private Rigidbody rigidbody;

    // Gameobjects for arms
    private GameObject LeftArm;
    private GameObject RightArm;

    // How Aggressively the character will move
    private int aggression = 0;

    // How often to change movement
    private float moveTimeMax = 2f;
    [SerializeField]
    private float moveTimer = 0f;

    /// <summary>
    /// Get the controllers
    /// </summary>
    public void GetArms()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag("arm_R"))
            {
                RightArm = child.gameObject;
            }
            if (child.CompareTag("arm_L"))
            {
                LeftArm = child.gameObject;
            }
        }

        LeftArm.GetComponent<ArmController>().Initialise();
        RightArm.GetComponent<ArmController>().Initialise();      
    }

    /// <summary>
    /// Reset the enemy to the starting position
    /// </summary>
    public void ResetEnemy()
    {
        transform.localPosition = new Vector3(-10f, -0.85f, 1.645f);
        healthCurrent = healthMax;
    }
    
    /// <summary>
    /// Subtracts damage taken from current health, taking into account blocking and parrying
    /// </summary>
    /// <param name="damage">The base damage dealt by the attack</param>
    public void TakeDamage(int damage)
    {
        if (LeftArm.GetComponent<ArmController>().state.ToString() == "parrying" || RightArm.GetComponent<ArmController>().state.ToString() == "parrying")
        {
            healthCurrent -= damage / 2;
        }
        else if (LeftArm.GetComponent<ArmController>().state.ToString() != "blocking" || RightArm.GetComponent<ArmController>().state.ToString() != "blocking")
        {
            healthCurrent -= damage;
        }
    }

    public void SetAggression(int aggro)
    {
        aggression += aggro;
    }

    private void Start()
    {
        wins = 0;
        rigidbody = GetComponent<Rigidbody>();
        currentSpeed = speed;
    }

    private void Update()
    {
        GetComponent<MeshRenderer>().material.color = new Color ((float)(healthCurrent / 100f), 0f, 0f);

        // Call the appropriate arm function
        if(LeftArm.name == "Sword_Arm_No_ANN(Clone)" || RightArm.name == "Sword_Arm_No_ANN(Clone)")
        {
            SwordArms();
        }
        if (LeftArm.name == "Shield_Arm_No_ANN(Clone)" || RightArm.name == "Shield_Arm_No_ANN(Clone)")
        {
            ShieldArms();
        }
        if (LeftArm.name == "Gun_Arm_No_ANN(Clone)" || RightArm.name == "Gun_Arm_No_ANN(Clone)")
        {
            GunArms();
        }

        // Increment move timer
        moveTimer += Time.deltaTime;

        // If timer is at or over max, check for movement change
        if(moveTimer >= moveTimeMax)
        {
            // move towards or away from opponent based on aggression
            switch (UnityEngine.Random.Range(0, 100))
            {
                case int n when n < aggression:
                    currentSpeed = speed;
                    break;
                default:
                    currentSpeed = -speed;
                    break;
            }
            moveTimer = 0;
        }
        rigidbody.velocity = new Vector3(currentSpeed, 0f, 0f);

        if ((RightArm.GetComponent<ArmController>().state.ToString() == "idle" && LeftArm.GetComponent<ArmController>().state.ToString() == "idle") && action != null)
        {
            StopCoroutine(action);
            action = null;
        }

    }

    private void SwordArms()
    {
        // Sometimes parry, sometimes swing
        switch (UnityEngine.Random.Range(0, 100))
        {
            case int n when n < 25:
                if (LeftArm.name == "Sword_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(LeftArm.GetComponent<SwordController>().ActionTwo());
                }
                if (RightArm.name == "Sword_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(RightArm.GetComponent<SwordController>().ActionTwo());
                }
                break;
            case int n when n >= 25 && n < 85:
                if (LeftArm.name == "Sword_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(LeftArm.GetComponent<SwordController>().ActionOne());
                }
                if (RightArm.name == "Sword_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(RightArm.GetComponent<SwordController>().ActionOne());
                }
                break;
            default:
                break;
        }
    }

    private void ShieldArms()
    {
        // Sometimes Block, sometimes bash
        switch (UnityEngine.Random.Range(0, 100))
        {
            case int n when n < 65:
                if (LeftArm.name == "Shield_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(LeftArm.GetComponent<ShieldController>().ActionOne());
                }
                if (RightArm.name == "Shield_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(RightArm.GetComponent<ShieldController>().ActionOne());
                }
                break;
            case int n when n >= 65 && n < 85:
                if (LeftArm.name == "Shield_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(LeftArm.GetComponent<ShieldController>().ActionTwo());
                }
                if (RightArm.name == "Shield_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(RightArm.GetComponent<ShieldController>().ActionTwo());
                }
                break;
            default:
                break;
        }
    }

    private void GunArms()
    {
        // Always reload
        if (LeftArm.name == "Gun_Arm_No_ANN(Clone)" && !LeftArm.GetComponent<GunController>().IsLoaded())
        {
            action = StartCoroutine(LeftArm.GetComponent<GunController>().ActionTwo());
        }
        if (RightArm.name == "Gun_Arm_No_ANN(Clone)" && !RightArm.GetComponent<GunController>().IsLoaded())
        {
            action = StartCoroutine(RightArm.GetComponent<GunController>().ActionTwo());
        }

        // Sometimes Shoot
        switch (UnityEngine.Random.Range(0, 100))
        {
            case int n when n < 75:
                if (LeftArm.name == "Gun_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(LeftArm.GetComponent<GunController>().ActionOne());
                }
                if (RightArm.name == "Gun_Arm_No_ANN(Clone)")
                {
                    action = StartCoroutine(RightArm.GetComponent<GunController>().ActionOne());
                }
                break;
            default:
                break;
        }
    }
}
