using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaughtyAttributes;


public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator globalVolume;
    public GameObject main;
    public GameObject config;


    [Header("Config Allies")]
    public TextMeshProUGUI numberofUnit;
    public TextMeshProUGUI numberofAirUnit;
    public TextMeshProUGUI numberofGroundUnit;
    
    [Header("Config Ennemies")]
    public TextMeshProUGUI numberofUnitEnnemeis;
    public TextMeshProUGUI numberofAirennemeisUnit;
    public TextMeshProUGUI numberofGroundEnnemeisUnit;

    /*[Header("Config Ennemis")] 
    public Button AddAirUnit;
    public TextMeshProUGUI numberofAirUnit;
    [Space]
    public Button AddGroundUnit;
    public TextMeshProUGUI numberofGroundUnit;
    [Space]
    public Button RemoveAllAllies;*/

    public static Menu instance;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        main.SetActive(true);
        config.SetActive(false);
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        numberofUnit.text = "X " + (howManyAirUnit+howManyGroundUnit);
        numberofAirUnit.text = "X " + howManyAirUnit;
        numberofGroundUnit.text = "X " + howManyGroundUnit;
        
        numberofUnitEnnemeis.text = "X " + (howManyAirEnnemiesUnit+howManyGroundEnnemiesUnit);
        numberofAirennemeisUnit.text = "X " + howManyAirEnnemiesUnit;
        numberofGroundEnnemeisUnit.text = "X " + howManyGroundEnnemiesUnit;
    }


    public void DirectPlay()
    {
        globalVolume.SetTrigger("Depart");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }    

    public void Leave()
    {
        Application.Quit();
    }

    public void Config()
    {
        main.SetActive(false);
        config.SetActive(true);
    }






    public int howManyAirUnit;
    public int howManyGroundUnit;

    public int howManyAirEnnemiesUnit;
    public int howManyGroundEnnemiesUnit;


    #region NaughtyAttributes

    [Button("------------------")]
    void Separation2()
    {
        
    }
    
    [Button()]
    public void AjouterAllierAerien()
    {
        howManyAirUnit++;
        //InstantiateAlliesFlock();
    }
    
    [Button()]
    public void AjouterAllierTerrestre()
    {
        howManyGroundUnit++;
    }
    [Button()]
    public void RemoveAllAllies()
    {
        howManyAirUnit = 0;
        howManyGroundUnit = 0;
        
    }


    [Button("------------------")]
    void Separation()
    {
        
    }
    
    [Button()]
    public void AjouterEnnemieAerien()
    {
        howManyAirEnnemiesUnit++;
    }
    
    [Button()]
    public void AjouterEnnemieTerrestre()
    {
        howManyGroundEnnemiesUnit++;
    }
    [Button()]
    public void RemoveAllEnnemis()
    {
        howManyAirEnnemiesUnit = 0;
        howManyGroundEnnemiesUnit = 0;
    }

    #endregion
    
    
}
