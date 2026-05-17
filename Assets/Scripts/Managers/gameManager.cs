using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public enum gameState
    {
        running,
        paused,
    }
    public LayerMask obstacle;
    public LayerMask enemy;
    public gameState state = gameState.running;
    public GameObject pauseMenu;

    private void Start()
    {
        instance = this;

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
}
