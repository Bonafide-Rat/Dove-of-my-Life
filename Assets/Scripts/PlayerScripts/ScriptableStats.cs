using UnityEngine;

namespace TarodevController
{
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject
    {
        [Header("LAYERS")] [Tooltip("Set this to the layer your player is on")]
        public LayerMask PlayerLayer;

        [Header("MOVEMENT")] [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 4;

        [Tooltip("The player's capacity to gain horizontal speed")]
        public float Acceleration = 150;

        [Tooltip("The pace at which the player comes to a stop")]
        public float GroundDeceleration = 300;

        [Tooltip("Deceleration in air only after stopping input mid-air")]
        public float AirDeceleration = 30;

        [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
        public float GroundingForce = -1.5f;

        [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        [Header("JUMP")] [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 17;

        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 25;

        [Tooltip("The glide fall speed")]
        public float GlideFallSpeed = 3;

        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float FallAcceleration = 25;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 5;

        [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public float CoyoteTime = .3f;

        [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
        public float JumpBuffer = .2f;
    }
}