using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test01_Production_A_ButtonController : MonoBehaviour
{
    [Tooltip("선택한 이미지를 표시할 게임 오브젝트")]
    public GameObject SelectedImage;

    [Tooltip("선택한 이미지의 이름을 표시할 Text 오브젝트")]
    public Text textObject;

    [Tooltip("Azure CV 스크립트")]
    public Test01_Production_A_Azure_CV_RawImage AzureCVScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test(GameObject ClickedObject)
    {
        print("You Clicked " + ClickedObject.name);
    }

    public void SetTexture(RawImage ClickedImage)
    {
        print(ClickedImage.name);
        SelectedImage.transform.GetComponent<RawImage>().texture = ClickedImage.texture;
        SetText(ClickedImage.texture.name + "를 선택했습니다.");
    }

    public void GetKetwords(RawImage ClickedImage)
    {
        if(ClickedImage.texture != null)
        {            
            string imagePath = Application.dataPath + "/Test_Azure/Images/" + ClickedImage.texture.name + ".jpg";
            print("get keywords from " + ClickedImage.texture.name + " image ==> " + Path.GetFileName(imagePath));
            
            SetText(ClickedImage.texture.name + "의 키워드를 추출합니다.");
            AzureCVScript.AzureCV(imagePath);
        }
        else
        {
            print("no raw image texture");
            SetText("이미지를 다시 선택해 주세요.");
        }
    }

    public void SetText(string textInput)
    {
        print(textInput);
        textObject.GetComponent<Text>().text = textInput;
    }
}
