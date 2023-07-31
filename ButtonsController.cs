using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class ButtonsController : MonoBehaviour
{   
    public GameObject addFavoriteSprite;
    public GameObject addUnFavoriteSprite;
    public GameObject editFavoriteSprite;
    public GameObject editUnFavoriteSprite;
    public GameObject itemFavoriteSprite;
    public GameObject itemUnFavoriteSprite;
    public GameObject settingsFavoriteSprite;
    public GameObject settingsUnFavoriteSprite;
    public GameObject classicColorSchemeSprite;
    public GameObject darkColorSchemeSprite;
    public GameObject lightColorSchemeSprite;

    private GlobalVariables globalVariables;
    private ShowHideController showHideController;
    private FlexDictionary flexDictionary;
    private ButtonsController buttonsController;
    private InputFieldsController inputFieldsController;
    private DropdownsController dropdownsController;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private ColorsController colorsController;

    private static ButtonsController instance = null;
    private ButtonsController(){}

    public ButtonsController Instance { get { return instance; } }

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
            globalVariables.writeToDebugLog("ButtonsController started");
        }
        globalVariables.setCurrentMenu("Main Menu");
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            showHideController = globalVariables.getShowHideController();
            flexDictionary = globalVariables.getFlexDictionary();
            inputFieldsController = globalVariables.getInputFieldsController();
            dropdownsController = globalVariables.getDropdownsController();
            dictionaryListController = globalVariables.getDictionaryListController();
            textsController = globalVariables.getTextsController();
            colorsController = globalVariables.getColorsController();
        }
    }

    public void onAddButtonClick(){
        inputFieldsController.getAddKeyInputField().text = "";
        inputFieldsController.getAddValueInputField().text = "";
        showHideController.show("Add Item Menu");
    }

    public void onFavoriteButtonClick(){
        globalVariables.switchFavoriteButtonState();
        switch (globalVariables.getCurrentMenuName()){
            case "Add Item Menu":
                addFavoriteSprite.SetActive(globalVariables.getFavoriteButtonState());
                addUnFavoriteSprite.SetActive(!globalVariables.getFavoriteButtonState());
                break;
            case "Item Menu":
                itemFavoriteSprite.SetActive(globalVariables.getFavoriteButtonState());
                itemUnFavoriteSprite.SetActive(!globalVariables.getFavoriteButtonState());
                break;
            case "Edit Item Menu":
                editFavoriteSprite.SetActive(globalVariables.getFavoriteButtonState());
                editUnFavoriteSprite.SetActive(!globalVariables.getFavoriteButtonState());
                break;
            case "Settings Menu":
                settingsFavoriteSprite.SetActive(globalVariables.getFavoriteButtonState());
                settingsUnFavoriteSprite.SetActive(!globalVariables.getFavoriteButtonState());
                break;
            default: 
                break;
        }
    }

    public void refreshAddFavoriteButton(){
        addFavoriteSprite.SetActive(false);
        addUnFavoriteSprite.SetActive(true);
    }
    public void refreshItemFavoriteButton(){
        itemFavoriteSprite.SetActive(globalVariables.isFavorite());
        itemUnFavoriteSprite.SetActive(!globalVariables.isFavorite());
    }

    public void refreshEditFavoriteButton(){
        editFavoriteSprite.SetActive(globalVariables.isFavorite());
        editUnFavoriteSprite.SetActive(!globalVariables.isFavorite());
    }

    public void refreshSettingsFavoriteButton(){
        settingsFavoriteSprite.SetActive(globalVariables.isShowOnlyFavorite());
        settingsUnFavoriteSprite.SetActive(!globalVariables.isShowOnlyFavorite());
    }

    public void onShareButtonClick(){
        StartCoroutine(shareVocabulary());
    }
    
    // Start is called before the first frame update
    private IEnumerator shareVocabulary(){
        yield return new WaitForEndOfFrame();

	    NativeShare share = new NativeShare();
        string filePath = Path.Combine(Application.persistentDataPath, "Vocabulary", "SaveData", "Vocabulary.vf");
        //Debug.Log(filePath);
        share.AddFile(filePath);
        share.SetSubject("Subject").SetText("Im sharing the vocabulary" );//.SetUrl( "https://github.com/yasirkula/UnityNativeShare" );
        //share.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) );
        share.Share();

	    // Share on WhatsApp only, if installed (Android only)
	    //if( NativeShare.TargetExists( "com.whatsapp" ) )
	    //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }

    public void onAcceptButtonClick(){
        string itemKey;
        string itemValue;
        string itemHyperlink;
        bool success;

        switch (globalVariables.getCurrentMenuName()){
            case "Add Item Menu":
                itemKey = inputFieldsController.getAddKeyInputField().text.Trim();
                itemValue = inputFieldsController.getAddValueInputField().text.Trim();
                itemHyperlink = inputFieldsController.getAddHyperlinkInputField().text.Trim();
                if (itemKey.Length == 0){
                    getRecursiveFindChild(inputFieldsController.getAddKeyInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Вы забыли ввести имя термина!";
                }
                if (itemValue.Length == 0){
                    getRecursiveFindChild(inputFieldsController.getAddValueInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Вы забыли ввести описание термина!";
                }
                if ((itemKey.Length > 0) && (itemValue.Length > 0)){
                    success = flexDictionary.addItem(itemKey, new Item(itemKey, itemValue, itemHyperlink, globalVariables.getFavoriteButtonState()));
                    if (success){
                        dictionaryListController.refreshList();
                        showHideController.show("Main Menu");
                    } else {
                        showHideController.show("Replace Choice Menu");
                        Debug.Log("Ошибка! Термин '" + itemKey + 
                                  "' не добавлен! Описание: '" + itemValue + 
                                  "'. Гиперссылка: '" + itemHyperlink + 
                                  "'. isFavotrite: " + globalVariables.getFavoriteButtonState());
                    }
                }
                break;
            case "Edit Item Menu":
                itemKey = inputFieldsController.getEditKeyInputField().text.Trim();
                itemValue = inputFieldsController.getEditValueInputField().text.Trim();
                itemHyperlink = inputFieldsController.getEditHyperlinkInputField().text.Trim();
                if (itemKey.Length==0){
                    getRecursiveFindChild(inputFieldsController.getEditKeyInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Вы забыли ввести имя термина!";
                }
                if (itemValue.Length==0){
                    getRecursiveFindChild(inputFieldsController.getEditValueInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Вы забыли ввести описание термина!";
                }
                if ((itemKey.Length > 0) && (itemValue.Length > 0)){
                    // добавить случай, когда имя термина уже есть в словаре
                    success = flexDictionary.updatePickedItem(itemKey, itemValue, itemHyperlink, globalVariables.getFavoriteButtonState());
                    if (success){
                        globalVariables.setKeyPicked(itemKey);
                        showHideController.show("Item Menu");
                    } else {
                        showHideController.show("Replace Choice Menu");
                        Debug.Log("Ошибка! Термин '" + itemKey + 
                                  "' не изменен! Описание: '" + itemValue + 
                                  "'. Гиперссылка: '" + itemHyperlink + 
                                  "'. isFavotrite: " + globalVariables.getFavoriteButtonState());
                    }
                }
                break;
            case "Edit Choice Menu":
                itemKey = inputFieldsController.getEditKeyInputField().text.Trim();
                itemValue = inputFieldsController.getEditValueInputField().text.Trim();
                itemHyperlink = inputFieldsController.getEditValueInputField().text.Trim();
                success = flexDictionary.updatePickedItem(itemKey, itemValue, itemHyperlink, globalVariables.getFavoriteButtonState());
                if (success){
                    globalVariables.setKeyPicked(itemKey);
                }
                showHideController.show("Item Menu");
                break;
            case "Replace Choice Menu":
                switch (globalVariables.getPreviousMenu()){
                    case "Add Item Menu":
                        itemKey = inputFieldsController.getAddKeyInputField().text.Trim();
                        itemValue = inputFieldsController.getAddValueInputField().text.Trim();
                        itemHyperlink = inputFieldsController.getAddValueInputField().text.Trim();
                        globalVariables.setKeyPicked(itemKey);
                        flexDictionary.updatePickedItem(itemKey, itemValue, itemHyperlink, globalVariables.getFavoriteButtonState());
                        break;
                    case "Edit Item Menu":
                        itemKey = inputFieldsController.getEditKeyInputField().text.Trim();
                        itemValue = inputFieldsController.getEditValueInputField().text.Trim();
                        itemHyperlink = inputFieldsController.getEditValueInputField().text.Trim();
                        globalVariables.setKeyPicked(itemKey);
                        flexDictionary.updatePickedItem(itemKey, itemValue, itemHyperlink, globalVariables.getFavoriteButtonState());
                        break;
                    default:
                        break;
                }
                showHideController.show("Main Menu");
                break;
            case "Delete Choice Menu":
                flexDictionary.removeItem(globalVariables.getKeyPicked());
                dictionaryListController.removeItem(globalVariables.getKeyPicked());
                dictionaryListController.refreshList();
                showHideController.show("Main Menu");
                break;
            default: break;
        }
    }

    public void onDiscardButtonClick(){
        switch (globalVariables.getCurrentMenuName()){
            case "Add Item Menu":
                getRecursiveFindChild(inputFieldsController.getAddKeyInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Введите термин...";
                getRecursiveFindChild(inputFieldsController.getAddValueInputField().gameObject, "Placeholder").GetComponent<TMP_Text>().text = "Введите описание термина...";
                showHideController.show("Main Menu");
                break;
            case "Edit Item Menu":
                showHideController.show("Item Menu");
                break;
            case "Edit Choice Menu":
                showHideController.show("Edit Item Menu");
                break;
            case "Replace Choice Menu":
                showHideController.show("Main Menu");
                break;
            case "Delete Choice Menu":
                showHideController.show("Item Menu");
                break;
            default: break;
        }
    }

    public void onBackButtonClick(){
        switch (globalVariables.getCurrentMenuName()){
            case "Item Menu":
                if (globalVariables.isFavorite() != globalVariables.getFavoriteButtonState()){
                    globalVariables.setFavorite(globalVariables.getFavoriteButtonState());
                    flexDictionary.serializeDictionary();
                }
                showHideController.show("Main Menu");
                break;
            case "Settings Menu":
                globalVariables.setShowOnlyFavorite(globalVariables.getFavoriteButtonState());
                if (globalVariables.isShowOnlyFavorite()){
                    flexDictionary.showOnlyFavoritedDictionary();
                } else {
                    flexDictionary.showAllDictionary();
                }
                globalVariables.setSorting(dropdownsController.getSortOrderDropdown().value);
                globalVariables.serializeSettings();
                showHideController.show("Main Menu");
                break;
            default:
                break;
        }
    }

    public void onItemButtonClick(){
        globalVariables.setKeyPicked(EventSystem.current.currentSelectedGameObject.name);
        showHideController.show("Item Menu");
    }

    public void onDeleteItemButtonClick(){
        showHideController.show("Delete Choice Menu");
    }

    public void onSettingsButtonClick(){
        showHideController.show("Settings Menu");
    }

    public void onEditItemButtonClick(){
        inputFieldsController.getEditKeyInputField().text = globalVariables.getKeyPicked();
        inputFieldsController.getEditValueInputField().text = flexDictionary.getValue(globalVariables.getKeyPicked()).getValue();
        inputFieldsController.getEditHyperlinkInputField().text = flexDictionary.getValue(globalVariables.getKeyPicked()).getHyperlink();
        showHideController.show("Edit Item Menu");
    }

    public void onSearchButtonClick(){
        inputFieldsController.getSearchInputField().gameObject.SetActive(true);
        inputFieldsController.getSearchInputField().Select();
        inputFieldsController.getSearchInputField().ActivateInputField();
    }

    public void onSortOrderDropdownChange(){
    }

    public void onHyperlinkButtonClick(){
        Application.OpenURL(flexDictionary.getValue(globalVariables.getKeyPicked()).getHyperlink());
    }

    private GameObject getRecursiveFindChild(GameObject parent, string childName){
        foreach (Transform child in parent.transform){
            if(child.name == childName){
                return child.gameObject;
            } else{
                GameObject found = getRecursiveFindChild(child.gameObject, childName);
                if (found != null){
                return found;
                }
            }
        }   
    return null;
    }

    public void refreshColorButtons(){
        setGlobalVariables();
        switch (globalVariables.colorScheme){
            case 0:{
                onClassicColorSchemeButtonClick();
                break;
            }
            case 1:{
                onDarkColorSchemeButtonClick();
                break;
            }
            case 2:{
                onLightColorSchemeButtonClick();
                break;
            }
            default:{
                break;
            }
        }
    }

    public void onClassicColorSchemeButtonClick(){
        changeAlpha(classicColorSchemeSprite.GetComponent<Image>(), 1f);
        changeAlpha(darkColorSchemeSprite.GetComponent<Image>(), 0f);
        changeAlpha(lightColorSchemeSprite.GetComponent<Image>(), 0f);
        globalVariables.colorScheme = 0;
        colorsController.setNewColorScheme(0);
    }

    public void onDarkColorSchemeButtonClick(){
        changeAlpha(classicColorSchemeSprite.GetComponent<Image>(), 0f);
        changeAlpha(darkColorSchemeSprite.GetComponent<Image>(), 1f);
        changeAlpha(lightColorSchemeSprite.GetComponent<Image>(), 0f);
        globalVariables.colorScheme = 1;
        colorsController.setNewColorScheme(1);
    }

    public void onLightColorSchemeButtonClick(){
        changeAlpha(classicColorSchemeSprite.GetComponent<Image>(), 0f);
        changeAlpha(darkColorSchemeSprite.GetComponent<Image>(), 0f);
        changeAlpha(lightColorSchemeSprite.GetComponent<Image>(), 1f);
        globalVariables.colorScheme = 2;
        colorsController.setNewColorScheme(2);
    }

    private void changeAlpha(Image img, float newAlpha){
        var color = img.color;
        color.a = newAlpha;
        img.color = color;
    }

}
