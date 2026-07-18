using UnityEngine;

public class rangeUp : cardClass, ICardEffect
{
    public float baseRange;

    private new void Start()
    {
        baseRange = Player.instance.range;
    }
    public void effect()
    {
        Player.instance.range += baseRange * 0.2f;
    }
    public void CardEffect()
    {
        effect();
    }
}
