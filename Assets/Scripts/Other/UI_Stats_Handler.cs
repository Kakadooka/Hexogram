using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

public class UI_Stats_Handler : MonoBehaviour
{
    public int amountOfAllHexes, amountOfGuessedHexes, amountOfWrongGuesses, percentComplete;
    Hexgrid_Handler hexgridHandler;
    Save_And_Load saveAndLoad;

    TextMeshProUGUI mistakesText, percentCompleteText, timeText;
    GameObject mistakeObject, percentCompleteObject, gradeObject, timeObject, camera;
    Image mistakeImage, percentCompleteImage, gradeImage;

    public Sprite[] sprites = new Sprite[1];
    
    public float timeMistakeLight = 0f,timeClock = 0f, updateClock = 0f;

    void loadStatsIfLoadedGame(){
        if(PlayerPrefs.GetInt("newGame") == 0){
            saveAndLoad = gameObject.GetComponent<Save_And_Load>();
            Save_And_Load.UIStatsInfo uiStats = saveAndLoad.getUIStats();

            amountOfAllHexes = uiStats.amountOfAllHexes;
            amountOfGuessedHexes = uiStats.amountOfGuessedHexes;
            amountOfWrongGuesses = uiStats.amountOfWrongGuesses;
            percentComplete = uiStats.percentComplete;
            timeClock = uiStats.timeClock;

            calculateAndSetTime();
            updateUI();
            mistakesText.text = amountOfWrongGuesses.ToString();
        }
    }

    void initializeHud(){
        hexgridHandler = gameObject.GetComponent<Hexgrid_Handler>();
        mistakeObject = GameObject.Find("mistakesObject");
        mistakesText = GameObject.Find("mistakesText").GetComponent<TextMeshProUGUI>();
        mistakeImage = mistakeObject.GetComponent<Image>();

        percentCompleteObject = GameObject.Find("percentObject");
        percentCompleteText = GameObject.Find("percentText").GetComponent<TextMeshProUGUI>();
        percentCompleteImage = percentCompleteObject.GetComponent<Image>();

        timeObject = GameObject.Find("timeText"); 
        timeText = timeObject.GetComponent<TextMeshProUGUI>();

        gradeObject = GameObject.Find("gradeImage");
        gradeImage = gradeObject.GetComponent<Image>();
        gradeObject.SetActive(false);

        camera = GameObject.Find("Main Camera");

        amountOfAllHexes = (3*hexgridHandler.hexSize*hexgridHandler.hexSize)-(hexgridHandler.hexSize*3)+1;
    }

    void Start()
    {
        initializeHud();
        loadStatsIfLoadedGame();
    }

    public bool win = false;
    int wingrade;
    string screenShotDirectory;

    void saveScreenshot(){
        camera.transform.localPosition = new Vector3(0f,0f,-10f);
        camera.GetComponent<Camera>().orthographicSize = hexgridHandler.hexSize*2;
        screenShotDirectory = Application.persistentDataPath+"/Completed Games";

        if (!Directory.Exists(screenShotDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(screenShotDirectory);
            Debug.Log(screenShotDirectory);
        }
        ScreenCapture.CaptureScreenshot(screenShotDirectory+"/"+DateTime.Now.ToString("dd-MM-yyyy HH mm")+".png");
    }

    void choosePlayerGrade(){
        gradeObject.SetActive(true);
        if(amountOfWrongGuesses == 0){
            wingrade = 0;
        }
        else if(amountOfWrongGuesses <= amountOfAllHexes/30 || amountOfWrongGuesses == 1){
            wingrade = 1;
        }
        else if(amountOfWrongGuesses <= amountOfAllHexes/30*2 || amountOfWrongGuesses == 2){
            wingrade = 2;
        }
        else{
            wingrade = 3;
        }
    }

    void doWin(){
        updateClock = 0f;
        win = true;
        timeObject.transform.localPosition = new Vector2(-119f, -7f);
        choosePlayerGrade();
        saveScreenshot();
        hexgridHandler.enabled = false;
    }

    void updateUI(){
        percentComplete = amountOfGuessedHexes*100/amountOfAllHexes;
        percentCompleteText.text = percentComplete.ToString()+'%';
        BATTTERRRYYYYYY();
        if(percentComplete == 100){
            doWin();
        }
    }

    bool goToPreviousSpriteMistakeActive = false;

    void MISTAKEEEEE(){
        timeMistakeLight = 0f;
        amountOfWrongGuesses++;
        mistakesText.text = amountOfWrongGuesses.ToString();
        mistakeImage.sprite = sprites[1];
        goToPreviousSpriteMistakeActive = true;
    }
    void BATTTERRRYYYYYY(){
        //crushing all deceivers mashing nonbelievers
        if(percentComplete > 75){
            percentCompleteImage.sprite = sprites[4];
        }
        else if(percentComplete > 50){
            percentCompleteImage.sprite = sprites[3];
        }
        else if(percentComplete > 25){
            percentCompleteImage.sprite = sprites[2];
        }
    }

    public void wrongGuess(){
        amountOfGuessedHexes++;
        MISTAKEEEEE();
        updateUI();
    }
    public void correctGuess(){
        amountOfGuessedHexes++;
        updateUI();
    }

    void changeMistakeToNormalAppereance(){
        if(goToPreviousSpriteMistakeActive){
            timeMistakeLight += Time.deltaTime;
            if(timeMistakeLight > 0.1f){
                mistakeImage.sprite = sprites[0];
                goToPreviousSpriteMistakeActive = false;
            }
        }
    }
    int seconds, minutes, hours, timeClockInt;
    string strSeconds, strMinutes, strHours;
    
    void calculateAndSetTime(){
        timeClockInt = (int)timeClock;
        seconds = timeClockInt % 60;
        minutes = (timeClockInt / 60) % 60;
        hours = timeClockInt / 3600;

        strSeconds = seconds > 9 ? seconds.ToString() : '0' + seconds.ToString();
        strMinutes = minutes > 9 ? minutes.ToString() : '0' + minutes.ToString();
        strHours = hours > 9 ? hours.ToString() : '0' + hours.ToString();

        timeText.text = strHours + ':' + strMinutes + ':' + strSeconds;
    }

    void updateClockTime(){
        timeClock += Time.deltaTime;
        updateClock += Time.deltaTime;

        if(updateClock > 1f && !win){
            updateClock = 0f;
            calculateAndSetTime();
        }
    }

    void flashWin(){
        if(win){
            if((updateClock)%1<0.5f){
                gradeImage.sprite = sprites[5+(wingrade*2)];
            }
            else{
                gradeImage.sprite = sprites[6+(wingrade*2)];
            }
        }
    }

    void Update()
    {
        changeMistakeToNormalAppereance();
        updateClockTime();
        flashWin();
    }
}
