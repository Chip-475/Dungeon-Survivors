using UnityEngine;

public class rangeUp : cardClass, ICardEffect
{
    public float baseRange;

    private new void Start()
    {
        baseRange = player.playerInstance.range;
    }
    public void effect()
    {
        player.playerInstance.range -= baseRange * 0.2f;
    }
    public void cardEffect()
    {
        effect();
    }
}
