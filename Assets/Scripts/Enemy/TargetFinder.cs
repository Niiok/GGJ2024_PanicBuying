using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.PanicBuying.Character;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

namespace PanicBuying
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Standby
    }

    public class TargetFinder : MonoBehaviour
    {
        public float patrolSpeed;
        public float chasingSpeed;
        public float patrolStartTime;

        public List<PatrolRoute> routes;
        protected NavMeshAgent agent;
        public BoxCaster caster;
        public NetworkAnimator networkAnimator;

        protected EnemyState state = EnemyState.Standby;

        public int routeIdx;
        public int patrolPointIdx;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            networkAnimator = GetComponentInChildren<NetworkAnimator>();

            GameObject[] routeObjects = GameObject.FindGameObjectsWithTag("EnemyPatrolRoute");
            foreach (GameObject go in routeObjects)
            {
                routes.Add(go.GetComponent<PatrolRoute>());
            }

            Invoke("StartPatrolling", 3.0f);
        }

        private void Update()
        {
            switch (state)
            {
                case EnemyState.Patrolling:
                    networkAnimator.Animator.SetBool("Walk", true);
                    networkAnimator.Animator.SetBool("Run", false);
                    break;
                case EnemyState.Chasing:
                    networkAnimator.Animator.SetBool("Run", true);
                    networkAnimator.Animator.SetBool("Walk", true);
                    break;
                case EnemyState.Standby:
                    networkAnimator.Animator.SetBool("Run", false);
                    networkAnimator.Animator.SetBool("Walk", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (state == EnemyState.Standby) return;
            if ((agent.destination - transform.position).magnitude <= agent.stoppingDistance)
            {
                if (state == EnemyState.Patrolling)
                    Invoke("GetNextPatrolPoint", patrolStartTime);
                else if (state == EnemyState.Chasing)
                    Invoke("StartPatrolling", patrolStartTime);
            }
        }

        public virtual void setTarget(Vector3 targetPos)
        {
            caster.transform.LookAt(targetPos);
            caster.BoxCast();
            if (caster.isHit)
            {
                {
                    state = EnemyState.Chasing;
                    agent.speed = chasingSpeed;
                    agent.SetDestination(targetPos);
                }
            }
        }

        private void StartPatrolling()
        {
            routeIdx = 0;
            patrolPointIdx = 0;
            float shortestDistance = float.MaxValue;

            // Find closest route
            for (int i = 0; i < routes.Count; i++)
            {
                for (int j = 0; j < routes[i].patrolPoints.Length; j++)
                {
                    if (shortestDistance > (routes[i].patrolPoints[j].position - transform.position).magnitude)
                    {
                        routeIdx = i;
                        patrolPointIdx = j;
                        shortestDistance = (routes[i].patrolPoints[j].position - transform.position).magnitude;
                    }
                }
            }

            SetPatrolPoint(routeIdx, patrolPointIdx);
        }

        private void GetNextPatrolPoint()
        {
            patrolPointIdx++;
            if (patrolPointIdx >= routes[routeIdx].patrolPoints.Length)
            {
                patrolPointIdx = 0;
            }

            SetPatrolPoint(routeIdx, patrolPointIdx);
        }

        protected virtual void SetPatrolPoint(int routeIdx, int patrolPointIdx)
        {
            CancelInvoke();
            state = EnemyState.Patrolling;
            agent.speed = patrolSpeed;

            agent.SetDestination(routes[routeIdx].patrolPoints[patrolPointIdx].position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("PlayerCharacter"))
            {
                PlayerControl control = collision.gameObject.GetComponent<PlayerControl>();
                collision.collider.GetComponentInChildren<Light>().intensity = 0;
                control.walkSpeed = 0;
                control.sneakSpeed = 0;
                control.runSpeed = 0;
                control.jumpForce = 0;
                collision.gameObject.SetActive(false);

                //Invoke("disapear", 5f);
            }
        }

        private void disapear()
        {
            
            NetworkManager.Singleton.Shutdown();
        }
    }
}