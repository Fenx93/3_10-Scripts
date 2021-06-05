[System.Serializable]
public class StorableDeeds {

    public StorableDeeds(int objectID, string id, bool done, string text)
    {
        ID = id;
        ObjectID = objectID;
        Text = text;
        IsDone = done;
    }

    public int ObjectID { get; private set; }
    public string ID { get; private set; }
    public bool IsDone { get; set; }
    public string Text { get; set; }
    
}
