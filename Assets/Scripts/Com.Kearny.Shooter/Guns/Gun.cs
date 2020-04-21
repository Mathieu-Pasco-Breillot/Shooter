using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    // J'ai ici l'impression que ta classe représente plus une arme à feu en général plus qu'un pistolet vu que tu as une classe enfant Pistol.
    // Pourquoi ne pas renommer cette classe Weapon ou FireArm au pire
    public abstract class Gun : MonoBehaviour
    {
        protected abstract FireMode FireMode { get; set; }
        public float MsBetweenShots { get; set; } = 100;
        public float MuzzleVelocity { get; set; } = 35;
        public Transform Muzzle { get; set; }
        public Projectile Projectile { get; set; }
        public Transform Shell { get; set; }
        public Transform ShellEjection { get; set; }

        /// <summary>
        /// Vérifie que l'arme peut utiliser le mode de tir : <paramref name="fireModeToCheck"/>
        /// </summary>
        /// <param name="fireModeToCheck">Le mode de tir à appliquer à l'arme.</param>
        /// <returns>Vrai si le mode demandé peut être utilisé par l'arme; Faux sinon.</returns>
        protected abstract bool IsFireModeAllow(FireMode fireModeToCheck);

        private MuzzleFlash _muzzleFlash;

        private float _nextShotTime;

        private bool _triggerReleasedSinceLastShot = true;
        private int _shotsRemainingInBurst;
        
        private const int BURST_COUNT = 3;

        private void Start()
        {
            _muzzleFlash = GetComponent<MuzzleFlash>();
            _shotsRemainingInBurst = BURST_COUNT;
        }

        private void Shoot()
        {
            if (!(Time.time >= _nextShotTime)) return;
            
            if (FireMode == FireMode.Burst && _shotsRemainingInBurst == 0)
            {
                return;
            }

            switch (FireMode)
            {
                case FireMode.Burst:
                    _shotsRemainingInBurst--;
                    break;
                case FireMode.SemiAuto when !_triggerReleasedSinceLastShot:
                    return;
            }

            _nextShotTime = Time.time + MsBetweenShots / 1000;
            var newProjectile = Instantiate(Projectile, Muzzle.position, Muzzle.rotation);
            newProjectile.SetSpeed(MuzzleVelocity);

            Instantiate(Shell, ShellEjection.position, ShellEjection.rotation);

            _muzzleFlash.Activate();
        }

        public void OnTriggerHold()
        {
            Shoot();
            _triggerReleasedSinceLastShot = false;
        }

        public void OnTriggerRelease()
        {
            _triggerReleasedSinceLastShot = true;
            _shotsRemainingInBurst = BURST_COUNT;
        }
    }
}