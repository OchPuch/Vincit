using FMODUnity;
using General;
using RayFire;
using UnityEngine;

namespace Environment
{
    public class RayFireAudio : GamePlayBehaviour
    {
        [SerializeField] private StudioEventEmitter _crushEmitter;
        [SerializeField] private RayfireRigid _rayfireRigid;

        private void Awake()
        {
            _rayfireRigid.Demolished += OnDemolish;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _rayfireRigid.Demolished -= OnDemolish;
        }

        private void OnDemolish()
        {
            if (_crushEmitter) _crushEmitter.Play();
        }
    }
}