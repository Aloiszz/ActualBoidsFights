using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockManager : MonoBehaviour
{
    [BoxGroup("Flock Creation")] [Header("General Settings Flocks")]
    // public FlockType currentFlock;
    public List<FlockType> currentFlock;

    [BoxGroup("Flock Creation")]
    public GameObject flock;
    
    [Space]
    [BoxGroup("Flock Creation")]
    public int howManyAlliesFlocks;
    [BoxGroup("Flock Creation")]
    public int howManyEnnemiFlocks;
    
    private Vector3 whereToSpawn = new Vector3(-50,10,-40);
    [BoxGroup("Flock Creation")]
    public List<Vector3> WhereToSpawns = new List<Vector3>() ;
    [HideInInspector]public int count;
    [HideInInspector]public int countLocation = 0;
    
    
    private Vector3 whereToSpawnEnnemis = new Vector3(-50,0,80);
    [BoxGroup("Flock Creation")]
    public List<Vector3> WhereToSpawnsEnnemis = new List<Vector3>() ;
    [HideInInspector]public int countEnnemy = 0;
    [BoxGroup("Flock Creation")]
    public int countLocationEnnemy = 0;
    
    

    public static FlockManager instance;

    [BoxGroup("Flock Creation")]
    [Header("Specific Parameters")] 
    public float flockSpeedDeplacement;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (count = 0; count < howManyAlliesFlocks; count++)
        {
            WhereToSpawns.Add(whereToSpawn);
            whereToSpawn += new Vector3(100, transform.position.y, 0);
            //countLocation++;
            GameObject flockManager = Instantiate(flock, WhereToSpawns[count], quaternion.identity, transform);
            flockManager.GetComponent<Flock>().currentFlock = currentFlock[0];
            flockManager.name = "Flock " + (count);
        } 
        
        for (countEnnemy = 0; countEnnemy < howManyEnnemiFlocks; countEnnemy++)
        {
            WhereToSpawnsEnnemis.Add(whereToSpawnEnnemis);
            whereToSpawnEnnemis += new Vector3(100, 0, 0);
            //countLocation++;
            GameObject flockManager = Instantiate(flock, WhereToSpawnsEnnemis[countEnnemy], quaternion.identity, transform);
            flockManager.name = "Flock Ennemy " + (countEnnemy);
            flockManager.GetComponent<Flock>().currentFlock = currentFlock[1];
            flockManager.GetComponent<Flock>().isUnitEnnemy = true;
        }
    }

    void Update()
    {
        
    }


    [Button()]
    void IntialiseFlock()
    {
        
    }
}

public enum FlockType
{
    Aerien,
    Terrestre,
    SousTerrain
}


