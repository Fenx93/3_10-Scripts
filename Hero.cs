using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero  {
    private readonly string heroName, heroTitle;
    private readonly int total, start, end;

    public Hero(string hName, string hTitle, int years, int started, int ended)
    {
        this.heroName = hName;
        this.heroTitle = hTitle;
        this.total = years;
        this.start = started;
        this.end = ended;
    }

    public string GetName()
    {
        return heroName;
    }

    public string GetTitle()
    {
        return heroTitle;
    }

    public int GetTotal()
    {
        return total;
    }

    public string GetYears()
    {
        return start + " - " + end;
    }
}
