using UnityEngine;

public class damageUp : cardClass, ICardEffect
{
    public void effect()
    {
        player.playerInstance.atk += player.playerInstance.atk * 0.2f;
    }

    public void cardEffect()
    {
        effect();
    }
}
