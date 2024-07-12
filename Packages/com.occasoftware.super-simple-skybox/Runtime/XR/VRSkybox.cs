using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    /// <summary>
    /// This component synchronizes the position of the VR Skybox with the targeted camera component.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/VR-Compatible Skybox")]
    public class VRSkybox : MonoBehaviour
    {
        [SerializeField]
        Camera target;

        void Start()
        {
            if (target == null)
                return;
            transform.position = target.transform.position;
        }

        int farClipPlaneCached;

        void LateUpdate()
        {
            if (target == null)
                return;
            transform.position = target.transform.position;

            if (farClipPlaneCached != (int)target.farClipPlane)
            {
                float targetScale = (int)target.farClipPlane - 1;
                farClipPlaneCached = (int)target.farClipPlane;
                transform.localScale = FloatToV3(targetScale);
            }
        }

        Vector3 FloatToV3(float x)
        {
            return new Vector3(x, x, x);
        }
    }
}
