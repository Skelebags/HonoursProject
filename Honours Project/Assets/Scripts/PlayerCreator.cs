using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreator : MonoBehaviour
{
    public PlayerAgent player;
    public GameObject enemy;

    // Arm prefabs with ANNs
    public GameObject swordArm;
    public GameObject shieldArm;
    public GameObject gunArm;

    // Arm prefabs without ANNs
    public GameObject swordArmNG;
    public GameObject shieldArmNG;
    public GameObject gunArmNG;

    // MNN Arms
    private GameObject arm_R;
    private GameObject arm_L;

    // NGram Arms
    private GameObject e_arm_R;
    private GameObject e_arm_L;

    // Start is called before the first frame update
    void Start()
    {
        MakePlayer();
        MakeEnemy();
    }

    private void MakePlayer()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                arm_L = Instantiate(shieldArm);
                arm_L.transform.SetParent(player.transform);
                arm_L.transform.position = player.transform.position;
                arm_L.transform.rotation = player.transform.rotation;
                arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                arm_L.transform.localScale = new Vector3(arm_L.transform.localScale.x * -1, arm_L.transform.localScale.y, arm_L.transform.localScale.z);
                break;
            case 1:
                arm_L = Instantiate(swordArm);
                arm_L.transform.SetParent(player.transform);
                arm_L.transform.position = player.transform.position;
                arm_L.transform.rotation = player.transform.rotation;
                arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                arm_L.transform.localScale = new Vector3(arm_L.transform.localScale.x * -1, arm_L.transform.localScale.y, arm_L.transform.localScale.z);
                break;
            case 2:
                arm_L = Instantiate(gunArm);
                arm_L.transform.SetParent(player.transform);
                arm_L.transform.position = player.transform.position;
                arm_L.transform.rotation = player.transform.rotation;
                arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                arm_L.transform.localScale = new Vector3(arm_L.transform.localScale.x * -1, arm_L.transform.localScale.y, arm_L.transform.localScale.z);
                break;
            default:
                break;
        }

        arm_L.gameObject.tag = "arm_L";

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                arm_R = Instantiate(shieldArm);
                arm_R.transform.SetParent(player.transform);
                arm_R.transform.position = player.transform.position;
                arm_R.transform.rotation = player.transform.rotation;
                arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                break;
            case 1:
                arm_R = Instantiate(swordArm);
                arm_R.transform.SetParent(player.transform);
                arm_R.transform.position = player.transform.position;
                arm_R.transform.rotation = player.transform.rotation;
                arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                break;
            case 2:
                arm_R = Instantiate(gunArm);
                arm_R.transform.SetParent(player.transform);
                arm_R.transform.position = player.transform.position;
                arm_R.transform.rotation = player.transform.rotation;
                arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                break;
            default:
                break;
        }

        arm_R.gameObject.tag = "arm_R";

        player.GetArms();
    }

    private void MakeEnemy()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                e_arm_L = Instantiate(shieldArmNG);
                e_arm_L.transform.SetParent(enemy.transform);
                e_arm_L.transform.position = enemy.transform.position;
                e_arm_L.transform.rotation = enemy.transform.rotation;
                e_arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                e_arm_L.transform.localScale = new Vector3(e_arm_L.transform.localScale.x * -1, e_arm_L.transform.localScale.y, e_arm_L.transform.localScale.z);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(25);
                break;
            case 1:
                e_arm_L = Instantiate(swordArmNG);
                e_arm_L.transform.SetParent(enemy.transform);
                e_arm_L.transform.position = enemy.transform.position;
                e_arm_L.transform.rotation = enemy.transform.rotation;
                e_arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                e_arm_L.transform.localScale = new Vector3(e_arm_L.transform.localScale.x * -1, e_arm_L.transform.localScale.y, e_arm_L.transform.localScale.z);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(40);
                break;
            case 2:
                e_arm_L = Instantiate(gunArmNG);
                e_arm_L.transform.SetParent(enemy.transform);
                e_arm_L.transform.position = enemy.transform.position;
                e_arm_L.transform.rotation = enemy.transform.rotation;
                e_arm_L.transform.localPosition = new Vector3(-.5f, 0f, 0f);
                e_arm_L.transform.localScale = new Vector3(e_arm_L.transform.localScale.x * -1, e_arm_L.transform.localScale.y, e_arm_L.transform.localScale.z);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(10);
                break;
            default:
                break;
        }

        e_arm_L.gameObject.tag = "arm_L";

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                e_arm_R = Instantiate(shieldArmNG);
                e_arm_R.transform.SetParent(enemy.transform);
                e_arm_R.transform.position = enemy.transform.position;
                e_arm_R.transform.rotation = enemy.transform.rotation;
                e_arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(25);
                break;
            case 1:
                e_arm_R = Instantiate(swordArmNG);
                e_arm_R.transform.SetParent(enemy.transform);
                e_arm_R.transform.position = enemy.transform.position;
                e_arm_R.transform.rotation = enemy.transform.rotation;
                e_arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(40);
                break;
            case 2:
                e_arm_R = Instantiate(gunArmNG);
                e_arm_R.transform.SetParent(enemy.transform);
                e_arm_R.transform.position = enemy.transform.position;
                e_arm_R.transform.rotation = enemy.transform.rotation;
                e_arm_R.transform.localPosition = new Vector3(.5f, 0f, 0f);
                enemy.GetComponent<RuleBasedSystem>().SetAggression(10);
                break;
            default:
                break;
        }

        e_arm_R.gameObject.tag = "arm_R";

        enemy.GetComponent<RuleBasedSystem>().GetArms();
    }
}
