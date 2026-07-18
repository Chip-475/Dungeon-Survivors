using UnityEngine;

public class Swarm : cardClass, ICardEffect
{
    public static bool isActive;

    public void CardEffect()
    {
        isActive = true;
    }
}
