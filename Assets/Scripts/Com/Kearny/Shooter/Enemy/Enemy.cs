using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Com.Kearny.Shooter.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : LivingEntity
    {
        private enum State
        {
            Idle,
            Chasing,
            Attacking
        };

        private State _currentState;

        private NavMeshAgent _pathFinder;
        private Transform _target;
        private bool _isTargetAlive;
        private Material _skinMaterial;

        private Color _originalColor;

        private const float AttackDistanceThreshold = 1.5f;
        private const float TimeBetweenAttacks = 1;

        private float _nextAttackTime;
        private float _myCollisionRadius;
        private float _targetCollisionRadius;

        protected override void Start()
        {
            base.Start();

            _pathFinder = GetComponent<NavMeshAgent>();
            _skinMaterial = GetComponent<Renderer>().material;
            _originalColor = _skinMaterial.color;

            _currentState = State.Chasing;

            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _isTargetAlive = true;

            _myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            _targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }

        private void Update()
        {
            if (!(Time.time > _nextAttackTime)) return;

            var squareDistanceToTarget = (_target.position - transform.position).sqrMagnitude;
            if (squareDistanceToTarget <
                Mathf.Pow(AttackDistanceThreshold + _myCollisionRadius + _targetCollisionRadius, 2))
            {
                _nextAttackTime = Time.time + TimeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }

        private IEnumerator Attack()
        {
            _currentState = State.Attacking;

            var transformPosition = transform.position;
            Vector3 originalPosition = transformPosition;

            var targetPosition = _target.position;
            Vector3 directionToTarget = (targetPosition - transformPosition).normalized;
            Vector3 attackPosition = targetPosition - directionToTarget;

            const float attackSpeed = 3;
            float percent = 0;

            _skinMaterial.color = Color.red;

            while (percent <= 1)
            {
                percent += Time.deltaTime * attackSpeed;
                var interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }

            _skinMaterial.color = _originalColor;

            _currentState = State.Chasing;
        }

        private IEnumerator UpdatePath()
        {
            const float refreshRate = 0.5f;

            while (_isTargetAlive)
            {
                if (!isDead && _currentState == State.Chasing)
                {
                    var targetPosition = _target.position;
                    Vector3 directionToTarget = (targetPosition - transform.position).normalized;
                    Vector3 targetedPosition =
                        targetPosition - directionToTarget *
                        (_myCollisionRadius + _targetCollisionRadius + AttackDistanceThreshold / 2);
                    _pathFinder.SetDestination(targetedPosition);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}