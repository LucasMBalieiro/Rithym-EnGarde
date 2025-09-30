using System;
using Rhythm;
using UnityEngine;
using Utils;

public class Metronome : SingletonObj<Metronome>
{
    [SerializeField] private bool isCounting;
    
    [SerializeField] private int _bpm;
    private float BeatDurationMs => (60 / _bpm) * 1000;
    
    private float _nextBeatPosition;
    private int _activeBeat, _lastBeat;
    [SerializeField] private float errorMarginInMs;
    private float StartBeatPosition => _nextBeatPosition - errorMarginInMs;
    private float EndBeatPosition => _nextBeatPosition + errorMarginInMs;
    public static event Action<int> EnterBeat, ExitBeat;
    
    public void InitializeMetronome()
    {
        _lastBeat = 0;
        _activeBeat = -1;
        _nextBeatPosition = BeatDurationMs;
    }

    public void ToggleIsCounting(bool toggle)
    {
        isCounting = toggle;
    }
    
    private void Update()
    {
        if (!isCounting)
            return;
        
        var musicTime = DataStorage.MainTrackTimePositionMs;
        
        if (_activeBeat == -1 && musicTime > StartBeatPosition)
        {
            _activeBeat = ++_lastBeat;
            EnterBeat?.Invoke(_activeBeat);
        }
        else if (_activeBeat != -1 && musicTime > EndBeatPosition)
        {
            var temp = _activeBeat;
            _activeBeat = -1;
            _nextBeatPosition += BeatDurationMs;
            
            ExitBeat?.Invoke(temp);
        }
    }
}
