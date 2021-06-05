using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardUI : MonoBehaviour
{
    public GameObject[] charsObjects;
    public GameObject[] deedsObjects;
    public GameObject[] deathObjects;

    [SerializeField] private Sprite black;

    [SerializeField] private TMP_Text deedsText, deathsText, charactersText;
    [SerializeField] private TMP_Text completedDeedsText, completedDeathsText, completedCharactersText;
    [SerializeField] private Image deedsFill, deathsFill, charactersFill;

    [SerializeField] private TMP_Text highScore1, highScore2, highScore3;

    [SerializeField] private TMP_Text[] highScores;

    public void UpdateCharacterObject(StorableCharacters character, Sprite sprite)
    {
        var obj = charsObjects[character.ObjectID];
        if (character.HasMet)
        {
            obj.GetComponent<Image>().sprite = sprite;
            obj.GetComponentInChildren<TMP_Text>().text = character.GetName();
        }
        else
        {
            obj.GetComponent<Image>().sprite = black;
            obj.GetComponentInChildren<TMP_Text>().text = "???";
        }
    }
    public void DisplayCharacters(List<StorableCharacters> characters)
    {
        float x = 0.0f;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].HasMet)
                x++;
        }
        completedCharactersText.text = x.ToString();
        charactersText.text = "  \\" + characters.Count + " characters met";

        charactersFill.fillAmount = (x == 0) ?
            0 : x / characters.Count;
    }


    public void DisplayDeaths(List<StorableDeaths> deaths)
    {
        float x = 0.0f;
        for (int i = 0; i < deaths.Count; i++)
        {
            if (deaths[i].HasDied)
                x++;
        }
        completedDeathsText.text = x.ToString();
        deathsText.text = "  \\" + deaths.Count + " deaths suffered";

        deathsFill.fillAmount = (x == 0) ?
            0 : x / deaths.Count;
    }

    public void UpdateDeathObject(StorableDeaths death, Sprite sprite)
    {
        var obj = deathObjects[death.ObjectID];
        if (death.HasDied)
        {
            obj.GetComponent<Image>().sprite = sprite;
            obj.GetComponentInChildren<TMP_Text>().text = GetHeroStatsText(death.Hero);
        }
        else
        {
            obj.GetComponent<Image>().sprite = black;
            obj.GetComponentInChildren<TMP_Text>().text = "???";
        }
    }

    public void DisplayDeeds(List<StorableDeeds> deeds)
    {
        float x = 0.0f;
        for (int i = 0; i < deedsObjects.Length; i++)
        {
            if (deeds[i].IsDone)
                x++;
        }
        completedDeedsText.text = x.ToString();
        deedsText.text = "  \\" + deedsObjects.Length + " objectives completed";

        deedsFill.fillAmount = (x == 0) ?
            0 : x / deeds.Count;

    }

    public void UpdateDeedObject(StorableDeeds deed)
    {
        var obj = deathObjects[deed.ObjectID];
        if (deed.IsDone)
        {
            obj.GetComponentInChildren<TMP_Text>().text = deed.Text;
            obj.transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(true);
        }
        else
        {
            obj.GetComponentInChildren<TMP_Text>().text = "???";
            obj.transform.Find("Background").gameObject.transform.Find("Checkmark").gameObject.SetActive(false);
        }
    }
    //sort and print hero's scores to high score menu and to menu(only 3)
    public void SortAndPrintScore(List<Hero> heroList)
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

    /// <summary>
    /// Get associated death picture
    /// </summary>
    public Sprite GetDeathPicture(StorableDeaths d, GameObject deathObj)
    {
        return d.HasDied ?
            deathObj.GetComponentInChildren<DeathObjectHolder>().deathSprite : black;
    }
}
