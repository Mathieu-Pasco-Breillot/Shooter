using Com.Kearny.Shooter.GameMechanics;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Projectile : MonoBehaviour
    {
        public LayerMask collisionMask;

        private float _speed;

        private const float DAMAGE = 1;
        private const float LIFETIME = 3;
        private const float SKIN_WIDTH = .1f;

        private void Start()
        {
            Destroy(gameObject, LIFETIME);

            var initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
            if (initialCollisions.Length > 0)
            {
                OnHitObject(initialCollisions[0], transform.position);
            }
        }

        // Quel intérêt de faire une méthode qui fait le travail d'un setter ? Pour quoi ne pas remplacer ton champ par une propriété public ?
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
            if (Physics.Raycast(ray, out var hit, moveDistance + SKIN_WIDTH, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit.collider, hit.point);
            }
        }

        private void OnHitObject(Component colliderComponent, Vector3 hitPoint)
        {
            colliderComponent.GetComponent<IDamageable>()?.TakeHit(DAMAGE, hitPoint, transform.forward);

            Destroy(gameObject);
        }
    }
}