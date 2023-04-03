using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab; // prefab du flock
    private List<FlockAgent> agents = new List<FlockAgent>(); // permet de stocker tout les agents
    public FLockBehavior behavior;
    public StayInRadiusBehavior radiusBehavior;

    [Range(10, 1500)] 
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

    [Header("Flock State")]
    public bool isCombat;
    public bool isMoving;
    public bool isWaiting;

    
    [Header("Information about Combat")] 
    public int engagementDistance = 50; // distance d'engagement requise pour attaquer une flock ennemy
    public bool canEngageAuto; // permet d'engager les flocks ennemy can entre dans la range
    
    
    [Header("Information about flock")]
    public bool isUnitSelected; // Savoir si unité selectioner
    public bool isUnitEnnemy; //Savoir si unité est Allié ou ennemi
    public bool isUnitShooting;
    
    private GameObject _SphereFlockSelection;
    private Renderer m_FlockSelection; // Material pour la selection de la flock
    [HideInInspector]public Color colorSelection;
    
    public Vector3 centerRadius; //récupération du centre Radius depuis SO RadiusBehavior
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
        
        if (isUnitEnnemy)
        {
            centerRadius = FlockManager.instance.WhereToSpawnsEnnemis[FlockManager.instance.countLocationEnnemy];
            radiusBehavior.center = new Vector3(0, 0, 50);
        }
        else
        {
            centerRadius = FlockManager.instance.WhereToSpawns[FlockManager.instance.countLocation];
            radiusBehavior.center = new Vector3(0, 0, 0);
        }
        
        radius = radiusBehavior.radius;
        
        //centerRadius = radiusBehavior.center;
    }

    

    void Start()
    {
        Secure_SO();

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;


        _SphereFlockSelection = GameObject.FindGameObjectWithTag("SphereIndication");
        
        Invoke(nameof(ChooseColorFlock), 0.01f);

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity + centerRadius, //Permet d'instantié object dans une range Spherique
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), //Permet d'instantié object dans toute les directions
                FlockManager.instance.transform
            );
            newAgent.name = "Flock "+ (FlockManager.instance.countLocation) + " Agent " + i;
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
        Gizmos.color = new Color(1,0.92f,.016f,1f);
        Gizmos.DrawSphere(centerRadius, radiusBehavior.radius);
    }


    void DrawRayScreentToWorldPoint() // Deplacement des troupes vers un points sur le terrain
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                if (isUnitSelected) // vérifier si l'unité est selectionne
                {
                    if (hit.collider.CompareTag("Terrain"))
                    {
                        Debug.Log("Hit Terrain!" + hit.point);
                        Debug.DrawRay(ray.origin - new Vector3(0,-1,0) , ray.direction * 10000, Color.green, 1, false);
                        centerRadius = new Vector3(hit.point.x, 0, hit.point.z); // Radius limite de la flock
                        
                        /*GetComponent<SphereCollider>().center = radiusBehavior.center;
                        _SphereFlockSelection.transform.position = radiusBehavior.center;*/
                        gameObject.transform.position = centerRadius;
                        
                    }
                    if (/*hit.collider.CompareTag("Flock")*/ hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock"))
                    {
                        isUnitSelected = false;
                        Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                        
                        colorSelection.a = 0.05f;
                        gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;
                    }
                }
                else
                {
                    if ( hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) //hit.collider.CompareTag("Flock")
                    {
                        isUnitSelected = true;
                        Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                        
                        colorSelection.a = 0.2f;
                        gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.red, 1, false);
                        gameObject.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = colorSelection;
                    }
                }
            }
        }
        
    }
    
    
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //agent.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

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

            Combat();

        }
        
        //DrawRayScreentToWorldPoint();
    }


    public void Combat()
    {
        Shoot();
        if (isCombat)
        {
            isUnitShooting = true;
        }
        else
        {
            isUnitShooting = false;
        }
        
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
