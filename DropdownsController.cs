using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class DropdownsController : MonoBehaviour
{
    //public TMP_Dropdown itemKeyFontDropdown;
    //public TMP_Dropdown itemKeySizeDropdown;
    //public TMP_Dropdown itemValueFontDropdown;
    //public TMP_Dropdown itemValueSizeDropdown;
    public TMP_Dropdown selectedDictionaryDropdown;
    public TMP_Dropdown sortOrderDropdown;
    public TMP_Dropdown interfaceLanguageDropdown;
    
    private GlobalVariables globalVariables;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private FlexDictionary flexDictionary;
    private InputFieldsController inputFieldsController;
    private ButtonsController buttonsController;

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
            inputFieldsController = globalVariables.getInputFieldsController();
            buttonsController = globalVariables.getButtonsController();
        }
    }

    public TMP_Dropdown getSelectedDictionaryDropdown(){
        return selectedDictionaryDropdown;
    }

    public void setDictionaryDropdownOptions(List<string> options){
        setGlobalVariables();
        List<TMP_Dropdown.OptionData> tempOptions = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData temp;
        foreach(string s in options){
            temp = new TMP_Dropdown.OptionData();
            temp.text = s;
            tempOptions.Add(temp);
        }
        sortOrderDropdown.ClearOptions();
        selectedDictionaryDropdown.AddOptions(tempOptions);
        selectedDictionaryDropdown.value = selectedDictionaryDropdown.options.FindIndex((i) => { return i.text.Equals(globalVariables.getLastSelectedDictionary()); });
    }

    public void setSortOrderDropdownOptions(List<string> options){
        setGlobalVariables();
        List<TMP_Dropdown.OptionData> tempOptions = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData temp;
        foreach(string s in options){
            temp = new TMP_Dropdown.OptionData();
            temp.text = s;
            tempOptions.Add(temp);
        }
        sortOrderDropdown.ClearOptions();
        sortOrderDropdown.AddOptions(tempOptions);
        selectedDictionaryDropdown.value = globalVariables.getSorting();
    }

    public TMP_Dropdown getSortOrderDropdown(){
        return sortOrderDropdown;
    }

    public void setInterfaceLanguageDropdownOption(string optionName){
        interfaceLanguageDropdown.value = interfaceLanguageDropdown.options.FindIndex(option => option.text == optionName);
        interfaceLanguageDropdown.RefreshShownValue();
        textsController.switchLanguage(globalVariables.getLanguage());
    }

    public void onInterfaceLanguageDropdownChange(){
        globalVariables.setLanguage(interfaceLanguageDropdown.options[interfaceLanguageDropdown.value].text);
        textsController.switchLanguage(globalVariables.getLanguage());
    }
/*
    public void onItemKeyFontDropdownChange(){
        setGlobalVariables();
        globalVariables.keyFont = itemKeyFontDropdown.value;
        textsController.updateSettingsTexts();
    }

    public void onItemValueFontDropdownChange(){
        setGlobalVariables();
        globalVariables.valueFont = itemValueFontDropdown.value;
        textsController.updateSettingsTexts();
    }
*/
    public void onSelectedDictionaryDropdownChange(){
        if (flexDictionary.loaded){
            globalVariables.setLastSelectedDictionary(selectedDictionaryDropdown.options[selectedDictionaryDropdown.value].text);
            flexDictionary.deserializeDictionary();
        }
        inputFieldsController.setDictionaryNameInputFieldText(selectedDictionaryDropdown.options[selectedDictionaryDropdown.value].text);
    }

    public void setNewFileName(string filename){
        selectedDictionaryDropdown.options[selectedDictionaryDropdown.value].text = filename;
        selectedDictionaryDropdown.RefreshShownValue();
    }

    public void addNewDictionaryToDictionaryDropdown(string name){
        TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
        temp.text = name;
        selectedDictionaryDropdown.options.Add(temp);
        selectedDictionaryDropdown.value = selectedDictionaryDropdown.options.FindIndex(option => option.text == name);
        selectedDictionaryDropdown.RefreshShownValue();
        inputFieldsController.setDictionaryNameInputFieldText(name);
    }

    public void removeDictionaryFromDictionaryDropdown(string name){
        foreach(TMP_Dropdown.OptionData option in selectedDictionaryDropdown.options){
            if (option.text == name){
                selectedDictionaryDropdown.options.Remove(option);
                break;
            }
        }
        if (selectedDictionaryDropdown.options.Count == 0){
            buttonsController.onAddDictionaryButtonClick();
        } else{
            selectedDictionaryDropdown.value = 0;
            selectedDictionaryDropdown.RefreshShownValue();
            inputFieldsController.setDictionaryNameInputFieldText(selectedDictionaryDropdown.options[0].text);
        }
    }
}
