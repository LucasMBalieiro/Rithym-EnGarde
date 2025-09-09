using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

public class BeatController : MonoBehaviour
{
    [Header("Music BPM")]
    [SerializeField] private int bpm;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;
    
    [Header("Note Spawning")]
    [SerializeField] private Transform spawnCrosshair;
    [SerializeField] private Transform targetCrosshair;
    [SerializeField] private float noteTravelTimeInSeconds = 2f;
    
    private Queue<float> beatQueue = new Queue<float>();
    
    private void Start()
    {
        if (intervals.Length > 0)
        {
            float beatLength = intervals[0].GetBeatLength(bpm);
            float songDuration = audioSource.clip.length;
            
            for (float beatTime = beatLength; beatTime < songDuration; beatTime += beatLength)
            {
                beatQueue.Enqueue(beatTime);
            }
        }
    }
    
    private void Update()
    {
        if (beatQueue.Count > 0)
        {
            float nextBeatTime = beatQueue.Peek();
            
            if (audioSource.time >= nextBeatTime - noteTravelTimeInSeconds)
            {
                SpawnNote();
                beatQueue.Dequeue();
            }
        }
        
        foreach (Intervals interval in intervals)
        {
            float sampledTime = (audioSource.timeSamples / (audioSource.clip.frequency * interval.GetBeatLength(bpm)));
            
            interval.CheckForNewInterval(sampledTime);
        }
    }
    
    private void SpawnNote()
    {
        GameObject noteObject = GameManager.Instance.GetNote();
        
        
        Note note = noteObject.GetComponent<Note>();
            
        note.Initialize(spawnCrosshair.position, targetCrosshair.position, noteTravelTimeInSeconds);
    }
    
}
