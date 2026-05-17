using UnityEngine;

public class rangeUp : cardClass, ICardEffect
{
    public void effect()
    {
        player.playerInstance.range += player.playerInstance.range * 0.2f;
    }
    public void cardEffect()
    {
        effect();
    }
}
