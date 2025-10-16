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

        public static event Action BeatEnter, BeatExit;
        public static event Action<bool> AttackOnBeat;
        
        private void OnEnable()
        {
            var rhythmParam = parameters.parameters;
            
            DataStorage.InitializeStorage(rhythmParam);
            
            _musicPlayer = new MusicPlayer(rhythmParam, this.transform);
            _metronome = new Metronome(rhythmParam);
            _judge = new RhythmJudge(rhythmParam);
        }
        private void OnDisable()
        {
            _judge.OnDisable();
            _metronome.OnDisable();
            _musicPlayer.OnDisable();
            DataStorage.Cleanup();
        }
        
        private void Start()
        {
            this.OnEnable();

            // Rude initialization, change later
            var baseTrack = DataStorage.Parameters.soundsToPlay[0];
            _musicPlayer.AddTrack(baseTrack);
            _metronome.ToggleIsCounting(true);
        }

        public static void CallBeatEnter() => BeatEnter?.Invoke();
        public static void CallBeatExit() => BeatExit?.Invoke();
        public static void CallAttack(bool onBeat) => AttackOnBeat?.Invoke(onBeat);
        
        private void Update()
        {
            _musicPlayer.Update();
            _metronome.Update();
        }
    }
}
