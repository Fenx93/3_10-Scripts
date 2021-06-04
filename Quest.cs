using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
// Old way to do quest objects - changed to testEvent, testQuest and testDeath
public class Quest : ScriptableObject {

    public string questName, questText, leftOptionText, rightOptionText;
    //public Character character;
    //public string questgiverName;
    public Sprite deathImage;
    [Header("Death specifics")]
    public bool deathLeft, deathRight;

    [Header("Left stats")]
    public int healthLeft, moneyLeft, moodLeft, popularityLeft;

    [Header("Right stats")]
    public int healthRight;
    public int moneyRight;
    public int moodRight;
    public int popularityRight;
    
    [Header("Affects and loads")]
    public bool isLinkedLeft;
    public bool isLinkedRight;

   // public bool combatLeft;
   // public bool combatRight;

    public bool loadLeft;
    public bool looadRight;
    //public Combat combat;

    public Quest linkedLeftQuest;
    public Quest linkedRightQuest;

    //No idea what this one is for
    public Quest[] loadedQuests;
   // public Sprite image;

    public bool firstTimeMet = false;
    public GameObject character;
}
