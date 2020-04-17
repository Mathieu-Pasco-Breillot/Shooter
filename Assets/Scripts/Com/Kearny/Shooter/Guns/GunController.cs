using System;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class GunController : MonoBehaviour
    {
        public Transform weaponHold;
        public Gun startingGun;

        private Gun _equippedGun;

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
        }

        public static void RotateGun()
        {
            throw new NotImplementedException();
        }
    }
}