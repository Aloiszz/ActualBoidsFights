using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCombatEngagmeent : MonoBehaviour
{

    [SerializeField] private int rangeSphere;


    private void Start()
    {
        rangeSphere = GetComponentInParent<Flock>().engagementDistance;
        GetComponent<SphereCollider>().radius = rangeSphere;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flock") && GetComponentInParent<Flock>().canEngageAuto) // Permet de verifier si unit attaque ou non 
        {
            if (other.GetComponentInParent<Flock>().isUnitEnnemy)
            {
                GetComponentInParent<Flock>().isCombat = true;
            }

            if (GetComponentInParent<Flock>().isUnitEnnemy)
            {
                if (other.GetComponentInParent<Flock>().isUnitEnnemy)
                {
                    GetComponentInParent<Flock>().isCombat = false;
                }
                else
                {
                    GetComponentInParent<Flock>().isCombat = true;
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flock")) // si flock et si ennemy alors attaque
        {
            if (other.GetComponentInParent<Flock>().isUnitEnnemy)
            {
                GetComponentInParent<Flock>().isCombat = false;
            }

            if (!other.GetComponentInParent<Flock>().isUnitEnnemy)
            {
                GetComponentInParent<Flock>().isCombat = false;
            }
        }
    }
}
