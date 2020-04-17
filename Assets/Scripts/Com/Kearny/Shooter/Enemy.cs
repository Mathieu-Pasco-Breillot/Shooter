using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Com.Kearny.Shooter
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        private NavMeshAgent _pathFinder;
        private Transform _target;
        private bool _isTargetAlive;

        private void Start()
        {
            _pathFinder = GetComponent<NavMeshAgent>();
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _isTargetAlive = true;
            StartCoroutine(UpdatePath());
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private IEnumerator UpdatePath()
        {
            const float refreshRate = 0.5f;

            while (_isTargetAlive)
            {
                _pathFinder.SetDestination(_target.position);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}