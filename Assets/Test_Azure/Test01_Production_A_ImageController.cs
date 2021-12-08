using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Test01_Production_A_ImageController : MonoBehaviour
{
    [Tooltip("Panel_Images GameObject 삽입")]
    public GameObject Panel_Images;

    [Tooltip("선택한 이미지의 이름을 표시할 Text 오브젝트")]
    public Text textObject;

    RawImage[] RawImages;
    // Start is called before the first frame update
    void Start()
    {
        RawImages = new RawImage[Panel_Images.transform.childCount];
        for (int i = 0; i < Panel_Images.transform.childCount; i++)
        {            
            print(Panel_Images.transform.GetChild(i).name);
            RawImages[i] = Panel_Images.transform.GetChild(i).GetComponent<RawImage>();
            print(RawImages[i].texture.name);
        }

        GetFilesList(Application.dataPath + "/Test_Azure/Images");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(string textInput)
    {
        print(textInput);
        textObject.GetComponent<Text>().text = textInput;
    }

    public List<string> GetFilesList(string dir)
    {
        List<string> Files = new List<string>();
        var info = new DirectoryInfo(dir);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo) {
            print(file);
        }
        return Files;
    }
}
