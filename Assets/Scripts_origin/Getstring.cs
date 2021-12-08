using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq; //https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347

public class Getstring : MonoBehaviour
{
    public string temp;
    public Text intext;
    public Text outext;
    public GameObject SelectedImage;


    public void uploads()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://211.58.81.52:8080/img");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        intext.text = "getTexted!";
        if (www.isHttpError)
        {
            intext.text = "Error While Sending: " + www.error;
        }
        else
        {
            // Show results as text
            outext.text = www.downloadHandler.text;
            temp = www.downloadHandler.text;
            Debug.Log(temp);
        }
        intext.text = "text Done!";
    }

    public Renderer image;

    public void getExist()
    {
        temp = "1633744170007.jpeg";
        StartCoroutine(GetTexture());
        temp = "";
        intext.text = "Exist done";
    }

    public void getTexture()
    {
        if (temp != "")
            StartCoroutine(GetTexture());
        else
            intext.text = "temp is null!";
    }

    public Texture2D myTexture;

    IEnumerator GetTexture()
    {
        //
        intext.text = "getTextured!";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://211.58.81.52:8080/uploads/" + temp);
        www.downloadHandler = new DownloadHandlerTexture();
        yield return www.SendWebRequest();
        if (www.isHttpError)
        {
            intext.text = "Error While Sending: " + www.error;
        }
        else
        {
            myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            SelectedImage.transform.GetComponent<RawImage>().texture = myTexture;
            image.material.mainTexture = myTexture;
        }

        intext.text = "texture Done!";
    }



    public void get_translate()
    {
        StartCoroutine(GetTranslate());
    }

    IEnumerator GetTranslate()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://211.58.81.52:8080/unity/translate?req=" + gpttext.text);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        intext.text = "getTranslated!";
        if (www.isHttpError)
        {
            intext.text = "Error While Sending: " + www.error;
        }
        else
        {
            
            // Show results as text
            gpttext.text = www.downloadHandler.text;
            temp = www.downloadHandler.text;
            Debug.Log(temp);
                        int start = temp.IndexOf("\"translatedText\":\"") + 18;
                        int end = temp.IndexOf("engineType") - 3;
                        print("===========translateResponse===========\n");
                        string resultext = "";
                        for(int i = start; i < end; i++)
                        {
                            resultext += temp[i];
                        }
                        gpttext.text = resultext; 
            //gpttext.text = temp;
            //print("Translate text:" + resultext);
        }
        intext.text = "translate Done!";
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

    string restext;
    public Text gpttext;

    IEnumerator AzureAPI(Texture2D Texture)
    {
        string endpoint = $"{baseEndpoint}"; //analyze, describe, detect, areaOfInterest
        endpoint += options;

        WWWForm webForm = new WWWForm(); //AddField() πÃ¿€µø
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
            restext = "";
            //restext = jsonResponse;
            var jo = JObject.Parse(jsonResponse);
            var tags = jo["description"]["tags"];
            foreach (string tag in tags)
                restext += tag + " ";
            tags = jo["description"]["captions"];
            var caps = tags[0];
            restext += caps["text"] + " ";
            gpttext.text = restext;
            print("CV text:" + restext);
        }
    }


}