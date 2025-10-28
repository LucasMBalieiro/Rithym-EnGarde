using System;
using Player;
using Rhythm._Referee;
using Rhythm.Beat_Metronome;
using Rhythm.Storage;
using Rhythm.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = System.Object;

namespace Rhythm.Rhythm_Judge
{
    public class RhythmJudge
    {
        public bool IsOnBeat()
        {
            var onBeat = RhythmDataStorage.ActiveBeat;
            BeatManager.CallInputEvent(onBeat);
            return onBeat;
        }
    }
}
