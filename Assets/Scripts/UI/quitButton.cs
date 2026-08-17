using UnityEngine;

public class quitButton : MonoBehaviour
{
    public void onClick()
    {
        //Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }
}
