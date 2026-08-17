using UnityEngine;

public class resumeButton : MonoBehaviour
{
    public GameManager manager;

    public void onClick()
    {
        manager.togglePause();
    }
}

