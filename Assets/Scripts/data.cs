using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    // Misc
    public static bool isPaused;
    public static float master=1f;
    public static float music=1f;
    public static float sfx=1f;

    // Player
    public static int level;
    public static float xp;
    public static Queue<float> xpQueue = new Queue<float>();
    public static float xpMax = 100;

    // Cards
    public static bool electroAura;
    public static bool iceAura;
    public static int fireAspectLvl;
    public static int fireAreaLvl;
    public static bool moveSpeed;
    public static bool orbitingBlades;
    public static bool rangeIncrease;

    // Counters
    public static int killCount=0;
    public static int waveEnemy = 0;

    public static void reset()
    {
        isPaused = false;
        level = 0;
        xp = 0;
        xpMax = 100;
        xpQueue.Clear();
        killCount = 0;
        waveEnemy = 0;
        electroAura = false;
        iceAura = false;
        fireAreaLvl = 0;
        fireAspectLvl = 0;
        moveSpeed = false;
        orbitingBlades = false;
        rangeIncrease = false;
        master = 1f;
        music = 1f;
        sfx = 1f;
    }
}
