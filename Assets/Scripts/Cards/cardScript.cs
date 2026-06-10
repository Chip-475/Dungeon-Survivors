using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class cardScript : MonoBehaviour
{
    private cardManager manager;
    private cardManager.CardEntry entry;

    public Image image;
    public List<Sprite> sprites = new List<Sprite>();

    public void setup(cardManager manager, cardManager.CardEntry entry)
    {
        this.manager = manager;
        this.entry = entry;

        if(!entry.levelable)
        {
            image.sprite = sprites[5];
        }
        else
        {
            switch(entry.effect.lvl)
            {
                case 0:
                    image.sprite = sprites[0];
                    break;
                case 1:
                    image.sprite = sprites[1];
                    break;
                case 2:
                    image.sprite = sprites[2];
                    break;
                case 3:
                    image.sprite = sprites[3];
                    break;
                case 4:
                    image.sprite = sprites[4];
                    break;
            }
        }
    }

    public void onClick()
    {
        if (manager != null && entry != null)
        {
            manager.pickCard(entry);
        }
    }
}