using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawRay : MonoBehaviour
{
    private Camera cam;

    public GameObject flock;
    public GameObject flock2;
    public GameObject[] flocks;

    [SerializeField] private TextMeshProUGUI  _UnitSelected;
    [SerializeField] private TextMeshProUGUI  _UnitType;
    
    public TextMeshProUGUI fpsText;
    private float deltaTime;
    
    public float slowMotionTimescale;
    private float startTimescale;
    private float startFixedDeltaTime;

    [SerializeField] private GameObject flockDetails;
    [SerializeField] private Toggle toggleEngagementAuto;

    private RaycastHit hit;
    private Ray ray;

    private bool isFlockEmpty;
    
    void Start()
    {
        cam = Camera.main;
        Invoke(nameof(FindFlock), 0.01f);
        
        startTimescale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        
        flockDetails.SetActive(false);
        toggleEngagementAuto.isOn = true;
    }

    void FindFlock()
    {
        flocks = GameObject.FindGameObjectsWithTag("Flock");
    }

    private void FixedUpdate()
    {
        FlockDetails();
    }

    // Update is called once per frame
    void Update()
    {
        ShowFps(); // permet de voir le frame rate 
        SlowMotion();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DrawRayScreentToWorldPoint();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DeSelectUnit();
        }

        if (flock == null)
        {
            _UnitSelected.text = "Unit Selected : ";
            _UnitType.text = "Unit Type : ";
        }
        else
        {
            _UnitSelected.text = "Unit Selected : " + flock.name + " is ennemy " + flock.GetComponent<Flock>().isUnitEnnemy;
            _UnitType.text = "Unit Type : " + flock.GetComponent<Flock>().currentFlock.ToString();
        }
    }
    
    void DrawRayScreentToWorldPoint() // Deplacement des troupes vers un points sur le terrain
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 10000))
            {
                
                if (flock == null)
                {
                    WhenNoUnitIsSelected();
                }
                else
                {
                    if (hit.collider.CompareTag("Terrain")) // Unit Selected and moving toward position
                    {
                        Debug.DrawRay(ray.origin - new Vector3(0,-1,0) , ray.direction * 10000, Color.green, 1, false);
                        flock.transform.gameObject.GetComponent<Flock>().centerRadius = new Vector3(hit.point.x,  flock.transform.gameObject.GetComponent<Flock>().heightOfFlocks, hit.point.z); // Radius limite de la flock
                        flock.transform.transform.position = flock.transform.gameObject.GetComponent<Flock>().centerRadius;
                        //DeSelectUnit();
                    }
                    WhenAUnitIsSelected();
                }

            }
        }
    }

    void WhenNoUnitIsSelected()
    {
        if (hit.transform.gameObject.GetComponent<Flock>().isUnitSelected)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) // Unit UnSelected
            {
                hit.transform.gameObject.GetComponent<Flock>().isUnitSelected = false;
                flock = null;
                isFlockEmpty = false;
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                
                hit.transform.gameObject.GetComponent<Flock>().colorSelection.a = 0.05f;
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
            }
        }
        else
        {
            if ( hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) // Unit Selected
            {
                hit.transform.gameObject.GetComponent<Flock>().isUnitSelected = true;
                flock = hit.transform.gameObject;
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                
                hit.transform.gameObject.GetComponent<Flock>().colorSelection.a = 0.2f;
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
            }
            else // No Unit Selected
            {
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.red, 1, false);
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
            }
            
        }
    }

    void WhenAUnitIsSelected()
    {
        //isFlockEmpty = true;
        
        if (hit.transform.gameObject.GetComponent<Flock>().isUnitSelected)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) // Unit UnSelected
            {
                hit.transform.gameObject.GetComponent<Flock>().isUnitSelected = false;
                flock = null;
                flock2 = null;
                isFlockEmpty = false;
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                
                hit.transform.gameObject.GetComponent<Flock>().colorSelection.a = 0.05f;
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
            }
        }
        else
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) // Unit Selected
            {
                hit.transform.gameObject.GetComponent<Flock>().isUnitSelected = true;
                flock2 = hit.transform.gameObject;
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.blue, 1, false);
                
                hit.transform.gameObject.GetComponent<Flock>().colorSelection.a = 0.2f;
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
                
                
                flock.transform.gameObject.GetComponent<Flock>().centerRadius = new Vector3(hit.point.x, flock.transform.gameObject.GetComponent<Flock>().heightOfFlocks, hit.point.z); // Radius limite de la flock
                flock.transform.transform.position = flock.transform.gameObject.GetComponent<Flock>().centerRadius;
                
                flock2.transform.gameObject.GetComponent<Flock>().centerRadius = new Vector3(hit.point.x, flock2.transform.gameObject.GetComponent<Flock>().heightOfFlocks , hit.point.z); // Radius limite de la flock
                flock2.transform.transform.position = flock.transform.gameObject.GetComponent<Flock>().centerRadius;
            }
            else // No Unit Selected
            {
                Debug.DrawRay(ray.origin  - new Vector3(0,-1,0), ray.direction * 10000, Color.red, 1, false);
                hit.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = hit.transform.gameObject.GetComponent<Flock>().colorSelection;
            }
            
        }
    }

    

    void DeSelectUnit() // déselectionne toute les unités.
    {
        foreach (var i in flocks)
        {
            isFlockEmpty = false;
            flock = null;
            flock2 = null;
            i.GetComponent<Flock>().isUnitSelected = false;
            i.transform.gameObject.GetComponent<Flock>().colorSelection.a = 0.05f;
            i.transform.transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = i.transform.gameObject.GetComponent<Flock>().colorSelection;
        }
    }
    
    void OnGUI() // Affichage information d'écran
    {
        Vector3 point = new Vector3();
        Event   currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y ; 
        
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
        
    }

    void ShowFps()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "Frame Rate : " + Mathf.Ceil (fps).ToString ();

        if (fps > 60)
        {
            fpsText.color = Color.green;
        }
        else if (fps < 60 || fps > 30)
        {
            fpsText.color = Color.yellow;
        } 
        
        if (fps < 30)
        {
            fpsText.color = Color.red;
        }
    }


    void SlowMotion()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = slowMotionTimescale;
            Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimescale;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = startTimescale;
            Time.fixedDeltaTime = startFixedDeltaTime;
        }
    }
    
    
    bool eventTrigger;
    void FlockDetails()
    {
        
        if (flock != null)
        {
            flockDetails.SetActive(true);
            toggleEngagementAuto.isOn = flock.transform.gameObject.GetComponent<Flock>().canEngageAuto;
            
        }
        else
        {
            toggleEngagementAuto.isOn = true;
            flockDetails.SetActive(false);
        }
    }

    public void EngagementAuto()
    {
        if (flock.transform.gameObject.GetComponent<Flock>().canEngageAuto)
        {
            flock.transform.gameObject.GetComponent<Flock>().canEngageAuto = false;
            toggleEngagementAuto.isOn = false;
        }
        else
        {
            flock.transform.gameObject.GetComponent<Flock>().canEngageAuto = true;
            toggleEngagementAuto.isOn = true;
        }
    }
}
