using UnityEngine;

namespace Com.Kearny.Shooter
{
    public interface IDamageable
    {
        void TakeHit(float damage, RaycastHit hit);
    }
}