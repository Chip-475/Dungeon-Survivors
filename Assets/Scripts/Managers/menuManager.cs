using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuManager : MonoBehaviour
{
    public GameObject pause;
    public GameObject options;
    public GameObject inventory;
    public void onMenuClick()
    {
        pause.SetActive(true);
        options.SetActive(false);
    }
    public void onOptionsClick()
    {
        pause.SetActive(false);
        options.SetActive(true);
    }
    public void toggleInventory(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }
        inventory.SetActive(!inventory.activeSelf);
    }
}
