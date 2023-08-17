using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Item
{
    [JsonProperty("key")]
    private string key;
    [JsonProperty("value")]
    private string value;
    [JsonProperty("hyperlink")]
    private string hyperlink;
    [JsonProperty("creationDate")]
    private DateTime creationDate;
    [JsonProperty("changeDate")]
    private DateTime changeDate;
    [JsonProperty("favorite")]
    private bool favorite;

    public Item(string key, string value, string hyperlink, bool favorite){
        this.key = key;
        this.value = value;
        this.hyperlink = hyperlink;
        this.creationDate = DateTime.Now;
        this.changeDate = DateTime.Now;
        this.favorite = favorite;
    }

    public void updateItem(string key, string value, string hyperlink, bool favorite){
        this.key = key;
        this.value = value;
        this.hyperlink = hyperlink;
        this.changeDate = DateTime.Now;
        this.favorite = favorite;
    }

    public string getKey(){
        return key;
    }

    public string getValue(){
        return value;
    }

    public string getHyperlink(){
        return hyperlink;
    }

    public DateTime getCreationDate(){
        return creationDate;
    }

    public DateTime getChangeDate(){
        return changeDate;
    }

    public bool isFavorite(){
        return favorite;
    }

    public void setKey(string key){
        this.key = key;
    }

    public void setValue(string value){
        this.value = value;
    }

    public void setHyperlink(string hyperlink){
        this.hyperlink = hyperlink;
    }
    
    public void setFavorite(bool favorite){
        this.favorite = favorite;
    }
}
