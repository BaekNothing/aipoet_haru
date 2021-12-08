using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test01_Production_A_ButtonController : MonoBehaviour
{
    [Tooltip("������ �̹����� ǥ���� ���� ������Ʈ")]
    public GameObject SelectedImage;

    [Tooltip("������ �̹����� �̸��� ǥ���� Text ������Ʈ")]
    public Text textObject;

    [Tooltip("Azure CV ��ũ��Ʈ")]
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
        SetText(ClickedImage.texture.name + "�� �����߽��ϴ�.");
    }

    public void GetKetwords(RawImage ClickedImage)
    {
        if(ClickedImage.texture != null)
        {            
            string imagePath = Application.dataPath + "/Test_Azure/Images/" + ClickedImage.texture.name + ".jpg";
            print("get keywords from " + ClickedImage.texture.name + " image ==> " + Path.GetFileName(imagePath));
            
            SetText(ClickedImage.texture.name + "�� Ű���带 �����մϴ�.");
            AzureCVScript.AzureCV(imagePath);
        }
        else
        {
            print("no raw image texture");
            SetText("�̹����� �ٽ� ������ �ּ���.");
        }
    }

    public void SetText(string textInput)
    {
        print(textInput);
        textObject.GetComponent<Text>().text = textInput;
    }
}
