using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class ColorsController : MonoBehaviour
{   
    private GlobalVariables globalVariables;

    private List<Image> backgroundColorList = new List<Image>();
    private List<Image> buttonMainColorList = new List<Image>();
    private List<Image> buttonLogoColorList = new List<Image>();
    private List<Image> inputFieldColorList = new List<Image>();
    private List<TMP_Text> textColorList = new List<TMP_Text>();
    private List<TMP_Text> elementTextColorList = new List<TMP_Text>();
    private List<TMP_Text> placeholderTextColorList = new List<TMP_Text>();
    private List<Image> elementBackgroundColorList = new List<Image>();

    public List<Image> inactiveGraphicElements = new List<Image>();
    public List<Image> inactiveGraphicElements2 = new List<Image>();
    public List<TMP_Text> inactiveTextElements = new List<TMP_Text>();

    private string[,] colorSchemesArray = new string[3,8]{
        {"#587C8C", "#002A49", "#ACE1FF", "#5996B7", "#003052", "#002331", "#416F80", "#40738E"}, // classic theme
        {"#040605", "#444439", "#B4BC9F", "#33332b", "#989A8B", "#989A8B", "#545873", "#151e1b"}, // dark theme
        {"#dddcda", "#AE9F91", "#e5eced", "#ECE5D9", "#2C2613", "#575757", "#968B76", "#F8F3E4"}  // light theme
    };

    private static ColorsController instance = null;
    private ColorsController(){}

    public ColorsController Instance { get { return instance; } }

    private void Awake(){
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start(){
        setGlobalVariables();
        if (globalVariables.isTesting()){
            globalVariables.writeToDebugLog("ColorsController started");
        }
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
        }
    }

    public void setGraphicObjectsLists(){
        setGlobalVariables();
        
        GameObject[] allObjectsWithGraphic = GameObject.FindGameObjectsWithTag("GraphicElement");
        foreach(GameObject go in allObjectsWithGraphic){
            switch (ColorUtility.ToHtmlStringRGB(go.GetComponent<Image>().color)){
                case "587C8C":
                    backgroundColorList.Add(go.GetComponent<Image>());
                    break;
                case "002A49":
                    buttonMainColorList.Add(go.GetComponent<Image>());
                    break;
                case "ACE1FF":
                    buttonLogoColorList.Add(go.GetComponent<Image>());
                    break;
                case "5996B7":
                    inputFieldColorList.Add(go.GetComponent<Image>());
                    break;
                case "31566A":
                    elementBackgroundColorList.Add(go.GetComponent<Image>());
                    break;
                default:
                    break;
            }
        }
        GameObject[] allTexts = GameObject.FindGameObjectsWithTag("Text");
        GameObject[] allMultilanguageTexts = GameObject.FindGameObjectsWithTag("MultilanguageText");
        List<GameObject> texts = new List<GameObject>();
        texts.AddRange(allTexts);
        texts.AddRange(allMultilanguageTexts);
        allTexts = texts.ToArray();
        foreach(GameObject go in allTexts){
            switch (ColorUtility.ToHtmlStringRGB(go.GetComponent<TMP_Text>().color)){
                case "003052":
                    textColorList.Add(go.GetComponent<TMP_Text>());
                    break;
                case "C8C8C8":
                    elementTextColorList.Add(go.GetComponent<TMP_Text>());
                    break;
                case "416F80":
                    placeholderTextColorList.Add(go.GetComponent<TMP_Text>());
                    break;
                default:
                    break;
            }
        }

        inputFieldColorList.AddRange(inactiveGraphicElements);
        buttonMainColorList.AddRange(inactiveGraphicElements2);
        textColorList.AddRange(inactiveTextElements);
    }

    public void setNewColorScheme(int colorScheme){
        Color backgroundColor;
        Color buttonMainColor;
        Color buttonLogoColor;
        Color inputFieldColor;
        Color textColor;
        Color elementTextColor;
        Color placeholderTextColor;
        Color elementBackgroundColor;

        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 0], out backgroundColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 1], out buttonMainColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 2], out buttonLogoColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 3], out inputFieldColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 4], out textColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 5], out elementTextColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 6], out placeholderTextColor);
        ColorUtility.TryParseHtmlString(colorSchemesArray[colorScheme, 7], out elementBackgroundColor);

        changeColorsInList(backgroundColorList, backgroundColor);
        changeColorsInList(buttonMainColorList, buttonMainColor);
        changeColorsInList(buttonLogoColorList, buttonLogoColor);
        changeColorsInList(inputFieldColorList, inputFieldColor);
        changeColorsInList(textColorList, textColor);
        changeColorsInList(elementTextColorList, elementTextColor);
        changeColorsInList(placeholderTextColorList, placeholderTextColor);
        changeColorsInList(elementBackgroundColorList, elementBackgroundColor);
    }

    private void changeColorsInList(List<Image> list, Color color){
        foreach(Image image in list){
            image.color = color;
        }
    }

    private void changeColorsInList(List<TMP_Text> list, Color color){
        foreach(TMP_Text text in list){
            text.color = color;
        }
    }

    private void changeAlpha(Image img, float newAlpha){
        var color = img.color;
        color.a = newAlpha;
        img.color = color;
    }

    public Color getElementBackgroundColor(){
        setGlobalVariables();
        Color color;
        ColorUtility.TryParseHtmlString(colorSchemesArray[globalVariables.colorScheme, 7], out color);
        return color;
    }

    public Color getElementTextColor(){
        setGlobalVariables();
        Color color;
        ColorUtility.TryParseHtmlString(colorSchemesArray[globalVariables.colorScheme, 5], out color);
        return color;
    }

}
