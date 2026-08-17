using UnityEngine;

public class resumeButton : MonoBehaviour
{
    public gameManager manager;

    public void onClick()
    {
        manager.togglePause();
    }
}

