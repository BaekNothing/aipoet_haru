using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq; //https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347
using TMPro;

public class creater : MonoBehaviour
{
    public TextMesh textMesh;
    public GameObject textMeshContainor;

    public string tempString = "HelloWorld!";
    public GameObject SelectedImage;
    public GameObject creatorbody;
    public Renderer g_renderer;
    int colliderflag = 0;
    public Material mat_basic;
    public Material mat_progress;
    public Material mat_outlined;


    private void Update()
    {
        //Right.eulerAngles = new Vector3(0, 0, gameObject.transform.rotation.y);

        creatorbody.transform.rotation = Quaternion.Euler(creatorbody.transform.rotation.x, 0, creatorbody.transform.rotation.z);
        textMeshContainor.transform.position = creatorbody.transform.position;
        textMeshContainor.transform.rotation = creatorbody.transform.rotation;

        creatorbody.transform.localScale = new Vector3(tempString.Length * 1.15f, 1.1f, 5f);
        
        textMesh.text = tempString;
    }

    public void delete()
    {
        if(tempString.Length > 1)
            tempString = tempString.Substring(0, tempString.Length - 1);
        else
            this.GetComponent<Renderer>().material = mat_outlined;
    }


    private void generatePoem()
    {
        if (colliderflag == 0)
        {
            colliderflag = 1;
            StartCoroutine(sequential());
        }
    }

    public void generateNext(string monitorText)
    {
        if (colliderflag == 0)
        {
            colliderflag = 1;
            StartCoroutine(gptOnlySequential(monitorText));
        }
    }     

    public void Start()
    {
        tempString = "HelloWorld!";
        g_renderer = creatorbody.GetComponent<Renderer>();
        //StartCoroutine(sequential());
    }

    IEnumerator gptOnlySequential(string monitorText)
    {
        Debug.Log(this.name + "GPT only option STart");
        while (!(c_monitor.colliderflag == 0))
            yield return new WaitForSeconds(10);

        tempString = monitorText;
        c_monitor.colliderflag = 1;
        this.GetComponent<Renderer>().material = mat_progress;
        yield return StartCoroutine(GetGPT());

        //g_renderer.material.color = new Color(255, 255, 255);

        this.GetComponent<Renderer>().material = mat_outlined;
        yield return new WaitForSecondsRealtime(60);
        this.GetComponent<Renderer>().material = mat_basic;
        c_monitor.colliderflag = 0;
        colliderflag = 0;
    }

    IEnumerator sequential()
    {
        Debug.Log(this.name + "Coroutine Start");
        while (!(c_monitor.colliderflag == 0))
            yield return new WaitForSeconds(10);

        c_monitor.colliderflag = 1;
        this.GetComponent<Renderer>().material = mat_progress;
        
        Debug.Log("step1");
        yield return StartCoroutine(GetText());
        Debug.Log("step2");
        yield return StartCoroutine(GetTexture());
        Debug.Log("step3");
        yield return StartCoroutine(AzureAPI(myTexture));
        Debug.Log("step4");
        yield return StartCoroutine(GetTranslate());
        Debug.Log("step5 temptext is : " + tempString);
        yield return StartCoroutine(GetGPT());
        Debug.Log("finish temptext is : " + tempString);

        this.GetComponent<Renderer>().material = mat_outlined;
        yield return new WaitForSecondsRealtime(60);
        this.GetComponent<Renderer>().material = mat_basic;
        c_monitor.colliderflag = 0;
        c_monitor.monitorText = tempString;
        colliderflag = 0;
    }


    public void uploads()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://211.58.81.52:8080/img");
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

        yield return (1);
    }

    public Renderer image;

    
    public void getTexture()
    {
        if (tempString != "")
            StartCoroutine(GetTexture());
    }

    public Texture2D myTexture;
    
    IEnumerator GetTexture()
    {
        //
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://211.58.81.52:8080/uploads/" + tempString);
        www.downloadHandler = new DownloadHandlerTexture();
        yield return www.SendWebRequest();
        if (www.isHttpError)
        {
            Debug.Log("Error While Sending: " + www.error);        }
        else
        {
            myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            SelectedImage.transform.GetComponent<RawImage>().texture = myTexture;
            image.material.mainTexture = myTexture;
        }
        yield return (1);
    }


    /// <summary>
    /// yolo
    /// </summary>

    public string baseEndpoint = "https://koreacentral.api.cognitive.microsoft.com/vision/v3.2/";
    public string key = "3acd5d77775e47048c9c6f98abf529ff";
    //private string imageName = "face.jpg";
    public string options = "describe"; //analyze, describe, detect, areaOfInterest
    private RawImage RawImage;

    public Test01_Production_A_JSON_Controller JSON;

    public void AzureCV()
    {
        print("try yolo");
        StartCoroutine(AzureAPI(myTexture));

    }

    IEnumerator AzureAPI(Texture2D Texture)
    {
        string endpoint = $"{baseEndpoint}"; //analyze, describe, detect, areaOfInterest
        endpoint += options;

        WWWForm webForm = new WWWForm(); //AddField() 미작동
        //https://docs.unity3d.com/ScriptReference/WWWForm.html

        //imageName = rawImage.GetComponent<RawImage>().texture.name;
        //string imagePath = Application.dataPath + "/" + imageFolder + "/" + imageName;
        byte[] imageBytes = Texture.EncodeToPNG();

        using (UnityWebRequest www = UnityWebRequest.Post(endpoint, webForm))
        {
            www.SetRequestHeader("Ocp-Apim-Subscription-Key", key);
            www.SetRequestHeader("Content-Type", "application/octet-stream");
            www.uploadHandler.contentType = "application/octet-stream";
            www.uploadHandler = new UploadHandlerRaw(imageBytes);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();
            string jsonResponse = www.downloadHandler.text;
            print("===========jsonResponse===========\n" + jsonResponse);
            string restext = "";
            //restext = jsonResponse;
            var jo = JObject.Parse(jsonResponse);
            var tags = jo["description"]["tags"];
            foreach (string tag in tags)
                restext += tag + " ";
            tags = jo["description"]["captions"];
            var caps = tags[0];
            restext += caps["text"] + " ";
            tempString = restext;
            print("CV text:" + restext);
        }
        yield return (1);
    }

    public void get_translate()
    {
        StartCoroutine(GetTranslate());
    }

    IEnumerator GetTranslate()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://211.58.81.52:8080/unity/translate?req=" + tempString);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.isHttpError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {

            // Show results as text
            tempString = www.downloadHandler.text;
            Debug.Log(tempString);
            int start = tempString.IndexOf("\"translatedText\":\"") + 18;
            int end = tempString.IndexOf("engineType") - 3;
            print("===========translateResponse===========\n");
            string resultext = "";
            for (int i = start; i < end; i++)
            {
                resultext += tempString[i];
            }
            tempString = resultext;
        }
        yield return (1);
    }

    int gptflag = 0;

    public void GPTuploads()
    {
        if (gptflag == 0)
        {
            gptflag = 1;
            StartCoroutine(GetGPT());
        }
    }

    IEnumerator GetGPT()
    {
        string reqstr = "http://211.58.81.52:8080/unity/gpt_request?req=" + tempString;
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
                tempString = www.downloadHandler.text;
            else
            {
                tempString = www.downloadHandler.text.Substring(8, www.downloadHandler.text.Length - 9);
                tempString = tempString.Replace("\n", " ");
                tempString += "\n";
            }
            // Or retrieve results as binary data
            Debug.Log(tempString);
        }
        gptflag = 0;
        yield return (1);
    }
}