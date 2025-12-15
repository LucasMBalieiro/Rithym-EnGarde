using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class SnapshotFilter : MonoBehaviour
    {
        [SerializeField] private AudioMixer mainMixer;
        private const string SnapshotUnpaused = "Unpaused";
        private const string SnapshotPaused = "Paused";

        private AudioMixerSnapshot defaultSnapshot;
        private AudioMixerSnapshot dreamySnapshot;

        private void Start()
        {
            defaultSnapshot = mainMixer.FindSnapshot(SnapshotUnpaused);
            dreamySnapshot = mainMixer.FindSnapshot(SnapshotPaused);
        }

        private void OnEnable()
        {
            SnapshotActions.SetDefaultFilter += SetDefaultFilter;
            SnapshotActions.SetMuffledFilter += SetMuffledFilter;
        }

        private void OnDisable()
        {
            SnapshotActions.SetDefaultFilter -= SetDefaultFilter;
            SnapshotActions.SetMuffledFilter -= SetMuffledFilter;
        }

        private void SetDefaultFilter() => defaultSnapshot.TransitionTo(0.5f);
        private void SetMuffledFilter() => dreamySnapshot.TransitionTo(0.5f);
    }
}
