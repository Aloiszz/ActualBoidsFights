using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRay : MonoBehaviour
{
    private Camera cam;

    public GameObject flock;

    public GameObject[] flocks;
    void Start()
    {
        cam = Camera.main;
        Invoke(nameof(FindFlock), 0.01f);
    }

    void FindFlock()
    {
        flocks = GameObject.FindGameObjectsWithTag("Flock");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DrawRayScreentToWorldPoint();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DeSelectUnit();
        }
    }
    
    void DrawRayScreentToWorldPoint() // Deplacement des troupes vers un points sur le terrain
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                if (hit.collider.CompareTag("Terrain")) // Unit Selected and moving toward position
                {
                    Debug.DrawRay(ray.origin - new Vector3(0,-1,0) , ray.direction * 10000, Color.green, 1, false);
                    flock.transform.gameObject.GetComponent<Flock>().centerRadius = new Vector3(hit.point.x, 0, hit.point.z); // Radius limite de la flock
                    flock.transform.transform.position = flock.transform.gameObject.GetComponent<Flock>().centerRadius;
                }
                
                if (hit.transform.gameObject.GetComponent<Flock>().isUnitSelected)
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Flock")) // Unit UnSelected
                    {
                        hit.transform.gameObject.GetComponent<Flock>().isUnitSelected = false;
                        flock = null;
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
        }
    }

    void DeSelectUnit() // déselectionne toute les unités.
    {
        foreach (var i in flocks)
        {
            flock = null;
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
}
