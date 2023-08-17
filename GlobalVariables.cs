using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public sealed class GlobalVariables : MonoBehaviour
{
    // settings
    public int keyFont = 0;
    public int keyFontSize = 40;
    public int valueFont = 0;
    public int valueFontSize = 30;
    public int colorScheme = 0;
    public int sorting = 0;
    public bool showOnlyFavorite = false;
    public string lastSelectedDictionary;
    public string language = "English";

    public TMP_FontAsset[] fonts;
    public float listStartingPosition;
    
    private string saveDirectoryPath;
    private string[] availableDictionaryFiles;
    private int[] settingsData;

    private string currentMenu = "Main Menu";
    private string previousMenu;
    private Item pickedItem;

    private bool testing = true; //DEBUG
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
    private GameObject[] allMultilanguageTexts;
    
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

        allMultilanguageTexts = GameObject.FindGameObjectsWithTag("MultilanguageText");

        if (!Directory.Exists(getSaveDirectoryPath())){
            Directory.CreateDirectory(getSaveDirectoryPath());
            if (testing){
                writeToDebugLog("Создана папка");
            }
        }

        if (!testing){

            availableDictionaryFiles = Directory.GetFiles(saveDirectoryPath);
            for(int i = 0; i < availableDictionaryFiles.Length; i++){
                availableDictionaryFiles[i] = Path.GetFileName(availableDictionaryFiles[i]).Substring(0, Path.GetFileName(availableDictionaryFiles[i]).IndexOf(".vf"));
            }

            if (availableDictionaryFiles.Length == 0){
                availableDictionaryFiles = new string[]{"NewDictionary"};
                lastSelectedDictionary = availableDictionaryFiles[0];
                flexDictionary.serializeDictionary();
            }

            deserializeSettings();
        } else{
            writeToDebugLog("GlobalVariables loaded");
        }

        listStartingPosition = listOfContent.GetComponent<RectTransform>().position.y;

        //textsController.saveLanguage();
    }

    public void deserializeSettings(){
        if (File.Exists(Path.Combine(Application.persistentDataPath, "settings.ini"))){
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "settings.ini"));
            string[] jsonSettings = new string[9];
            jsonSettings = JsonConvert.DeserializeObject<string[]>(json);
            scrollbarsController.setFontSizeScrollbarValue(0);
            keyFont = int.Parse(jsonSettings[0]);
            keyFontSize = int.Parse(jsonSettings[1]);
            valueFont = int.Parse(jsonSettings[2]);
            valueFontSize = int.Parse(jsonSettings[3]);
            colorScheme = int.Parse(jsonSettings[4]);
            sorting = int.Parse(jsonSettings[5]);
            showOnlyFavorite = (int.Parse(jsonSettings[6]) == 1) ? true : false;

            bool isFileExists = false;
            foreach(string fileName in availableDictionaryFiles){
                if (fileName == jsonSettings[7]){
                    isFileExists = true;
                }
            }
            if (isFileExists){
                lastSelectedDictionary = jsonSettings[7];
            } else{
                lastSelectedDictionary = availableDictionaryFiles[0];
                Debug.Log("Словаря " + jsonSettings[7] + " не существует! Создан словарь " + lastSelectedDictionary);
            }

            language = jsonSettings[8];
            
        } else{
            // if settings file not found
            serializeSettings();
        }

        colorsController.setGraphicObjectsLists();
        colorsController.setNewColorScheme(colorScheme);
        scrollbarsController.setFontSizeScrollbarValue(keyFontSize);
        dropdownsController.setDictionaryDropdownOptions(availableDictionaryFiles.ToList());
        inputFieldsController.setDictionaryNameInputFieldText(lastSelectedDictionary);
        dropdownsController.setInterfaceLanguageDropdownOption(language);

        flexDictionary.deserializeDictionary();
    }

    public void serializeSettings(){
        string[] jsonSettings = new string[9];
        jsonSettings[0] = keyFont.ToString();
        jsonSettings[1] = keyFontSize.ToString();
        jsonSettings[2] = valueFont.ToString();
        jsonSettings[3] = valueFontSize.ToString();
        jsonSettings[4] = colorScheme.ToString();
        jsonSettings[5] = sorting.ToString();
        jsonSettings[6] = (showOnlyFavorite ? 1 : 0).ToString();
        jsonSettings[7] = lastSelectedDictionary;
        jsonSettings[8] = language;
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

    public string getLastSelectedDictionary(){
        return lastSelectedDictionary != null ? lastSelectedDictionary : lastSelectedDictionary = availableDictionaryFiles[0];
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

    public Item getPickedItem(){
        return pickedItem;
    }

    public void setPickedItem(Item item){
        pickedItem = item;
    }

    public void setLastSelectedDictionary(string filename){
        lastSelectedDictionary = filename;
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
        return pickedItem.isFavorite();
    }

    public void setFavorite(bool favorite){
        flexDictionary.setFavorite(pickedItem, favorite);
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

    public void addAvailableDictionaryFile(string file){
        availableDictionaryFiles = availableDictionaryFiles.Concat(new string[]{file}).ToArray();
    }

    public string getLanguage(){
        return language;
    }

    public void setLanguage(string newLanguage){
        language = newLanguage;
    }

    public GameObject[] getAllMultilanguageTexts(){
        return allMultilanguageTexts;
    }
}
