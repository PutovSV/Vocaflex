using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public sealed class TextsController : MonoBehaviour
{
    public TMP_Text templateText;
    //public TMP_Text itemValueTemplateText;
    public TMP_Text itemKeyText;
    public TMP_Text itemValueText;
    public TMP_Text editItemKeyText;
    public TMP_Text editItemValueText;
    public TMP_Text editItemHyperlinkText;
    public TMP_Text favoriteText;
    public TMP_Text testText;

    private GlobalVariables globalVariables;
    private DropdownsController dropdownsController;
    private string keyInputFieldPlaceholderWarning;
    private string valueInputFieldPlaceholderWarning;
    private string keyInputFieldPlaceholder;
    private string valueInputFieldPlaceholder;
    
    private static TextsController instance = null;
    private TextsController(){}

    public TextsController Instance { get { return instance; } }

    private void Awake(){
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start()
    {
        setGlobalVariables();
        testText.text = "Screen Width = " + Screen.width + " Screen Height = " + Screen.height;
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            dropdownsController = globalVariables.getDropdownsController();
        }
    }

    public TMP_Text gettemplateText(){
        return templateText;
    }
/*
    public TMP_Text getItemValueTemplateText(){
        return itemValueTemplateText;
    }
*/
    public TMP_Text getItemKeyText(){
        return itemKeyText;
    }

    public TMP_Text getItemValueText(){
        return itemValueText;
    }

    public void setItemKeyText(string itemKey){
        itemKeyText.text = itemKey;
    }

    public void setItemValueText(string itemValue){
        itemValueText.text = itemValue;
    }
/*
    public TMP_Text getActionText(){
        return itemValueTemplateText;
    }
*/
    public void updateSettingsTexts(){
        setGlobalVariables();
        Debug.Log("Settings Menu Fonts & font sizes updated");
        templateText.fontSize = globalVariables.keyFontSize;
        //itemKeyTemplateText.font = globalVariables.fonts[globalVariables.keyFont];
        //itemValueTemplateText.fontSize = globalVariables.valueFontSize;
        //itemValueTemplateText.font = globalVariables.fonts[globalVariables.valueFont];
    }

    public void updateItemMenuTexts(){
        setGlobalVariables();
        Debug.Log("Item Menu Fonts & font sizes updated");
        itemKeyText.fontSize = globalVariables.keyFontSize;
        //itemKeyText.font = globalVariables.fonts[globalVariables.keyFont];
        itemValueText.fontSize = globalVariables.valueFontSize;
        //itemValueText.font = globalVariables.fonts[globalVariables.valueFont];
    }

    public void updateEditItemMenuTexts(){
        setGlobalVariables();
        Debug.Log("Edit Item Menu Fonts & font sizes updated");
        editItemKeyText.fontSize = globalVariables.keyFontSize;
        //itemKeyText.font = globalVariables.fonts[globalVariables.keyFont];
        editItemValueText.fontSize = globalVariables.valueFontSize;
        //itemValueText.font = globalVariables.fonts[globalVariables.valueFont];
        editItemHyperlinkText.fontSize = globalVariables.valueFontSize;
        //itemValueText.font = globalVariables.fonts[globalVariables.valueFont];
    }

    public void switchLanguage(string newLanguage){
        setGlobalVariables();
        var temp = (TextAsset)Resources.Load(newLanguage);
        string json = temp.text;
        List<Texts> jsonTextsList = new List<Texts>();
        jsonTextsList = JsonConvert.DeserializeObject<List<Texts>>(json);
        foreach(GameObject go in globalVariables.getAllMultilanguageTexts()){
            foreach(Texts t in jsonTextsList){
                if (go.name == t.textObjectName){
                    go.GetComponent<TMP_Text>().text = t.textObjectText;
                    continue;
                }
            }
        }
        foreach(Texts t in jsonTextsList){
            switch (t.textObjectName){
                case "SortOrderDropdown":
                    dropdownsController.setSortOrderDropdownOptions(new List<string>(t.textObjectText.Split("|")));
                    break;
                case "keyInputFieldPlaceholderWarning":
                    keyInputFieldPlaceholderWarning = t.textObjectText;
                    break;
                case "valueInputFieldPlaceholderWarning":
                    valueInputFieldPlaceholderWarning = t.textObjectText;
                    break;
                case "keyInputFieldPlaceholder":
                    keyInputFieldPlaceholder = t.textObjectText;
                    break;
                case "valueInputFieldPlaceholder":
                    valueInputFieldPlaceholder = t.textObjectText;
                    break;
                default:
                    break;
            }
        }
        Debug.Log("Language switched to " + newLanguage);
    }

    public class Texts{

        [JsonProperty("name")]
        public string textObjectName;

        [JsonProperty("text")]
        public string textObjectText;

        public Texts(string name, string text){
            textObjectName = name;
            textObjectText = text;
        }
    }

    public void saveLanguage(){
        GameObject[] allTexts = GameObject.FindGameObjectsWithTag("MultilanguageText");
        List<Texts> textsList = new List<Texts>();
        foreach(GameObject go in allTexts){
            textsList.Add(new Texts(go.name, go.GetComponent<TMP_Text>().text));
        }
        string json = JsonConvert.SerializeObject(textsList, Formatting.Indented);
        //JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented)
        string filePath = Path.Combine(Application.persistentDataPath, "Russian.lng");
        File.WriteAllText(filePath, json);
    }

    public string getKeyInputFieldPlaceholderWarning(){
        return keyInputFieldPlaceholderWarning;
    }

    public string getValueInputFieldPlaceholderWarning(){
        return valueInputFieldPlaceholderWarning;
    }

    public string getKeyInputFieldPlaceholder(){
        return keyInputFieldPlaceholder;
    }

    public string getValueInputFieldPlaceholder(){
        return valueInputFieldPlaceholder;
    }
}
