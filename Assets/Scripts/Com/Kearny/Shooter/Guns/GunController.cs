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

        private void Update()
        {
            if (!_isGunEquipped) return;

            if (_equippedGun.FireType == FireType.SemiAutomatic)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else if (_equippedGun.FireType == FireType.Automatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
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

        private void Shoot()
        {
            _equippedGun.Shoot();
        }
    }
}