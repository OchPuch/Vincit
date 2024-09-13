using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

namespace KinematicCharacterController.Core
{
    /// <summary>
    /// The system that manages the simulation of KinematicCharacterMotor and PhysicsMover
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class KinematicCharacterSystem : GamePlayBehaviour, IKccMotorRegister, IKccPhysicsMoverRegister
    {
        public readonly List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>();
        public readonly List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>();

        private float _lastCustomInterpolationStartTime = -1f;
        private float _lastCustomInterpolationDeltaTime = -1f;

        public KCCSettings settings;
        
        /// <summary>
        /// Sets the maximum capacity of the character motors list, to prevent allocations when adding characters
        /// </summary>
        /// <param name="capacity"></param>
        public void SetCharacterMotorsCapacity(int capacity)
        {
            if (capacity < CharacterMotors.Count)
            {
                capacity = CharacterMotors.Count;
            }
            CharacterMotors.Capacity = capacity;
        }

        /// <summary>
        /// Registers a KinematicCharacterMotor into the system
        /// </summary>
        public void RegisterCharacterMotor(KinematicCharacterMotor motor)
        {
            CharacterMotors.Add(motor);
        }

        /// <summary>
        /// Unregisters a KinematicCharacterMotor from the system
        /// </summary>
        public void UnregisterCharacterMotor(KinematicCharacterMotor motor)
        {
            CharacterMotors.Remove(motor);
        }

        /// <summary>
        /// Sets the maximum capacity of the physics movers list, to prevent allocations when adding movers
        /// </summary>
        /// <param name="capacity"></param>
        public void SetPhysicsMoversCapacity(int capacity)
        {
            if (capacity < PhysicsMovers.Count)
            {
                capacity = PhysicsMovers.Count;
            }
            PhysicsMovers.Capacity = capacity;
        }

        /// <summary>
        /// Registers a PhysicsMover into the system
        /// </summary>
        public void RegisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Add(mover);

            mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        /// <summary>
        /// Unregisters a PhysicsMover from the system
        /// </summary>
        public void UnregisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Remove(mover);
        }
        

        public void FixedUpdate()
        {
            if (settings.AutoSimulation)
            {
                float deltaTime = Time.deltaTime;

                if (settings.Interpolate)
                {
                    PreSimulationInterpolationUpdate(deltaTime);
                }

                Simulate(deltaTime, CharacterMotors, PhysicsMovers);

                if (settings.Interpolate)
                {
                    PostSimulationInterpolationUpdate(deltaTime);
                }
            }
        }

        public void LateUpdate()
        {
            if (settings.Interpolate)
            {
                CustomInterpolationUpdate();
            }
        }

        /// <summary>
        /// Remembers the point to interpolate from for KinematicCharacterMotors and PhysicsMovers
        /// </summary>
        public  void PreSimulationInterpolationUpdate(float deltaTime)
        {
            // Save pre-simulation poses and place transform at transient pose
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.InitialTickPosition = motor.TransientPosition;
                motor.InitialTickRotation = motor.TransientRotation;

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                mover.InitialTickPosition = mover.TransientPosition;
                mover.InitialTickRotation = mover.TransientRotation;

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover.Rigidbody.position = mover.TransientPosition;
                mover.Rigidbody.rotation = mover.TransientRotation;
            }
        }

        /// <summary>
        /// Ticks characters and/or movers
        /// </summary>
        public static void Simulate(float deltaTime, List<KinematicCharacterMotor> motors, List<PhysicsMover> movers)
        {
            int characterMotorsCount = motors.Count;
            int physicsMoversCount = movers.Count;

#pragma warning disable 0162
            // Update PhysicsMover velocities
            for (int i = 0; i < physicsMoversCount; i++)
            {
                movers[i].VelocityUpdate(deltaTime);
            }

            // Character controller update phase 1
            for (int i = 0; i < characterMotorsCount; i++)
            {
                motors[i].UpdatePhase1(deltaTime);
            }

            // Simulate PhysicsMover displacement
            for (int i = 0; i < physicsMoversCount; i++)
            {
                PhysicsMover mover = movers[i];

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover.Rigidbody.position = mover.TransientPosition;
                mover.Rigidbody.rotation = mover.TransientRotation;
            }

            // Character controller update phase 2 and move
            for (int i = 0; i < characterMotorsCount; i++)
            {
                KinematicCharacterMotor motor = motors[i];

                motor.UpdatePhase2(deltaTime);

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }
#pragma warning restore 0162
        }

        /// <summary>
        /// Initiates the interpolation for KinematicCharacterMotors and PhysicsMovers
        /// </summary>
        public  void PostSimulationInterpolationUpdate(float deltaTime)
        {
            _lastCustomInterpolationStartTime = Time.time;
            _lastCustomInterpolationDeltaTime = deltaTime;

            // Return interpolated roots to their initial poses
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.Transform.SetPositionAndRotation(motor.InitialTickPosition, motor.InitialTickRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                if (mover.MoveWithPhysics)
                {
                    mover.Rigidbody.position = mover.InitialTickPosition;
                    mover.Rigidbody.rotation = mover.InitialTickRotation;

                    mover.Rigidbody.MovePosition(mover.TransientPosition);
                    mover.Rigidbody.MoveRotation(mover.TransientRotation);
                }
                else
                {
                    mover.Rigidbody.position = (mover.TransientPosition);
                    mover.Rigidbody.rotation = (mover.TransientRotation);
                }
            }
        }

        /// <summary>
        /// Handles per-frame interpolation
        /// </summary>
        private  void CustomInterpolationUpdate()
        {
            float interpolationFactor = Mathf.Clamp01((Time.time - _lastCustomInterpolationStartTime) / _lastCustomInterpolationDeltaTime);

            // Handle characters interpolation
            for (int i = 0; i < CharacterMotors.Count; i++)
            {
                KinematicCharacterMotor motor = CharacterMotors[i];

                motor.Transform.SetPositionAndRotation(
                    Vector3.Lerp(motor.InitialTickPosition, motor.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(motor.InitialTickRotation, motor.TransientRotation, interpolationFactor));
            }

            // Handle PhysicsMovers interpolation
            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];
                
                mover.Transform.SetPositionAndRotation(
                    Vector3.Lerp(mover.InitialTickPosition, mover.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(mover.InitialTickRotation, mover.TransientRotation, interpolationFactor));

                Vector3 newPos = mover.Transform.position;
                Quaternion newRot = mover.Transform.rotation;
                mover.PositionDeltaFromInterpolation = newPos - mover.LatestInterpolationPosition;
                mover.RotationDeltaFromInterpolation = Quaternion.Inverse(mover.LatestInterpolationRotation) * newRot;
                mover.LatestInterpolationPosition = newPos;
                mover.LatestInterpolationRotation = newRot;
            }
        }
    }
}