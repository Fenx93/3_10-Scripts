using Assets.Scripts.Serializibles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameQuest[] quests, randomQuests;
    private readonly List<GameQuest> unlockedQuests = new List<GameQuest>();
    private int currentQuest = 0;

    private int playerMood, playerPopularity, playerMoney, playerHealth;

    //Other scripts
    private Drag drag;
    private Scoreboard scoreboard;
    private UIController ui;

    [HideInInspector]
    public string currentHeroName;
    // Added when an achievement is unlocked by this hero
    private readonly string currentHeroTitle = "";
    private readonly string[] heroNames = { "Ivan", "Nikita", "Jaroslav", "Dobrynia", "Ilja", "Afanasy", "Alexander", "Bogdan", "Boris", "Branislav", "Casimir", 
        "Danko", "Dobroslav", "Dragomir", "Gavrilo", "Igor", "Jaromir", "Jaroslav", "Kvetoslav", "Lev", "Miloslav", "Milomir", "Miroslav", "Mstislav", "Mykola", 
        "Neven", "Oleg", "Radek", "Radim", "Radion", "Radoslav", "Ratimir", "Rostislav", "Slavko", "Slawomir", "Stanislav", "Svetislav", "Taras" };

    private int livingYears, currentYear;
    [SerializeField] private GameDeath[] deaths;

    public GameObject draggableSquare;
    [SerializeField] private CascadeAnimating anim;

    [HideInInspector] public bool menuOpened = false;

    void Start ()
    {
        drag = FindObjectOfType<Drag>();
        scoreboard = GetComponent<Scoreboard>();
        ui = GetComponent<UIController>();
        //Instantiate(obj);
        currentYear = 0;
        ui.SetYearUI(livingYears, currentYear);
        SetStartingStats();
        GenerateName("");
        currentQuest++;
        for (int i = 0; i < randomQuests.Length; i++)
            unlockedQuests.Add(randomQuests[i]);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && menuOpened == false)
        {
            menuOpened = true;
            ui.LoadAdditionalMenuScreen();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && menuOpened)
        {
            menuOpened = false;
            ui.LoadMainScreenSoundless();
        }
        if (Input.GetKeyDown(KeyCode.Space))
            anim.Animate();
    }

    public void SetStartingStats ()
    {
        playerMood = 50;
        playerPopularity = 50;
        playerMoney = 50;
        playerHealth = 50;
        livingYears = 0;
        ui.ChangeStats(playerHealth, playerMoney, playerPopularity, playerMood);
    }

    // Method that generates hero name and returns parameters to their basci values
    public void GenerateName(string prevName)
    {
        currentHeroName = heroNames[Random.Range(0, heroNames.Length)];

        if (currentHeroName == prevName)
            GenerateName(currentHeroName);
        else
            ui.heroName.text = currentHeroName;
    }

    #region Load Next
    // Is called when loading next quest from quests chain
    public void NextScripted(GameEvent load)
    {
        //unlockedQuests.Remove(unlockedQuests[currentQuest]);
        AddDragStats();
        TestDeadNext(load);
        //currentQuest++;
    }

    // Is called when loading another random quest
    public void Next()
    {
        livingYears++;
        currentYear++;
        ui.SetYearUI(livingYears, currentYear);

        AddDragStats();
        //select a random next quest
        currentQuest = Random.Range(0, unlockedQuests.Count);
        //test if the player is dead, then load needed quest
        TestDeadNext(null);
        //dr.GetData(unlockedQuests[currentQuest]);
        //Should ignore locked questlines. So add to unlockedQuests list only next parts of the quest and remove previous ones
        //dr.GetData(unlockedQuests[Random.value(1, unlockedQuests.lenght)]);
    }

    private void AddDragStats()
    {
        playerMood += drag.mood;
        playerHealth += drag.health;
        playerPopularity += drag.popularity;
        playerMoney += drag.money;
        ui.ChangeStats(playerHealth, playerMoney, playerPopularity, playerMood);
    }
    #endregion

    #region Dying

    private void TestDeadNext(GameEvent nextScriptedQuest)
    {
        if (!drag.isDead)
        {
            GameDeath testDeath = null;
            // health/popularity/money/mood = 0
            if (playerHealth <= 0)          testDeath = SelectDeath("lowHealth");
            else if (playerPopularity <= 0) testDeath = SelectDeath("lowPopularity");
            else if (playerMood <= 0)       testDeath = SelectDeath("lowMood");
            else if (playerMoney <= 0)      testDeath = SelectDeath("lowMoney");
            // health/popularity/money/mood = 100
            else if (playerHealth >= 100)   testDeath = SelectDeath("highHealth");
            else if (playerPopularity >= 100) testDeath = SelectDeath("highPopularity");
            else if (playerMood >= 100)     testDeath = SelectDeath("highMood");
            else if (playerMoney >= 100)    testDeath = SelectDeath("highMoney");

            if (testDeath != null)
            {
                drag.isDead = true;
                livingYears--;
                HasDied(testDeath);
                scoreboard.UploadDeaths(testDeath.questId, new Hero(currentHeroName, currentHeroTitle, livingYears, currentYear - livingYears, currentYear));
            }
            else
            {
                drag.GetData(nextScriptedQuest != null ?
                    nextScriptedQuest : unlockedQuests[currentQuest]);
            }
        }

    }
    private GameDeath SelectDeath(string deathId) { return deaths.Where(x => x.questId == deathId).First(); }

    // Write dead hero stats to list
    private void HasDied(GameDeath death)
    {
        //save stats and scores
        scoreboard.heroList.Add(new Hero(currentHeroName, currentHeroTitle, livingYears, currentYear - livingYears, currentYear));
        //sort stats and return them in correct order in menu scene
        scoreboard.SortAndPrintScore();

        int startYear = currentYear - livingYears;
        ui.deadHeroInitials.text = currentHeroName + " " + currentHeroTitle + "\n" + "(" + startYear + " - " + currentYear + ")";
        //change death scene
        drag.GetData(death);
        
    }

    #endregion

    /* private Save CreateScoreboardGameObject()
     {
         Save save = new Save();
         int i = 0;

         save.scr = scr;
         save.shots = shots;

         return save;
     }*/

}
