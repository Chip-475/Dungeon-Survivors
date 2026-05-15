using System.Collections.Generic;
using UnityEngine;

public class cardManager : MonoBehaviour
{
    [System.Serializable]
    public class CardEntry
    {
        public GameObject prefab;
        public cardClass effect;
        public int level = 1;
    }

    public static cardManager instance;
    public List<CardEntry> cards = new List<CardEntry>();
    public Transform cardParent;
    public int choices = 3;
    public GameObject cardLabel;

    void Awake()
    {
        instance = this;
    }

    public void spawnCards()
    {
        if (cardParent == null)
        {
            Debug.LogError("cardManager: cardParent is not assigned.");
            return;
        }

        List<CardEntry> availableCards = getAvailableCards();
        int cardsToSpawn = Mathf.Min(choices, availableCards.Count);
        Debug.Log($"cardManager: {availableCards.Count} available cards out of {cards.Count} total.");

        if (cardsToSpawn == 0)
        {
            Debug.LogWarning("cardManager: no valid cards to spawn.");
            return;
        }

        for(int i = 0; i < cardsToSpawn; i++)
        {
            int index = Random.Range(0, availableCards.Count);
            CardEntry entry = availableCards[index];
            GameObject spawnedCard = Instantiate(entry.prefab, cardParent);
            spawnedCard.name = $"{entry.prefab.name} lvl {entry.level}";

            cardChoice choice = spawnedCard.GetComponent<cardChoice>();
            if (choice != null)
            {
                choice.setup(this, entry);
            }
            else
            {
                spawnedCard.AddComponent<cardChoice>().setup(this, entry);
            }
            availableCards.RemoveAt(index);
        }
    }

    public void pickCard(CardEntry entry)
    {
        if (!canSpawn(entry)) return;

        entry.effect.lvl = entry.level;
        clearSpawnedCards();
        cardLabel.SetActive(false);
    }

    private List<CardEntry> getAvailableCards()
    {
        List<CardEntry> availableCards = new List<CardEntry>();

        foreach (CardEntry entry in cards)
        {
            if (canSpawn(entry))
            {
                availableCards.Add(entry);
            }
        }

        return availableCards;
    }

    private bool canSpawn(CardEntry entry)
    {
        if (entry == null)
        {
            Debug.LogWarning("cardManager: skipped null card entry.");
            return false;
        }
        if (entry.prefab == null)
        {
            Debug.LogWarning("cardManager: skipped card entry with missing prefab.");
            return false;
        }
        if (entry.effect == null)
        {
            Debug.LogWarning($"cardManager: skipped {entry.prefab.name} lvl {entry.level} because effect is missing.");
            return false;
        }

        int maxLevel = entry.effect.lvlMax > 0 ? entry.effect.lvlMax : 5;
        if (entry.effect.lvl >= maxLevel)
        {
            Debug.Log($"cardManager: skipped {entry.prefab.name} lvl {entry.level}; effect is already maxed at {entry.effect.lvl}/{maxLevel}.");
            return false;
        }
        if (entry.level > maxLevel)
        {
            Debug.Log($"cardManager: skipped {entry.prefab.name} lvl {entry.level}; card level is above max {maxLevel}.");
            return false;
        }

        bool isNextLevel = entry.level == entry.effect.lvl + 1;
        if (!isNextLevel)
        {
            Debug.Log($"cardManager: skipped {entry.prefab.name} lvl {entry.level}; effect level is {entry.effect.lvl}, next valid level is {entry.effect.lvl + 1}.");
        }

        return isNextLevel;
    }

    private void clearSpawnedCards()
    {

        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
    }
}
