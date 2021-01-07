using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SwordAgent : ArmAgent
{
    // Enum to store the suggested action
    public enum Action { attack, parry, idle}
    public Action action;


    /// <summary>
    /// Collect relevant observatoins
    /// </summary>
    /// <param name="sensor">The observations</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        // If frozen, do not collect observations
        if (frozen) return;
       
        // Observe enemy's current health (1 observation)
        sensor.AddObservation(enemy.GetComponent<RuleBasedSystem>().healthCurrent);

        // Observe player's current health (1 observation
        sensor.AddObservation(transform.parent.GetComponent<PlayerAgent>().healthCurrent);

        // 2 Total Observations
    }

    /// <summary>
    /// vectorAction[i] represents:
    /// [0]: -1 = suggest action 1, 0 = suggest no action, +1 = suggest action 2
    /// </summary>
    /// <param name="vectorAction">The output action array</param>
    public override void OnActionReceived(float[] vectorAction)
    {
        switch(vectorAction[0])
        {
            case -1f:
                output = (float)Action.attack;
                break;
            case 0f:
                output = (float)Action.idle;
                break;
            case 1f:
                output = (float)Action.parry;
                break;
            default:
                break;
        }
    }

    public override void OnEpisodeBegin()
    {
        // Set action state to idle
        action = Action.idle;
    }

}
