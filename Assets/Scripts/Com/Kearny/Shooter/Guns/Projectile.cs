using Com.Kearny.Shooter.GameMechanics;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Projectile : MonoBehaviour
    {
        public LayerMask collisionMask;

        private float _speed;

        private const float Damage = 1;

        private const float Lifetime = 3;

        private float skinWidth = .1f;

        private void Start()
        {
            Destroy(gameObject, Lifetime);

            var initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
            if (initialCollisions.Length > 0)
            {
                OnHitObject(initialCollisions[0]);
            }
        }

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
            var ray = new Ray(localTransform.position, localTransform.forward);
            if (Physics.Raycast(ray, out var hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }

        private void OnHitObject(RaycastHit hit)
        {
            hit.collider.GetComponent<IDamageable>()?.TakeHit(Damage, hit);

            Destroy(gameObject);
        }

        private void OnHitObject(Component colliderComponent)
        {
            colliderComponent.GetComponent<IDamageable>()?.TakeDamage(Damage);

            Destroy(gameObject);
        }
    }
}