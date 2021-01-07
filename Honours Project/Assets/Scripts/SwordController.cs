using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : ArmController
{
    // the sword's material
    private Material swordMaterial;

    // The colour that shows attacking
    private Color attackColor = Color.red;

    // The colour that shows parrying
    private Color parryColor = Color.green;

    /// <summary>
    /// Initialise the sword arm
    /// </summary>
    public override void Initialise()
    {
        // Find the sword's material
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        swordMaterial = meshRenderer.material;

        state = State.idle;

        // Get the arm's agent
        agent = GetComponent<ArmAgent>();
    }

    /// <summary>
    /// Swing the sword
    /// </summary>
    public override IEnumerator ActionOne()
    {
        if (state == State.idle)
        {
            state = State.attacking;
            Color original = swordMaterial.color;
            swordMaterial.color = attackColor;
            yield return new WaitForSeconds(0.5f);
            swordMaterial.color = original;
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
            state = State.parrying;
            Color original = swordMaterial.color;
            swordMaterial.color = parryColor;
            yield return new WaitForSeconds(0.5f);
            swordMaterial.color = original;
            state = State.idle;
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnterOrStay(Collider collision)
    {
        if (state == State.attacking)
        {
            if (collision.gameObject.CompareTag("player") || collision.gameObject.transform.parent.CompareTag("player"))
            {
                collision.gameObject.GetComponent<PlayerAgent>().TakeDamage(10);
            }
            else if (collision.gameObject.CompareTag("enemy") || collision.gameObject.transform.parent.CompareTag("enemy"))
            {
                collision.gameObject.GetComponent<RuleBasedSystem>().TakeDamage(10);
                agent.AddReward(0.3f);
                gameObject.GetComponentInParent<PlayerAgent>().AddReward(0.3f);
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
