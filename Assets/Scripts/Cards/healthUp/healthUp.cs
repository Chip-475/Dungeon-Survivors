using UnityEngine;

public class healthUp : cardClass, ICardEffect
{
    public void effect()
    {
        player.playerInstance.hpMax += player.playerInstance.hpMax * 0.2f;
        StartCoroutine(player.playerInstance.hpBar.hpBarMovement(player.playerInstance.hp, player.playerInstance.hp + player.playerInstance.hp * 0.2f));
    }
    public void cardEffect()
    {
        effect();
    }
}
