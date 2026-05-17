using UnityEngine;

public class healthUp : cardClass, ICardEffect
{
    public void effect()
    {
        player.playerInstance.hpMax += player.playerInstance.hpMax * 0.2f;
        player.playerInstance.hp = player.playerInstance.hpMax;
    }
    public void cardEffect()
    {
        effect();
    }
}
