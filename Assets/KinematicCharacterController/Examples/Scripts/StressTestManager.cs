using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace KinematicCharacterController.Examples
{
    public class StressTestManager : MonoBehaviour
    {
        public Camera Camera;
        public LayerMask UIMask;

        public InputField CountField;
        public Image RenderOn;
        public Image SimOn;
        public Image InterpOn;
        public ExampleCharacterController CharacterPrefab;
        public ExampleAIController AIController;
        public int SpawnCount = 100;
        public float SpawnDistance = 2f;

        private KinematicCharacterSystem _kcc;
        
        [Inject]
        private void Construct(KinematicCharacterSystem kinematicCharacterSystem)
        {
            _kcc = kinematicCharacterSystem;
        }

        private void Start()
        {
            CountField.text = SpawnCount.ToString();
            UpdateOnImages();

            _kcc.settings.AutoSimulation = false;
            _kcc.settings.Interpolate = false;
        }

        private void Update()
        {
            KinematicCharacterSystem.Simulate(Time.deltaTime, _kcc.CharacterMotors, _kcc.PhysicsMovers);
        }

        private void UpdateOnImages()
        {
            RenderOn.enabled = Camera.cullingMask == -1;
            SimOn.enabled = Physics.autoSimulation;
            InterpOn.enabled = _kcc.settings.Interpolate;
        }

        public void SetSpawnCount(string count)
        {
            if (int.TryParse(count, out int result))
            {
                SpawnCount = result;
            }
        }

        public void ToggleRendering()
        {
            if(Camera.cullingMask == -1)
            {
                Camera.cullingMask = UIMask;
            }
            else
            {
                Camera.cullingMask = -1;
            }
            UpdateOnImages();
        }

        public void TogglePhysicsSim()
        {
            Physics.autoSimulation = !Physics.autoSimulation;
            UpdateOnImages();
        }

        public void ToggleInterpolation()
        {
            _kcc.settings.Interpolate = !_kcc.settings.Interpolate;
            UpdateOnImages();
        }

        public void Spawn()
        {
            for (int i = 0; i < AIController.Characters.Count; i++)
            {
                Destroy(AIController.Characters[i].gameObject);
            }
            AIController.Characters.Clear();

            int charsPerRow = Mathf.CeilToInt(Mathf.Sqrt(SpawnCount));
            Vector3 firstPos = ((charsPerRow * SpawnDistance) * 0.5f) * -Vector3.one;
            firstPos.y = 0f;

            for (int i = 0; i < SpawnCount; i++)
            {
                int row = i / charsPerRow;
                int col = i % charsPerRow;
                Vector3 pos = firstPos + (Vector3.right * row * SpawnDistance) + (Vector3.forward * col * SpawnDistance);

                ExampleCharacterController newChar = Instantiate(CharacterPrefab);
                newChar.Motor.SetPosition(pos);

                AIController.Characters.Add(newChar);
            }
        }
    }
}