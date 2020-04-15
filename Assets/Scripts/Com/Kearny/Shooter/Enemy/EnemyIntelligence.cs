using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Com.Kearny.Shooter.Enemy
{
    public class EnemyIntelligence : MonoBehaviour
    {
        public float speed = 1.5f;

        public float health = 100f;

        private NavMeshAgent agent;

        private Transform target;

        private Animator anim;

        private static readonly int Hit = Animator.StringToHash("Hit");

        private static readonly int Dead = Animator.StringToHash("Dead");

        // Start is called before the first frame update
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;

            target = GameObject.FindWithTag("Player").transform;

            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (agent.enabled)
                agent.SetDestination(target.position);
        }

        public void Damage(int amount)
        {
            health -= amount;
            agent.speed = 0;
            anim.SetTrigger(Hit);

            if (health < 1)
            {
                agent.speed = 0;
                anim.SetTrigger(Dead);
                agent.enabled = false;
            }
            else
            {
                StartCoroutine(HitRoutine());
            }
        }

        private IEnumerator HitRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            agent.speed = 1.5f;
        }
    }
}