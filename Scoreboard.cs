using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class Scoreboard : MonoBehaviour {

    [HideInInspector]public List<Hero> heroList = new List<Hero>();

    private List<Characters> characters = new List<Characters>();
    [SerializeField] private GameObject[] chars;
    [SerializeField] private Sprite[] characterSprites;

    private List<Deeds> deeds = new List<Deeds>();
    [SerializeField] private GameObject[] deedsObjects;

    private List<Deaths> deaths = new List<Deaths>();
    [SerializeField] private GameObject[] deathObjects;

    private bool isFirstGame = true;

    [SerializeField] private Sprite black;

    [SerializeField] private TMP_Text deedsText, deathsText, charactersText;
    [SerializeField] private Image deedsFill, deathsFill, charactersFill;

    [SerializeField] private TMP_Text highScore1, highScore2, highScore3;

    [SerializeField] private TMP_Text[] highScores;
    
    private void Awake()
    {
        LoadGameStats();
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
            for (int i = 0; i < chars.Length; i++)
            {
                LoadNewCharacters(chars[i], i);
            }
            for (int i = 0; i < deathObjects.Length; i++)
            {
                LoadNewDeaths(deathObjects[i], deaths[i]);
            }
            for (int i = 0; i < deedsObjects.Length; i++)
            {
                LoadNewDeeds(deedsObjects[i], deeds[i]);
            }
            SortAndPrintScore();
        }
        SaveGameStats();
    }

    //load game stats from file
    private void LoadGameStats()
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
        for (int i = 0; i < chars.Length; i++)
        {
            characters.Add(new Characters(chars[i].GetComponentInChildren<TMP_Text>().text, false));
            chars[i].GetComponent<SpriteRenderer>().sprite = GetCharacterPicture(characters[i], i);
        /*}
        //assign characters to required GameObjects
        for (int i = 0; i < chars.Length; i++)
        {*/
            LoadNewCharacters(chars[i], i);
        }
        DisplayCharacters();

        //load all deaths to system
        for (int i = 0; i < deathObjects.Length; i++)
        {
            var obj = deathObjects[i];
            deaths.Add(new Deaths(obj.GetComponentInChildren<DeathObjectHolder>().id, obj.GetComponentInChildren<TMP_Text>().text, false, new Hero(" ", " ", 0, 0, 0)));
            obj.GetComponent<SpriteRenderer>().sprite = GetDeathPicture(deaths[i], obj);
            obj.GetComponentInChildren<TMP_Text>().text = deaths[i].GetHero();
        }
        DisplayDeaths();

        //load all achievements
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            deeds.Add(new Deeds(deedsObjects[i].GetComponentInChildren<TMP_Text>().text, false));
            deedsObjects[i].transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(false);
        }
        DisplayDeeds();
    }

    //Call when character meets another person, dies or gets an achievement done to save progress
    private void SaveGameStats()
    {
        // create instance of save stats class and give it required values
        SaveStats save = new SaveStats(isFirstGame, heroList, characters, deaths, deeds);

        // create binary formater that converts class into data and saves it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameStatsSave.txt");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Stats Saved");
    }

    #region Characters
    //find character object that is passed in parentheses and change it's bool value + invoke changing method for this property in menus
    public void UploadCharacters(GameObject g)
    {
        for (int z = 0; z < chars.Length; z++)
        {
            if (g.gameObject.name == chars[z].gameObject.name)
            {
                characters[z].HasMet = true;
                LoadNewCharacters(chars[z], z);
            }
        }
        SaveGameStats();
    }

    //change character objects in a persons menu
    private void LoadNewCharacters(GameObject g, int i)
    {
        g.GetComponent<Image>().sprite = GetCharacterPicture(characters[i], i);
        g.GetComponentInChildren<TMP_Text>().text = characters[i].GetName();
        DisplayCharacters();
    }

    //get associated character picture
    private Sprite GetCharacterPicture(Characters c, int x)
    {
        return c.HasMet ?
            characterSprites[x] : black;
    }

    private void DisplayCharacters()
    {
        float x = 0.0f;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].HasMet)
                x++;
        }
        charactersText.text = x + "\\" + characters.Count + " characters met";

        charactersFill.fillAmount = (x == 0) ?
            0 : x / characters.Count;
    }

    #endregion

    #region Deaths

    ///<summary>
    /// Find death object that is associated with passed in parentheses number and change it's bool value + invoke changing method for this property in menus
    ///</summary>
    public void UploadDeaths(string id, Hero x)
    {
        //if hero dies like that for the first time add this death to deaths list with hero's initials and other stats
        Deaths death = deaths.Where(x => x.ID == id).First();
        if (!death.HasDied)
        {
            death.HasDied = true;
            death.Hero = x;
            // TO DO: FIX THIS!
            //LoadNewDeaths(deathObjects[i], death);
        }
        //if player has already died this way, do nothing

        SaveGameStats();
    }

    /// <summary>
    /// Change death objects in a deaths menu
    /// </summary>
    private void LoadNewDeaths(GameObject deathObject, Deaths d)
    {
        deathObject.GetComponent<SpriteRenderer>().sprite = GetDeathPicture(d, deathObject);
        deathObject.GetComponentInChildren<TMP_Text>().text = d.GetHero();
        DisplayDeaths();
    }

    /// <summary>
    /// Get associated death picture
    /// </summary>
    private Sprite GetDeathPicture(Deaths d, GameObject deathObj)
    {
        return d.HasDied ?
            deathObj.GetComponentInChildren<DeathObjectHolder>().deathSprite : black;
    }

    private void DisplayDeaths()
    {
        float x = 0.0f;
        for (int i = 0; i < deaths.Count; i++)
        {
            if (deaths[i].HasDied)
                x++;
        }
        deathsText.text = x + "\\" + deaths.Count + " deaths suffered";

        deathsFill.fillAmount = (x == 0) ?
            0 : x / deaths.Count;
    }

    #endregion

    #region Deeds
    public void UploadDeeds(GameObject g)
    {
        for (int z = 0; z < deedsObjects.Length; z++)
        {
            if (g.gameObject.name == deedsObjects[z].gameObject.name)
            {
                deeds[z].IsDone = true;
                LoadNewDeeds(deedsObjects[z], deeds[z]);
            }
        }
        SaveGameStats();
    }

    //change deed objects in deeds menu
    private void LoadNewDeeds(GameObject d, Deeds deed)
    {
        Debug.Log("Loading New deeds: ");
        d.transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(deed.IsDone);
        //turn the sprite/ toggle according to a true/false condition
        DisplayDeeds();
    }

    private void DisplayDeeds()
    {
        float x = 0.0f;
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            if (deeds[i].IsDone)
                x++;
        }
        deedsText.text = x + "\\" + deedsObjects.Length + " objectives completed";

        deedsFill.fillAmount = (x == 0) ?
            0 : x / deeds.Count;

    }
    
    #endregion

    //sort and print hero's scores to high score menu and to menu(only 3)
    public void SortAndPrintScore()
    {
        // sort heroes list
        if (heroList.Count > 0)
        {
            heroList.Sort(delegate (Hero a, Hero b) {
                return (b.Total).CompareTo(a.Total);
            });
        }

        int maxListValue = heroList.Count > 10 ? 
            10 : heroList.Count;

        // iterate through list, print first three heroes in the menu stats, also print first ten in the high scores screen
        for (int i = 0; i < maxListValue; i++)
        {
            Hero hero = heroList[i];
            switch (i)
            {
                case 0:
                    highScore1.text = GetHeroStatsText(hero);
                    break;
                case 1:
                    highScore2.text = GetHeroStatsText(hero);
                    break;
                case 2:
                    highScore3.text = GetHeroStatsText(hero);
                    break;
                default:
                    break;
            }

            highScores[i].text = GetHeroStatsText(hero);
        }
    }

    private string GetHeroStatsText(Hero hero)
    {
        return hero.Name + " " + "\n" + hero.Total + " years of heroism  (" + hero.GetYears() + ")";
    }

}

[System.Serializable]
public class SaveStats
{
    public bool firstGame;
    public List<Hero> herosSaved = new List<Hero>();
    public List<Characters> charactersSaved = new List<Characters>();
    public List<Deaths> deathsSaved = new List<Deaths>();
    public List<Deeds> deedsSaved = new List<Deeds>();

    public SaveStats(bool firstGame, List<Hero> herosSaved, List<Characters> charactersSaved, List<Deaths> deathsSaved, List<Deeds> deedsSaved)
    {
        this.firstGame = firstGame;
        this.herosSaved = herosSaved;
        this.charactersSaved = charactersSaved;
        this.deathsSaved = deathsSaved;
        this.deedsSaved = deedsSaved;
    }
}
