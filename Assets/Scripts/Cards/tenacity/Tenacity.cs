using UnityEngine;

public class Tenacity : cardClass, ICardEffect
{
    public static bool isActive;

    public void CardEffect()
    {
        isActive = true;
    }
}
