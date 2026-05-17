using System.Collections.Generic;
using UnityEngine;

public class cardManager : MonoBehaviour
{
    public static cardManager instance;

    [System.Serializable]
    public class CardEntry
    {
        public GameObject prefab;
        public cardClass effect;
        public bool levelable;
    }

    public List<CardEntry> cards = new List<CardEntry>();
    [Space]
    private List<CardEntry> spawnableCards = new List<CardEntry>();
    public List<Transform> spawnPoints = new List<Transform>();
    [Space]
    private List<GameObject> spawnedCards = new List<GameObject>();
    public List<CardEntry> pickedCards = new List<CardEntry>();

    public GameObject cardPanel;

    void Awake()
    {
        instance = this;
        spawnableCards = utilitiesDB.DeepClone(cards);
    }

    [ContextMenu("Run spawnCards")]
    public void spawnCards()
    {
        if(spawnableCards.Count == 0) { Debug.LogWarning("out of cards"); return; }

        Time.timeScale = 0;
        cardPanel.SetActive(true);
        int cardsToSpawn = Mathf.Min(3, spawnableCards.Count,spawnPoints.Count);

        List<int> index = new List<int>();
        for (int i = 0; i < cardsToSpawn; i++)
        {
            int x = Random.Range(0, spawnableCards.Count);
            if (index.Contains(x)) { i--; continue; }
            index.Add(x);
            print($"index {x}");

            CardEntry entry = spawnableCards[x];
            if (entry.effect.lvl==5)
            {
                spawnableCards.Remove(entry);
                if (spawnableCards.Count == 0) return;
                i--;
                continue;
               
            }

            spawnedCards.Add(Instantiate(entry.prefab, spawnPoints[i].transform.position, Quaternion.identity, cardPanel.transform));
            print("card spawned");

            spawnedCards[i].TryGetComponent(out cardScript choice);
            if (choice != null)
            {
                choice.setup(instance, entry);
            }
            else
            {
                print($"card {spawnedCards[i]} doesnt contain cardChoice");
                i--;
                continue;
            }
        }
    }

    public void pickCard(CardEntry entry)
    {
        if (!canSpawn(entry)) return;

        pickedCards.Add(entry);
        entry.effect.GetComponent<ICardEffect>().cardEffect();
        if (!entry.levelable)
        {
            spawnableCards.Remove(entry);
        }
        else entry.effect.lvl++;
        clearSpawnedCards();
        cardPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private bool canSpawn(CardEntry entry)
    {
        if (entry.levelable && entry.effect.lvl == 5)
        {
            Debug.Log($"cardManager: skipped {entry.prefab.name} lvl {entry.effect.lvl}; effect is already maxed at {entry.effect.lvl}/{5}.");
            return false;
        }

        return true;
    }

    private void clearSpawnedCards()
    {
        foreach (GameObject x in spawnedCards)
        {
            Destroy(x);
        }
        spawnedCards.Clear();
    }
}