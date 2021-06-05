[System.Serializable]
public class StorableCharacters  {

    public StorableCharacters(int objectID, string name, bool met)
    {
        ObjectID = objectID;
        Name = name;
        HasMet = met;
    }

    public string GetName()
    {
        return HasMet ?
            Name : "???";
    }

    public string Name { get; private set; }
    public int ObjectID { get; private set; }
    public bool HasMet { get; set; }


}
