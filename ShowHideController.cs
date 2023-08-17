using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ShowHideController : MonoBehaviour
{
    private GlobalVariables globalVariables;
    private ShowHideController showHideController;
    private FlexDictionary flexDictionary;
    private ButtonsController buttonsController;
    private InputFieldsController inputFieldsController;
    private DropdownsController dropdownsController;
    private DictionaryListController dictionaryListController;
    private TextsController textsController;
    private ScrollbarsController scrollbarsController;

    public GameObject itemKeyScrollView;
    public GameObject itemValueScrollView;
    public GameObject editKeyScrollView;
    public GameObject editValueScrollView;
    public GameObject editHyperlinkScrollView;
    public GameObject addKeyScrollView;
    public GameObject addValueScrollView;
    public GameObject addHyperlinkScrollView;

    public void show(string menuName)
    {
        globalVariables.getCurrentMenu().SetActive(false);
        Debug.Log(globalVariables.getCurrentMenuName() + " switched to " + menuName);
        globalVariables.setCurrentMenu(menuName);

        switch (menuName){
            case "Main Menu":
                dictionaryListController.refreshList();
                globalVariables.getMainMenu().SetActive(true);
                //inputFieldsController.getSearchInputField().gameObject.SetActive(false);
                if (globalVariables.isTesting()){
                    globalVariables.writeToDebugLog("ShowHideController showing main menu ended");
                } else{
                    globalVariables.setDebugLogEnabled(false);
                }
                break;
            case "Add Item Menu": 
                inputFieldsController.getAddKeyInputField().text = "";
                inputFieldsController.getAddValueInputField().text = "";
                inputFieldsController.getAddHyperlinkInputField().text = "";
                globalVariables.setFavoriteButtonState(false);
                buttonsController.refreshAddFavoriteButton();
                //addValueScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(addValueScrollView.GetComponent<RectTransform>().sizeDelta.x, 500);
                globalVariables.getAddItemMenu().SetActive(true);
                break;
            case "Edit Item Menu": 
                globalVariables.setFavoriteButtonState(globalVariables.isFavorite());
                buttonsController.refreshEditFavoriteButton();
                textsController.updateEditItemMenuTexts();
                //editValueScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(editValueScrollView.GetComponent<RectTransform>().sizeDelta.x, 500);
                globalVariables.getEditItemMenu().SetActive(true);
                break;
            case "Edit Choice Menu": 
                globalVariables.getEditChoiceMenu().SetActive(true);
                break;
            case "Replace Choice Menu": 
                globalVariables.getReplaceChoiceMenu().SetActive(true);
                break;
            case "Delete Choice Menu": 
                globalVariables.getDeleteChoiceMenu().SetActive(true);
                break;
            case "Item Menu":
                textsController.getItemKeyText().text = globalVariables.getPickedItem().getKey();
                textsController.getItemKeyText().fontSize = globalVariables.keyFontSize;
                textsController.getItemValueText().text = globalVariables.getPickedItem().getValue();
                textsController.getItemValueText().fontSize = globalVariables.valueFontSize;
                textsController.updateItemMenuTexts();
                //itemValueScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(itemValueScrollView.GetComponent<RectTransform>().sizeDelta.x, 500);
                //itemValueScrollView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 100, 0);
                globalVariables.getItemMenu().SetActive(true);
                globalVariables.setFavoriteButtonState(globalVariables.isFavorite());
                buttonsController.refreshItemFavoriteButton();
                break;
            case "Test Menu": 
                globalVariables.getTestMenu().SetActive(true);
                break;
            case "Settings Menu":
                globalVariables.setFavoriteButtonState(globalVariables.isShowOnlyFavorite());
                buttonsController.refreshSettingsFavoriteButton();
                globalVariables.getSettingsMenu().SetActive(true);
                buttonsController.refreshColorButtons();
                break;
            default: 
                Debug.Log("NO SUCH MENU: " + menuName);
                break;
        }
    }

    void Start()
    {
        setGlobalVariables();
        hideAllMenus();
        if (globalVariables.isTesting()){
            globalVariables.writeToDebugLog("ShowHideController started");
        }
        //show("testMenu"); //DEBUG
        if (globalVariables.isTesting()){
            globalVariables.writeToDebugLog("ShowHideController showing main menu started");
        }
        show("Main Menu");
    }

    private void hideAllMenus(){
        globalVariables.getMainMenu().SetActive(false);
        globalVariables.getAddItemMenu().SetActive(false);
        globalVariables.getEditItemMenu().SetActive(false);
        globalVariables.getEditChoiceMenu().SetActive(false);
        globalVariables.getReplaceChoiceMenu().SetActive(false);
        globalVariables.getDeleteChoiceMenu().SetActive(false);
        globalVariables.getItemMenu().SetActive(false);
        globalVariables.getTestMenu().SetActive(false);
        globalVariables.getSettingsMenu().SetActive(false);
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            showHideController = globalVariables.getShowHideController();
            flexDictionary = globalVariables.getFlexDictionary();
            buttonsController = globalVariables.getButtonsController();
            inputFieldsController = globalVariables.getInputFieldsController();
            dropdownsController = globalVariables.getDropdownsController();
            dictionaryListController = globalVariables.getDictionaryListController();
            textsController = globalVariables.getTextsController();
            scrollbarsController = globalVariables.getScrollbarsController();
        }
        
    }

    void Update(){
        /*
        if (globalVariables.getDictionaryClass().getCreatedItemsCount() > 19){
            if (globalVariables.getScrollbarPosition() < 0.5f){
                Debug.Log(globalVariables.getScrollbarPosition());
                //Destroy(globalVariables.getListOfContent().transform.Find(globalVariables.getDictionaryClass().getDictionaryArray()[0]).gameObject);
                //Debug.Log("Destroyed Object: " + globalVariables.getDictionaryClass().getDictionaryArray()[0]);
            }
        }
        */
    }
}
