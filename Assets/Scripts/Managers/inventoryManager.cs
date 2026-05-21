using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static cardManager;
using Unity.VisualScripting;

public class inventoryManager : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<CardEntry> invCards = new List<CardEntry>();
    public GameObject content;
    public void OnEnable()
    {

        Time.timeScale = 0;
        invCards = utilitiesDB.DeepClone(cardManager.instance.pickedCards);
        List<CardEntry> instantiableCards = new List<CardEntry>();
        for (int i = 0; i< content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
            if(content.transform.GetChild(i) != null)
            {
                print("not destroyed");
            }
        }
        
        foreach (var card in cardManager.instance.cards)
        {
            card.prefab.GetComponent<Button>().interactable = false;

            ColorBlock colors=card.prefab.GetComponent<Button>().colors;
            colors.disabledColor=Color.white;
            card.prefab.GetComponent<Button>().colors=colors;
        }

        foreach(var card in invCards.Distinct())
        {
            if(card.levelable)
            {
                int lvl = invCards.Count(c => c.prefab == card.prefab);
                Debug.Log(lvl);
                switch (lvl)
                {
                    case 1:
                        card.prefab.GetComponent<Image>().sprite = sprites[0];
                        break;
                    case 2:
                        card.prefab.GetComponent<Image>().sprite = sprites[1];
                        break;
                    case 3:
                        card.prefab.GetComponent<Image>().sprite = sprites[2];
                        break;
                    case 4:
                        card.prefab.GetComponent<Image>().sprite = sprites[3];
                        break;
                    case 5:
                        card.prefab.GetComponent<Image>().sprite = sprites[4];
                        break;
                }
            }
            else
            {
                card.prefab.GetComponent<Image>().sprite = sprites[5];
            }
            instantiableCards.Add(card);
        }
        foreach (var card in instantiableCards.DistinctBy(c=>c.prefab))
        {
            Debug.Log(card.prefab.name);
            Instantiate(card.prefab, content.transform);
        }
    }

    private void OnDisable()
    {
        foreach (var card in cardManager.instance.cards)
        {
            card.prefab.GetComponent<Button>().interactable = true;
        }
        Time.timeScale = 1;
    }
}
