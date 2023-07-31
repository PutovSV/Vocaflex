using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public sealed class GlobalVariables : MonoBehaviour
{
    public int keyFont = 0;
    public int keyFontSize = 40;
    public int valueFont = 0;
    public int valueFontSize = 30;
    public int colorScheme = 0;
    public TMP_FontAsset[] fonts;
    public float listStartingPosition;
    
    private string saveDirectoryPath;
    private string[] availableDictionaryFiles;
    private int[] settingsData;

    private int selectedDictionary = 0;
    private int sorting = 0;

    private string currentMenu = "Main Menu";
    private string previousMenu;
    private string keyPicked;

    public bool showOnlyFavorite = false;
    private bool testing = false; //DEBUG
    private bool favoriteButtonState;
    
    public GameObject mainCamera;
    public GameObject listOfContent;
    public GameObject mainMenu;
    public GameObject addItemMenu;
    public GameObject editItemMenu;
    public GameObject editChoiceMenu;
    public GameObject replaceChoiceMenu;
    public GameObject deleteChoiceMenu;
    public GameObject itemMenu;
    public GameObject testMenu;
    public GameObject settingsMenu;
    public GameObject favoriteButton;
    public GameObject favoriteSettingsButton;
    
    public TMP_FontAsset font0;
    public TMP_FontAsset font1;
    public TMP_FontAsset font2;
    public TMP_FontAsset font3;
    public TMP_FontAsset font4;

    public TMP_Text debugLog;

    private ShowHideController showHideController;
    private FlexDictionary flexDictionary;
    private ButtonsController buttonsController;
    private InputFieldsController inputFieldsController;
    private DropdownsController dropdownsController;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private ScrollbarsController scrollbarsController;
    private ColorsController colorsController;

    private static GlobalVariables instance = null;
    private GlobalVariables(){}

    public static GlobalVariables Instance { get { return instance; } }

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
        if (testing){
            writeToDebugLog("GlobalVariables started");
        }

        showHideController = mainCamera.GetComponent<ShowHideController>();
        flexDictionary = mainCamera.GetComponent<FlexDictionary>();
        buttonsController = mainCamera.GetComponent<ButtonsController>();
        inputFieldsController = mainCamera.GetComponent<InputFieldsController>();
        dropdownsController = mainCamera.GetComponent<DropdownsController>();
        dictionaryListController = mainCamera.GetComponent<DictionaryListController>();
        textsController = mainCamera.GetComponent<TextsController>();
        scrollbarsController = mainCamera.GetComponent<ScrollbarsController>();
        colorsController = mainCamera.GetComponent<ColorsController>();

        saveDirectoryPath = Path.Combine(Application.persistentDataPath, "Dictionary", "SaveData");

        fonts = new TMP_FontAsset[5];
        fonts[0] = font0;
        fonts[1] = font1;
        fonts[2] = font2;
        fonts[3] = font3;
        fonts[4] = font4;

        if (!Directory.Exists(getSaveDirectoryPath())){
            Directory.CreateDirectory(getSaveDirectoryPath());
            if (testing){
                writeToDebugLog("Создана папка");
            }
        }

        availableDictionaryFiles = Directory.GetFiles(saveDirectoryPath);
        for(int i = 0; i < availableDictionaryFiles.Length; i++){
            availableDictionaryFiles[i] = Path.GetFileName(availableDictionaryFiles[i]).Substring(0, Path.GetFileName(availableDictionaryFiles[i]).IndexOf(".vf"));
        }
        
        if (availableDictionaryFiles.Length == 0){
            availableDictionaryFiles = new string[]{ "NewDictionary" };
        }

        deserializeSettings();

        if (testing){
            writeToDebugLog("GlobalVariables loaded");
        }

        listStartingPosition = listOfContent.GetComponent<RectTransform>().position.y;
        //DEBUG
        /*foreach(var s in availableVocabularyFiles){
            Debug.Log(s);
        }*/

        /*
        foreach(Transform child in itemPrefab.transform){
            if (child.gameObject.name.Equals("ItemKey")){
                child.gameObject.GetComponent<TMP_Text>().fontSize = keyFontSize;
            }
            if (child.gameObject.name.Equals("ItemValue")){
                child.gameObject.GetComponent<TMP_Text>().fontSize = valueFontSize;
            }
        }*/

    }

    public void deserializeSettings(){
        if (File.Exists(Path.Combine(Application.persistentDataPath, "settings.ini"))){
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "settings.ini"));
            int[] jsonSettings = new int[6];
            jsonSettings = JsonConvert.DeserializeObject<int[]>(json);
            
            scrollbarsController.setFontSizeScrollbarValue(0);

            keyFont = jsonSettings[0];
            keyFontSize = jsonSettings[1];
            valueFont = jsonSettings[2];
            valueFontSize = jsonSettings[3];
            colorScheme = jsonSettings[4];
            showOnlyFavorite = (jsonSettings[5] == 1) ? true : false;
            
            colorsController.setGraphicObjectsLists();
            colorsController.setNewColorScheme(colorScheme);
            scrollbarsController.setFontSizeScrollbarValue(keyFontSize);
            textsController.updateSettingsTexts();
        }
    }

    public void serializeSettings(){
        int[] jsonSettings = new int[6];
            jsonSettings[0] = keyFont;
            jsonSettings[1] = keyFontSize;
            jsonSettings[2] = valueFont;
            jsonSettings[3] = valueFontSize;
            jsonSettings[4] = colorScheme;
            jsonSettings[5] = showOnlyFavorite ? 1 : 0;
        string json = JsonConvert.SerializeObject(jsonSettings);
        string filePath = Path.Combine(Application.persistentDataPath, "settings.ini");
        File.WriteAllText(filePath, json);
    }

    public void writeToDebugLog(string newLine){
        debugLog.text = debugLog.text + "\n" + newLine;
    }

    public void replaceDebugLog(string line){
        debugLog.text = line;
    }

    public string getSaveDirectoryPath(){
        return saveDirectoryPath;
    }

    public string getSelectedDictionaryPath(){
        return availableDictionaryFiles[selectedDictionary];
    }

    public GameObject getCurrentMenu(){
        switch (currentMenu)
        {
            case "Main Menu": 
                return mainMenu;
            case "Add Item Menu": 
                return addItemMenu;
            case "Edit Item Menu": 
                return editItemMenu;
            case "Edit Choice Menu": 
                return editChoiceMenu;
            case "Replace Choice Menu": 
                return replaceChoiceMenu;
            case "Delete Choice Menu": 
                return deleteChoiceMenu;
            case "Item Menu": 
                return itemMenu;
            case "Test Menu": 
                return testMenu;
            case "Settings Menu":
                return settingsMenu;
            default: 
            Debug.Log("NULL returned");
                return null;
        }
    }

    public string getCurrentMenuName(){
        return currentMenu;
    }

    public void setCurrentMenu(string nextMenu){
        previousMenu = currentMenu;
        currentMenu = nextMenu;
    }

    public string getPreviousMenu(){
        return previousMenu;
    }

    public void setPreviousMenu(string menu){
        previousMenu = menu;
    }

    public string getKeyPicked(){
        return keyPicked;
    }

    public void setKeyPicked(string key){
        keyPicked = key;
    }

    public void setSelectedDictionary(int number){
        selectedDictionary = number;
    }
    
    public ShowHideController getShowHideController(){
        return showHideController;
    }
    
    public FlexDictionary getFlexDictionary(){
        return flexDictionary;
    }

    public ButtonsController getButtonsController(){
        return buttonsController;
    }

    public InputFieldsController getInputFieldsController(){
        return inputFieldsController;
    }
    
    public DropdownsController getDropdownsController(){
        return dropdownsController;
    }

    public DictionaryListController getDictionaryListController(){
        return dictionaryListController;
    }

    public TextsController getTextsController(){
        return textsController;
    }

    public ScrollbarsController getScrollbarsController(){
        return scrollbarsController;
    }

    public ColorsController getColorsController(){
        return colorsController;
    }

    public void setSorting(int sortType){
        sorting = sortType;
    }

    public int getSorting(){
        return sorting;
    }

    public void setShowOnlyFavorite(bool favorite){
        showOnlyFavorite = favorite;
    }

    public bool isShowOnlyFavorite(){
        return showOnlyFavorite;
    }

    public GameObject getMainCamera(){
        return mainCamera;
    }

    public GameObject getListOfContent(){
        return listOfContent;
    }
    
    public GameObject getMainMenu(){
        return mainMenu;
    }

    public GameObject getAddItemMenu(){
        return addItemMenu;
    }

    public GameObject getEditItemMenu(){
        return editItemMenu;
    }

    public GameObject getEditChoiceMenu(){
        return editChoiceMenu;
    }

    public GameObject getReplaceChoiceMenu(){
        return replaceChoiceMenu;
    }

    public GameObject getDeleteChoiceMenu(){
        return deleteChoiceMenu;
    }

    public GameObject getItemMenu(){
        return itemMenu;
    }

    public GameObject getTestMenu(){
        return testMenu;
    }

    public GameObject getSettingsMenu(){
        return settingsMenu;
    }

    public bool isTesting(){
        return testing;
    }

    public void setDebugLogEnabled(bool state){
        debugLog.enabled = state;
    }

    public bool isFavorite(){
        return flexDictionary.isFavorite(keyPicked);
    }

    public void setFavorite(bool favorite){
        flexDictionary.setFavorite(keyPicked, favorite);
    }

    public bool getFavoriteButtonState(){
        return favoriteButtonState;
    }

    public void setFavoriteButtonState(bool favorite){
        favoriteButtonState = favorite;
    }

    public void switchFavoriteButtonState(){
        favoriteButtonState = !favoriteButtonState;
    }

    public string[] getAvailableDictionaryFiles(){
        return availableDictionaryFiles;
    }
}
