using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // State machine for all possible arm actions
    public enum State { idle, attacking, parrying, blocking, bashing, reloading, shooting}
    public State state;

    protected ArmAgent agent;

    /// <summary>
    /// Initialise the arm with animator and animations
    /// </summary>
    public virtual void Initialise()
    {
        Debug.Log("Initialise called but not implemented");
    }

    /// <summary>
    /// Perform the first assigned action
    /// </summary>
    public virtual IEnumerator ActionOne()
    {
        Debug.Log("First action called but no action implemented");

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Perform the second assigned action
    /// </summary>
    public virtual IEnumerator ActionTwo()
    {
        Debug.Log("Second action called but no action implemented");

        yield return new WaitForSeconds(0.5f);
    }
}
