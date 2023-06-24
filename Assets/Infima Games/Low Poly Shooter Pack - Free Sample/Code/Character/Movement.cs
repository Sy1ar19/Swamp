// Copyright 2021, Infima Games. All Rights Reserved.

using System.Linq;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Movement : MovementBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Audio Clips")]
        
        [Tooltip("The audio clip that is played while walking.")]
        [SerializeField]
        private AudioClip audioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        [SerializeField]
        private AudioClip audioClipRunning;

        [Header("Speeds")]

        [SerializeField]
        private float speedWalking = 5.0f;

        [Tooltip("How fast the player moves while running."), SerializeField]
        private float speedRunning = 9.0f;

        private float maxVelocityChange = 10f;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode jumpKey = KeyCode.Space;
        public float jumpPower = 1f;

        #endregion

        #region PROPERTIES

        //Velocity.
        private Vector3 Velocity
        {
            //Getter.
            get => rigidBody.velocity;
            //Setter.
            set => rigidBody.velocity = value;
        }

        #endregion

        #region FIELDS

        private Rigidbody rigidBody;
        private CapsuleCollider capsule;
        private AudioSource audioSource;
        private bool grounded;
        private CharacterBehaviour playerCharacter;
        private WeaponBehaviour equippedWeapon;
        private readonly RaycastHit[] groundHits = new RaycastHit[8];

        #endregion

        #region UNITY FUNCTIONS

        protected override void Awake()
        {
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        }

        protected override  void Start()
        {
            //Rigidbody Setup.
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            //Cache the CapsuleCollider.
            capsule = GetComponent<CapsuleCollider>();

            //Audio Source Setup.
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClipWalking;
            audioSource.loop = true;
        }

        /// Checks if the character is on the ground.
        private void OnCollisionStay()
        {
            //Bounds.
            Bounds bounds = capsule.bounds;
            //Extents.
            Vector3 extents = bounds.extents;
            //Radius.
            float radius = extents.x - 0.01f;
            
            //Cast. This checks whether there is indeed ground, or not.
            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);
            
            //We can ignore the rest if we don't have any proper hits.
            if (!groundHits.Any(hit => hit.collider != null && hit.collider != capsule)) 
                return;
            
            //Store RaycastHits.
            for (var i = 0; i < groundHits.Length; i++)
                groundHits[i] = new RaycastHit();

            //Set grounded. Now we know for sure that we're grounded.
            grounded = true;
        }
			
        protected override void FixedUpdate()
        {
            //Move.
            MoveCharacter();

/*            Jump();*/
            
            //Unground.
            grounded = false;
        }

        protected override  void Update()
        {
            //Get the equipped weapon!
            equippedWeapon = playerCharacter.GetInventory().GetEquipped();
            
            //Play Sounds!
            PlayFootstepSounds();
        }

        #endregion

        #region METHODS

        private void MoveCharacter()
        {
            Vector2 frameInput = playerCharacter.GetInputMovement();
            Vector3 targetVelocity = new Vector3(frameInput.x, 0.0f, frameInput.y);
            if ( Input.GetKey(sprintKey) && playerCharacter.IsRunning())
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * speedRunning;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rigidBody.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            else
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * speedWalking;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rigidBody.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        public void Jump()
        {
            if (grounded == true)
            {
                rigidBody.AddForce(0f, jumpPower*1.5f, 0f, ForceMode.VelocityChange);
                grounded = false;
            }
        }

        /*        private void MoveCharacter()
                {
                    #region Calculate Movement Velocity

                    //Get Movement Input!
                    Vector2 frameInput = playerCharacter.GetInputMovement();
                    //Calculate local-space direction by using the player's input.
                    var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);

                    //Running speed calculation.
                    if(playerCharacter.IsRunning())
                        movement *= speedRunning;
                    else
                    {
                        //Multiply by the normal walking speed.
                        movement *= speedWalking;
                    }

                    //World space velocity calculation. This allows us to add it to the rigidbody's velocity properly.
                    movement = transform.TransformDirection(movement);

                    #endregion

                    //Update Velocity.
                    Velocity = new Vector3(movement.x, 0.0f, movement.z);
                }*/

        private void PlayFootstepSounds()
        {
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (grounded && rigidBody.velocity.sqrMagnitude > 0.1f)
            {
                //Select the correct audio clip to play.
                audioSource.clip = playerCharacter.IsRunning() ? audioClipRunning : audioClipWalking;
                //Play it!
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (audioSource.isPlaying)
                audioSource.Pause();
        }

        #endregion
    }
}