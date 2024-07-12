using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    [ExecuteAlways]
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Sun (Static)")]
    public class SunStatic : MonoBehaviour
    {
        private void Update()
        {
            Shader.SetGlobalVector(ShaderParams._SunDirection, -transform.forward);
        }
    }
}
