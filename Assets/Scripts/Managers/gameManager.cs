using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public enum gameState
    {
        running,
        paused,
        deathScreen
    }
    public LayerMask obstacle;
    public LayerMask enemy;
    public gameState state = gameState.running;
    public GameObject pauseMenu;
    public GameObject deathScreen;

    private void Start()
    {
        instance = this;
        Time.timeScale = 1.0f;
        obstacle = LayerMask.GetMask("Obstacle");
        enemy = LayerMask.GetMask("Enemy");
    }

    public void togglePause()
    {
        if (state == gameState.paused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            state = gameState.running;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            state = gameState.paused;
        }
    }

    public IEnumerator death()
    {
        AnimationCurve timeScaleCurve = AnimationCurve.EaseInOut(0, 1, 2, 0);

        var time = 0f;
        while(time < 2)
        {
            Time.timeScale = timeScaleCurve.Evaluate(time);

            time += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0;

        deathScreen.SetActive(true);
    }

    public void startDeath()
    {
        StartCoroutine(death());
        state = gameState.deathScreen;
    }
}
