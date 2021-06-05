using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Assets.Scripts.Serializibles;

public class Scoreboard : MonoBehaviour {

    [HideInInspector]public List<Hero> heroList = new List<Hero>();

    private List<StorableCharacters> characters = new List<StorableCharacters>();
    private List<StorableDeeds> deeds = new List<StorableDeeds>();
    private List<StorableDeaths> deaths = new List<StorableDeaths>();

    [SerializeField] private GameDeath[] existingDeaths;
    [SerializeField] private Character[] existingCharacters;
    [SerializeField] private GameDeed[] existingDeeds;

    private GameObject[] deedsObjects;

    private bool isFirstGame = true;
    private ScoreboardUI scoreUI;
    
    private void Awake()
    {
        scoreUI = FindObjectOfType<ScoreboardUI>();

        deedsObjects = scoreUI.deedsObjects;
        LoadGameStatsFromFile();
        //load all characters if this is the first time the game being played
        if (isFirstGame)
        {
            LoadStartingStats();
            Debug.Log("Loading Starting Stats");
            //save first game condition using prefabs!
            isFirstGame = false;
        }
        else 
        {
            //load previously saved Stats(characters)
            Debug.Log("Loading Previous Stats");
            for (int i = 0; i < existingCharacters.Length; i++)
            {
                LoadNewCharacters(characters[i]);
            }
            for (int i = 0; i < existingDeaths.Length; i++)
            {
                LoadNewDeaths(deaths[i]);
            }
            for (int i = 0; i < existingDeeds.Length; i++)
            {
                LoadNewDeeds(deeds[i]);
            }
            SortAndPrintScore();
        }
        SaveGameStats();
    }

    //load game stats from file
    private void LoadGameStatsFromFile()
    {
        // search for save file
        if (File.Exists(Application.persistentDataPath + "/gameStatsSave.txt"))
        {
            Debug.Log("Loading Game Stats from file");
            // convert it from data to class
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.Open(Application.persistentDataPath + "/gameStatsSave.txt", FileMode.Open);
            SaveStats saveStats = (SaveStats)bf1.Deserialize(file1);
            file1.Close();

            Debug.Log("Loading Stats from file");
            isFirstGame = saveStats.firstGame; // is this a first game
            characters = saveStats.charactersSaved; // all saved characters list
            heroList = saveStats.herosSaved; // saved heroes list
            deaths = saveStats.deathsSaved; // saved heroes deaths
            deeds = saveStats.deedsSaved; // saved heroes deeds
            Debug.Log("Game stats Loaded from file");
        }
        else
        {
            Debug.Log("No game stats saved!");
            isFirstGame = true;
        }
    }

    private void LoadStartingStats()
    {
        //load all characters to system and set all sprites to black versions
        for (int i = 0; i < existingCharacters.Length; i++)
        {
            var obj = existingCharacters[i];
            StorableCharacters character = new StorableCharacters(i, obj.GetName(), false);
            characters.Add(character);
            scoreUI.UpdateCharacterObject(character, obj.GetSprite());
        }
        scoreUI.DisplayCharacters(characters);

        //load all deaths to system
        for (int i = 0; i < existingDeaths.Length; i++)
        {
            var obj = existingDeaths[i];
            StorableDeaths death = new StorableDeaths(i, obj.questId, false, new Hero(" ", " ", 0, 0, 0));
            deaths.Add(death);
            scoreUI.UpdateDeathObject(death, obj.deathImage);
        }
        scoreUI.DisplayDeaths(deaths);

        //load all achievements
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            var obj = existingDeeds[i];
            StorableDeeds deed = new StorableDeeds(i, obj.deedId, false, obj.deedText);
            deeds.Add(deed);
            scoreUI.UpdateDeedObject(deed);
        }
        scoreUI.DisplayDeeds(deeds);
    }

    //Call when character meets another person, dies or gets an achievement done to save progress
    private void SaveGameStats()
    {
        // Create instance of save stats class and pass required values
        SaveStats save = new SaveStats(isFirstGame, heroList, characters, deaths, deeds);

        // create binary formater that converts class into data and saves it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameStatsSave.txt");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Stats Saved");
    }

    #region Characters

    /// <summary>
    /// Find stored character and change it's bool value + invoke changing method for this property in menus
    /// </summary>
    /// <param name="characterGameObject"></param>
    public void UploadCharacters(GameObject characterGameObject)
    {
        Character character = characterGameObject.GetComponent<Character>();
        StorableCharacters storedCharacter = characters.Where(x => x.Name == character.GetName()).First();
        if (!storedCharacter.HasMet)
        {
            storedCharacter.HasMet = true;
            LoadNewCharacters(storedCharacter);
        }
        SaveGameStats();
    }

    //change character objects in a persons menu
    private void LoadNewCharacters(StorableCharacters character)
    {
        Character obj = existingCharacters.Where(x => x.GetName() == character.Name).First();
        scoreUI.UpdateCharacterObject(character, obj.GetSprite());
        scoreUI.DisplayCharacters(characters);
    }

    #endregion

    #region Deaths

    ///<summary>
    /// Find death object that is associated with passed in parentheses number and change it's bool value + invoke changing method for this property in menus
    ///</summary>
    public void UploadDeaths(string id, Hero x)
    {
        //if hero dies like that for the first time add this death to deaths list with hero's initials and other stats
        StorableDeaths death = deaths.Where(x => x.ID == id).First();
        if (!death.HasDied)
        {
            death.HasDied = true;
            death.Hero = x;
            LoadNewDeaths(death);
        }
        //if player has already died this way, do nothing

        SaveGameStats();
    }

    /// <summary>
    /// Change death objects in a deaths menu
    /// </summary>
    private void LoadNewDeaths(StorableDeaths death)
    {
        GameDeath obj = existingDeaths.Where(x => x.questId == death.ID).First();
        scoreUI.UpdateDeathObject(death, obj.deathImage);
        scoreUI.DisplayDeaths(deaths);
    }

    #endregion

    #region Deeds
    public void UploadDeeds(string id)
    {
        StorableDeeds deed = deeds.Where(x => x.ID == id).First();
        if (!deed.IsDone)
        {
            deed.IsDone = true;
            LoadNewDeeds(deed);
        }
        SaveGameStats();
    }

    //change deed objects in deeds menu
    private void LoadNewDeeds(StorableDeeds deed)
    {
        GameDeed obj = existingDeeds.Where(x => x.deedId == deed.ID).First();
        scoreUI.UpdateDeedObject(deed);
        //turn the sprite/ toggle according to a true/false condition
        scoreUI.DisplayDeeds(deeds);
    }

    #endregion

    public void SortAndPrintScore()
    {
        scoreUI.SortAndPrintScore(heroList);
    }
}

[System.Serializable]
public class SaveStats
{
    public bool firstGame;
    public List<Hero> herosSaved = new List<Hero>();
    public List<StorableCharacters> charactersSaved = new List<StorableCharacters>();
    public List<StorableDeaths> deathsSaved = new List<StorableDeaths>();
    public List<StorableDeeds> deedsSaved = new List<StorableDeeds>();

    public SaveStats(bool firstGame, List<Hero> herosSaved, List<StorableCharacters> charactersSaved, List<StorableDeaths> deathsSaved, List<StorableDeeds> deedsSaved)
    {
        this.firstGame = firstGame;
        this.herosSaved = herosSaved;
        this.charactersSaved = charactersSaved;
        this.deathsSaved = deathsSaved;
        this.deedsSaved = deedsSaved;
    }
}
