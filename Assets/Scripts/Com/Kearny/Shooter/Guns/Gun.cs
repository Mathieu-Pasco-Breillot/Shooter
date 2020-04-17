using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        public Transform muzzle;
        public Projectile projectile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35;

        private float _nextShotTime;
        
        public void Shoot()
        {
            if (!(Time.time >= _nextShotTime)) return;
            
            _nextShotTime = Time.time + msBetweenShots / 1000;
            var newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation);
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}