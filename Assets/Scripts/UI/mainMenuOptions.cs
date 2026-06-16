using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
       public GameObject options;

    public void onOptionsClick()
    {
        options.SetActive(true);
    }
}
