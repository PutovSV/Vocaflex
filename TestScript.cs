using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public GameObject testMenu;
    public GameObject testButton1;
    public GameObject testButton2;
    public GameObject testButton3;
    public GameObject testInputField1;
    public GameObject testInputField2;
    public GameObject testInputField3;
    public GameObject testInputField4;
    public TMP_Text testText;
    // Start is called before the first frame update
    void Start()
    {
        testText.text = "Screen W=" + Screen.width + " Screen H=" + Screen.height;
    }

    public void onChangeTextField1(){
        try {
            testButton1.GetComponent<RectTransform>().sizeDelta = new Vector2(
                float.Parse(testInputField1.GetComponent<TMP_InputField>().text), 
                testButton1.GetComponent<RectTransform>().rect.height);
        }
        catch{
        }
    }

    public void onChangeTextField2(){
        try {
            testButton1.GetComponent<RectTransform>().sizeDelta = new Vector2(
                testButton1.GetComponent<RectTransform>().rect.width, 
                float.Parse(testInputField2.GetComponent<TMP_InputField>().text));
          
        }
        catch{}
    }

    public void onChangeTextField3(){
        try {
            testButton1.gameObject.transform.position = new Vector3(
                float.Parse(testInputField3.GetComponent<TMP_InputField>().text),
                testButton1.transform.position.y,
                0f);
        }
        catch{}
    }

    public void onChangeTextField4(){
        try {
            testButton1.gameObject.transform.position = new Vector3(
                testButton1.transform.position.x,
                float.Parse(testInputField4.GetComponent<TMP_InputField>().text),
                0f);
        }
        catch{}
    }
}
