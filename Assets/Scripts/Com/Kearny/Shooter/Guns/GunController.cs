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

            if (Input.GetButton("Fire1"))
            {
                OnTriggerHold();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                OnTriggerRelease();
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

        public void OnTriggerHold()
        {
            _equippedGun.OnTriggerHold();
        }

        public void OnTriggerRelease()
        {
            _equippedGun.OnTriggerRelease();
        }
    }
}