using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public GameObject[] enemyList;
    public int[] enemyCost;
    public int waves = 0;
    public int spawnLimit;
    public static int enemyCount;
    public bool isSpawning = false;
    public float Offset = 0.1f;
    private void Start()
    {
        waves = 0;
    }
    private void Update()
    {
        if (enemyCount <= 0 && !isSpawning)
        {
            if(!tenacityEffect.instance.tenacity) player.playerInstance.hp += player.playerInstance.hpMax * 0.2f;
            Invoke(nameof(newWave), 2.5f);
            isSpawning = true;
            waves++;
        }
    }
    
    [ContextMenu("Run Function")]
    public void newWave()
    {
        spawnLimit = waves * 10;
        if (swarmEffect.swarm)
        {
            spawnLimit *= 2;
        }
        int waveCost = 0;
        int index = 0;
        enemyCount = 0;
        while (waveCost < spawnLimit)
        {
            index = UnityEngine.Random.Range(0, enemyList.Length);
            if (waveCost + enemyCost[index] <= spawnLimit)
            {
                Instantiate(enemyList[index], getPosition(), Quaternion.identity);
                enemyCount++;
                waveCost += enemyCost[index];
            }

        }
        isSpawning = false;
    }
    public Vector3 getPosition()
    {
        float x = 0f;
        float y = 0f;
        int side = UnityEngine.Random.Range(0, 4);
        switch (side)
        {
            case 0:
                x=UnityEngine.Random.Range(0f, 1f);
                y = -Offset;
                break;
            case 1:
                x = UnityEngine.Random.Range(0f, 1f);
                y = -Offset;
                break;
            case 2:
                x = -Offset;
                y=UnityEngine.Random.Range(0f, 1f);
                break;
            case 3:
                x = 1f + Offset;
                y = UnityEngine.Random.Range(0f, 1f);
                break;
        }
        Vector3 spawnPos=Camera.main.ViewportToWorldPoint(new Vector3(x,y,10f));
        return spawnPos;
    }
}
