using System;
using Rhythm.Music_Player;
using Rhythm.Beat_Metronome;
using Rhythm.Rhythm_Judge;
using Rhythm.Storage;
using Rhythm.Utils;
using UnityEngine;
using Utils;

namespace Rhythm._Referee
{
    public class BeatManager : SingletonObj<BeatManager>
    {
        [SerializeField] private ParametersScriptable parameters;
        
        private MusicPlayer _musicPlayer;
        private Metronome _metronome;
        private RhythmJudge _judge;

        private void Start()
        {
            DataStorage.InitializeStorage(parameters.parameters);
            
            _musicPlayer = new MusicPlayer(parameters.parameters, this.transform);
            _metronome = new Metronome(parameters.parameters);
            _judge = new RhythmJudge(parameters.parameters);
            
            _musicPlayer.AddTrack(parameters.parameters.soundsToPlay[0]);
            _metronome.ToggleIsCounting(true);
        }

        private void Update()
        {
            _musicPlayer.Update();
            _metronome.Update();
            
            // Probably should put this in a timer instead
            _judge.ResetAction();
        }

        private void OnDisable()
        {
            _judge.OnDisable();
            _metronome.OnDisable();
            _musicPlayer.OnDisable();
        }
    }
}
