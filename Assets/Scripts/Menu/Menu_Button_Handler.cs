using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Button_Handler : MonoBehaviour
{
    Menu_Handler menuHandler;
    Menu_Animation_Handler menuAnimation;
    Sound_Handler soundHandler;
    Save_And_Load saveAndLoad;

    void Start(){
        menuAnimation = GameObject.Find("tutorial").GetComponent<Menu_Animation_Handler>();
        menuHandler = gameObject.GetComponent<Menu_Handler>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        saveAndLoad = gameObject.GetComponent<Save_And_Load>();

        makeFirstMenuButtons();
    }

    public void makeQuickGameButtons(){
        menuHandler.createMenuButtonOrSentence(4,2,"NEWBIE", true);
        menuHandler.createMenuButtonOrSentence(4,4,"EASY", true);
        menuHandler.createMenuButtonOrSentence(4,6,"MEDIUM", true);
        menuHandler.createMenuButtonOrSentence(4,8,"HARD", true);
        menuHandler.createMenuButtonOrSentence(29-4,2,"OMFG", true);
        menuHandler.createMenuButtonOrSentence(29-10,4,"IMPOSSIBLE", true); 
        menuHandler.createMenuButtonOrSentence(29-12,6,"HELL0IS0HERE", true);  
        menuHandler.createMenuButtonOrSentence(29-9,8,"HEROBRINE", true);
        menuHandler.createMenuButtonOrSentence(11,9,"GO0BACK", true);
        scrollTitle ="QUICKGAME-";
    }

    public void makeCustomGameButtons(){
        menuHandler.createMenuButtonOrSentence(2,2,"SIZE", false);

        menuHandler.createMenuButtonOrSentence(7,3,"005", false, "number");
        menuHandler.createMenuButtonOrSentence(5,3,"<", true, "-size");
        menuHandler.createMenuButtonOrSentence(2,3,"<<", true, "--size");
        menuHandler.createMenuButtonOrSentence(11,3,">", true, "+size");
        menuHandler.createMenuButtonOrSentence(13,3,">>", true, "++size");

        menuHandler.createMenuButtonOrSentence(21,2,"BLUE0HEXES", false);
        
        menuHandler.createMenuButtonOrSentence(23,3,"50%", false, "number");
        menuHandler.createMenuButtonOrSentence(21,3,"<", true, "-percent");
        menuHandler.createMenuButtonOrSentence(18,3,"<<", true, "--percent");
        menuHandler.createMenuButtonOrSentence(27,3,">", true, "+percent");
        menuHandler.createMenuButtonOrSentence(29,3,">>", true, "++percent");

        menuHandler.createMenuButtonOrSentence(9,5,"LEFT0NUMBERS:", false);
        menuHandler.createMenuButtonOrSentence(4,6,"TOP0RIGHT0NUMBERS:", false);
        menuHandler.createMenuButtonOrSentence(1,7,"BOTTOM0RIGHT0NUMBERS:", false);

        menuHandler.createMenuButtonOrSentence(23,5,"ENABLED0", true, "leftNumbers");
        menuHandler.createMenuButtonOrSentence(23,6,"ENABLED0", true, "topRightNumbers");
        menuHandler.createMenuButtonOrSentence(23,7,"ENABLED0", true, "bottomRightNumbers");

        menuHandler.createMenuButtonOrSentence(20,9,"EMBARK", true);

        menuHandler.createMenuButtonOrSentence(7,9,"GO0BACK", true);

        scrollTitle ="CUSTOMGAME-";  
    }

    public void makeCreditsMenuButtons(){
        menuHandler.createMenuButtonOrSentence(1,2,"MADE0BY0KAKADOOKA", false);
        menuHandler.createMenuButtonOrSentence(1,4,"SPECIAL0THANKS0TO0HYPEROUS", false);
        menuHandler.createMenuButtonOrSentence(1,5,"FOR0HELPING0ME0WITH0CODE", false);
        menuHandler.createMenuButtonOrSentence(1,6,"AND0PLAYTESTING", false);        
        menuHandler.createMenuButtonOrSentence(1,8,"GO0BACK", true);
                
        scrollTitle="CREDITS-";
    }

    public void makeFirstMenuButtons(){
        if(saveAndLoad.checkIfSaveFilesExist()){
        menuHandler.createMenuButtonOrSentence(4,3,"CONTINUE", true);
        }
        menuHandler.createMenuButtonOrSentence(29-10,4,"QUICK0GAME", true);
        menuHandler.createMenuButtonOrSentence(4,5,"CUSTOM0GAME", true);
        menuHandler.createMenuButtonOrSentence(29-11,6,"HOW0TO0PLAY", true);
        menuHandler.createMenuButtonOrSentence(4,7,"CREDITS", true);
        menuHandler.createMenuButtonOrSentence(29-4,8,"EXIT", true); 
        menuHandler.createMenuButtonOrSentence(0,10,"TWITTER.COM/KAKADOOKA00:)", true, "twitter");
        scrollTitle="HEXOGRAM-";
    }



    float time = 0f;
    string hexogramTitle = "";
    public string scrollTitle;
    int index = 0;

    void scrollHexogramTitle(){
        time += Time.deltaTime;
        if(time > 0.3f){
            time = 0f; index++; hexogramTitle = "";
            for(int i = 0; i < 33; i++){
                hexogramTitle += scrollTitle[(index+i)%scrollTitle.Length];
            }
            menuHandler.createMenuButtonOrSentence(0,0,hexogramTitle, false);
        }
    }

    void Update(){
        scrollHexogramTitle();
    }

    public int percentOfBlue = 50, size = 5, newGame = 0;
    string stringPercantage, stringSize;

    int convertBoolToInt(bool boolean){
        return boolean ? 1 : 0;
    }

    void setGameOptions(){
        PlayerPrefs.SetInt("percentage", percentOfBlue);
        PlayerPrefs.SetInt("size", size);
        PlayerPrefs.SetInt("newGame", newGame);
        PlayerPrefs.SetInt("leftNumbers", convertBoolToInt(leftNumbers));
        PlayerPrefs.SetInt("topRightNumbers", convertBoolToInt(topRightNumbers));
        PlayerPrefs.SetInt("bottomRightNumbers",convertBoolToInt(bottomRightNumbers));       
    }

    void OnDisable(){
        if(scrollTitle != "HEXOGRAM-"){
            setGameOptions();
        }
        else{
            PlayerPrefs.SetInt("newGame", 0);
        }
    }

    void changePercentage(int amount){
        percentOfBlue += amount;
        if(percentOfBlue>100){percentOfBlue=100;}
        else if(percentOfBlue<0){percentOfBlue=0;}
    }

    string addPercentSignToTheEnd(int percent){
        if(percent < 10){
            return "0"+percent.ToString()+"%";
        }
        else if(percent < 100){
            return percent.ToString()+"%";
        }
        else{
            return percent.ToString();
        }
    }

    string addZerosToTheBeginning(int number){
        if(number < 10){
            return "00"+number.ToString();
        }
        else if(number < 100){
            return "0"+number.ToString();
        }
        else{
            return number.ToString();
        }
    }

    void changeSize(int amount){
        size += amount;
        if(amount > 0){
            if(size>50){
            menuHandler.createMenuButtonOrSentence(3,4,"GONNA0TAKE0A0LONG0TIME0000000", false);
            if(size>100){
                menuHandler.createMenuButtonOrSentence(3,4,"ARE0YOU0CRAZY0OR0WHAT?0000000", false);
                if(size>150){
                    menuHandler.createMenuButtonOrSentence(3,4,"STOP0IT0YOUR0CPU0WILL0EXPLODE", false);
                    if(size>=200){
                        menuHandler.createMenuButtonOrSentence(3,4,"THATS0IT0NO0MORE0FOR0YOU00000", false);
                        size=200;
                    }}}}
        }
        else if(size<1){size=1;}
    }

    void setQuickGameVariablesAndPlayGame(int size, int percentOfBlue){
        this.size = size; this.percentOfBlue = percentOfBlue;
        leftNumbers = true; topRightNumbers = true; bottomRightNumbers = true;

        SceneManager.LoadScene("main game");
    }

    bool leftNumbers = true, topRightNumbers = true, bottomRightNumbers = true;

    public void doAnActionDependingOnWhatButtonWasPressed(string buttonType){
        switch(scrollTitle){
            case "HEXOGRAM-":
                switch(buttonType){
                    case "QUICK0GAME":
                        newGame = 1;
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeQuickGameButtons();
                    break;
                    case "CUSTOM0GAME":
                        size = 5; percentOfBlue = 50; newGame = 1;
                        leftNumbers = true; topRightNumbers = true; bottomRightNumbers = true;

                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeCustomGameButtons();
                    break;
                    case "HOW0TO0PLAY":
                        menuAnimation.makeTutorialGoUp();
                    break;
                    case "CONTINUE":
                        newGame = 0;
                        soundHandler.playButtonClick();
                        saveAndLoad.setHexgridDataIntoPlayerPrefs();
                        SceneManager.LoadScene("main game");
                    break;
                    case "CREDITS":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeCreditsMenuButtons();
                    break;
                    case "EXIT":
                        Application.Quit();
                        Debug.Log("quit");
                    break;
                    case "twitter":
                        soundHandler.playButtonClick();
                        Application.OpenURL("https://twitter.com/kakadooka");
                    break;
                }
            break;
            case "QUICKGAME-":
                soundHandler.playButtonClick();
                switch(buttonType){
                    case "NEWBIE":
                        setQuickGameVariablesAndPlayGame(2,50);
                    break;
                    case "EASY":
                        setQuickGameVariablesAndPlayGame(4,50);
                    break;
                    case "MEDIUM":
                        setQuickGameVariablesAndPlayGame(6,50);
                    break;
                    case "HARD":
                        setQuickGameVariablesAndPlayGame(10,50);
                    break;
                    case "OMFG":
                        setQuickGameVariablesAndPlayGame(15,50);
                    break;
                    case "IMPOSSIBLE":
                        setQuickGameVariablesAndPlayGame(20,45);
                    break;
                    case "HELL0IS0HERE":
                        setQuickGameVariablesAndPlayGame(25,40);
                    break;
                    case "HEROBRINE":
                        setQuickGameVariablesAndPlayGame(30,35);
                    break;
                    case "GO0BACK":
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break;
                }
            break;
            case "CUSTOMGAME-":
                switch(buttonType){
                    case "-size":
                        changeSize(-1);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "--size":
                        changeSize(-10);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "+size":
                        changeSize(1);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "++size":
                        changeSize(10);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "-percent":
                        changePercentage(-1);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "--percent":
                        changePercentage(-10);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "+percent":
                        changePercentage(1);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "++percent":
                        changePercentage(10);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "leftNumbers":
                        menuHandler.createMenuButtonOrSentence(23,5,leftNumbers?"DISABLED":"ENABLED0", true, "leftNumbers");
                        leftNumbers = leftNumbers?false:true;
                    break;
                    case "topRightNumbers":
                        menuHandler.createMenuButtonOrSentence(23,6,topRightNumbers?"DISABLED":"ENABLED0", true, "topRightNumbers");
                        topRightNumbers = topRightNumbers?false:true;
                    break;
                    case "bottomRightNumbers":
                        menuHandler.createMenuButtonOrSentence(23,7,bottomRightNumbers?"DISABLED":"ENABLED0", true, "bottomRightNumbers");
                        bottomRightNumbers = bottomRightNumbers?false:true;
                    break;
                    case "EMBARK":
                        soundHandler.playButtonClick();
                        SceneManager.LoadScene("main game");
                    break;
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break;
                }
            break;            
            case "CREDITS-":
                switch(buttonType){
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break; 
                }
            break;          
        }
    }


}
