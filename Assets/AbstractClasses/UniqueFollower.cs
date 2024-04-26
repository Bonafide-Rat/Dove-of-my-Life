using UnityEngine;

namespace AbstractClasses
{
    public abstract class UniqueFollower : MonoBehaviour
    {
        [Tooltip("Force Applied on x-axis when thrown")]
        public float throwPowerForward;
        [Tooltip("Force Applied on y-axis when thrown")]
        public float throwPowerUp;
        [Tooltip("Time until follower returns to player")]
        public float cooldown;
        [Tooltip("Identifier, used for comparisons in other scripts. Must be all lowercase.")]
        public string followerName;

        protected Rigidbody2D Rb;
        private new Collider2D collider;
        private Vector2 throwAngle;
        public bool throwable;

        
        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            
            DisableRbAndCollider();  
        }

        private void Update()
        {
            
        }

        public void Throw()
        {
            Rb.isKinematic = false;
            collider.enabled = true;
            if (PlayerController.isFacingRight)
            {
                throwAngle = new Vector2(throwPowerForward, throwPowerUp);
            }
            else
            {
                throwAngle = new Vector2(-throwPowerForward, throwPowerUp);
            }
            Rb.velocity = throwAngle;
        }

        public void DisableRbAndCollider()
        {
            collider.enabled = false;
            Rb.isKinematic = true;
        }

        public abstract void UseAbility();
    }
}
