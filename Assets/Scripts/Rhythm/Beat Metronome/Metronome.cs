using System;
using Rhythm._Referee;
using Rhythm.Storage;
using Rhythm.Utils;
using UnityEngine;
using Utils;

namespace Rhythm.Beat_Metronome
{
    public class Metronome
    {
        // Toggle Metronome
        private bool IsCounting { get; set; }
        
        // BPM info
        private readonly int _bpm;
        private double BeatDurationMs => (60d / _bpm) * 1000d;
        
        // Beat indicator
        private bool _isBeatActive;
    
        // Beat position control
        private double _nextBeatPosition;
        private readonly double _errorMarginInMs;
        private double StartBeatPosition => _nextBeatPosition - _errorMarginInMs;
        private double EndBeatPosition => _nextBeatPosition + _errorMarginInMs;
    
        public Metronome(RhythmParameters parameters)
        {
            _bpm = parameters.bpm;
            _errorMarginInMs = parameters.errorMarginMs;

            IsCounting = false;
            _isBeatActive = false;
            _nextBeatPosition = BeatDurationMs;
        }

        public void OnDisable()
        {
            ToggleIsCounting(false);
            
            _isBeatActive = false;
            BeatManager.CallBeatExit();
        }

        public void ToggleIsCounting(bool toggle)
        {
            IsCounting = toggle;
        }
    
        public void Update()
        {
            if (!IsCounting)
                return;
        
            var musicTime = RhythmDataStorage.MainTrackRealTime;
            
            if (!_isBeatActive && musicTime > StartBeatPosition)
            {
                _isBeatActive = true;
                BeatManager.CallBeatEnter();
                
                // Debug.Log($"Beat! at {musicTime}ms.\nBeat window opened at {StartBeatPosition}\nBeat window closes in {EndBeatPosition - musicTime}ms");
            }
            else if (_isBeatActive && musicTime > EndBeatPosition)
            {
                _isBeatActive = false;
                BeatManager.CallBeatExit();
            
                _nextBeatPosition += BeatDurationMs;

                // Debug.Log($"End Beat! at {musicTime}ms.\nNext beat at {_nextBeatPosition}ms");
            }
        }
    }
}
