using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crit : MonoBehaviour
{
    public int critFlag = 0;
    public GameObject prev = null;
    public Material mat_basic;
    public Material mat_outlined;
    public AudioSource[] bump = new AudioSource[3];


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name +  "Entered!");
        if (critFlag == 0 && "createrbody" == collision.gameObject.name) // && prev != collision.gameObject)
        {
            critFlag = 1;
            StartCoroutine("Delete", collision.gameObject);
            bump[Random.Range(0, 3)].Play();
            prev = collision.gameObject;
        }
    }

    IEnumerator Delete(GameObject gameObject)
    {
        if(Random.Range(0,100) > 85)
            gameObject.SendMessage("delete");
        this.SendMessage("distanceSet", 80f, 0);
        this.GetComponent<Renderer>().material = mat_outlined;
        yield return new WaitForSeconds(15);
        critFlag = 0;
        this.SendMessage("distanceSet", 10f, 0);
        this.GetComponent<Renderer>().material = mat_basic;
    }
}
