using System.Collections;
using UnityEngine;
using Assets.Scripts.Serializibles;

public class Drag : MonoBehaviour {

    //scripts
    private GameManager gm;
    private AudioController audioController;
    private UIController ui;
    private DragUI dragUI;
    private Option rightOption, leftOption;
    private Scoreboard score;
    private GameObject character;

    [HideInInspector] public int mood, money, health, popularity;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool answerChosen = false;

    private bool isMouseOver = false;

    [SerializeField] private float statsPreviewDistance = 3f, choiceReleaseDistance = 3f;
    [SerializeField] private GameEvent startingQuest;

    private Vector3 initialCardPosition;
    [Header("Settings for first time drag display")]
    [SerializeField] private float smoothMove = 1.25f;
    [SerializeField] private float waitTime = 0.1f;

    [SerializeField] private bool doInitialDrag = true;
    private bool doingInitialDrag = false;

    private struct Option
    {
        public int Popularity { get; private set; }
        public int Health { get; private set; }
        public int Mood { get; private set; }
        public int Money { get; private set; }
        public bool IsLinked { get { return NextEvent != null; } }
        public GameEvent NextEvent { get; private set; }
        public bool AvailableNewEvents { get { return AddedEvents != null; } }
        public GameEvent[] AddedEvents { get; private set; }
        public bool IsDeedUnlocked { get { return Deed != null; } }
        public GameDeed Deed { get; private set; }

        public Option(int popularity, int health, int mood, int money, GameEvent nextEvent, GameEvent[] addedEvents, GameDeed deed)
        {
            Popularity = popularity;
            Health = health;
            Mood = mood;
            Money = money;
            NextEvent = nextEvent;
            AddedEvents = addedEvents;
            Deed = deed;
        }

        public void ResetStats()
        {
            Popularity = 0;
            Health = 0;
            Mood = 0;
            Money = 0;
        }
    }

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
        Vector3 leftTargetDragPos = new Vector3(-(choiceReleaseDistance + 0.5f), 0, 100);
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
                ReleaseToChoose(rightOption);
            }
            // Released at the left size
            else if (this.transform.position.x < -choiceReleaseDistance)
            {
                ReleaseToChoose(leftOption);
            }
            // Return dragable card to the start position
            this.transform.position = initialCardPosition;
        }

        //when object is at start position, make text and icons transparent
        if (Mathf.Abs(this.transform.position.x) <= statsPreviewDistance)
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
            // display icon changes
            IconChanges(rightOption);
        }
        // show left option text and left icon changes
        else if (this.transform.position.x < -statsPreviewDistance)
        {
            dragUI.leftText.enabled = true;
            // display icon changes
            IconChanges(leftOption);
        }
    }

    private void ReleaseToChoose(Option selectedOption)
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
        if (selectedOption.Popularity != 0) TestColor(selectedOption.Popularity, ui.popularityIcon);
        if (selectedOption.Health != 0) TestColor(selectedOption.Health, ui.healthIcon);
        if (selectedOption.Mood != 0) TestColor(selectedOption.Mood, ui.moodIcon);
        if (selectedOption.Money != 0) TestColor(selectedOption.Money, ui.moneyIcon);

        this.money = selectedOption.Money;
        this.popularity = selectedOption.Popularity;
        this.health = selectedOption.Health;
        this.mood = selectedOption.Mood;

        if (selectedOption.AvailableNewEvents)
        {
            gm.UnlockNewEvents(selectedOption.AddedEvents);
        }
        if (selectedOption.IsDeedUnlocked)
        {
            score.UploadDeeds(selectedOption.Deed.deedId);
        }

        if (selectedOption.IsLinked)
        {
            gm.NextScripted(selectedOption.NextEvent);
        }
        else
        {
            gm.Next();
        }
    }

    private void IconChanges(Option selectedOption)
    {
        ChangeIcon(ui.healthIcon, selectedOption.Health); // health
        ChangeIcon(ui.moneyIcon, selectedOption.Money); // money
        ChangeIcon(ui.moodIcon, selectedOption.Mood); // mood
        ChangeIcon(ui.popularityIcon, selectedOption.Popularity); // popularity
    }

    public void GetData(GameEvent currentEvent)
    {
        //alive
        if (!isDead)
        {
            GameQuest currentQuest = (GameQuest)currentEvent;
            //load new quest
            Character character = currentQuest.character.GetComponent(typeof(Character)) as Character;

            GameEvent[] addedEventsFromRight = currentQuest.loadRight ? currentQuest.loadedQuests : null;
            GameEvent[] addedEventsFromLeft = currentQuest.loadLeft ? currentQuest.loadedQuests : null;
            //Assign stats from the current Quest
            rightOption = new Option(currentQuest.popularityRight, currentQuest.healthRight, currentQuest.moodRight,
                currentQuest.moneyRight, currentQuest.linkedRightQuest, addedEventsFromRight, currentQuest.rightDeed);

            leftOption = new Option(currentQuest.popularityLeft, currentQuest.healthLeft, currentQuest.moodLeft,
                currentQuest.moneyLeft, currentQuest.linkedLeftQuest, addedEventsFromLeft, currentQuest.leftDeed);

            dragUI.SetUIValues(currentQuest.rightOptionText, currentQuest.leftOptionText, character.GetSprite(), currentQuest.questText, character.GetName());

            //If met new character - add it to characters portraits menu  - SOUNDS LIKE BULLSHIT TO ME - D.S.
            if (currentQuest.firstTimeMet)
            {
                this.character = currentQuest.character;
                score.UploadCharacters(this.character);
            }
        }
        //died
        else
        {
            GameDeath currentQuest = (GameDeath)currentEvent;
            //make personName, and option texts invisible
            audioController.musicSource.clip = audioController.deathClip;
            audioController.musicSource.Play();

            dragUI.SetUIValues(currentQuest.rightOptionText, currentQuest.leftOptionText, currentQuest.deathImage, currentQuest.questText, "");

            //disable main text
            dragUI.text.gameObject.SetActive(false);
            //activate death text (with special font) and assign current deaths text to it
            dragUI.deathText.gameObject.SetActive(true);
            dragUI.deathText.text = currentQuest.questText;

            //reset stats
            rightOption.ResetStats();
            leftOption.ResetStats();
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
    
    /// <summary>
    /// Change color of icon(Red - will decresae, green - will increase).
    /// Should be called only when x!= 0
    /// </summary>
    private void TestColor(int value, GameObject icon)
    {
        //green if > 0, red if < 0
        IEnumerator coroutine = (value > 0)? 
            dragUI.FadeGreen(icon) : dragUI.FadeRed(icon);

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
