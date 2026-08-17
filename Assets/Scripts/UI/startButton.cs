using UnityEngine;
using UnityEngine.SceneManagement;

public class startButton : MonoBehaviour
{
    public async void onClick()
    {
        Data.reset();
        Time.timeScale = 1.0f;
        await SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync("gameScene", LoadSceneMode.Single);
    }
}
