using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCombatEngagmeent : MonoBehaviour
{

    private int rangeSphere;
    [SerializeField] private SphereCollider sphere;

    [SerializeField] private List<FlockAgent> otherFlockAgent;

    private void Start()
    {
        rangeSphere = GetComponentInParent<FlockAgent>().engagementDistance;
        sphere.radius = rangeSphere;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlockAgent") && GetComponentInParent<FlockAgent>().canEngageAuto) // Permet de verifier si unit attaque ou non 
        {
            
            if (other.GetComponentInParent<FlockAgent>().isEnnemy)
            {
                GetComponentInParent<FlockAgent>().isAttacking = true;
            }
            if (GetComponentInParent<FlockAgent>().isEnnemy)
            {
                if (other.GetComponentInParent<FlockAgent>().isEnnemy)
                {
                    GetComponentInParent<FlockAgent>().isAttacking = false;
                }
                else
                {
                    GetComponentInParent<FlockAgent>().isAttacking = true;
                    otherFlockAgent.Add(other.GetComponentInParent<FlockAgent>());
                }
            }
            else
            {
                if (other.GetComponentInParent<FlockAgent>().isEnnemy)
                {
                    otherFlockAgent.Add(other.GetComponentInParent<FlockAgent>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlockAgent")) // si flock et si ennemy alors attaque
        {
            if (other.GetComponentInParent<FlockAgent>().isEnnemy)
            {
                GetComponentInParent<FlockAgent>().isAttacking = false;
                otherFlockAgent.Remove(other.GetComponentInParent<FlockAgent>());
            }

            if (!other.GetComponentInParent<FlockAgent>().isEnnemy)
            {
                GetComponentInParent<FlockAgent>().isAttacking = false;
                otherFlockAgent.Remove(other.GetComponentInParent<FlockAgent>());
            }
        }
    }
}
