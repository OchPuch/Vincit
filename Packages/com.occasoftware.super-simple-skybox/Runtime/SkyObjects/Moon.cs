using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Moon")]
    public class Moon : DirectionalLight
    {
        protected override void Update()
        {
            base.Update();
            Shader.SetGlobalVector(ShaderParams._MoonDirection, -transform.forward);
        }
    }
}
