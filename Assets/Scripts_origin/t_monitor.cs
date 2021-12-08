using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_monitor : MonoBehaviour
{
    //[SerializeField]
    public GameObject crit;
    public GameObject crits;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 360; i += 40) //count는 오브젝트 생성 갯수
        {
            Vector3 pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), 0, Mathf.Sin(i * Mathf.Deg2Rad));
            pos -= new Vector3(0, 10f, 0);
            GameObject Some = Instantiate(crit);
            Some.transform.up = pos;
            Some.transform.position = pos * Random.Range(60f, 80f);
            Some.transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
            Some.SetActive(true);
            Some.transform.SetParent(crits.transform);

            Some.SendMessage("targetSet", c_monitor.creators[(int)Random.Range(0, 8)], 0);
            Some.SendMessage("yposSet", 0f, 0);
            Some.SendMessage("distanceSet", 10f, 0);
        }
    }
}
