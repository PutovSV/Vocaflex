using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;

public sealed class DictionaryListController : MonoBehaviour
{
    public GameObject itemPrefab;
    public ScriptableObject itemScriptableObject;
    
    private Scrollbar listScrollbar;

    private GlobalVariables globalVariables;
    private ButtonsController buttonsController;
    private FlexDictionary flexDictionary;
    private GameObject listOfContent;
    private ColorsController colorsController;

    //private int maxListLength = 999999;
    //private int position = 0;

    private static DictionaryListController instance = null;
    private DictionaryListController(){}

    public DictionaryListController Instance { get { return instance; } }

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
        //globalVariables.getListOfContent().GetComponent<VerticalLayoutGroup>().padding.right = 1035 - (int) (Screen.width * 0.66f);
        updateTexts();
    }

    public void updateTexts(){
        foreach(Transform child in listOfContent.transform){
            foreach(Transform grandChild in child){
                if (grandChild.name == "ItemKey"){
                    grandChild.gameObject.GetComponent<TMP_Text>().fontSize = globalVariables.keyFontSize;
                    grandChild.gameObject.GetComponent<TMP_Text>().font = globalVariables.fonts[globalVariables.keyFont];
                }else {
                    grandChild.gameObject.GetComponent<TMP_Text>().fontSize = globalVariables.valueFontSize;
                    grandChild.gameObject.GetComponent<TMP_Text>().font = globalVariables.fonts[globalVariables.valueFont];
                }
            }
        }
    }

    private GameObject addItem(Item item){
        GameObject button = Instantiate(itemPrefab);
        //button.GetComponent<RectTransform>().sizeDelta = new Vector2(0, globalVariables.keyFontSize * 2.5f);
        button.name = item.getKey();
        button.GetComponent<Image>().color = colorsController.getElementBackgroundColor();
        button.GetComponent<Button>().onClick.AddListener(delegate {
            buttonsController.onItemButtonClick();
            });
        button.transform.SetParent(listOfContent.transform);
        TMP_Text itemKey = button.transform.Find("ItemKey").GetComponent<TMP_Text>();
        TMP_Text itemValue = button.transform.Find("ItemValue").GetComponent<TMP_Text>();
        //itemKey.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, globalVariables.keyFontSize * 2.5f);
        //itemKey.transform.localScale = new Vector3(1, 1, 1);
        itemKey.color = colorsController.getElementTextColor();
        itemKey.text = item.getKey();
        itemKey.fontSize = globalVariables.keyFontSize;
        itemKey.font = globalVariables.fonts[globalVariables.keyFont];
        //itemKey.transform.position = new Vector3(10, globalVariables.valueFontSize * 0.5f, 0);
        itemValue.color = colorsController.getElementTextColor();
        //Debug.Log(item.Value.getValue());
        itemValue.text = item.getValue();
        itemValue.fontSize = globalVariables.valueFontSize;
        itemValue.font = globalVariables.fonts[globalVariables.valueFont];
        return button;
        //Debug.Log("item " + item.Key + " added");
    }
/*
    public void removeItem(string key){
        foreach(Transform child in listOfContent.transform){
            if (child.name == key){
                Destroy(child.gameObject);   
            }
        }
    }
*/
    public void refreshList(){
        Debug.Log("Обновление списка");
        setGlobalVariables();
        
        clearItemsList();

        //Thread childThread = new Thread(setItemsList);
        //childThread.Start();

        setItemsList();
    }

    private void clearItemsList(){
        foreach (Transform child in listOfContent.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            listOfContent = globalVariables.getListOfContent();
            listScrollbar = GameObject.Find("ListScrollbar").GetComponentInChildren<Scrollbar>();
            buttonsController = globalVariables.getButtonsController();
            flexDictionary = globalVariables.getFlexDictionary();
            colorsController = globalVariables.getColorsController();
        }
    }

    public float getScrollbarPosition(){
        return listScrollbar.value;
    }

    public void setScrollbarPosition(float position){
        listScrollbar.value = position;
    }
/*
    void Update(){
        int calculatedPosition = (int) Math.Round((globalVariables.listStartingPosition - listOfContent.GetComponent<RectTransform>().position.y) / -160);
        globalVariables.replaceDebugLog(calculatedPosition.ToString());
        if (calculatedPosition != position){
            position = calculatedPosition;
            refreshList();
        }
    }
*/
    public void setItemsList(){
        setGlobalVariables();
        foreach(Item item in flexDictionary.getDictionary()){
            addItem(item);
            Debug.Log(item.getKey());
        }
        Debug.Log("Dictionary List Controller: ItemsList set.");
    }

}
