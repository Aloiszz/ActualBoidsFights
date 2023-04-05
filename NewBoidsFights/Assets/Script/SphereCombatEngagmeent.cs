using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCombatEngagmeent : MonoBehaviour
{

    private int rangeSphere;
    [SerializeField] private SphereCollider sphere;

    private void Start()
    {
        rangeSphere = GetComponentInParent<FlockAgent>().engagementDistance;
        sphere.radius = rangeSphere;
    }

    private void OnTriggerStay(Collider other)
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
                    Debug.Log(other.name);
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
            }

            if (!other.GetComponentInParent<FlockAgent>().isEnnemy)
            {
                GetComponentInParent<FlockAgent>().isAttacking = false;
            }
        }
    }
}
