using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero  {
    private readonly int start, end;

    public Hero(string name, string title, int years, int started, int ended)
    {
        Name = name;
        Title = title;
        Total = years;
        this.start = started;
        this.end = ended;
    }

    public string Name { get; private set; }
    public string Title { get; private set; }
    public int Total { get; private set; }

    public string GetYears()
    {
        return start + " - " + end;
    }
}
