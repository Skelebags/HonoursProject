using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAgent : Agent
{
    // The state machine for the player
    [HideInInspector]
    public enum State { making, playing};
    public State state;

    [Tooltip("Player Movement Speed")]
    public float speed = 1f;

    [Tooltip("Player Max Health")]
    public const int healthMax = 100;

    // Also track current health
    public int healthCurrent { get; private set; }

    public int wins { get; private set; }

    // Current Action
    public Coroutine action;

    [Tooltip("Is this agent in training mode?")]
    public bool trainingMode;

    [Tooltip("The Enemy")]
    public GameObject enemy;

    // Initialise the left and right arms, do not define yet
    private GameObject LeftArm { get; set; }
    private GameObject RightArm { get; set; }

    private ArmAgent LeftAgent { get; set; }
    private ArmAgent RightAgent { get; set; }

    // The agent's rigidbody
    new private Rigidbody rigidbody;

    private bool frozen = false;

    private StatsRecorder recorder;

    /// <summary>
    /// Initialise the agent
    /// </summary>
    public override void Initialize()
    {
        recorder = Academy.Instance.StatsRecorder;
        rigidbody = GetComponent<Rigidbody>();
        state = State.making;
        frozen = true;
        wins = 0;

        // If not in training mode, play forever
        if (!trainingMode) MaxStep = 0;
    }

    public override void OnEpisodeBegin()
    {
        // Set current health to maximum
        healthCurrent = healthMax;

        // Zero out movement
        rigidbody.velocity = Vector3.zero;

        // Reset position
        transform.localPosition = new Vector3(0.45f, -0.846f, 1.645f);

        // Reset the enemy
        enemy.GetComponent<RuleBasedSystem>().ResetEnemy();

        // Reset the arm episodes
        LeftAgent.EndEpisode();
        RightAgent.EndEpisode();
    }

    /// <summary>
    /// Called on input received from either player or neural network]
    /// 
    /// vectorAction[i] represents:
    /// index 0 = x movement (-1 = left, 0 = none, 1 = right)
    /// index 1 = action decision (-1 -> -.51 = Left Arm Action 1, -.5 -> -.01 = Left Arm Action 2, 0 = none, .01 -> .5 = Right Arm Action 1, .51 -> 1 = Right Arm Action 2
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        // Don't do anything if frozen
        if (frozen) return;

        // Calculate movement vector
        Vector3 movementVector = new Vector3(vectorAction[0], 0f, 0f);

        rigidbody.velocity = movementVector * speed;

        // Perform chosen action
        switch (vectorAction[1])
        {
            case float n when (n >= -1f && n <= -.51f):
                if(LeftArm.GetComponent<ArmController>().state.ToString() == "idle" && action == null)
                {
                    action = StartCoroutine(LeftArm.GetComponent<ArmController>().ActionOne());
                }
                break;
            case float n when (n >= -.5f && n < 0f):
                if (LeftArm.GetComponent<ArmController>().state.ToString() == "idle" && action == null)
                {
                    action = StartCoroutine(LeftArm.GetComponent<ArmController>().ActionTwo());
                }
                break;
            case 0f:
                break;
            case float n when (n > 0f && n <= .5f):
                if (RightArm.GetComponent<ArmController>().state.ToString() == "idle" && action == null)
                {
                    action = StartCoroutine(RightArm.GetComponent<ArmController>().ActionOne());
                }
                break;
            case float n when (n >= .51f && n <= 1f):
                if (RightArm.GetComponent<ArmController>().state.ToString() == "idle" && action == null)
                {
                    action = StartCoroutine(RightArm.GetComponent<ArmController>().ActionTwo());
                }
                break;
        }
    }

    /// <summary>
    /// Collect the observations from the environment and child agents
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        // If frozen, do not collect observations
        if (frozen) return;

        // Observe current health ( 1 observation )
        sensor.AddObservation(healthCurrent);

        // Observe enemy current health ( 1 observation )
        sensor.AddObservation(enemy.GetComponent<RuleBasedSystem>().healthCurrent);

        // Observe left arm suggestion ( 1 observation )
        sensor.AddObservation(LeftAgent.output);

        // Observe right arm suggestion ( 1 observation )
        sensor.AddObservation(RightAgent.output);

        // Observe opponent arm states ( 2 observations )
        sensor.AddObservation((float)enemy.GetComponentsInChildren<ArmController>()[0].state);
        sensor.AddObservation((float)enemy.GetComponentsInChildren<ArmController>()[1].state);

        // 6 Total Observations
    }

    /// <summary>
    /// Get the controllers and agents for the chosen arms
    /// </summary>
    public void GetArms()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag("arm_R"))
            {
                RightArm = child.gameObject;
                RightAgent = RightArm.GetComponent<ArmAgent>();
            }
            if (child.CompareTag("arm_L"))
            {
                LeftArm = child.gameObject;
                LeftAgent = LeftArm.GetComponent<ArmAgent>();
            }
        }

        LeftArm.GetComponent<ArmController>().Initialise();
        RightArm.GetComponent<ArmController>().Initialise();

        state = State.playing;

        frozen = false;

        LeftAgent.IsFrozen(frozen);
        LeftAgent.IsTraining(trainingMode);
        LeftAgent.enemy = enemy;
        RightAgent.IsFrozen(frozen);
        RightAgent.IsTraining(trainingMode);
        RightAgent.enemy = enemy;
    }

    public void TakeDamage(int damage)
    {
        if (LeftArm.GetComponent<ArmController>().state.ToString() == "blocking" || RightArm.GetComponent<ArmController>().state.ToString() == "blocking")
        {
            AddReward(0.25f);
        }
        else if (LeftArm.GetComponent<ArmController>().state.ToString() == "parrying" || RightArm.GetComponent<ArmController>().state.ToString() == "parrying")
        {
            healthCurrent -= damage / 2;
            AddReward(-0.5f);
        }
        else 
        {
            healthCurrent -= damage;
            AddReward(-1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("boundary"))
        {
            AddReward(-0.75f);
        }
    }

    private void Update()
    {
        if (state == State.playing)
        {
            if ((RightArm.GetComponent<ArmController>().state.ToString() == "idle" && LeftArm.GetComponent<ArmController>().state.ToString() == "idle") && action != null)
            {
                StopCoroutine(action);
                action = null;
            }

            if (healthCurrent <= 0)
            {
                AddReward(-1f);
                enemy.GetComponent<RuleBasedSystem>().wins++;
                recorder.Add("RBSWins", enemy.GetComponent<RuleBasedSystem>().wins, StatAggregationMethod.MostRecent);
                EndEpisode();
            }

            if(enemy.GetComponent<RuleBasedSystem>().healthCurrent <= 0)
            {
                AddReward(1f);
                wins++;
                recorder.Add("MLAgentWins", wins, StatAggregationMethod.MostRecent);
                EndEpisode();
            }
            
            GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, (float)(healthCurrent / 100f));
        }
    }
}
