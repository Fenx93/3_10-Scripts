using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Scoreboard : MonoBehaviour {

    [SerializeField]
    public List<Hero> heroList = new List<Hero>();

    [SerializeField]
    private List<Characters> characters = new List<Characters>();
    [SerializeField]
    private GameObject[] chars;
    [SerializeField]
    private Sprite[] characterSprites;

    [SerializeField]
    private List<Deeds> deeds = new List<Deeds>();
    [SerializeField]
    private GameObject[] deedsObjects;

    [SerializeField]
    private List<Deaths> deaths = new List<Deaths>();
    [SerializeField]
    private Sprite[] deathSprites;
    [SerializeField]
    private GameObject[] deathObjects;

    [SerializeField]
    private bool firstGame;

    [SerializeField]
    private int totalCards;

    [SerializeField]
    private Sprite black;

    [SerializeField]
    private TMP_Text deedsText, deathsText, charactersText;
    [SerializeField]
    private Image deedsFill, deathsFill, charactersFill;

    //private int max = 0;
    [SerializeField]
    private TMP_Text tx1, tx2, tx3;

    [SerializeField]
    private TMP_Text[] highScores;
    
    private void Awake()
    {
        LoadGameStats();
        //load all characters if this is the first time the game being played
        if (firstGame)
        {
            LoadStartingStats();
            Debug.Log("Loading Starting Stats");
            //save first game condition using prefabs!
            firstGame = false;
        }
        else 
        {
            //load previously saved Stats(characters)
            Debug.Log("Loading Previous Stats");
            LoadPreviousStats();
        }
        SaveGameStats();
    }

    private void LoadPreviousStats()
    {
        //assign previous characters to required GameObjects
        for (int i = 0; i < chars.Length; i++)
        {
            LoadNewCharacters(chars[i], i);
        }
        for (int i = 0; i < deathObjects.Length; i++)
        {
            LoadNewDeaths(deathObjects[i], i);
        }
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            LoadNewDeeds(deedsObjects[i], deeds[i]);
        }
        SortAndPrintScore();
    }

    private SaveStats CreateSaveStatsGameObject()
    {
        SaveStats saveStats = new SaveStats
        {
            firstGame = firstGame,
            herosSaved = heroList,
            charactersSaved = characters,
            deathsSaved = deaths,
            deedsSaved = deeds
        };

        return saveStats;
    }

    //call when character meets another person, dies or gets an achievement done to save progress
    public void SaveGameStats()
    {
        // create instance of save stats class and give it required values
        SaveStats save = CreateSaveStatsGameObject();

        // create binary formater that converts class into data and saves it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameStatsSave.txt");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Stats Saved");
    }

    //load game stats from file
    public void LoadGameStats()
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
            firstGame = saveStats.firstGame; // is this a first game
            characters = saveStats.charactersSaved; // all saved characters list
            heroList = saveStats.herosSaved; // saved heroes list
            deaths = saveStats.deathsSaved; // saved heroes deaths
            deeds = saveStats.deedsSaved; // saved heroes deeds
            Debug.Log("Game stats Loaded from file");
        }
        else
        {
            Debug.Log("No game stats saved!");
            firstGame = true;
        }
    }

    private void LoadStartingStats()
    {
        //load all characters to system and set all sprites to black versions
        for (int i = 0; i < chars.Length; i++)
        {
            characters.Add(new Characters(chars[i].GetComponentInChildren<TMP_Text>().text, false/*, chars[i].GetComponent<SpriteRenderer>().sprite, black*/));
            chars[i].GetComponent<SpriteRenderer>().sprite = GetCharacterPicture(characters[i], i);
        }
        //assign characters to required GameObjects
        for (int i = 0; i < chars.Length; i++)
        {
            LoadNewCharacters(chars[i], i);
        }

        //load all deaths to system
        for (int i = 0; i < deathSprites.Length; i++)
        {
            deaths.Add(new Deaths(deathObjects[i].GetComponentInChildren<TMP_Text>().text, false, new Hero(" ", " ", 0, 0, 0)/*, deathsSprites[i], black*/));
            deathObjects[i].GetComponent<SpriteRenderer>().sprite = GetDeathPicture(deaths[i], i);
            deathObjects[i].GetComponentInChildren<TMP_Text>().text = deaths[i].GetHero();
        }

        //load all achievements
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            deeds.Add(new Deeds(deedsObjects[i].GetComponentInChildren<TMP_Text>().text, false));
            //Debug.Log(deedsObjects[i].transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject);
            deedsObjects[i].transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(false);
        }
        PrintDeeds();
        PrintCharacters();
        PrintDeaths();
    }

    //find character object that is passed in parentheses and change it's bool value + invoke changing method for this property in menus
    public void UploadCharacters(GameObject g)
    {
        for (int z = 0; z < chars.Length; z++)
        {
            if (g.gameObject.name == chars[z].gameObject.name)
            {
                characters[z].SetMet(true);
                LoadNewCharacters(chars[z], z);
            }
        }
        SaveGameStats();
    }

    //find death object that is associated with passed in parentheses number and change it's bool value + invoke changing method for this property in menus
    public void UploadDeaths(int i, Hero x)
    {
        //if hero dies like that for the first time add this death to deaths list with hero's initials and other stats
            if (!deaths[i].Died())
            {
                deaths[i].SetDeath(true);
                deaths[i].SetHero(x);
                LoadNewDeaths(deathObjects[i], i);
            }
        //if player has already died this way, do nothing
    
        SaveGameStats();
    }

    public void UploadDeeds(GameObject g)
    {
        for (int z = 0; z < deedsObjects.Length; z++)
        {
            if (g.gameObject.name == deedsObjects[z].gameObject.name)
            {
                deeds[z].SetDeed(true);
                LoadNewDeeds(deedsObjects[z], deeds[z]);
            }
        }
        SaveGameStats();
    }
    //change character objects in persons menu
    private void LoadNewCharacters(GameObject g, int i)
    {
        g.GetComponent<Image>().sprite = GetCharacterPicture(characters[i],i);
        g.GetComponentInChildren<TMP_Text>().text = characters[i].GetName();
        PrintCharacters();
    }

    //change death objects in deaths menu
    private void LoadNewDeaths(GameObject death, int i)
    {
        death.GetComponent<SpriteRenderer>().sprite = GetDeathPicture(deaths[i], i);
        death.GetComponentInChildren<TMP_Text>().text = deaths[i].GetHero();
        PrintDeaths();
    }

    //change deed objects in deeds menu
    private void LoadNewDeeds(GameObject d, Deeds deed)
    {
        Debug.Log("Loading New deeds: ");
        d.transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(deed.Done());
        //turn the sprite/ toggle accoording to true/false condition
        PrintDeeds();
    }

    //get associated death picture
    private Sprite GetDeathPicture(Deaths d, int x)
    {
        return d.Died() ? deathSprites[x] : black;
    }

    //get associated character picture
    private Sprite GetCharacterPicture(Characters c, int x)
    {
        return c.Met() ? characterSprites[x] : black;
    }

    private void PrintCharacters()
    {
        float x = 0.0f;
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i].Met())
            {
                x++;
            }
        }
        charactersText.text = x + "\\" + characters.Count + " characters met";

        charactersFill.fillAmount = x == 0 ? 0 : x / characters.Count;
    }

    private void PrintDeaths()
    {
        float t = 0.0f;
        for (int i = 0; i < deaths.Count; i++)
        {
            if (deaths[i].Died())
            {
                t++;
            }
        }
        deathsText.text = t + "\\" + deaths.Count + " deaths suffered";

        deathsFill.fillAmount = t == 0 ? 0 : t / deaths.Count;
    }

    private void PrintDeeds()
    {
        float x = 0.0f;
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            if (deeds[i].Done())
            {
                x++;
            }
        }
        deedsText.text = x + "\\" + deedsObjects.Length + " objectives completed";

        deedsFill.fillAmount = x == 0 ? 0 : x / deeds.Count;
            
    }

    //sort and print hero's scores to high score menu and to menu(only 3)
    public void SortAndPrintScore()
    {
        // sort heroes list
        if (heroList.Count > 0)
        {
            heroList.Sort(delegate (Hero a, Hero b) {
                return (b.GetTotal()).CompareTo(a.GetTotal());
            });
        }

        int maxListValue = heroList.Count > 10 ? 10 : heroList.Count;

        // iterate through list, print first three heroes in the menu stats, also print first ten in the high scores screen
        for (int i = 0; i < maxListValue; i++)
        {
            Hero hero = heroList[i];
            switch (i)
            {
                case 0:
                    tx1.text = GetHeroStatsText(hero);
                    break;
                case 1:
                    tx2.text = GetHeroStatsText(hero);
                    break;
                case 2:
                    tx3.text = GetHeroStatsText(hero);
                    break;
                default:
                    break;
            }

            highScores[i].text = GetHeroStatsText(hero);
        }
    }

    private string GetHeroStatsText(Hero hero)
    {
        return hero.GetName() + " " + "\n" + hero.GetTotal() + " years of heroism  (" + hero.GetYears() + ")";
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
}
