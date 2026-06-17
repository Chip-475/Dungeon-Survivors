using UnityEngine;
using UnityEngine.SceneManagement;

public class toMainMenuButton : MonoBehaviour
{
    public async void onClick()
    {
        data.reset();
        Time.timeScale = 1.0f;
        await SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync("mainMenu", LoadSceneMode.Single);
        data.killCount = 0;
    }
}
