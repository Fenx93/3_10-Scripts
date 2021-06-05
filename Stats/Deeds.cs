using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deeds {

    private readonly string deedName;

    public Deeds(string name, bool done)
    {
        this.deedName = name;
        IsDone = done;
    }

    public string GetName()
    {
        return !IsDone ?
            deedName : "???";
    }

    public bool IsDone { get; set; }
    
}
