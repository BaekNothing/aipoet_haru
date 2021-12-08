using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01_Production_A_3DObjectButtonColtroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print(gameObject.name);
    }

    /*
     Ray ray;
     RaycastHit hit;
     
     void Update()
     {
         ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         if(Physics.Raycast(ray, out hit))
         {
             if(Input.GetMouseButtonDown(0))
                 print(hit.collider.name);
         }
     }
    */
}
