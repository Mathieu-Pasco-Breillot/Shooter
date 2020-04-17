using System;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class GunController : MonoBehaviour
    {
        public Transform weaponHold;
        public Gun startingGun;
        private Gun _equippedGun;
        private bool _isGunEquipped = false;

        private void Start()
        {
            if (startingGun != null)
            {
                EquipGun(startingGun);
            }
        }

        public void EquipGun(Gun gunToEquip)
        {
            if (_equippedGun != null)
            {
                Destroy(_equippedGun.gameObject);
            }

            _equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation, weaponHold);
            _isGunEquipped = true;
        }

        public void Shoot()
        {
            if (_isGunEquipped)
            {
                _equippedGun.Shoot();
            }
        }
    }
}