using Com.Kearny.Shooter.Enemy;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Projectile : MonoBehaviour
    {
        public LayerMask collisionMask;

        private float _speed;
        private readonly float _damage = 1;

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }

        // Update is called once per frame
        private void Update()
        {
            var moveDistance = (Time.deltaTime * _speed);
            CheckCollisions(moveDistance);
            transform.Translate(Vector3.forward * moveDistance);
        }

        private void CheckCollisions(float moveDistance)
        {
            var localTransform = transform;
            Ray ray = new Ray(localTransform.position, localTransform.forward);
            if (Physics.Raycast(ray, out var hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }

        private void OnHitObject(RaycastHit hit)
        {
            hit.collider.GetComponent<IDamageable>()?.TakeHit(_damage, hit);

            Destroy(gameObject);
        }
    }
}