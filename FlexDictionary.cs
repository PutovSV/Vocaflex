using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public sealed class FlexDictionary : MonoBehaviour
{
    private Dictionary<string, Item> dictionary = new Dictionary<string, Item>(); // == 0
    private Dictionary<string, Item> filteredDictionary;                          // == 1
    private Dictionary<string, Item> temporaryDictionary;                         // == 2
    
    public GameObject scrollView;
    public TMP_Text log;

    private bool changed = false;
    private int activeDictionary = 0;
    private GlobalVariables globalVariables;
    private DictionaryListController dictionaryListController;
    public string[] dictionaryArray;

    private static FlexDictionary instance = null;
    private FlexDictionary(){}   

    public FlexDictionary Instance { get { return instance; } }

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
            for (int i = 0; i < 1000; i++){
                dictionary.Add(i.ToString(), new Item(i.ToString(), i.ToString(), false));
            }
            setActiveDictionary(0);
            dictionaryListController.refreshList();
        }
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            dictionaryListController = globalVariables.getDictionaryListController();
        }
    }

    public Dictionary<string, Item> getDictionary(){
        return getActiveDictionary();
    }

    public bool addItem(string key, Item value){
        if (!dictionary.ContainsKey(key)){
            dictionary.Add(key, value);
            sortDictionary(globalVariables.getSorting());
            if (globalVariables.isShowOnlyFavorite()){
                showOnlyFavoritedDictionary();
            }
            if (!globalVariables.isTesting()){
                serializeDictionary();
            }
            setDictionaryArray();
            changed = true;
            return true;
        }
        return false;
    }
 
    public bool removeItem(string itemKey){
        if (dictionary.ContainsKey(itemKey)){
            dictionary.Remove(itemKey);
            if (!globalVariables.isTesting()){
                serializeDictionary();
            }
            setDictionaryArray();
            changed = true;
            return true;
        }
        return false;
    }

    private bool updateItem(string key, Item value){
        dictionary[key] = value;
        //globalVariables.getListOfContent().transform.Find(key).transform.Find("ItemValue").GetComponent<TMP_Text>().text = value.getDescription();//сделать это в DictionaryListController
        if (!globalVariables.isTesting()){
            serializeDictionary();
        }
        changed = true;
        return true;
    }

    public bool updatePickedItem(string itemKey, string itemValue, string itemHyperlink, bool isFavorite){
        if (!dictionary.ContainsKey(itemKey)){
            dictionary.Remove(globalVariables.getKeyPicked());
            return addItem(itemKey, new Item(itemValue, itemHyperlink, isFavorite));
        } else{
            return updateItem(itemKey, new Item(itemValue, itemHyperlink, isFavorite));
        }
    }

    public KeyValuePair<string, Item> getItem (string key){
        return new KeyValuePair<string, Item> (key, dictionary[key]);
    }

    public Item getValue(string key){
        return dictionary[key];
    }

    public bool isFavorite(string key){
        return dictionary[key].isFavorite();
    }

    public void setFavorite(string key, bool favorite){
        dictionary[key].setFavorite(favorite);
        changed = true;
    }

    //DEBUG
    public void getAllItems(){
        Debug.Log("-------- Values in vocabulary: " + dictionary.Count + " --------");
        foreach(var item in dictionary)
            Debug.Log("Key: " + item.Key + " Value: " + item.Value);
    }

    public string getJsonString(){
        Dictionary<string, string> jsonDictionary= new Dictionary<string, string>();
        foreach(KeyValuePair<string, Item> item in dictionary){
            jsonDictionary.Add(item.Key, JsonConvert.SerializeObject(item.Value));
        //    Debug.Log(JsonConvert.SerializeObject(item.Value)); //DEBUG
        }
        return JsonConvert.SerializeObject(jsonDictionary);
    }

    public void importVocabulary(string json){
        dictionary = JsonConvert.DeserializeObject<Dictionary<string, Item>>(json);
        dictionaryListController.refreshList();
    }

    public void serializeDictionary(){
        string json = getJsonString();
        string filePath = Path.Combine(globalVariables.getSaveDirectoryPath(),
                                       globalVariables.getSelectedDictionaryPath() + ".vf");
        Directory.CreateDirectory(globalVariables.getSaveDirectoryPath());
        File.WriteAllText(filePath, json);
        if (globalVariables.isTesting()){
            globalVariables.writeToDebugLog("словарь сохранен: " + filePath);
        }
        Debug.Log("Словарь сохранен!");
        changed = false;
        setActiveDictionary(0);
        dictionaryListController.setItemsList();
    }

    public void deserializeDictionary(){

        setGlobalVariables();
        globalVariables.writeToDebugLog("Загрузка словаря");
        
        if (File.Exists(Path.Combine(globalVariables.getSaveDirectoryPath(), 
                                     globalVariables.getSelectedDictionaryPath() + ".vf"))){
            string json = File.ReadAllText(Path.Combine(globalVariables.getSaveDirectoryPath(), 
                                                        globalVariables.getSelectedDictionaryPath() + ".vf"));
            Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();
            jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            
            foreach(KeyValuePair<string, string> item in jsonDictionary){
                dictionary.Add(item.Key, JsonConvert.DeserializeObject<Item>(item.Value));
                //Debug.Log(item.Value);
            }
            if (globalVariables.isTesting()){
                globalVariables.writeToDebugLog("Файл словаря загружен: " + Path.Combine(globalVariables.getSaveDirectoryPath(),
                                                                                         globalVariables.getSelectedDictionaryPath() + ".vf"));
            }
            Debug.Log("Словарь загружен!");
            //controller.switchTo("mainMenu");
        } else {
            if (globalVariables.isTesting()){
                globalVariables.writeToDebugLog("Файла словаря нет: " + Path.Combine(globalVariables.getSaveDirectoryPath(),
                                                                                     globalVariables.getSelectedDictionaryPath() + ".vf"));
            }
            Debug.Log("Нечего загрузить!");
        }

        setActiveDictionary(0);
        dictionaryListController.setItemsList();
    }

    public void getSearchResult(string searchText){
        temporaryDictionary = new Dictionary<string, Item>();
        foreach(KeyValuePair<string, Item> item in dictionary){
            if (item.Key.IndexOf(searchText) != -1){
                temporaryDictionary.Add(item.Key, item.Value);
            }
        filteredDictionary = temporaryDictionary;
        setActiveDictionary(1);
        }
/*
        foreach(KeyValuePair<string, string> item in searchResult){
            Debug.Log(item.Key + " = " + item.Value);
        }
*/
    }

    public void sortDictionary(int sortType){

        List<KeyValuePair<string, Item>> filteredList = getActiveDictionary().ToList();

        switch (sortType){
            case 0:{
                break;
            }
            case 1:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair1.Key.CompareTo(pair2.Key);
                    }
                );
                break;
            }
            case 2:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair2.Key.CompareTo(pair1.Key);
                    }
                );
                break;
            }
            case 3:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair1.Value.getCreationDate().CompareTo(pair2.Value.getCreationDate());
                    }
                );
                break;
            }
            case 4:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair2.Value.getCreationDate().CompareTo(pair1.Value.getCreationDate());
                    }
                );
                break;
            }
            case 5:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair1.Value.getChangeDate().CompareTo(pair2.Value.getChangeDate());
                    }
                );
                break;
            }
            case 6:{
                filteredList.Sort(
                    delegate(KeyValuePair<string, Item> pair1,
                    KeyValuePair<string, Item> pair2){
                        return pair2.Value.getChangeDate().CompareTo(pair1.Value.getChangeDate());
                    }
                );
                break;
            }
            default: break;
        }

        temporaryDictionary = new Dictionary<string, Item>();

        foreach (KeyValuePair<string, Item> item in filteredList){
	        temporaryDictionary.Add(item.Key, item.Value);
            //Debug.Log(item.Key);
        }

        filteredDictionary = temporaryDictionary;
        setActiveDictionary(1);
    }

    public void showOnlyFavoritedDictionary(){
        
        temporaryDictionary = new Dictionary<string, Item>();

        foreach (KeyValuePair<string, Item> item in getActiveDictionary()){
            if (item.Value.isFavorite()){
	            temporaryDictionary.Add(item.Key, item.Value);
            }
        }
        filteredDictionary = temporaryDictionary;
        setActiveDictionary(1);
    }

    public void showAllDictionary(){
        setActiveDictionary(0);
    }

    public Dictionary<string, Item> getActiveDictionary(){
        switch(activeDictionary){
            case 0:{
                return dictionary;
            }
            case 1:{
                return filteredDictionary;
            }
            case 2:{
                return temporaryDictionary;
            }
            default: return null;
        }
    }

    private void setActiveDictionary(int number){
        activeDictionary = number;
        setDictionaryArray();
    }

    private void setDictionaryArray(){
        dictionaryArray = new string[getActiveDictionary().Count];
        int n = 0;
        foreach(KeyValuePair<string, Item> item in getActiveDictionary()){
            dictionaryArray[n++] = item.Key;
        }
        Debug.Log("Dictionary array set");
    }

    public string[] getDictionaryArray(){
        return dictionaryArray;
    }

    public bool isChanged(){
        return changed;
    }
}
