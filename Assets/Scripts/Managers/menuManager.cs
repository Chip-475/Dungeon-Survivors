using UnityEngine;

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
    [ContextMenu("toggleInv")]
    public void toggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }
}
