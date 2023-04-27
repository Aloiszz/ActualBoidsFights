using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

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
    private float healtPointMax;
    public bool isEnnemy;
    public bool isAttacking;
    private bool isShooting;
    
    
    [Header("Information about Combat")] 
    public int engagementDistance = 50; // distance d'engagement requise pour attaquer une flock ennemy
    public bool canEngageAuto; // permet d'engager les flocks ennemy can entre dans la range

    [Header("Information about Weapon")] [MinMaxSlider(0.0f, 100.0f)]
    public Vector2 damageMinMax = new Vector2(15, 50);
    private float damage = 25; // Valeur min et max de dégat pouvant etre pris
    
    [Range(0, 100)] public float RayDistance = 10; // Distance de Tir 
    [Range(0, 100)] public float delayBetweenRay = .15f; // Delay entre chaque tir
    public bool canHitUnderGroundUnit = false;

    [Space] 
    [Header("Visual Part")]
    public RawImage healtBar;
    [SerializeField] private Gradient healtBarColor;
    private float transitionGrandient = 1;
    
    public Transform ObjectLook;
    private GameObject LookAt;
    
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
        healtBar.color = healtBarColor.Evaluate(transitionGrandient);
        
        //ObjectLook = transform.GetComponent<Transform>();
        LookAt = GameObject.FindGameObjectWithTag("MainCamera");
        
        healtPointMax = HealtPoint;
    }
    private void Update()
    {
        if (ObjectLook)
        {
            ObjectLook.LookAt(2 * ObjectLook.position - LookAt.transform.position);
        }
        
        transitionGrandient = (HealtPoint / healtPointMax);
        healtBar.color = healtBarColor.Evaluate(transitionGrandient);
        
        
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
                ChooseDamageValue(damageMinMax);
                if (isEnnemy) 
                {
                    if (isAttacking && !hitData.collider.GetComponent<FlockAgent>().isEnnemy) // Verif si ennemi est un ennemi et non un alliee
                    {
                        Debug.DrawRay(ray.origin, ray.direction * RayDistance , Color.green);
                        hitData.collider.GetComponent<FlockAgent>().HealtPoint -= damage;
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
                        hitData.collider.GetComponent<FlockAgent>().HealtPoint -= damage;
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


    void ChooseDamageValue(Vector2 damageMinMax) // permet de choisir des dégat aléatoire de la plage donner au game designer
    {
        damage = Random.Range(damageMinMax.x, damageMinMax.y);
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
