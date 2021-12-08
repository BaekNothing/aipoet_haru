using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


using UnityEngine.Networking;

public class GetGpt : MonoBehaviour
{
    public string temp;
    public Text intext;
    public Text outext;
    int flag = 0;

    public void uploads()
    {
        if (flag == 0)
        {
            flag = 1;
            StartCoroutine(GetText());
        }
    }

    IEnumerator GetText()
    {
        string reqstr = "http://211.58.81.52:8080/unity/gpt_request?req=" + intext.text;
        UnityWebRequest www = UnityWebRequest.Get(reqstr);
        yield return www.SendWebRequest();

        if (www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text == "Too many requests, please try again later.")
                intext.text = www.downloadHandler.text;
            else
                outext.text = www.downloadHandler.text;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            Debug.Log(results);
        }
        flag = 0;
    }
}
