using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Drag : MonoBehaviour {

    private bool linkedRight, linkedLeft;
    private Quest nextRight, nextLeft;

    //scripts
    private GameManager gm;
    private AudioController audioController;
    private UIController ui;
    private DragUI dragUI;

    //gainable option paramethers
    private int moneyRight, moodRight, popularityRight, healthRight,
        moneyLeft, moodLeft, popularityLeft, healthLeft;

    //meeting character
    private Scoreboard score;
    private GameObject character;

    [HideInInspector] public int mood, money, health, popularity;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool answerChosen = false;

    private bool isMouseOver = false;

    [SerializeField] private float statsPreviewDistance = 3f, choiceReleaseDistance = 3f;
    [SerializeField] private Quest startingQuest;

    private Vector3 initialCardPosition;
    [Header("Settings for first time drag display")]
    [SerializeField] private float smoothMove = 1.25f;
    [SerializeField] private float waitTime = 0.1f;

    [SerializeField] private bool doInitialDrag = true;
    private bool doingInitialDrag = false;


    private void Awake()
    {
        //set initial position to transform position, as this object is a child of ui, so regular (0,0,0) are somewhat different
        initialCardPosition = this.transform.position;
        dragUI = GetComponent<DragUI>();
        audioController = FindObjectOfType<AudioController>();
        gm = FindObjectOfType<GameManager>();
        score = FindObjectOfType<Scoreboard>();
        ui = FindObjectOfType<UIController>();
        dragUI.rightText.enabled = false;
        dragUI.leftText.enabled = false;
    }

    private void Start()
    {
        GetData(startingQuest);
        // Call player guiding square dragging
        if (doInitialDrag)
        {
            Debug.LogWarning("Initial dragging started");
            doingInitialDrag = true;
            doInitialDrag = false;
            StartCoroutine(nameof(ExplainingDrag));
        }
    }

    // Should be called only when starting game - shows player that game is played by dragging this square to the left or right
    private IEnumerator ExplainingDrag()
    {
        float currentTime = 0.0f;
        float lerpValue = 0.0f;
        var posStart = initialCardPosition;
        Vector3 leftTargetDragPos = new Vector3(-(choiceReleaseDistance+0.5f), 0, 100);
        Vector3 rightTargetDragPos = new Vector3((choiceReleaseDistance + 0.5f), 0, 100);
        var posEnd = rightTargetDragPos;

        while(doingInitialDrag)
        {
            lerpValue = Mathf.InverseLerp(0, smoothMove, currentTime);
            currentTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(posStart, posEnd, lerpValue);
            if (this.transform.position == posEnd)
            {
                posStart = posEnd;
                currentTime = 0.0f;
                posEnd = (posEnd == rightTargetDragPos) ? 
                    leftTargetDragPos : rightTargetDragPos;
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }

    }
    
    private void Update()
    {
        // card movement
        if (Input.GetMouseButton(0) && isMouseOver)
        {
            doingInitialDrag = false;
            Vector3 targetScreenPos  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetScreenPos.z = this.transform.position.z;
            this.transform.position = targetScreenPos;
        }

        // Released mouse button
        if (Input.GetMouseButtonUp(0))
        {
            dragUI.rightText.enabled = false;

            // Released at the right size
            if (this.transform.position.x > choiceReleaseDistance)
            {
                ReleaseToChoose(popularityRight, healthRight, moodRight, moneyRight, linkedRight, nextRight);
            }
            // Released at the left size
            else if (this.transform.position.x < -choiceReleaseDistance)
            {
                ReleaseToChoose(popularityLeft, healthLeft, moodLeft, moneyLeft, linkedLeft, nextLeft);
            }
            // Return dragable card to the start position
            this.transform.position = initialCardPosition;
        }

        //when object is at start position, make text and icons transparent
        if (Mathf.Abs(this.transform.position.x) <= statsPreviewDistance)//this.transform.position.x >= -1 || this.transform.position.x <= 1) // 
        {
            dragUI.rightText.enabled = false;
            dragUI.leftText.enabled = false;

            //make icons transparent
            if (!answerChosen)
            {
                ui.healthIcon.SetActive(false);
                ui.popularityIcon.SetActive(false);
                ui.moneyIcon.SetActive(false);
                ui.moodIcon.SetActive(false);
            }
        }
        // show right option text and right icon changes
        else if (this.transform.position.x > statsPreviewDistance)
        {
            dragUI.rightText.enabled = true;

            // icon changes
            ChangeIcon(ui.healthIcon, healthRight); // health
            ChangeIcon(ui.moneyIcon, moneyRight); // money
            ChangeIcon(ui.moodIcon, moodRight); // mood
            ChangeIcon(ui.popularityIcon, popularityRight); // popularity
        }
        // show left option text and left icon changes
        else if (this.transform.position.x < -statsPreviewDistance)
        {
            dragUI.leftText.enabled = true;

            // icon changes
            ChangeIcon(ui.healthIcon, healthLeft); // health
            ChangeIcon(ui.moneyIcon, moneyLeft); // money
            ChangeIcon(ui.moodIcon, moodLeft); // mood
            ChangeIcon(ui.popularityIcon, popularityLeft); // popularity
        }

    }

    public void GetData(Quest currentQuest)
    {
        //alive
        if (!isDead)
        {
            //load new quest
            Character character = currentQuest.character.GetComponent(typeof(Character)) as Character;
            nextLeft = currentQuest.linkedLeftQuest;
            nextRight = currentQuest.linkedRightQuest;
            dragUI.SetUIValues(currentQuest.rightOptionText, currentQuest.leftOptionText, character.GetSprite(), currentQuest.questText, character.GetName());
            linkedRight = currentQuest.isLinkedRight;
            linkedLeft = currentQuest.isLinkedLeft;

            //Assign stats from the current Quest
            moneyRight = currentQuest.moneyRight;
            moodRight = currentQuest.moodRight;
            popularityRight = currentQuest.popularityRight;
            healthRight = currentQuest.healthRight;

            moneyLeft = currentQuest.moneyLeft;
            moodLeft = currentQuest.moodLeft;
            popularityLeft = currentQuest.popularityLeft;
            healthLeft = currentQuest.healthLeft;

            //If met new character - add it to characters portraits menu
            if (currentQuest.firstTimeMet)
            {
                this.character = currentQuest.character;
                score.UploadCharacters(this.character);
            }
        }
        //died
        else
        {
            //make personName, and option texts invisible
            audioController.musicSource.clip = audioController.deathClip;
            audioController.musicSource.Play();

            nextLeft = currentQuest.linkedLeftQuest;
            nextRight = currentQuest.linkedRightQuest;

            dragUI.SetUIValues(currentQuest.rightOptionText, currentQuest.leftOptionText, currentQuest.deathImage, currentQuest.questText, "");

            linkedRight = currentQuest.isLinkedRight;
            linkedLeft = currentQuest.isLinkedLeft;

            //disable main text
            dragUI.text.gameObject.SetActive(false);
            //activate death text (with special font) and change it to the death
            dragUI.deathText.gameObject.SetActive(true);
            dragUI.deathText.text = currentQuest.questText;

            //reset stats
            moneyRight = 0;
            moodRight = 0;
            popularityRight = 0;
            healthRight = 0;
            moneyLeft = 0;
            moodLeft = 0;
            popularityLeft = 0;
            healthLeft = 0;

        }
    }


    private void ReleaseToChoose(int popularity, int health, int mood, int money, bool isLinkedQuest, Quest nextQuest)
    {
        if (isDead)
        {
            //change hero name and years
            gm.SetStartingStats();
            gm.GenerateName(gm.currentHeroName);
            //should load menu
            //load previous text paramethers
            dragUI.text.gameObject.SetActive(true);
            dragUI.deathText.gameObject.SetActive(false);
            isDead = false;
            ui.LoadDeathScreen();
        }

        // Change color of icon (Red - will decresae, green - will increase)
        if (popularity != 0) TestColor(popularity, ui.popularityIcon);
        if (health != 0)     TestColor(health, ui.healthIcon);
        if (mood != 0)       TestColor(mood, ui.moodIcon);
        if (money != 0)      TestColor(money, ui.moneyIcon);

        this.money = money;
        this.popularity = popularity;
        this.health = health;
        this.mood = mood;

        if (isLinkedQuest)
        {
            gm.NextScripted(nextQuest);
        }
        else
        {
            gm.Next();
        }
    }


    private void ChangeIcon(GameObject icon, int value)
    {
        if (Mathf.Abs(value) >= 15)
        {
            //set to big size
            icon.SetActive(true);
            icon.transform.localScale = new Vector3(30, 30, 1);
        }
        else if (Mathf.Abs(value) > 0)
        {
            //set to small size
            icon.SetActive(true);
            icon.transform.localScale = new Vector3(20, 20, 1);
        }
    }

    // Change color of icon (Red - will decresae, green - will increase).
    // Should be called only when x!= 0
    private void TestColor(int value, GameObject icon)
    {
        //green if > 0, red if < 0
        IEnumerator coroutine = value > 0? dragUI.FadeGreen(icon) : dragUI.FadeRed(icon);

        answerChosen = true;
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(coroutine);
        }
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    /* Android
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }*/
}
