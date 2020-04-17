using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Projectile : MonoBehaviour
    {
        private float speed;

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }
    }
}
