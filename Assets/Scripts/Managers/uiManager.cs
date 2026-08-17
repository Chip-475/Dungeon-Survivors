using UnityEngine;
using TMPro;
public class uiManager : MonoBehaviour
{
    public TMP_Text wave;
    public TMP_Text enemyKilled;
    public TMP_Text enemyRemaining;
    public SpawnManager spawnManager;
    public TMP_Text hpText;
    public TMP_Text xpText;
    public TMP_Text lvlText;


    public Player player;
    void Update()
    {
        wave.text = "wave: " + spawnManager.waves;
        enemyKilled.text = "killed: " + Data.killCount;
        enemyRemaining.text = "remaining: " + SpawnManager.enemyCount;
        if (player.hp < 1f && player.hp > 0f) hpText.text = "0.1 / " + Mathf.RoundToInt(player.hpMax);
        else hpText.text = Mathf.RoundToInt(player.hp) + " / " + Mathf.RoundToInt(player.hpMax);
        xpText.text = Mathf.RoundToInt(Data.xp) + " / " +Mathf.RoundToInt(Data.xpMax);
        lvlText.text = Data.level.ToString();
    }
}
