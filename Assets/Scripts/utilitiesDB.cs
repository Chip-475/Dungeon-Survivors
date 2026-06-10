using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using static cardManager;

public class utilitiesDB : MonoBehaviour
{
    // Rotates towards a position
    public static Quaternion LookAt2D(Vector2 forward)
    { 
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    // Calculates percentage
    public static float percentage(float percentageOf, float number)
    {
        return (percentageOf/100) * number;
    }

    // Loads scene toLoad
    public string toLoad;
    public async void loadFunc()
    {
        await SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync(toLoad, LoadSceneMode.Single);
    }

    public static List<CardEntry> DeepClone(List<CardEntry> original)
    {
        return original.Select(card => new CardEntry
        {
            prefab = card.prefab,
            effect = card.effect,
            levelable = card.levelable
        }).ToList();
    }
}
