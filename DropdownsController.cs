using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class DropdownsController : MonoBehaviour
{
    //public TMP_Dropdown itemKeyFontDropdown;
    //public TMP_Dropdown itemKeySizeDropdown;
    //public TMP_Dropdown itemValueFontDropdown;
    //public TMP_Dropdown itemValueSizeDropdown;
    public TMP_Dropdown selectedDictionaryDropdown;
    public TMP_Dropdown sortOrderDropdown;
    
    private GlobalVariables globalVariables;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private FlexDictionary flexDictionary;

    private static DropdownsController instance = null;
    private DropdownsController(){}

    public DropdownsController Instance { get { return instance; } }

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
        selectedDictionaryDropdown.AddOptions(new List<string>(globalVariables.getAvailableDictionaryFiles()));
        /*
        itemKeyFontDropdown.value = globalVariables.keyFont;
        itemKeySizeDropdown.value = itemKeySizeDropdown.options.FindIndex(option => option.text == globalVariables.keyFontSize.ToString());
        itemValueFontDropdown.value = globalVariables.valueFont;
        itemValueSizeDropdown.value = itemValueSizeDropdown.options.FindIndex(option => option.text == globalVariables.valueFontSize.ToString());
        */
        //Debug.Log(globalVariables.keyFontSize.ToString());
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            dictionaryListController = globalVariables.getDictionaryListController();
            textsController = globalVariables.getTextsController();
            flexDictionary = globalVariables.getFlexDictionary();
        }
    }

    public TMP_Dropdown getSelectedDictionaryDropdown(){
        return selectedDictionaryDropdown;
    }
/*
    public TMP_Dropdown getItemKeyFontDropdown(){
        return itemKeyFontDropdown;
    }
    
    public TMP_Dropdown getItemKeySizeDropdown(){
        return itemKeySizeDropdown;
    }

    public TMP_Dropdown getItemValueFontDropdown(){
        return itemValueFontDropdown;
    }

    public TMP_Dropdown getItemValueSizeDropdown(){
        return itemValueSizeDropdown;
    }
*/
    public TMP_Dropdown getSortOrderDropdown(){
        return sortOrderDropdown;
    }
/*
    public void onItemKeyFontDropdownChange(){
        setGlobalVariables();
        globalVariables.keyFont = itemKeyFontDropdown.value;
        textsController.updateSettingsTexts();
    }

    public void onItemKeySizeDropdownChange(){
        setGlobalVariables();
        globalVariables.keyFontSize = int.Parse(itemKeySizeDropdown.options[itemKeySizeDropdown.value].text);
        textsController.updateSettingsTexts();
    }

    public void onItemValueFontDropdownChange(){
        setGlobalVariables();
        globalVariables.valueFont = itemValueFontDropdown.value;
        textsController.updateSettingsTexts();
    }

    public void onItemValueSizeDropdownChange(){
        setGlobalVariables();
        globalVariables.valueFontSize = int.Parse(itemValueSizeDropdown.options[itemValueSizeDropdown.value].text);
        textsController.updateSettingsTexts();
    }
*/
    public void onSelectedDictionaryDropdownChange(){
        globalVariables.setSelectedDictionary(selectedDictionaryDropdown.value);
        flexDictionary.deserializeDictionary();
    }
}
