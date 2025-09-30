using System;
using Utils;

namespace Rhythm
{
    public static class DataStorage
    {
        public static float MainTrackTimePositionMs { get; set; }
        public static int ActiveBeat { get; set; }

        public static void InitializeStorage()
        {
            MainTrackTimePositionMs = 0;
            ActiveBeat = -1;
            
            Metronome.EnterBeat += SetActiveBeat;
            Metronome.ExitBeat  += ResetActiveBeat;
        }

        private static void SetActiveBeat(int activeBeat)
        {
            ActiveBeat = activeBeat;
        }
        private static void ResetActiveBeat(int activeBeat)
        {
            ActiveBeat = -1;
        }
        
        public static void Cleanup()
        {
            Metronome.EnterBeat -= SetActiveBeat;
            Metronome.ExitBeat  -= SetActiveBeat;
        }
    }
}
