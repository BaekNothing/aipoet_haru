using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test01_Production_A_Azure_CV_RawImage : MonoBehaviour
{
    public string baseEndpoint = "https://koreacentral.api.cognitive.microsoft.com/vision/v3.2/";
    public string key = "3acd5d77775e47048c9c6f98abf529ff";
    private string imageFolder = "Test_Azure/Images";
    private string imageName = "face.jpg";
    public string options = "describe"; //analyze, describe, detect, areaOfInterest
    private RawImage RawImage;

    public Test01_Production_A_JSON_Controller JSON;

    // Start is called before the first frame update
    void Start()
    {
        print("Azure Computer Vision");
        string imagePath = Application.dataPath + "/" + imageFolder + "/" + imageName;
        print("imagePath=" + imagePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }

    IEnumerator AzureAPI()
    {
        string endpoint = $"{baseEndpoint}"; //analyze, describe, detect, areaOfInterest
        endpoint += options;

        WWWForm webForm = new WWWForm(); //AddField() 미작동
        //https://docs.unity3d.com/ScriptReference/WWWForm.html

        string imagePath = Application.dataPath + "/" + imageFolder + "/" + imageName + ".jpg";
        byte[] imageBytes = GetImageAsByteArray(imagePath);

        using (UnityWebRequest www = UnityWebRequest.Post(endpoint, webForm))
        {
            www.SetRequestHeader("Ocp-Apim-Subscription-Key", key);
            www.SetRequestHeader("Content-Type", "application/octet-stream");
            www.uploadHandler.contentType = "application/octet-stream";
            www.uploadHandler = new UploadHandlerRaw(imageBytes);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();
            string jsonResponse = www.downloadHandler.text;
            print(jsonResponse);
        }
    }

    IEnumerator AzureAPI(string imagePath)
    {
        string endpoint = $"{baseEndpoint}"; //analyze, describe, detect, areaOfInterest
        endpoint += options;

        WWWForm webForm = new WWWForm(); //AddField() 미작동
        //https://docs.unity3d.com/ScriptReference/WWWForm.html

        //imageName = rawImage.GetComponent<RawImage>().texture.name;
        //string imagePath = Application.dataPath + "/" + imageFolder + "/" + imageName;
        byte[] imageBytes = GetImageAsByteArray(imagePath);

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
            string text = JSON.GetCVText(jsonResponse);
            print("CV text:" + text);
        }
    }

    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    public void AzureCV(string imagePath)
    {
        StartCoroutine(AzureAPI(imagePath));
    }
}




