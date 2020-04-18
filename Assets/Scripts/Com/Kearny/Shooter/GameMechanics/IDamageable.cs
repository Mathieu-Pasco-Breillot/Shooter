using UnityEngine;

namespace Com.Kearny.Shooter.GameMechanics
{
    public interface IDamageable
    {
        void TakeHit(float damage, RaycastHit hit);
        void TakeDamage(float damage);
    }
}