using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public GameObject[] enemyList;
    public GameObject boss;
    public int[] enemyCost;
    public int waves = 30;
    public int spawnLimit;
    public static int enemyCount;
    public bool isSpawning = false;
    public float Offset = 0.1f;
    public GameObject victoryScreen;

    [Header("corners")]
    public Transform topRight;
    public Transform topLeft;
    public Transform bottomRight;
    public Transform bottomLeft;
    private void Start()
    {
        waves = 0;
        victoryScreen.SetActive(false);
    }
    private void Update()
    {
        if (enemyCount <= 0 && !isSpawning)
        {
            waves++;
            //recupero del 25% della vita 
            if(!tenacityEffect.instance.tenacity)
            {
                float newHp=Mathf.Clamp(player.playerInstance.hp+player.playerInstance.hpMax*0.25f,0,player.playerInstance.hpMax);
                StartCoroutine(player.playerInstance.hpBar.hpBarMovement(player.playerInstance.hp, newHp));
            }
            Invoke(nameof(newWave), 2.5f);
            isSpawning = true;
        }
    }

    [ContextMenu("Run Function")]
    public void newWave()
    {
        if (waves == 31)
        {
            waves=30;
            victoryScreen.SetActive(true);
            Time.timeScale = 0f;
            return;
        }
        if (waves % 10 == 0)   // modifica per il boss per bug fix //2
        {
            enemyCount = 0;
            for (int i = 0; i < waves / 10; i++)//2
            {
                Instantiate(boss, getPosition(1.2f), Quaternion.identity);
                enemyCount++;
            }
            isSpawning = false;
            return;
        }
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
    public Vector3 getPosition(float radius=0.5f)
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
        if(Vector2.Distance(player.playerInstance.transform.position,spawnPos2D)>30f||Vector2.Distance(player.playerInstance.transform.position, spawnPos2D)<15f)
        { 
            return getPosition(radius);
        }
        Collider2D hitObstacle = Physics2D.OverlapCircle(spawnPos2D, 0.5f, gameManager.instance.obstacle);
        if (hitObstacle!=null)
        {
            Debug.Log("Ci ha provato "+hitObstacle.gameObject.name);
            return getPosition(radius); //overlapCircle cerca se ce un collider se si sovrappne a un cerchio immaginario proprio come prova
        }
        return spawnPos;
    }
}