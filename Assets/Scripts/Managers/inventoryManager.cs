using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static cardManager;

public class inventoryManager : MonoBehaviour
{
    public List<CardEntry> invCards;
    public GameObject content;
    private void OnEnable()
    {
        for(int i = 0; i< content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i));
        }
        List<CardEntry> invCards = cardManager.instance.pickedCards;
        foreach (var card in invCards)
        {
            card.prefab.GetComponent<Button>().interactable = false;
        }
        foreach(var card in invCards.Distinct())
        {
            int lvl=invCards.Count(c=>c.prefab==card.prefab);
            Debug.Log(lvl);
            switch (lvl)
            {
                case 1:
                    card.effect.lvl = 1;
                    break;
                case 2:
                    card.effect.lvl = 2;
                    break;
                case 3:
                    card.effect.lvl = 3;
                    break;
                case 4:
                    card.effect.lvl = 4;
                    break;
                case 5:
                    card.effect.lvl = 5;
                    break;
            }
            Instantiate(card.prefab,content.transform);
        }
    }
}
