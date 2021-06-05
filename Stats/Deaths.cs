[System.Serializable]
public class Deaths
{
    public Deaths(string id, string name, bool died, Hero h)
    {
        ID = id;
        Name = name;
        HasDied = died;
        Hero = h;
    }

    public string ID { get; private set; }
    public string Name { get; private set; }
    public Hero Hero { get; set; }
    public bool HasDied { get; set; }

    public string GetHero()
    {
        return !HasDied ? 
            "???" : Hero.Name + " " + Hero.Title + "\n" + Hero.GetYears();
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