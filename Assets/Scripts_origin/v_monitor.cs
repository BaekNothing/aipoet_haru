using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class v_monitor : MonoBehaviour
{
    //[SerializeField]
    public GameObject viewer;
    public GameObject viewers;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("checkWeather");
    }

    public string tempString = "";

    IEnumerator checkWeather()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://211.58.81.52:8080/unity/wather_request");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.isHttpError)
        {
            tempString = "Error While Sending: " + www.error;
        }
        else
        {
            // Show results as text
            tempString = www.downloadHandler.text;
            Debug.Log(tempString);
        }

        yield return null;

        int counter = int.Parse(tempString.Substring(0, 3)) / 20;

        for (int i = 0; i < 360; i += counter) //count는 오브젝트 생성 갯수
        {
            Vector3 pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), 0, Mathf.Sin(i * Mathf.Deg2Rad));
            GameObject Some = Instantiate(viewer);
            Some.transform.right = pos;
            Some.transform.position = pos * Random.Range(30f, 50f);
            Some.SetActive(true);
            Some.transform.SetParent(viewers.transform);

            Some.SendMessage("targetSet", c_monitor.creators[(int)Random.Range(0, 8)], 0);
            Some.SendMessage("yposSet", 0, 0);
            Some.SendMessage("distanceSet", 20f, 0);
        }

        yield return null;
    }
}
