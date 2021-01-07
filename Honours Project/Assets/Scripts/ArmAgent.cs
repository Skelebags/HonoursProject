using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ArmAgent : Agent
{
    // Is this agent frozen
    protected bool frozen = false;

    // Is this agent training
    protected bool trainingMode = false;

    // The enemy
    public GameObject enemy;

    // The suggestion from the network
    public float output = 0f;


    /// <summary>
    /// Freeze or unfreeze the agent
    /// </summary>
    /// <param name="freeze">True to freeze, False to unfreeze</param>
    public void IsFrozen(bool freeze)
    {
        frozen = freeze;
    }

    /// <summary>
    /// Turn on or off training modes
    /// </summary>
    /// <param name="mode">True to turn on training mode, False to turn off</param>
    public void IsTraining(bool mode)
    {
        trainingMode = mode;
    }
}
