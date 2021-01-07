using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : ArmController
{
    [Tooltip("The force applied on shield bash")]
    public float bashForce = 2.0f;
    
    // the shield's material
    private Material shieldMaterial;

    // The colour that shows blocking
    private Color blockColor = Color.cyan;

    // The colour that shows bashing
    private Color bashColor = new Color(.5f, 0f, .75f);

    /// <summary>
    /// Initialise the sword arm
    /// </summary>
    public override void Initialise()
    {
        // Find the sword's material
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        shieldMaterial = meshRenderer.material;

        state = State.idle;

        // Get the arm's agent
        agent = GetComponent<ArmAgent>();
    }

    /// <summary>
    /// Swing the sword
    /// </summary>
    public override IEnumerator ActionOne()
    {
        if(state == State.idle)
        {
            state = State.blocking;
            Color original = shieldMaterial.color;
            shieldMaterial.color = blockColor;
            yield return new WaitForSeconds(0.5f);
            shieldMaterial.color = original;
            state = State.idle;
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Perform a brief parry action
    /// </summary>
    public override IEnumerator ActionTwo()
    {
        if (state == State.idle)
        {
            state = State.bashing;
            Color original = shieldMaterial.color;
            shieldMaterial.color = bashColor;
            yield return new WaitForSeconds(0.5f);
            shieldMaterial.color = original;
            state = State.idle;
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnterOrStay(Collider collision)
    {
        if (state == State.bashing)
        {
            if (collision.gameObject.CompareTag("player") || collision.gameObject.transform.parent.CompareTag("player"))
            {
                collision.gameObject.GetComponent<PlayerAgent>().TakeDamage(5);
            }
            else if (collision.gameObject.CompareTag("enemy") || collision.gameObject.transform.parent.CompareTag("enemy"))
            {
                collision.gameObject.GetComponent<RuleBasedSystem>().TakeDamage(5);
                agent.AddReward(0.25f);
                gameObject.GetComponentInParent<PlayerAgent>().AddReward(0.25f);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        OnTriggerEnterOrStay(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        OnTriggerEnterOrStay(collision);
    }
}
