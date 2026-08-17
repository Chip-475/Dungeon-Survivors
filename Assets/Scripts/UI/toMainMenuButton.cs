using UnityEngine;
using UnityEngine.SceneManagement;

public class toMainMenuButton : MonoBehaviour
{
    public async void onClick()
    {
        await SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync("mainMenu", LoadSceneMode.Single);
    }
}
