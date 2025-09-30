using NaughtyAttributes;
using Rhythm;
using UnityEngine;

public class MusicPlayerTest : MonoBehaviour
{
    [SerializeField] private MusicPlayer target;
    
    [Space(20), Header("Test parameter(s)")] [SerializeField] private SoundData testSound;

    [Button("Add Track")]
    public void AddTrack()
    {
        target.AddTrack(testSound);
    }
    [Button("Remove Track")]
    public void RemoveTrack()
    {
        target.RemoveTrack(testSound.sound_id);   
    }
    
    [Button("Play Track")]
    public void PlayTrack()
    {
        target.PlayTrack(testSound.sound_id);
    }
    [Button("Pause Track")]
    public void PauseTrack()
    {
        target.PauseTrack(testSound.sound_id);   
    }
}
