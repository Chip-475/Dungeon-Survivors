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

    [Header("corners")]
    public Transform topRight;
    public Transform topLeft;
    public Transform bottomRight;
    public Transform bottomLeft;
    private void Start()
    {
        waves = 0;
    }
    private void Update()
    {
        if (enemyCount <= 0 && !isSpawning)
        {
            waves++;
            if (!tenacityEffect.instance.tenacity) StartCoroutine(player.playerInstance.hpBar.hpBarMovement(player.playerInstance.hp, player.playerInstance.hp + player.playerInstance.hpMax * 0.2f));
            Invoke(nameof(newWave), 2.5f);
            isSpawning = true;
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
        float minX= bottomLeft.position.x;
        float maxX= bottomRight.position.x;
        float minY= bottomLeft.position.y;
        float maxY= topLeft.position.y;
        //Debug.Log("minX: " + minX + " maxX: " + maxX + " minY: " + minY + " maxY: " + maxY);
        int side = UnityEngine.Random.Range(0, 4);
        switch (side)
        {
            case 0:
                x=UnityEngine.Random.Range(minX,maxX);
                y = maxY+Offset;
                break;
            case 1:
                x = UnityEngine.Random.Range(minX,maxX);
                y = minY-Offset;
                break;
            case 2:
                x = minY-Offset;
                y=UnityEngine.Random.Range(minX, maxX);
                break;
            case 3:
                x = maxX + Offset;
                y = UnityEngine.Random.Range(minX, maxX);
                break;
        }
        x = UnityEngine.Random.Range(minX, maxX);
        y = UnityEngine.Random.Range(minY, maxY);
        Vector3 spawnPos=new Vector3(x,y,0f);
        Vector2 spawnPos2D = new Vector2(x, y);
        if (Vector2.Distance(player.playerInstance.transform.position,spawnPos2D)>30f && Vector2.Distance(player.playerInstance.transform.position, spawnPos2D) < 15f)
        { 
            return getPosition();
        }
        return spawnPos;
    }
}
