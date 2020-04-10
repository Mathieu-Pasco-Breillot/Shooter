using System.Security.Cryptography;
using Com.Kearny.Shooter.ScriptableObjectGenerator;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Weapon : MonoBehaviour
    {
        #region Variables

        public Gun[] loadOut;
        public Transform weaponParent;

        private GameObject _currentWeapon;

        #endregion

        #region MonoBehaviour Callbacks

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Ampersand)) Equip(0);
        }

        #endregion

        #region Private Methods

        // Equip 0 for primary, 1 for second, etc...
        private void Equip(int index)
        {
            Destroy(_currentWeapon);

            GameObject newEquipment = Instantiate(loadOut[index].prefab, weaponParent.position, weaponParent.rotation,
                weaponParent);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;

            _currentWeapon = newEquipment;
        }

        #endregion
    }
}