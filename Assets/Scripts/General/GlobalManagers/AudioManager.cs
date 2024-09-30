using FMODUnity;
using Music;
using UnityEngine;
using Zenject;

namespace General.GlobalManagers
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Mixer Groups")]
        [SerializeField] private FmodAnimatedGlobalParameter master;
        [SerializeField] private FmodAnimatedGlobalParameter sfx;
        [SerializeField] private FmodAnimatedGlobalParameter music;
        
        [Header("Effects")]
        [SerializeField] private FmodAnimatedGlobalParameter slowMoConfig;
        [SerializeField] private StudioEventEmitter pauseSnapshotEmitter;

        
        [Inject]
        private void Construct(ITimeNotifier timeNotifier, IPauseNotifier pauseNotifier)
        {
            timeNotifier.TimeContinued += slowMoConfig.PlayOutro;
            timeNotifier.TimeStopped += slowMoConfig.PlayIntro;

            pauseNotifier.Paused += pauseSnapshotEmitter.Play;
            pauseNotifier.Resumed += pauseSnapshotEmitter.Stop;
        }

        private void Start()
        {
            slowMoConfig.SetDefault();
            sfx.SetParameter(GetSoundEffectsVolume01());
            music.SetParameter(GetMusicVolume01());
            master.SetParameter(GetMasterVolume01());
        }
      
        public void UpdateMasterVolume(float value)
        {
            master.SetParameter(value);
            PlayerPrefs.SetFloat(master.Parameter, value);
        }
        
        public void UpdateSfxVolume(float value)
        { 
            sfx.SetParameter(value);
            PlayerPrefs.SetFloat(sfx.Parameter, value);
        }

        public void UpdateMusicVolume(float value)
        { 
            music.SetParameter(value);
            PlayerPrefs.SetFloat(music.Parameter, value);
        }

        public float GetMasterVolume01()
        {
            return PlayerPrefs.GetFloat(master.Parameter, 0.5f);
        }
        
        public float GetSoundEffectsVolume01()
        {
            return PlayerPrefs.GetFloat(sfx.Parameter, 0.5f);
        }
        
        public float GetMusicVolume01()
        {
            return PlayerPrefs.GetFloat(music.Parameter, 0.5f);
        }
    }
}
