using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private GlobalVariables globalVariables;
    
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
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
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
}
