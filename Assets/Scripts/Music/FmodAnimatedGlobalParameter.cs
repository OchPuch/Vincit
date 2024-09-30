using FMODUnity;
using PrimeTween;
using UnityEngine;

namespace Music
{
    [CreateAssetMenu(menuName = "VINCIT/Music/AnimatedGlobalParameter", fileName = "Animated GP")]
    public class FmodAnimatedGlobalParameter : ScriptableObject
    {
        [field: ParamRef]
        [field: SerializeField]
        public string Parameter { get; private set; }

        [field: SerializeField] public float DefaultValue { get; private set; }
        [SerializeField] private TweenSettings<float> introTweenSettings;
        [SerializeField] private TweenSettings<float> outroTweenSettings;

        public void SetDefault()
        {
            RuntimeManager.StudioSystem.setParameterByName(Parameter, DefaultValue);
        }
        
        public void SetParameter(float value)
        {
            RuntimeManager.StudioSystem.setParameterByName(Parameter, value);
        }

        public void PlayIntro() => Tween.Custom(introTweenSettings, onValueChange: SetParameter);
        public void PlayOutro() => Tween.Custom(outroTweenSettings, onValueChange: SetParameter);

        
    }
}