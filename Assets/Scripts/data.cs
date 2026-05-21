using System.Collections.Generic;
using UnityEngine;

public class data : MonoBehaviour
{
    // Misc
    public static bool isPaused;
    public static float master;
    public static float music;
    public static float sfx;

    // Player
    public static int level;
    public static float xp;
    public static Queue<float> xpQueue = new Queue<float>();
    public static float xpMax = 100;

    // Cards
    public static bool electroAura;
    public static bool fireArea;
    public static bool iceAura;
    public static int fireAspectLvl;
    public static int fireAreaLvl;
    public static bool moveSpeed;
    public static bool orbitingBlades;
    public static bool rangeIncrease;

    // Counters
    public static int killCount=0;
    public static int waveEnemy = 0;
}
