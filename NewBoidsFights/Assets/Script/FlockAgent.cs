using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    //public GameObject vise;
    // Récupération des Flocks Collider
    private Collider agentCollider;

    [SerializeField] private Transform canon1;
    [SerializeField] private Transform canon2;
    [SerializeField] private GameObject ammo;


    [Header("Information about Unit")] 
    [Range(0, 50)] public float HealtPoint = 50;
    public bool isEnnemy;
    public bool isAttacking;
    private bool isShooting;
    
    
    [Header("Information about Combat")] 
    public int engagementDistance = 50; // distance d'engagement requise pour attaquer une flock ennemy
    public bool canEngageAuto; // permet d'engager les flocks ennemy can entre dans la range

    [Header("Information about Weapon")] [MinMaxSlider(0.0f, 100.0f)]
    public float damageMinMax = 25; // Valeur min et max de dégat pouvant etre pris
    
    [Range(0, 100)] public float RayDistance = 10; // Distance de Tir 
    [Range(0, 100)] public float delayBetweenRay = .15f; // Delay entre chaque tir
    public bool canHitUnderGroundUnit = false;
    
    
    public Collider AgentCollider
    {
        get
        {
            return agentCollider;
        }
    }
    
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }
    
    private void FixedUpdate()
    {
        
    }
    
    
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (!isEnnemy)
        {
            //Debug.DrawLine(transform.position, vise.transform.position, Color.cyan);
            Debug.DrawRay(ray.origin, ray.direction * RayDistance, Color.cyan);
        }
        else
        {
            //Debug.DrawLine(transform.position, vise.transform.position, Color.red);
            Debug.DrawRay(ray.origin, ray.direction * RayDistance , Color.red);
        }
        
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, RayDistance))
        {
            if (hitData.collider.CompareTag("FlockAgent"))
            {
                
                if (isEnnemy) 
                {
                    if (isAttacking && !hitData.collider.GetComponent<FlockAgent>().isEnnemy) // Verif si ennemi est un ennemi et non un alliee
                    {
                        Debug.DrawRay(ray.origin, ray.direction * RayDistance , Color.green);
                        hitData.collider.GetComponent<FlockAgent>().HealtPoint -= damageMinMax;
                        //Destroy(hitData.transform.gameObject);
                        if (hitData.collider.GetComponent<FlockAgent>().HealtPoint <= 0)
                        {
                            hitData.transform.gameObject.SetActive(false);
                        }
                        Debug.LogWarning(hitData.transform.name + " Was Killed by " + transform.name);
                        //isAttacking = false;
                        //Attacking();
                    }
                }
                else
                {
                    if (isAttacking && hitData.collider.GetComponent<FlockAgent>().isEnnemy) // Verif si ennemi est un ennemi et non un alliee
                    {
                        Debug.DrawRay(ray.origin, ray.direction * RayDistance , Color.green);
                        //Destroy(hitData.transform.gameObject);
                        hitData.collider.GetComponent<FlockAgent>().HealtPoint -= damageMinMax;
                        if (hitData.collider.GetComponent<FlockAgent>().HealtPoint <= 0)
                        {
                            hitData.transform.gameObject.SetActive(false);
                        }
                        Debug.LogWarning(hitData.transform.name + " Was Killed by " + transform.name);
                        //isAttacking = false;
                        //Attacking();
                    }
                }
                
                
            }
        }
        
    }
    
    
    

    public void Move(Vector3 velocity) // permet de tourner et orienter le flock vers la direction qu'il emprunte
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }

    
    
    
    
    void Attacking()
    {
        if (isAttacking)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shooting());
            }
            
        }
    }

    IEnumerator Shooting()
    {
        GameObject Ammo = Instantiate(ammo, canon1.transform.position, transform.rotation);
        Destroy(Ammo, 1);
        GameObject Ammo2 = Instantiate(ammo, canon2.transform.position, transform.rotation);
        Destroy(Ammo2, 1);
        yield return new WaitForSeconds(1f);
        isShooting = false;
    }
}
