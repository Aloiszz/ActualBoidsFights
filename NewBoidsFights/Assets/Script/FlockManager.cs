using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockManager : MonoBehaviour
{
    [BoxGroup("General Settings Flocks")] 
    public bool isInGame;
    [BoxGroup("General Settings Flocks")]
    public GameObject flock;
    
    [Space]
    
    [BoxGroup("General Settings Flocks")]
    [Header("General Settings Flocks")]
    // public FlockType currentFlock;
    public int howManyAlliesFlocks;
    int indexAllies;
    [HideInInspector] public int howManyAirUnit;  
    [HideInInspector] public int howManyGroundUnit; 
    [BoxGroup("General Settings Flocks")]
    public List<FlockType> currentFlock;
    [BoxGroup("General Settings Flocks")]
    public FlockColor currentFlockColor;

    [Space]
    [BoxGroup("General Settings Flocks")]
    public int howManyEnnemiFlocks;
    private int indexEnnemis;
    [HideInInspector] public int howManyAirEnnemiesUnit;  
    [HideInInspector] public int howManyGroundEnnemiesUnit; 
    [BoxGroup("General Settings Flocks")]
    public List<FlockType> currentFlockEnnemi;
    [BoxGroup("General Settings Flocks")]
    public FlockColor currentFlockEnnemiColor;
    

    [HideInInspector] public static float heightOfFlocks = 15;
    private Vector3 whereToSpawn = new Vector3(-50,heightOfFlocks,-40);
    [BoxGroup("Flock Creation")]
    [HideInInspector]public List<Vector3> WhereToSpawns = new List<Vector3>() ;
    [HideInInspector]public int count;
    [HideInInspector]public int countLocation = 0;
    
    
    private Vector3 whereToSpawnEnnemis = new Vector3(-50,heightOfFlocks,80);
    [BoxGroup("Flock Creation")]
    [HideInInspector]public List<Vector3> WhereToSpawnsEnnemis = new List<Vector3>() ;
    [HideInInspector]public int countEnnemy = 0;
    [BoxGroup("Flock Creation")]
    [HideInInspector]public int countLocationEnnemy = 0;
    
    

    public static FlockManager instance;

    [BoxGroup("Flock Creation")]
    [Header("Specific Parameters")] 
    [HideInInspector]public float flockSpeedDeplacement;
    private void Awake()
    {
        instance = this;

        if (isInGame)
        {
            GetMenuValues(); // récup Info des menu de selection;
        }
    }
    void Start()
    {
        /*if (currentFlock.Count == 0)
        {
            for (int j = 0; j < howManyAlliesFlocks; j++)
            {
                currentFlock.Add(FlockType.Aerien);
            }
        }
        else
        {
            howManyAlliesFlocks = currentFlock.Count;
        }

        if (currentFlockEnnemi.Count == 0)
        {
            for (int j = 0; j < howManyEnnemiFlocks; j++)
            {
                currentFlockEnnemi.Add(FlockType.Aerien);
            }
        }
        else
        {
            howManyEnnemiFlocks = currentFlockEnnemi.Count;
        }*/


        if (isInGame)
        {
            if (howManyAlliesFlocks == 0 && currentFlock.Count == 0)
            {
                howManyAlliesFlocks++;
                currentFlock.Add(FlockType.Aerien);
            }

            if (howManyEnnemiFlocks == 0&& currentFlockEnnemi.Count == 0)
            {
                howManyEnnemiFlocks++;
                currentFlockEnnemi.Add(FlockType.Aerien);
            }
        }
        
        
        
        InstantiateAlliesFlock();
        InstantiateEnnemiesFlock();
        
        //DontDestroyOnLoad(this);
    }
    
    
    void GetMenuValues()
    {
        howManyAlliesFlocks = Menu.instance.howManyAirUnit + Menu.instance.howManyGroundUnit;
        for (int i = 0; i < Menu.instance.howManyAirUnit; i++)
        {
            AjouterAllierAerien();
        }
        for (int i = 0; i < Menu.instance.howManyGroundUnit; i++)
        {
            AjouterAllierTerrestre();
        }

        howManyEnnemiFlocks = Menu.instance.howManyAirEnnemiesUnit + Menu.instance.howManyGroundEnnemiesUnit;
        for (int i = 0; i < Menu.instance.howManyAirEnnemiesUnit; i++)
        {
            AjouterEnnemieAerien();
        }
        for (int i = 0; i < Menu.instance.howManyGroundEnnemiesUnit; i++)
        {
            AjouterEnnemieTerrestre();
        }
    }

    void InstantiateAlliesFlock()
    {
        for (count = 0; count < howManyAlliesFlocks; count++)
        {
            WhereToSpawns.Add(whereToSpawn);
            whereToSpawn += new Vector3(50, transform.position.y, 0);
            //countLocation++;
            GameObject flockManager = Instantiate(flock, WhereToSpawns[count], quaternion.identity, transform);
            flockManager.GetComponent<Flock>().currentFlock = currentFlock[indexAllies];
            indexAllies++;
            flockManager.name = "Flock " + (count);
            flockManager.GetComponent<Flock>().currentFlockColor = currentFlockColor;
        }
    }

    void InstantiateEnnemiesFlock()
    {
        for (countEnnemy = 0; countEnnemy < howManyEnnemiFlocks; countEnnemy++)
        {
            WhereToSpawnsEnnemis.Add(whereToSpawnEnnemis);
            whereToSpawnEnnemis += new Vector3(50, transform.position.y, 0);
            //countLocation++;
            GameObject flockManager = Instantiate(flock, WhereToSpawnsEnnemis[countEnnemy], quaternion.identity, transform);
            flockManager.name = "Flock Ennemy " + (countEnnemy);
            flockManager.GetComponent<Flock>().currentFlock = currentFlockEnnemi[indexEnnemis];
            indexEnnemis++;
            flockManager.GetComponent<Flock>().isUnitEnnemy = true;
            flockManager.GetComponent<Flock>().currentFlockColor = currentFlockEnnemiColor;
        }
    }

    void Update()
    {
        
    }
    
    [Button("------------------")]
    void Separation2()
    {
        
    }
    
    [Button()]
    public void AjouterAllierAerien()
    {
        currentFlock.Add(FlockType.Aerien);
        howManyAirUnit++;
        //InstantiateAlliesFlock();
    }
    
    [Button()]
    public void AjouterAllierTerrestre()
    {
        currentFlock.Add(FlockType.Terrestre);
        howManyGroundUnit++;
    }
    [Button()]
    public void RemoveAllAllies()
    {
        howManyAirUnit = 0;
        howManyGroundUnit = 0;
        currentFlock.Clear();
    }


    [Button("------------------")]
    void Separation()
    {
        
    }
    
    [Button()]
    public void AjouterEnnemieAerien()
    {
        currentFlockEnnemi.Add(FlockType.Aerien);
        howManyAirEnnemiesUnit++;
    }
    
    [Button()]
    public void AjouterEnnemieTerrestre()
    {
        howManyGroundEnnemiesUnit++;
        currentFlockEnnemi.Add(FlockType.Terrestre);
    }
    [Button()]
    public void RemoveAllEnnemis()
    {
        howManyAirEnnemiesUnit = 0;
        howManyGroundEnnemiesUnit = 0;
        currentFlockEnnemi.Clear();
    }
    
    [Button("------------------")]
    void Separation1()
    {
        
    }
    
    [Button()]
    public void RemoveAllUnit()
    {
        currentFlock.Clear();
        currentFlockEnnemi.Clear();
    }
    
    
    
}

public enum FlockType
{
    Aerien,
    Terrestre,
    SousTerrain
}

public enum FlockColor
{
    Blue,
    Red,
    Green,
    Yellow
}


