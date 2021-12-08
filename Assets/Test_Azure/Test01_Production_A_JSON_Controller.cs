using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq; //https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347

public class Test01_Production_A_JSON_Controller : MonoBehaviour
{
    public string GetCVText(string jsonString)
    {
        //print("Resmote JSON String:" + jsonString);
        var jo = JObject.Parse(jsonString);
        //print(jo);

        var text = jo["description"]["captions"][0]["text"];
        //print(text);

        string text_string = jo["description"]["captions"][0]["text"].ToString();
        //print(text_string);

        return text_string;
    }

    public string GetCVTags(string jsonString)
    {
        //print("Resmote JSON String:" + jsonString);
        var jo = JObject.Parse(jsonString);
        //print(jo);

        var tags = jo["description"]["tags"];
        //print(tags);
        //string[] tagsArray = new string[tags.;
        foreach (string tag in tags)
        {
            //print(tag);
        }
        return tags.ToString();
    }
}

