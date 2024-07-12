using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Sun")]
    public class Sun : DirectionalLight
    {
        protected override void Update()
        {
            base.Update();
            Shader.SetGlobalVector(ShaderParams._SunDirection, -transform.forward);
        }
    }
}
