﻿using Com.Kearny.Shooter.ScriptableObjectGenerator;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Weapon : MonoBehaviour
    {
        #region Variables

        public Gun[] loadOut;
        public Transform weaponParent;

        private int _currentWeaponIndex = -1;
        private GameObject _currentWeaponGameObject;
        private Gun _currentWeaponGun;
        private Transform _anchor;
        private Transform _stateHip;
        private Transform _stateAds;

        // Shoot
        public GameObject impactEffect;
        private Transform _spawnCamera;
        private float _nextTimeToFire = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        // Update is called once per frame
        private void Start()
        {
            _spawnCamera = transform.Find("Cameras/Normal Camera");
        }

        private void Update()
        {
            // Draw weapon situated on & keyboard symbol
            if (Input.GetKeyDown(KeyCode.Ampersand)) Equip(0);

            if (_currentWeaponIndex > -1)
            {
                // 1 is right mouse button
                Aim(Input.GetButton("Fire2"));
            }

            if (Input.GetButton("Fire1") && _currentWeaponIndex > -1 && Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _currentWeaponGun.fireRate;
                Shoot();
            }
        }

        #endregion

        #region Private Methods

        private void Shoot() 
        {
            // Bloom
            var position = _spawnCamera.position;
            var bloom = position + _spawnCamera.forward * 1000;
            bloom += Random.Range(-_currentWeaponGun.bloom, _currentWeaponGun.bloom) * _spawnCamera.up;
            bloom += Random.Range(-_currentWeaponGun.bloom, _currentWeaponGun.bloom) * _spawnCamera.right;
            bloom -= position;
            bloom.Normalize();
            
            // Ray cast
            if (!Physics.Raycast(_spawnCamera.position, bloom, out var hit)) return;

            var impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.5f);

            // Apply force
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * _currentWeaponGun.impactForce);
            }
        }

        private void Aim(bool isAiming)
        {
            _anchor.position = Vector3.Lerp(_anchor.position,
                isAiming ? _stateAds.position : _stateHip.position,
                Time.deltaTime * _currentWeaponGun.aimSpeed);
        }

        // Equip 0 for primary, 1 for second, etc...
        private void Equip(int index)
        {
            Destroy(_currentWeaponGameObject);

            _currentWeaponIndex = index;

            GameObject newEquipment = Instantiate(loadOut[index].prefab, weaponParent.localPosition,
                weaponParent.localRotation,
                weaponParent);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;

            _currentWeaponGameObject = newEquipment;
            _currentWeaponGun = loadOut[index];

            var currentWeaponTransform = _currentWeaponGameObject.transform;

            _anchor = currentWeaponTransform.Find("Anchor");
            _stateHip = currentWeaponTransform.Find("States/Hip");
            _stateAds = currentWeaponTransform.Find("States/ADS");
        }

        #endregion
    }
}