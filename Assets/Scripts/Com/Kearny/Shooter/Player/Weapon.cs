﻿using Com.Kearny.Shooter.ScriptableObjectGenerator;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Weapon : MonoBehaviour
    {
        #region Variables

        public Gun[] loadOut;
        public Transform weaponParent;

        private int currentWeaponIndex = -1;
        private GameObject currentWeaponGameObject;
        private Gun currentWeaponGun;
        private Transform anchor;
        private Transform stateHip;
        private Transform stateAds;

        // Shoot
        public GameObject impactEffect;
        public GameObject impactBloodEffect;
        private Transform normalCamera;
        private float nextTimeToFire = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        // Update is called once per frame
        private void Start()
        {
            normalCamera = transform.Find("Cameras/Normal Camera");
        }

        private void Update()
        {
            // Draw weapon situated on & keyboard symbol
            if (Input.GetKeyDown(KeyCode.Ampersand)) Equip(0);

            if (currentWeaponIndex > -1)
            {
                // 1 is right mouse button
                Aim(Input.GetButton("Fire2"));
                
                // Shoot
                if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / currentWeaponGun.fireRate;
                    Shoot();
                }
                
                // Weapon elasticity
                currentWeaponGameObject.transform.localPosition =
                    Vector3.Lerp(currentWeaponGameObject.transform.localPosition, Vector3.zero, Time.deltaTime * 4);
            }
        }

        #endregion

        #region Private Methods

        private void Shoot() 
        {
            // Bloom
            var position = normalCamera.position;
            var bloom = position + normalCamera.forward * 1000;
            bloom += Random.Range(-currentWeaponGun.bloom, currentWeaponGun.bloom) * normalCamera.up;
            bloom += Random.Range(-currentWeaponGun.bloom, currentWeaponGun.bloom) * normalCamera.right;
            bloom -= position;
            bloom.Normalize();
            
            // Ray cast
            if (!Physics.Raycast(normalCamera.position, bloom, out var hit)) return;

            var transformTag = hit.transform.tag;

            if (transformTag == "Zombie")
            {
                var impact = Instantiate(impactBloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
            else
            {
                var impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }

            // Apply force on target
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * currentWeaponGun.impactForce);
            }
            
            // Gun FX
            currentWeaponGameObject.transform.Rotate(-currentWeaponGun.recoil, 0, 0);
            currentWeaponGameObject.transform.position -=
                currentWeaponGameObject.transform.forward * currentWeaponGun.kickBack;
        }

        private void Aim(bool isAiming)
        {
            anchor.position = Vector3.Lerp(anchor.position,
                isAiming ? stateAds.position : stateHip.position,
                Time.deltaTime * currentWeaponGun.aimSpeed);
        }

        // Equip 0 for primary, 1 for second, etc...
        private void Equip(int index)
        {
            Destroy(currentWeaponGameObject);

            currentWeaponIndex = index;

            var newEquipment = Instantiate(loadOut[index].prefab, weaponParent.localPosition,
                weaponParent.localRotation,
                weaponParent);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;

            currentWeaponGameObject = newEquipment;
            currentWeaponGun = loadOut[index];

            var currentWeaponTransform = currentWeaponGameObject.transform;

            anchor = currentWeaponTransform.Find("Anchor");
            stateHip = currentWeaponTransform.Find("States/Hip");
            stateAds = currentWeaponTransform.Find("States/ADS");
        }

        #endregion
    }
}