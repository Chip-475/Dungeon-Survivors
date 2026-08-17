using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioManager : MonoBehaviour
{
    public static audioManager manager;
    public AudioSource source;
    public AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
    }

    public void playSFX(AudioClip clip, Transform spawn, float volume)
    {
        AudioSource audioSource = Instantiate(source, spawn.position, Quaternion.identity); ;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void setMaster(float volume)
    {
        Data.master = volume;
        PlayerPrefs.SetFloat("master",volume);
        ApplyMixerVolume("Master", volume);
    }
    public void setSFX(float volume)
    {
        Data.sfx = volume;
        PlayerPrefs.SetFloat("sfx", volume);
        ApplyMixerVolume("SFX", volume);
    }
    public void setBGM(float volume)
    {
        Data.music = volume;
        PlayerPrefs.SetFloat("music", volume);
        ApplyMixerVolume("BGM", volume);
    }
    public void Start()
    {
        Data.master = PlayerPrefs.GetFloat("master",1f);
        Data.sfx = PlayerPrefs.GetFloat("sfx",1f);
        Data.music = PlayerPrefs.GetFloat("music", 1f);
        EnsureSliderReferences();
        SyncSlidersWithSavedValues();
        ApplyMixerVolume("Master", Data.master);
        ApplyMixerVolume("SFX", Data.sfx);
        ApplyMixerVolume("BGM", Data.music);
    }

    private void EnsureSliderReferences()
    {
        if (masterSlider == null)
        {
            GameObject masterObject = GameObject.Find("master");
            if (masterObject != null)
            {
                masterSlider = masterObject.GetComponent<Slider>();
            }
        }

        if (sfxSlider == null)
        {
            GameObject sfxObject = GameObject.Find("sfx");
            if (sfxObject != null)
            {
                sfxSlider = sfxObject.GetComponent<Slider>();
            }
        }

        if (musicSlider == null)
        {
            GameObject musicObject = GameObject.Find("music");
            if (musicObject != null)
            {
                musicSlider = musicObject.GetComponent<Slider>();
            }
        }
    }

    private void SyncSlidersWithSavedValues()
    {
        if (masterSlider != null)
        {
            masterSlider.SetValueWithoutNotify(Data.master);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(Data.sfx);
        }

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(Data.music);
        }
    }

    private void ApplyMixerVolume(string parameter, float volume)
    {
        if (volume <= 0f)
        {
            mixer.SetFloat(parameter, -80f);
            return;
        }

        mixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    }

    public void playSFXAtPosition(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource audioSource = Instantiate(source, position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}