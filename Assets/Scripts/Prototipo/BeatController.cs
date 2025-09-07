using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BeatController : MonoBehaviour
{
    [SerializeField] private int bpm;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;

    private void Update()
    {
        foreach (Intervals interval in intervals)
        {
            float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetBeatLength(bpm)));
            
            interval.CheckForNewInterval(sampledTime);
        }
    }
    
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent onBeat;
    
    private int _lastInterval;

    public float GetBeatLength(int bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if(Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            onBeat.Invoke();
        }
    }
}
