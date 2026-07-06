using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject options;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    void Start()
    {
        SyncSliders();
    }
    void OnEnable()
    {
        SyncSliders();
    }
    public void onOptionsClick()
    {
        options.SetActive(true);
    }
    private void SyncSliders()
    {
        masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("master", 1f));
        sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("sfx", 1f));
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("music", 1f));
    }
    public void exitClick()
    {
        options.SetActive(false);
    }

    public void onMasterChanged(float volume)
    {
        PlayerPrefs.SetFloat("master", volume);
    }

    public void onSFXChanged(float value)
    {
        PlayerPrefs.SetFloat("sfx", value);
    }

    public void onMusicChanged(float value)
    {
        PlayerPrefs.SetFloat("music", value);
    }
}
