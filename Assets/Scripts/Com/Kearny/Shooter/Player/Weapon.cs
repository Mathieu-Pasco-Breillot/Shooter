using Com.Kearny.Shooter.ScriptableObjectGenerator;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Weapon : MonoBehaviour
    {
        #region Variables

        public Gun[] loadOut;
        public Transform weaponParent;

        private int _currentWeaponIndex;
        private GameObject _currentWeapon;

        #endregion

        #region MonoBehaviour Callbacks

        // Update is called once per frame
        private void Start()
        {
        }

        private void Update()
        {
            // Draw weapon situated on & keyboard symbol
            if (Input.GetKeyDown(KeyCode.Ampersand)) Equip(0);

            if (_currentWeapon != null)
            {
                // 1 is right mouse button
                Aim(Input.GetMouseButton(1));
            }
        }
        
        #endregion

        #region Private Methods

        private void Aim(bool isAiming)
        {
            var anchor = _currentWeapon.transform.Find("Anchor");
            var stateHip = _currentWeapon.transform.Find("States/Hip");
            var stateAds = _currentWeapon.transform.Find("States/ADS");

            anchor.position = Vector3.Lerp(anchor.position,
                isAiming ? stateAds.position : stateHip.position,
                Time.deltaTime * loadOut[_currentWeaponIndex].aimSpeed);
        }

        
        // Equip 0 for primary, 1 for second, etc...
        private void Equip(int index)
        {
            Destroy(_currentWeapon);

            _currentWeaponIndex = index;

            GameObject newEquipment = Instantiate(loadOut[index].prefab, weaponParent.localPosition,
                weaponParent.localRotation,
                weaponParent);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;

            _currentWeapon = newEquipment;
        }

        #endregion
    }
}