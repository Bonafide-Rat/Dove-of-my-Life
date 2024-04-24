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

        protected Rigidbody2D rb;
        private new Collider2D collider;
        private Vector2 throwAngle;
        public bool throwable;

        
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            throwAngle = new Vector2(throwPowerForward, throwPowerUp);
            DisableRbAndCollider();  
        }

        private void Update()
        {
            // if (!PlayerController.isFacingRight)
            // {
            //     throwAngle.x *= 1;
            // }
        }

        public void Throw()
        {
            rb.isKinematic = false;
            collider.enabled = true;
            rb.velocity = throwAngle;
        }

        public void DisableRbAndCollider()
        {
            collider.enabled = false;
            rb.isKinematic = true;
        }
        public abstract void UseAbility();

        private bool playerFacingRight()
        {
            if (rb.velocity.x > 0f)
            {
                return true;
            }
            return false;
        }
    }
}
