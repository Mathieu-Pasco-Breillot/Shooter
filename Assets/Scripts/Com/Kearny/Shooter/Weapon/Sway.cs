using UnityEngine;

namespace Com.Kearny.Shooter.Weapon
{
    public class Sway : MonoBehaviour
    {
        #region Variables

        public float swayIntensity;
        public float smooth;

        private Quaternion _originRotation;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _originRotation = transform.localRotation;
        }

        private void Update()
        {
            UpdateSway();
        }

        #endregion

        #region Private Methods

        private  void UpdateSway()
        {
            var xMouse = Input.GetAxis("Mouse X");
            var yMouse = Input.GetAxis("Mouse Y");
            
            // Calculate target rotation
            var xAdjustment = Quaternion.AngleAxis(-swayIntensity * xMouse, Vector3.up);
            var yAdjustment = Quaternion.AngleAxis(swayIntensity * yMouse, Vector3.right);
            var targetRotation = _originRotation * xAdjustment * yAdjustment;
            
            // Rotate towards rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
        
        #endregion
    }
}