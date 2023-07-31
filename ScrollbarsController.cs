using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class ScrollbarsController : MonoBehaviour
{
    public Scrollbar fontSizeScrollbar;
    
    private GlobalVariables globalVariables;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private FlexDictionary flexDictionary;

    private static ScrollbarsController instance = null;
    private ScrollbarsController(){}

    public ScrollbarsController Instance { get { return instance; } }

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
            dictionaryListController = globalVariables.getDictionaryListController();
            textsController = globalVariables.getTextsController();
            flexDictionary = globalVariables.getFlexDictionary();
        }
    }

    public Scrollbar getFontSizeScrollbar(){
        return fontSizeScrollbar;
    }

    public int getFontSizeScrollbarValue(){
        return (int) (fontSizeScrollbar.value * 0.2f * 100 + 50);
    }

    public void setFontSizeScrollbarValue(int value){
        fontSizeScrollbar.value = (value - 50) / 100f / 0.2f;
    }
    public void onFontSizeScrollbarChange(){
        setGlobalVariables();
        globalVariables.keyFontSize = getFontSizeScrollbarValue();
        globalVariables.valueFontSize = (int) (getFontSizeScrollbarValue() * 0.75f);
        textsController.updateSettingsTexts();
    }

}
