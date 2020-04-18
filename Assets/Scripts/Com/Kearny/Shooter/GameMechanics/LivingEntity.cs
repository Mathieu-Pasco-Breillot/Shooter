using UnityEngine;

namespace Com.Kearny.Shooter.GameMechanics
{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;
        protected float health;
        protected bool isDead;

        public event System.Action OnDeath;

        protected virtual void Start()
        {
            health = startingHealth;
        }

        public void TakeHit(float damage, RaycastHit hit)
        {
           TakeDamage(damage);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0 && !isDead)
            {
                Die();
            }
        }

        [ContextMenu("Self Destruct")]
        private void Die()
        {
            isDead = true;
            if (OnDeath != null)
            {
                OnDeath();
            }
            Destroy(gameObject);
        }
    }
}