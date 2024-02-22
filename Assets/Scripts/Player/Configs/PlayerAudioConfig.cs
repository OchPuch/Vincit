using UnityEngine;

namespace Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerAudioConfig", menuName = "Player/PlayerAudioConfig")]
    public class PlayerAudioConfig : ScriptableObject
    {
        public AudioClip jumpSound;
        public AudioClip landSound;
        public AudioClip[] wallJumpSound;
        public AudioClip dashSound;
        public AudioClip slamSound;
        public AudioClip startSlideSound;
        public AudioClip slideSound;
        public AudioClip[] footstepSounds;
    }
}