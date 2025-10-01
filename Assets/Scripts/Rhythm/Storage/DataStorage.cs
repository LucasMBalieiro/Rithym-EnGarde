using Rhythm.Beat_Metronome;
using Rhythm.Utils;
using UnityEngine;

namespace Rhythm.Storage
{
    public static class DataStorage
    {
        public static RhythmParameters Parameters { get; private set; }
        
        public static double MainTrackStartTime { get; set; }
        public static double MainTrackTimePositionMs { get; set; }
        public static double MainTrackRealTime => MainTrackTimePositionMs - MainTrackStartTime;
        
        public static bool ActiveBeat { get; private set; }

        public static void InitializeStorage(RhythmParameters parameters)
        {
            Parameters = parameters;

            MainTrackStartTime = 0;
            MainTrackTimePositionMs = 0;
            ActiveBeat = false;
            
            Metronome.EnterBeat += SetActiveBeat;
            Metronome.ExitBeat  += ResetActiveBeat;
        }

        private static void SetActiveBeat() => ActiveBeat = true;
        private static void ResetActiveBeat() => ActiveBeat = false;
        public static void Cleanup()
        {
            Metronome.EnterBeat -= SetActiveBeat;
            Metronome.ExitBeat  -= SetActiveBeat;
        }
    }
}
