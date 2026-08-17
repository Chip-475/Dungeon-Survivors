using UnityEngine;

public class tenacityEffect : cardClass, ICardEffect
{
    public static tenacityEffect instance;
    public bool tenacity;
    private new void Start()
    {
        instance = this;
    }

    public void effect()
    {
        tenacity = true;
    }
    public void cardEffect()
    {
        effect();
    }
}
