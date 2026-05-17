using UnityEngine;
using TMPro;
public class uiManager : MonoBehaviour
{
    public TMP_Text wave;
    public TMP_Text enemyKilled;
    public TMP_Text enemyRemaining;
    public spawnManager spawnManager;
    public TMP_Text hpText;
    public TMP_Text xpText;
    //public TMP_Text lvlText;
    public player player;   
    void Update()
    {
        wave.text = "wave:" + spawnManager.waves;
        enemyKilled.text = "killed:" + data.killCount;
        enemyRemaining.text = "remaining:" + spawnManager.enemyCount;
        hpText.text = player.hp + "/" + player.hpMax;
        xpText.text = Mathf.RoundToInt(data.xp) + "/" + Mathf.RoundToInt(data.xpMax);
       // lvlText.text = data.level.ToString();
    }
}
