using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class c_monitor : MonoBehaviour
{
    public static int colliderflag = 0;
    public static string monitorText = "";
    public static int[] creatorsCounter;
    public string[] creatorText;

    
    int c_mount = 8;
    public GameObject[] uiText;
    public GameObject creatorParents;
    public GameObject creator;
    public static GameObject[] creators;
    public int nowNum = 0;
    public string prevText = "";

    public GameObject canvas;
    public GameObject debugPanel;
    public GameObject water;
    public int randomNum;

    private void Awake()
    {
        creators = new GameObject[c_mount];
        creatorsCounter = new int[c_mount];
        creatorText = new string[c_mount];
        uiText = new GameObject[c_mount];
        randomNum = Random.Range(1000000, 9999999);

        for (int i = 0; i < c_mount; i++)
        {
            creatorsCounter[i] = 0;
            creatorText[i] = "";
            creators[i] = Instantiate(creator, new Vector3(transform.position.x, transform.position.y, +24 + transform.position.z - 6f * i), Quaternion.identity);
            creators[i].SetActive(true);
            creators[i].transform.SetParent(creatorParents.transform);
            uiText[i] = canvas.transform.GetChild(i).gameObject;

            if (i > 0)
            {
                creators[i].transform.GetChild(0).SendMessage("targetSet", creators[i - 1], 0);
                creators[i].transform.GetChild(0).SendMessage("distanceSet", 12f, 0);
            }
            //else
              //  creators[i].transform.GetChild(0).SendMessage("targetSet", creatorParents.transform.GetChild(0), 0);
        }
        
        if (!debugPanel.activeSelf)
        {
            StartCoroutine("actualPlay");
        }
    }

    private void Update()
    {
        if(monitorText != prevText)
        {
            for(int i = 0; i < c_mount; i++)
            {
                if (creatorText[i] == "")
                {
                    uiText[i].GetComponent<Text>().text = monitorText;
                    creatorText[i] = monitorText;
                    StartCoroutine("SendText");
                    break;
                }
            }
        }
        prevText = monitorText;
    }

    public string tempString;
    IEnumerator SendText()
    {
        tempString = "";
        foreach(string s in creatorText)
        {
            if (s != "" && s != "Too many requests, please try again later.")
                tempString += s + "\n";
        }

        string reqstr = "http://211.58.81.52:8080/unity/text?req=" + tempString + "&num=" + randomNum.ToString();
        UnityWebRequest www = UnityWebRequest.Get(reqstr);
        
        yield return www.SendWebRequest();
    }

    IEnumerator actualPlay()
    {
        yield return new WaitForSecondsRealtime(15);
        makePoem();
        while (true)
        {
            yield return new WaitForSecondsRealtime(80);
            if (creatorsCounter[c_mount - 1] == 1)
            {
                Time.timeScale = 0;
                water.SetActive(false);
            }
            makeNext();
        }
    }

    public void makePoem()
    {
        if (nowNum + 1 == c_mount)
            Time.timeScale = 0;

        for (int i = 0; i < c_mount; i++)
        {
            if (creatorsCounter[i] == 0)
            {
                creators[i].transform.GetChild(0).SendMessage("generatePoem");
                creatorsCounter[i] = 1;
                break;
            }
            nowNum = i;
        }
    }



    public void makeNext()
    {
        if (creatorsCounter[0] != 0)
        {
            for (int i = 1; i < c_mount; i++)
            {
                if (creatorsCounter[i] == 0)
                {
                    creators[i].transform.GetChild(0).SendMessage("generateNext", monitorText, 0);
                    creatorsCounter[i] = 1;
                    break;
                }
            }
        }
    }
}
