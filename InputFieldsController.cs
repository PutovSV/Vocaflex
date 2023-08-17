using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public sealed class InputFieldsController : MonoBehaviour
{
    public TMP_InputField editKeyInputField;
    public TMP_InputField editValueInputField;
    public TMP_InputField editHyperlinkInputField;
    public TMP_InputField addKeyInputField;
    public TMP_InputField addValueInputField;
    public TMP_InputField addHyperlinkInputField;
    public TMP_InputField dictionaryNameInputField;
    public TMP_InputField searchInputField;

    private GlobalVariables globalVariables;
    private FlexDictionary flexDictionary;
    private ShowHideController showHideController;
    private DictionaryListController dictionaryListController;
    private DropdownsController dropdownsController;
    
    private static InputFieldsController instance = null;
    private InputFieldsController(){}

    public InputFieldsController Instance { get { return instance; } }

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
        editKeyInputField = GameObject.Find("EditKeyInputField").GetComponent<TMP_InputField>();
        editValueInputField = GameObject.Find("EditValueInputField").GetComponent<TMP_InputField>();
        editHyperlinkInputField = GameObject.Find("EditHyperlinkInputField").GetComponent<TMP_InputField>();
        addKeyInputField = GameObject.Find("AddKeyInputField").GetComponent<TMP_InputField>();
        addValueInputField = GameObject.Find("AddValueInputField").GetComponent<TMP_InputField>();
        addHyperlinkInputField = GameObject.Find("AddHyperlinkInputField").GetComponent<TMP_InputField>();
        searchInputField = GameObject.Find("SearchInputField").GetComponent<TMP_InputField>();
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            flexDictionary = globalVariables.getFlexDictionary();
            showHideController = globalVariables.getShowHideController();
            dictionaryListController = globalVariables.getDictionaryListController();
            dropdownsController = globalVariables.getDropdownsController();
        }
    }

    public TMP_InputField getEditKeyInputField(){
        return editKeyInputField;
    }
    
    public TMP_InputField getEditValueInputField(){
        return editValueInputField;
    }
    
    public TMP_InputField getEditHyperlinkInputField(){
        return editHyperlinkInputField;
    }

    public TMP_InputField getAddKeyInputField(){
        return addKeyInputField;
    }
    
    public TMP_InputField getAddValueInputField(){
        return addValueInputField;
    }
    
    public TMP_InputField getAddHyperlinkInputField(){
        return addHyperlinkInputField;
    }

    public TMP_InputField getSearchInputField(){
        return searchInputField;
    }

    public void onSearchInputFieldChange(){
        flexDictionary.setSearchText(searchInputField.text);
        dictionaryListController.refreshList();
    }

    public void onDictionaryNameInputFieldEndEdit(){
        File.Move(Path.Combine(globalVariables.getSaveDirectoryPath(), globalVariables.getLastSelectedDictionary() + ".vf"), 
                  Path.Combine(globalVariables.getSaveDirectoryPath(), dictionaryNameInputField.text + ".vf"));
        globalVariables.setLastSelectedDictionary(dictionaryNameInputField.text);
        dropdownsController.setNewFileName(dictionaryNameInputField.text);
        globalVariables.serializeSettings();
    }

    public void setDictionaryNameInputFieldText(string text){
        dictionaryNameInputField.text = text;
    }

    public string getDictionaryNameInputField(){
        return dictionaryNameInputField.text;
    }

}
