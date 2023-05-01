using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    
    void Start()
    {
        main.SetActive(true);
        config.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        numberofUnit.text = "X " + FlockManager.instance.currentFlock.Count;
        numberofAirUnit.text = "X " + FlockManager.instance.howManyAirUnit;
        numberofGroundUnit.text = "X " + FlockManager.instance.howManyGroundUnit;
        
        numberofUnitEnnemeis.text = "X " + FlockManager.instance.currentFlockEnnemi.Count;
        numberofAirennemeisUnit.text = "X " + FlockManager.instance.howManyAirEnnemiesUnit;
        numberofGroundEnnemeisUnit.text = "X " + FlockManager.instance.howManyGroundEnnemiesUnit;
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
}
