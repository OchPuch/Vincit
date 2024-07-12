using System;

using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Runtime
{
    /// <summary>
    /// This component automatically rotates the sun and sets the light intensity from the angle.
    /// </summary>

    [ExecuteAlways]
    [RequireComponent(typeof(Light))]
    // We hide this component in the inspector Add Component Menu
    // It doesn't do anything on its own and is intended only as a base class
    // It is extended by Sun.cs, Moon.cs as base functions for automatic light handling.
    [AddComponentMenu("OccaSoftware/Super Simple Skybox/")]
    public class DirectionalLight : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        private float rotationsPerHour = 1f;

        /// <summary>
        /// The rate of rotation of the directional light. Set as number of rotations per hour.
        /// </summary>
        public float RotationsPerHour
        {
            get => rotationsPerHour;
            set => rotationsPerHour = value;
        }

        [SerializeField]
        private bool automaticLightIntensity = true;

        /// <summary>
        /// When enabled, the light intensity will be set automatically based on the maximum intensity and alignment with the zenith.
        /// </summary>
        public bool AutomaticLightIntensity
        {
            get => automaticLightIntensity;
            set => automaticLightIntensity = value;
        }

        [SerializeField]
        [Min(0)]
        private float maximumLightIntensity = 2f;

        /// <summary>
        /// The light intensity when at the zenith.
        /// </summary>
        public float MaximumLightIntensity
        {
            get => maximumLightIntensity;
            set => maximumLightIntensity = value;
        }

        private Light _light = null;

        public event Action OnRise = null;
        public event Action OnSet = null;

        /// <summary>
        /// Returns the current state of the light (up or down).
        /// </summary>
        public LightState State
        {
            get => state;
        }
        private LightState state;

        private float lightAngle;

        private float GetLightAngle()
        {
            return Vector3.Dot(Vector3.down, transform.forward);
        }

        protected virtual void OnEnable()
        {
            _light = GetComponent<Light>();
            lightAngle = GetLightAngle();
            SetInitialLightState();
        }

        protected virtual void Update()
        {
            Rotate();
            lightAngle = GetLightAngle();
            SetLightState();
            SetLightIntensity();
        }

        private void SetInitialLightState()
        {
            state = LightState.Up;

            if (lightAngle < 0f)
                state = LightState.Down;
        }

        /// <summary>
        /// Updates the light state if needed.
        /// Triggers the appropriate callback.
        /// </summary>
        private void SetLightState()
        {
            if (lightAngle > 0f && state == LightState.Down)
            {
                state = LightState.Up;
                OnRise?.Invoke();
            }

            if (lightAngle < 0f && state == LightState.Up)
            {
                state = LightState.Down;
                OnSet?.Invoke();
            }
        }

        /// <summary>
        /// Rotates the transform based on the rotation speed.
        /// </summary>
        private void Rotate()
        {
            if (!Application.isPlaying)
                return;

            if (rotationsPerHour == 0f)
                return;

            transform.Rotate(
                transform.right * rotationsPerHour * Time.deltaTime * 0.1f,
                Space.World
            );
        }

        /// <summary>
        /// Sets the light intensity based on the alignment between the light direction and the sky peak.
        /// </summary>
        private void SetLightIntensity()
        {
            if (!automaticLightIntensity)
                return;

            if (state == LightState.Down)
            {
                _light.intensity = 0;
                return;
            }

            float t = Mathf.Clamp01(lightAngle);
            t = 1f - Mathf.Pow(1f - t, 2f);
            _light.intensity = t * maximumLightIntensity;
            _light.shadowStrength = t;
        }

        /// <summary>
        /// The state of the light object.
        /// </summary>
        public enum LightState
        {
            Down,
            Up
        }
    }
}
