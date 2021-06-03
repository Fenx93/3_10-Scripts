using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deeds {

    private string deedName;
    private bool hasDone;

    public Deeds(string name, bool done)
    {
        this.deedName = name;
        this.hasDone = done;
    }

    public string GetName()
    {
        if (hasDone == false)
        {
            return deedName;
        }
        else
        {
            return "???";
        }
    }

    public void SetDeed(bool doing)
    {
        hasDone = doing;
    }

    public bool Done()
    {
        return hasDone;
    }
    
}
