using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    [ExecuteAlways]
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Moon (Static)")]
    public class MoonStatic : MonoBehaviour
    {
        private void Update()
        {
            Shader.SetGlobalVector(ShaderParams._MoonDirection, -transform.forward);
        }
    }
}
