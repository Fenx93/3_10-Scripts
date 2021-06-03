using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deaths
{
    private string deathName;
    private bool hasDied;
    private Hero hero;

    public Deaths(string name, bool died, Hero h)
    {
        this.deathName = name;
        this.hasDied = died;
        this.hero = h;
    }

    public string GetName()
    {
        return deathName;
    }

    public string GetHero()
    {
        if(hasDied == false)
        {
            return "???";
        }
        else
        {
            return GetHeroName() + " " + GetHeroTitle() + "\n" + GetHeroLivingYears();
        }
    }

    public string GetHeroName()
    {
        return hero.GetName();
    }

    public string GetHeroTitle()
    {
        return hero.GetTitle();
    }

    public string GetHeroLivingYears()
    {
        return hero.GetYears();
    }

    public void SetHero(Hero h)
    {
        this.hero = h;
    }
    public void SetDeath(bool dying)
    {
        hasDied = dying;
    }

    public bool Died()
    {
        return hasDied;
    }

    /*
    public Sprite GetPicture()
    {
        if (hasDied)
        {
            return sprite;
        }
        else
        {
            return black;
        }
    }*/
}