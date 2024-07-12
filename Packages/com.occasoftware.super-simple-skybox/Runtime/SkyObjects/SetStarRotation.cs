using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    /// <summary>
    /// This component uses the local to world matrix of the transform to set the rotation of the stars.
    /// Typically, you would set this component on the main directional light in your scene.
    /// </summary>

    [ExecuteAlways]
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Set Star Rotation")]
    public class SetStarRotation : MonoBehaviour
    {
        void Update()
        {
            SetMatrix();
        }

        /// <summary>
        /// Sets the main light matrix for the star rotation in the shader.
        /// </summary>
        private void SetMatrix()
        {
            Shader.SetGlobalMatrix(ShaderParams._StarMatrix, transform.localToWorldMatrix);
        }
    }
}
