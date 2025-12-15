using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;
    
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;
    private float uiVolume;
    
    private static float FloatToDB(float value)
    {
        return value <= 0.001f ? -80f : Mathf.Log10(value) * 20;
    }

    private static float DBToFloat(float value)
    {
        return value >= -80f ? Mathf.Pow(10f, value / 20f) : 0;
    }
    
    private void OnEnable()
    {
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        audioMixer.GetFloat("SFXVolume", out sfxVolume);
        audioMixer.GetFloat("UIVolume", out uiVolume);
        
        masterVolume = DBToFloat(masterVolume);
        musicVolume = DBToFloat(musicVolume);
        sfxVolume = DBToFloat(sfxVolume);
        uiVolume = DBToFloat(uiVolume);
        
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value =  sfxVolume;
        uiSlider.value = uiVolume;
    }
    
    public void OnMasterChange(float volume)
    {
        audioMixer.SetFloat("MasterVolume", FloatToDB(volume));
    }
    
    public void OnMusicChange(float volume)
    {
        audioMixer.SetFloat("MusicVolume", FloatToDB(volume));
    }

    public void OnSFXChange(float volume)
    {
        audioMixer.SetFloat("SFXVolume", FloatToDB(volume));
    }

    public void OnUIChange(float volume)
    {
        audioMixer.SetFloat("UIVolume", FloatToDB(volume));
    }

    public void ApplyChanges()
    {
        masterVolume = masterSlider.value;
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;
        uiVolume = uiSlider.value;
    }

    public void RevertChanges()
    {
        audioMixer.SetFloat("MasterVolume", FloatToDB(masterVolume));
        audioMixer.SetFloat("MusicVolume", FloatToDB(musicVolume));
        audioMixer.SetFloat("SFXVolume", FloatToDB(sfxVolume));
        audioMixer.SetFloat("UIVolume", FloatToDB(uiVolume));
        
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value =  sfxVolume;
        uiSlider.value = uiVolume;
    }
    
}
