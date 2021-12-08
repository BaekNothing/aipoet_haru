using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic_move : MonoBehaviour
{
    public GameObject target = null;
    public float ypos = 0;
    public float distance = 0.2f;

    void targetSet(GameObject targetName)
    {
        target = targetName.transform.GetChild(0).gameObject ;
    }
    void distanceSet(float _distance)
    {
        distance = _distance;
    }


    void yposSet(float _ypos)
    {
        ypos = _ypos;
    }


    void Update()
    {
        //shaker();
        chaser();
        //forceDown();
    }


    float force = 0.05f;
    void shaker()
    {
        this.transform.position = this.transform.position + new Vector3(Random.Range(-force, force), 0, Random.Range(-force, force));
    }

    void chaser()
    {
        if (target != null) 
        {
            /*
            this.transform.position = Vector3.Lerp(this.transform.position
                - new Vector3(0, this.transform.position.y, 0), target.transform.position
                - new Vector3(0, target.transform.position.y, 0), 0.001f);
            */
            //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.001f);
            if (Vector3.Distance(this.transform.position, target.transform.position) >= distance)
                this.transform.position = Vector3.Lerp(new Vector3(this.transform.position.x, ypos, this.transform.position.z),
                    new Vector3(target.transform.position.x, ypos, target.transform.position.z), 0.001f);
            else
                this.transform.position = Vector3.Lerp(new Vector3(this.transform.position.x, ypos, this.transform.position.z),
                    new Vector3(this.transform.position.x + (this.transform.position.x - target.transform.position.x),
                        ypos,
                        this.transform.position.z + (this.transform.position.z - target.transform.position.z)), 0.001f);
        }
    }

    void forceDown()
    {
        this.transform.position = this.transform.position - new Vector3(0, this.transform.position.y, 0);
    }
}
