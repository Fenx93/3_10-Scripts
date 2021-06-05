[System.Serializable]
public class StorableDeaths
{
    public StorableDeaths(int objectID, string id, bool died, Hero h)
    {
        ObjectID = objectID;
        ID = id;
        HasDied = died;
        Hero = h;
    }

    public int ObjectID { get; private set; }
    public string ID { get; private set; }
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