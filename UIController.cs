using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIController : MonoBehaviour
{
    public Image healthBar, moneyBar, popularityBar, moodBar;
    public TMP_Text heroName, deadHeroInitials, livingYearsText, currentYearText;
    public GameObject mainScreen, settingsScreen, deathScreen, menuScreen, additionalMenuScreen,
        deedsScreen, deathsListScreen, charactersListScreen, scoreListScreen, menuIcon;
    public GameObject moneyIcon, moodIcon, popularityIcon, healthIcon;

    private GameManager gameManager;


    public void Awake()
    {
        gameManager = GetComponent<GameManager>();

        //disable choices icons indicators
        healthIcon.SetActive(false);
        popularityIcon.SetActive(false);
        moneyIcon.SetActive(false);
        moodIcon.SetActive(false);
    }

    public void ChangeStats(int health, int money, int popularity, int mood)
    {
        healthBar.fillAmount = 0.01f * health;
        moneyBar.fillAmount = 0.01f * money;
        popularityBar.fillAmount = 0.01f * popularity;
        moodBar.fillAmount = 0.01f * mood;
    }

    public void Year(int livingYears, int currentYear)
    {
        string day = livingYears == 1 ? " day" : " days";
        livingYearsText.text = livingYears.ToString() + day;
        currentYearText.text = currentYear.ToString();
    }

    #region Enable and disable screens

    //load main screen without sound
    public void LoadMainScreenSoundless()
    {
        StartCoroutine("Fade");
        gameManager.menuOpened = false;
        //activate movable object
        gameManager.draggableSquare.gameObject.SetActive(true);
        menuIcon.gameObject.SetActive(true);
        //enable main text and disable the rest
        mainScreen.gameObject.SetActive(true);
        additionalMenuScreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        additionalMenuScreen.gameObject.SetActive(false);

        //deactivate srating scene
        //startText.gameObject.SetActive(false);

        //deactivate all achievable scenes

        //disable additional menu screens
        scoreListScreen.gameObject.SetActive(false);
        deedsScreen.gameObject.SetActive(false);
        deathsListScreen.gameObject.SetActive(false);
        charactersListScreen.gameObject.SetActive(false);

    }

    //load main screen
    public void LoadMainScreen()
    {
        StartCoroutine("Fade");
        gameManager.menuOpened = false;
        //enable movable object
        gameManager.draggableSquare.gameObject.SetActive(true);
        //enable main screen, disable the rest
        mainScreen.gameObject.SetActive(true);
        menuIcon.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        additionalMenuScreen.gameObject.SetActive(false);
        scoreListScreen.gameObject.SetActive(false);
        deedsScreen.gameObject.SetActive(false);
        deathsListScreen.gameObject.SetActive(false);
        charactersListScreen.gameObject.SetActive(false);

        //deactivate srating scene
        //startText.gameObject.SetActive(false);

        //deactivate all achievable scenes


    }
    //load post death scene
    public void LoadAdditionalMenuScreen()
    {
        //enable additional menu object and disable the rest
        additionalMenuScreen.gameObject.SetActive(true);
        gameManager.draggableSquare.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(false);
        mainScreen.gameObject.SetActive(false);
        menuIcon.gameObject.SetActive(false);

        scoreListScreen.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        charactersListScreen.gameObject.SetActive(false);
        deathsListScreen.gameObject.SetActive(false);
    }

    //load death screen
    public void LoadDeathScreen()
    {
        //disable movable object and main screen
        gameManager.draggableSquare.gameObject.SetActive(false);
        mainScreen.gameObject.SetActive(false);
        //activate death scene
        deathScreen.gameObject.SetActive(true);
        //MusicSource.Stop();
    }

    //load main menu screen
    public void LoadMenuScreen()
    {
        //disable movable object
        gameManager.draggableSquare.gameObject.SetActive(false);
        //activate menu screen and disable the rest
        menuScreen.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        additionalMenuScreen.gameObject.SetActive(false);

        deedsScreen.gameObject.SetActive(false);
        deathsListScreen.gameObject.SetActive(false);
        charactersListScreen.gameObject.SetActive(false);
        scoreListScreen.gameObject.SetActive(false);
    }

    #region Enable subscreens in menu
    //load settings screen
    public void LoadSettingsScreen()
    {
        //enable settings screen and disable the rest
        settingsScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
        additionalMenuScreen.gameObject.SetActive(false);
        deathsListScreen.gameObject.SetActive(false);
    }

    //load high scores screen
    public void LoadHighScoresScreen()
    {
        //enable high scores screen and disable main menu
        scoreListScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
    }

    //load deeds screen
    public void LoadDeedsScreen()
    {
        //enable high scores screen and disable main menu
        deedsScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
    }

    //load deaths screen
    public void LoadDeathsScreen()
    {
        //enable high scores screen and disable main menu 
        deathsListScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
    }

    //load characters screen
    public void LoadCharactersScreen()
    {
        //enable characters screen and disable main menu 
        charactersListScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
    }

    #endregion
    #endregion

    private IEnumerator Fade()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
}