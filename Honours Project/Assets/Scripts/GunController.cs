using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : ArmController
{
    // Is the gun loaded?
    private bool loaded;

    // the gun's material
    private Material gunMaterial;

    // The colour that shows shooting
    private Color shootColor = Color.red;

    // The colour that show shooting while not loaded
    private Color badShotColor = Color.black;

    // The colour that shows reloading
    private Color reloadColor = new Color(.75f, .5f, 0f);

    // The enemy
    private GameObject enemy;

    /// <summary>
    /// Initialise the sword arm
    /// </summary>
    public override void Initialise()
    {
        // Get the enemy
        if (gameObject.GetComponentInParent<PlayerAgent>() != null)
        {
            enemy = gameObject.GetComponentInParent<PlayerAgent>().enemy;
        } 
        else if (gameObject.GetComponentInParent<RuleBasedSystem>() != null)
        {
            enemy = gameObject.GetComponentInParent<RuleBasedSystem>().enemy;
        }


        // Find the sword's material
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        gunMaterial = meshRenderer.material;

        // Get the arm's agent
        agent = GetComponent<ArmAgent>();

        state = State.idle;
    }

    /// <summary>
    /// Swing the sword
    /// </summary>
    public override IEnumerator ActionOne()
    {
        if(state == State.idle)
        {
            if (loaded)
            {
                state = State.shooting;
                Color original = gunMaterial.color;
                gunMaterial.color = shootColor;
                if (enemy.CompareTag("enemy"))
                {
                    enemy.GetComponent<RuleBasedSystem>().TakeDamage(20);
                    gameObject.GetComponentInParent<PlayerAgent>().AddReward(0.5f);
                    agent.AddReward(0.5f);
                }
                else
                {
                    enemy.GetComponent<PlayerAgent>().TakeDamage(20);
                }
                yield return new WaitForSeconds(0.5f);
                gunMaterial.color = original;
                loaded = false;
                state = State.idle;
            }
            else
            {
                Color original = gunMaterial.color;
                gunMaterial.color = badShotColor;
                yield return new WaitForSeconds(0.5f);
                gunMaterial.color = original;
            }
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
        
    }

    /// <summary>
    /// Reload the gun
    /// </summary>
    public override IEnumerator ActionTwo()
    {
        if(state == State.idle)
        {
            state = State.reloading;
            Color original = gunMaterial.color;
            gunMaterial.color = reloadColor;
            yield return new WaitForSeconds(2f);
            gunMaterial.color = original;
            state = State.idle;
            loaded = true;
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
    }

    public bool IsLoaded()
    {
        return loaded;
    }
}
