using UnityEngine;

public class fireAspectCard : cardClass, ICardEffect
{
    public Sprite FireScythe;
    public GameObject scythe;
    private void effect()
    {
        Data.fireAspectLvl++;
        print("fireAspect picked");
            if (Data.fireAspectLvl == 1)
            {
                scythe.GetComponent<SpriteRenderer>().sprite = FireScythe;
        }
    }
    public void CardEffect()
    {
        effect();
    }
}
