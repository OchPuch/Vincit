using FMODUnity;
using General;
using UnityEngine;


namespace Guns.Projectiles.View
{
    public class CloseRangeProjectileAudio : GamePlayBehaviour
    {
        [SerializeField] private StudioEventEmitter _swingEmitter;
        [SerializeField] private StudioEventEmitter _punchEmitter;

        public void PlaySwing()
        {
            if (_swingEmitter) _swingEmitter.Play();
        }

        public void PlayPunch()
        {
            if (_punchEmitter) _punchEmitter.Play();
        }

    }
}