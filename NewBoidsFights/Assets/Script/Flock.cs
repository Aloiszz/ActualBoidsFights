using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Collections;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;


public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab; // prefab du flock
    private List<FlockAgent> agents = new List<FlockAgent>(); // permet de stocker tout les agents
    public FLockBehavior behavior;
    public StayInRadiusBehavior radiusBehavior;

    [Range(1, 1500)] 
    public int startingCount = 250; // Population de flock Instantié
    public float AgentDensity = 0.01f;

    [Range(1f, 100f)] 
    public float driveFactor = 10f;
    
    [Range(1f, 100f)] 
    public float maxSpeed = 5f;

    [Range(1f, 10f)] 
    public float neighborRadius = 1.5f;

    [Range(0f, 1f)] 
    public float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;
    
    [Header("Flock Type")]
    [BoxGroup("Flock")]
    public FlockType currentFlock;
    
    [Header("Flock State")]
    [BoxGroup("Flock")]
    public bool isCombat;
    [BoxGroup("Flock")]
    public bool isMoving;
    [BoxGroup("Flock")]
    public bool isWaiting;
    
    [BoxGroup("Flock")]
    [Header("Information about Combat Transfered to the Unit")] 
    public int engagementDistance = 50; // distance d'engagement requise pour attaquer une flock ennemy
    [BoxGroup("Flock")]
    public bool canEngageAuto = true; // permet d'engager les flocks ennemy can entre dans la range
    
    [BoxGroup("Flock")]
    [Header("Information about flock")]
    public bool isUnitSelected; // Savoir si unité 
    [BoxGroup("Flock")]
    public bool isUnitEnnemy; //Savoir si unité est Allié ou ennemi
    [BoxGroup("Flock")]
    public bool isUnitShooting;
    
    private GameObject _SphereFlockSelection;
    private Renderer m_FlockSelection; // Material pour la selection de la flock
    [HideInInspector]public Color colorSelection;
    
    [HideInInspector] public Vector3 centerRadius; //récupération du centre Radius depuis SO RadiusBehavior
    private float radius; //récupération du Radius depuis SO RadiusBehavior

    public float SquareAvoidanceRadius
    {
        get
        {
            return squareAvoidanceRadius;
        }
    }

    void Secure_SO()
    {
        centerRadius = radiusBehavior.center;
        if (isUnitEnnemy)
        {
            centerRadius = FlockManager.instance.WhereToSpawnsEnnemis[FlockManager.instance.countLocationEnnemy];
            //gameObject.layer = 2;
            //radiusBehavior.center = new Vector3(0, 0, 50);
        }
        else
        {
            centerRadius = FlockManager.instance.WhereToSpawns[FlockManager.instance.countLocation];
            //radiusBehavior.center = new Vector3(0, 0, 0);
        }
        
        radius = radiusBehavior.radius;
    }

    

    void Start()
    {
        Secure_SO();

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;


        _SphereFlockSelection = GameObject.FindGameObjectWithTag("SphereIndication");
        
        Invoke(nameof(ChooseColorFlock), 0.01f);


        switch (currentFlock)
        {
            case FlockType.Aerien:
                CreateAerienFlock();
                break;
            case FlockType.Terrestre:
                CreateTerrestreFlock();
                break;
            case FlockType.SousTerrain:
                break;
        }

        
    }

    #region CreateTypeOfFlock

    void CreateAerienFlock()
    {
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity + centerRadius, //Permet d'instantié object dans une range Spherique
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), //Permet d'instantié object dans toute les directions
                FlockManager.instance.transform
            );
            newAgent.name = "Flock Aerien "+ (FlockManager.instance.countLocation) + " Agent " + i;
            if (isUnitEnnemy)
            {
                newAgent.isEnnemy = true;
            }
            agents.Add(newAgent);
        }
        FlockManager.instance.countLocation++;
        if (isUnitEnnemy)
        {
            FlockManager.instance.countLocationEnnemy++;
        }
    }
    void CreateTerrestreFlock()
    {
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity + centerRadius, //Permet d'instantié object dans une range Spherique
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), //Permet d'instantié object dans toute les directions
                FlockManager.instance.transform
            );
            newAgent.name = "Flock Terrestre "+ (FlockManager.instance.countLocation) + " Agent " + i;
            if (isUnitEnnemy)
            {
                newAgent.isEnnemy = true;
            }
            agents.Add(newAgent);
        }
        FlockManager.instance.countLocation++;
        if (isUnitEnnemy)
        {
            FlockManager.instance.countLocationEnnemy++;
        }
    }


    #endregion
    
    void ChooseColorFlock()
    {
        /*
        Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        foreach (FlockAgent agent in agents)
        {
            agent.GetComponent<Renderer>().material.color = color;
        }
        //m_FlockSelection.GetComponentInChildren<Renderer>().material.color = colorSelection;
        
        
        colorSelection = color;
        colorSelection.a = 0.05f;
        gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;*/

        if (isUnitEnnemy)
        {
            Color color = Color.red;
            foreach (FlockAgent agent in agents)
            {
                agent.GetComponent<Renderer>().material.color = color;
            }
            colorSelection = color;
            colorSelection.a = 0.05f;
            gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;
        }
        else
        {
            Color color = Color.cyan;
            foreach (FlockAgent agent in agents)
            {
                agent.GetComponent<Renderer>().material.color = color;
            }
            colorSelection = color;
            colorSelection.a = 0.05f;
            gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;
        }
        
    }
    
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(1,0.92f,.016f,.5f);
        Gizmos.DrawSphere(centerRadius, radiusBehavior.radius);
    }

    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //agent.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            if (currentFlock == FlockType.Aerien)
            {
                Vector3 move = behavior.CalculateMove(agent, context, this);
                
                Vector3 partialMove = StayInRadius(agent);
                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude < 0.01f * 0.01f)
                    {
                        partialMove.Normalize();
                        partialMove *= 0.01f;
                    }

                    move += partialMove;
                }
            
            
                move *= driveFactor;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agent.Move(move);
            }
            else
            {
                Vector3 move = behavior.CalculateMove(agent, context, this);
                move.y = 0;
                
                Vector3 partialMove = StayInRadius(agent);
                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude < 0.01f * 0.01f)
                    {
                        partialMove.Normalize();
                        partialMove *= 0.01f;
                    }

                    move += partialMove;
                }
            
            
                move *= driveFactor;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agent.Move(move);
            }
            
            Combat();

        }
    }


    void EngagementAuto()
    {
        foreach (var agent in agents)
        {
            agent.canEngageAuto = canEngageAuto;
            agent.engagementDistance = engagementDistance;
            if (agent.isAttacking)
            {
                isCombat = true;
            }
            else
            {
                isCombat = false;
            }
        }
    }


    public void Combat()
    {
        EngagementAuto();
        
        //Shoot();
        if (isCombat)
        {
            //isUnitShooting = true;
        }
        else
        {
            //isUnitShooting = false;
            
        }
        
    }

    IEnumerator CombatHIHI()
    {
        yield return new WaitForSeconds(0.1f);
        isCombat = false;
    }


    private void Shoot()
    {
        if (isUnitShooting) // All Unit will shoot
        {
            foreach (FlockAgent agent in agents)
            {
                agent.isAttacking = true;
            }
        }
        else
        {
            foreach (FlockAgent agent in agents)
            {
                agent.isAttacking = false;
            }
        }
    }
    
    
    public Vector3 StayInRadius(FlockAgent agent)
    {
        Vector3 centerOffset =  centerRadius - agent.transform.position; //FlockManager.instance.WhereToSpawns[FlockManager.instance.countLocation]
        float t = centerOffset.magnitude / radius;
        
        if (t < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius); // récupère les flock dans la range
        
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
