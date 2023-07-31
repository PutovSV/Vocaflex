using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string key;
    public string value;
    public string hyperlink;
    public DateTime creationDate;
    public DateTime changeDate;
    public bool favorite;

    public Item(){
        this.key = "";
        this.value = "";
        this.hyperlink = "";
        this.creationDate = DateTime.Now;
        this.changeDate = DateTime.Now;
        this.favorite = false;
    }

    public Item(string key, string value, bool favorite){
        this.key = key;
        this.value = value;
        this.hyperlink = "";
        this.creationDate = DateTime.Now;
        this.changeDate = DateTime.Now;
        this.favorite = favorite;
    }

    public Item(string key, string value, string hyperlink, bool favorite){
        this.key = key;
        this.value = value;
        this.hyperlink = hyperlink;
        this.creationDate = DateTime.Now;
        this.changeDate = DateTime.Now;
        this.favorite = favorite;
    }

    public Item(string key, string value, string hyperlink, DateTime creationTime, DateTime changeDate, bool favorite){
        this.key = key;
        this.value = value;
        this.hyperlink = hyperlink;
        this.creationDate = creationTime;
        this.changeDate = changeDate;
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

    public void setFavorite(bool favorite){
        this.favorite = favorite;
    }
}
