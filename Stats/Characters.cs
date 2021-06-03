using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Characters  {

    private string characterName;
    private bool hasMet;
    /*private Sprite sprite;
    private Sprite black;*/

    public Characters(string name, bool met/*, Sprite image, Sprite black*/)
    {
        this.characterName = name;
        this.hasMet = met;
       /* this.sprite = image;
        this.black = black;*/
    }

    public string GetName()
    {
        if (hasMet)
        {
            return characterName;
        }
        else
        {
            return "???";
        }
    }

    public void SetMet(bool meeting)
    {
        hasMet = meeting;
    }

    public bool Met()
    {
        return hasMet;
    }

    /*public Sprite GetPicture()
    {
        if (hasMet)
        {
            return sprite;
        }
        else
        {
            return black;
        }
    }*/
}
