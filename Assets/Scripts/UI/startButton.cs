using UnityEngine;
using UnityEngine.SceneManagement;

public class startButton : MonoBehaviour
{
    public async void onClick()
    {
        await SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync("gameScene", LoadSceneMode.Single);
    }
}
