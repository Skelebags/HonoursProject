using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerAgent player;
    public RuleBasedSystem enemy;

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text playerwinsText;
    public Text enemywinsText;

    private void OnGUI()
    {
        playerHealthText.text = ("MLAgent Health: " + player.healthCurrent);
        enemyHealthText.text = ("RBS Health: " + enemy.healthCurrent);
        playerwinsText.text = ("MLAgent Wins: " + player.wins);
        enemywinsText.text = ("RBS Wins: " + enemy.wins);
    }
}
