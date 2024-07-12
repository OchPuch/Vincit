using OccaSoftware.SuperSimpleSkybox.Runtime;

using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Demo
{
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/Sunrise and Sunset Callbacks Demo")]
    public class OnSunriseSunsetDemo : MonoBehaviour
    {
        private Sun sun;

        private void OnEnable()
        {
            sun = FindObjectOfType<Sun>();
            if (sun != null)
            {
                sun.OnRise += Sunrise;
                sun.OnSet += Sunset;
            }
        }

        private void OnDisable()
        {
            if (sun != null)
            {
                sun.OnRise -= Sunrise;
                sun.OnSet -= Sunset;
            }
        }

        private void Sunrise()
        {
            Debug.Log("Sunrise Event Triggered");
        }

        private void Sunset()
        {
            Debug.Log("Sunset Event Triggered");
        }
    }
}
