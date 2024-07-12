using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    internal static class ShaderParams
    {
        public static int _StarMatrix = Shader.PropertyToID("_MainLightMatrix");
        public static int _SunDirection = Shader.PropertyToID("_SunDirection");
        public static int _MoonDirection = Shader.PropertyToID("_MoonDirection");
    }
}
