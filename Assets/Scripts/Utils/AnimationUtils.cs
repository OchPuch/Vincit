using UnityEngine;

namespace Utils
{
    public static class AnimationUtils
    {
        public static int CurrentAnimationClipFrame(float time, float clipLength, float clipFrameRate) =>
            (int)((int)(time * (clipLength * clipFrameRate)) % (clipLength * clipFrameRate));

        public static int CurrentAnimationClipFrame(AnimatorClipInfo clipInfo, float time) =>
            (CurrentAnimationClipFrame(time, clipInfo.clip.length, clipInfo.clip.frameRate));
    }
}