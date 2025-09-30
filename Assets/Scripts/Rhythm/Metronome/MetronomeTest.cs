using System;
using NaughtyAttributes;
using Rhythm;
using UnityEngine;

public class MetronomeTest : MonoBehaviour
{
    [SerializeField] private Metronome target;

    private void Awake()
    {
        DataStorage.InitializeStorage();
        
        target.ToggleIsCounting(false);

        Metronome.EnterBeat += RegisterEnterBeat;
        Metronome.ExitBeat += RegisterExitBeat;
    }

    public void RegisterEnterBeat(int beat)
    {
        Debug.Log($"Enter Beat: {beat}");
    }
    public void RegisterExitBeat(int beat)
    {
        Debug.Log($"Exit beat: {beat}");
    }
    
    [Button("Initialize Metronome")]
    public void Initialize()
    {
        target.InitializeMetronome();
    }
    
    [Button("Start Metronome")]
    public void StartMetronome()
    {
        target.ToggleIsCounting(true);
    }
}
