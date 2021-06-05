using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Characters  {

    private readonly string characterName;

    public Characters(string name, bool met)
    {
        this.characterName = name;
        HasMet = met;
    }

    public string GetName()
    {
        return HasMet ?
            characterName : "???";
    }

    public bool HasMet { get; set; }


}
