using UnityEngine;

public class fireAspectCard : cardClass, ICardEffect
{
    public Sprite FireScythe;
    public GameObject scythe;
    private void effect()
    {
        data.fireAspectLvl++;
        print("fireAspect picked");
            if (data.fireAspectLvl == 1)
            {
                scythe.GetComponent<SpriteRenderer>().sprite = FireScythe;
        }
    }
    public void cardEffect()
    {
        effect();
    }
}
