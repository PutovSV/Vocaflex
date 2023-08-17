using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public sealed class FlexDictionary : MonoBehaviour
{
    private List<Item> dictionary = new List<Item>();
    private List<Item> dictionaryToShow;
    private List<Item> temporaryDictionary;
    
    public GameObject scrollView;
    public TMP_Text log;
    public bool loaded = false;

    private bool changed = false;
    private string searchText = null;
    private GlobalVariables globalVariables;
    private DictionaryListController dictionaryListController;

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
            var random = new System.Random();
            for (int i = 0; i < 1000; i++){
                dictionary.Add(new Item("Key #" + i.ToString(), "Value #" + i.ToString(), "Hyperlink #" + i.ToString(), false /*random.Next(2) == 1*/));
            }
        }
    }

    private void setGlobalVariables(){
        if (globalVariables == null){
            globalVariables = GameObject.Find("Main Camera").GetComponent<GlobalVariables>();
            dictionaryListController = globalVariables.getDictionaryListController();
        }
    }

    public void clearDictionary(){
        dictionary = new List<Item>();
    }

    public List<Item> getDictionary(){
        setGlobalVariables();
        dictionaryToShow = new List<Item>();
        dictionaryToShow = applySorting(dictionary, globalVariables.getSorting());
        dictionaryToShow = applyShowOnlyFavorite(dictionaryToShow, globalVariables.isShowOnlyFavorite());
        dictionaryToShow = getSearchResult(dictionaryToShow, searchText);     
        return dictionaryToShow;
    }

    public bool addItem(Item item){
        if (!dictionary.Contains(item)){
            dictionary.Add(item);
            if (!globalVariables.isTesting()){
                serializeDictionary();
            }
            changed = true;
            return true;
        }
        return false;
    }
 
    public bool removeItem(Item item){
        if (dictionary.Contains(item)){
            dictionary.Remove(item);
            if (!globalVariables.isTesting()){
                serializeDictionary();
            }
            changed = true;
            return true;
        }
        return false;
    }

    public bool removeItem(string key){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i].getKey() == key){
                dictionary.Remove(dictionary[i]);
                if (!globalVariables.isTesting()){
                    serializeDictionary();
                }
                changed = true;
                return true;
            }
        }
        return false;
    }

    public bool updateItem(Item item){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i].getKey() == globalVariables.getPickedItem().getKey()){
                dictionary[i] = item;
                if (!globalVariables.isTesting()){
                    serializeDictionary();
                }
                changed = true;
                return true;
            }
        } 
        return false;
    }

    public Item getItem(string key){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i].getKey() == key){
                return dictionary[i];
            }
        }
        return null;
    }

    public string getValue(string key){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i].getKey() == key){
                return dictionary[i].getValue();
            }
        }
        return null;
    }

    public bool isFavorite(string key){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i].getKey() == key){
                return dictionary[i].isFavorite();
            }
        }
        return false;
    }

    public void setFavorite(Item item, bool favorite){
        for(int i = 0; i < dictionary.Count; i++){
            if (dictionary[i] == item){
                dictionary[i].setFavorite(favorite);
            }
        }
    }

    public void setSearchText(string text){
        searchText = text;
    }

    //DEBUG
    public void getAllItems(){
        Debug.Log("-------- Values in dictionary: " + dictionary.Count + " --------");
        foreach(Item item in dictionary)
            Debug.Log("Key: " + item.getKey() + " Value: " + item.getValue());
    }

    public string getJsonString(){
        List<string> jsonDictionary= new List<string>();
        foreach(Item item in dictionary){
            jsonDictionary.Add(JsonConvert.SerializeObject(item));
            Debug.Log(JsonConvert.SerializeObject(item)); //DEBUG
        }
        return JsonConvert.SerializeObject(jsonDictionary);
    }

    public void importVocabulary(string json){
        dictionary = JsonConvert.DeserializeObject<List<Item>>(json);
    }

    public void serializeDictionary(){
        setGlobalVariables();
        string json = getJsonString();
        string filePath = Path.Combine(globalVariables.getSaveDirectoryPath(),
                                       globalVariables.getLastSelectedDictionary() + ".vf");
        Directory.CreateDirectory(globalVariables.getSaveDirectoryPath());
        File.WriteAllText(filePath, json);
        if (globalVariables.isTesting()){
            globalVariables.writeToDebugLog("словарь сохранен: " + filePath);
        }
        Debug.Log("Словарь " + globalVariables.getLastSelectedDictionary() + " сохранен!");
        changed = false;
    }

    public void deserializeDictionary(){

        setGlobalVariables();
        globalVariables.writeToDebugLog("Загрузка словаря");
        
        dictionary = new List<Item>();
        if (File.Exists(Path.Combine(globalVariables.getSaveDirectoryPath(), 
                                     globalVariables.getLastSelectedDictionary() + ".vf"))){
            string json = File.ReadAllText(Path.Combine(globalVariables.getSaveDirectoryPath(), 
                                                        globalVariables.getLastSelectedDictionary() + ".vf"));
            List<string> jsonDictionary = new List<string>();
            try{
                jsonDictionary = JsonConvert.DeserializeObject<List<string>>(json);
                foreach(string s in jsonDictionary){
                    dictionary.Add(JsonConvert.DeserializeObject<Item>(s));
                }
                if (globalVariables.isTesting()){
                    globalVariables.writeToDebugLog("Файл словаря загружен: " + Path.Combine(globalVariables.getSaveDirectoryPath(),
                                                                                             globalVariables.getLastSelectedDictionary() + ".vf"));
                }
                Debug.Log("Словарь " + globalVariables.getLastSelectedDictionary() + " загружен!");
            } catch (System.Exception ex){
                Debug.Log("Ошибка при загрузке словаря! " + globalVariables.getLastSelectedDictionary() + " " + ex);
            }
            //controller.switchTo("mainMenu");
        } else {
            if (globalVariables.isTesting()){
                globalVariables.writeToDebugLog("Файла словаря нет: " + Path.Combine(globalVariables.getSaveDirectoryPath(),
                                                                                     globalVariables.getLastSelectedDictionary() + ".vf"));
            }
            Debug.Log("Нечего загрузить!");
        }
        //getAllItems();
        loaded = true;
    }

    public List<Item> getSearchResult(List<Item> list, string searchText){
        if (searchText != null && searchText.Length > 0){
            temporaryDictionary = new List<Item>();
            searchText = searchText.ToUpper();
            foreach(Item item in list){
                if (item.getKey().ToUpper().IndexOf(searchText) != -1){
                    temporaryDictionary.Add(item);
                }
            }
            return temporaryDictionary;
        }
        return list;
/*
        foreach(KeyValuePair<string, string> item in searchResult){
            Debug.Log(item.Key + " = " + item.Value);
        }
*/
    }

    public List<Item> applySorting(List<Item> list, int sortType){

        switch (sortType){
            case 0:{
                break;
            }
            case 1:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item1.getKey().CompareTo(item2.getKey());
                    }
                );
                break;
            }
            case 2:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item2.getKey().CompareTo(item1.getKey());
                    }
                );
                break;
            }
            case 3:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item1.getCreationDate().CompareTo(item2.getCreationDate());
                    }
                );
                break;
            }
            case 4:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item2.getCreationDate().CompareTo(item1.getCreationDate());
                    }
                );
                break;
            }
            case 5:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item1.getChangeDate().CompareTo(item2.getChangeDate());
                    }
                );
                break;
            }
            case 6:{
                list.Sort(
                    delegate(Item item1, Item item2){
                        return item2.getChangeDate().CompareTo(item1.getChangeDate());
                    }
                );
                break;
            }
            default: break;
        }

        return list;
    }

    private List<Item> applyShowOnlyFavorite(List<Item> list, bool isShowOnlyFavorite){
        if (isShowOnlyFavorite){
            temporaryDictionary = new List<Item>();
            foreach (Item item in list){
                if (item.isFavorite()){
	                temporaryDictionary.Add(item);
                }
            }
            return temporaryDictionary;
        }
        return list;
    }

    public bool isChanged(){
        return changed;
    }
}
