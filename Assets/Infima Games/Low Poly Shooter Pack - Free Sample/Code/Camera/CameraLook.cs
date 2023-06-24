﻿// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{

    public class CameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Settings")]
        
        [Tooltip("Sensitivity when looking around.")]
        [SerializeField]
        private Vector2 sensitivity = new Vector2(1, 1);

        [Tooltip("Minimum and maximum up/down rotation angle the camera can have.")]
        [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);

        [Tooltip("Should the look rotation be interpolated?")]
        [SerializeField]
        private bool smooth;

        [Tooltip("The speed at which the look rotation is interpolated.")]
        [SerializeField]
        private float interpolationSpeed = 25.0f;
        
        #endregion
        
        #region FIELDS
        
        private CharacterBehaviour playerCharacter;
   
        private Rigidbody playerCharacterRigidbody;

        private Quaternion rotationCharacter;

        private Quaternion rotationCamera;

        #endregion
        
        #region UNITY

        private void Awake()
        {
            //Get Player Character.
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
            //Cache the rigidbody.
            playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
        }
        private void Start()
        {
            rotationCharacter = playerCharacter.transform.localRotation;
            rotationCamera = transform.localRotation;
        }
        private void LateUpdate()
        {
            //Frame Input. The Input to add this frame!
            Vector2 frameInput = playerCharacter.IsCursorLocked() ? playerCharacter.GetInputLook() : default;
            //Sensitivity.
            frameInput *= sensitivity;

            //Yaw.
            Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
            //Pitch.
            Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

            rotationCamera *= rotationPitch;
            rotationCharacter *= rotationYaw;

            Quaternion localRotation = transform.localRotation;

            //Smooth.
            if (smooth)
            {
                //Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                //Interpolate character rotation.
                playerCharacterRigidbody.MoveRotation(Quaternion.Slerp(playerCharacterRigidbody.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed));
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);

                //Rotate character.
                playerCharacterRigidbody.MoveRotation(playerCharacterRigidbody.rotation * rotationYaw);
            }
            
            //Set.
            transform.localRotation = localRotation;
        }

        #endregion

        #region FUNCTIONS

        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        #endregion
    }
}