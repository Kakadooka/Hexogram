using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Hexgrid_Handler : MonoBehaviour
{

    public int hexSize, leftNumbers, topRightNumbers, bottomRightNumbers;
    Pan_And_Zoom panAndZoom;
    Sound_Handler soundHandler;
    UI_Stats_Handler uiStats;
    Save_And_Load saveAndLoad;

    int percentOfTrueStates, newGame;
        

    void getComponents(){
        panAndZoom = gameObject.GetComponent<Pan_And_Zoom>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        uiStats = gameObject.GetComponent<UI_Stats_Handler>();
        saveAndLoad = gameObject.GetComponent<Save_And_Load>();
    }

    void setPlayerPrefs(){
        percentOfTrueStates = PlayerPrefs.GetInt("percentage");
        hexSize = PlayerPrefs.GetInt("size");
        newGame = PlayerPrefs.GetInt("newGame");
        leftNumbers = PlayerPrefs.GetInt("leftNumbers");
        topRightNumbers = PlayerPrefs.GetInt("topRightNumbers");
        bottomRightNumbers = PlayerPrefs.GetInt("bottomRightNumbers");  
    }

    void buildHexgrid(){
        placeHexagons();
        translateHexagons();
        placeNumbers();
    }

    void Start()
    {  
        getComponents();
        setPlayerPrefs();
        buildHexgrid();
    }

    void resetSquareOriginOnIdleMouseButtons(){
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            draggedSquareOrigin = "";
        }
        else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            UndrawThreePreviousGuidingLines();
        }
    }

    void Update(){
        resetSquareOriginOnIdleMouseButtons();
    }

    public string draggedSquareOrigin = "";
    int lastClosestAxis=0;

    bool checkIfHandlerArrayIsNotOutOfBounds(){
        if(hexHandlerArray.Count <= hexX || hexX < 0){                                       
            return false;
        }
        else if(hexHandlerArray[hexX].Count <= hexY || hexY < 0){
            return false;
        }  
        return true;
    }

    
    void changeHexStateDependingOnMouseUpInput(){
        if(checkIfHandlerArrayIsNotOutOfBounds()){
            if(Input.GetMouseButtonUp(0)){
                hexHandlerArray[hexX][hexY].guessHexIsTrue();
            }
            else if(Input.GetMouseButtonUp(1)){
                hexHandlerArray[hexX][hexY].guessHexIsFalse();
            }
            else{              
                hexHandlerArray[hexX][hexY].SetSpriteHoldAndDrag();
            }
        }
    }

    public bool isPerfect = true, anyUnknownHexes = false;
    void playConfirmSound(){
        if(anyUnknownHexes){
            if(isPerfect){
                soundHandler.playConfirmPerfect();
            }
            else{
                soundHandler.playConfirmError();
            }
        }
        isPerfect = true;
        anyUnknownHexes = false;
    }

    void selectInALineANumberOfHexesFromAStartingPosition(){
        for(int i = 0; i < panAndZoom.howManyHexesSelected; i++){
            changeHexStateDependingOnMouseUpInput();
            changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
        }
        playConfirmSound();
    }

    void drawLine(Action doSomethingToHoverLight, int startingDraggingHexX, int startingDraggingHexY){
        hexX = startingDraggingHexX; hexY = startingDraggingHexY;
        for(int i = 0; i < hexSize*2; i++){
            if(checkIfHandlerArrayIsNotOutOfBounds()){  
                doSomethingToHoverLight();
            }
            changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
        }
    }

    int startingDraggingHexX = 0, startingDraggingHexY = 0, previousStartingDraggingHexX, previousStartingDraggingHexY;
    public void UndrawThreePreviousGuidingLines(){
        previousStartingDraggingHexX = startingDraggingHexX; previousStartingDraggingHexY = startingDraggingHexY;
        hexX = previousStartingDraggingHexX; hexY = previousStartingDraggingHexY;
        changeHexVectorProperties(1,0,-1,-1,1);
        drawLine(() => hexHandlerArray[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);
        changeHexVectorProperties(0,1,1,1,0);
        drawLine(() => hexHandlerArray[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);
        changeHexVectorProperties(-1,-1,0,0,0);
        drawLine(() => hexHandlerArray[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);
    }

    public void DrawThreeGuidingLines(){
        if(!uiStats.win){
        startingDraggingHexX = hexX; startingDraggingHexY = hexY;
        changeHexVectorProperties(1,0,-1,-1,1);
        drawLine(() => hexHandlerArray[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
        changeHexVectorProperties(0,1,1,1,0);
        drawLine(() => hexHandlerArray[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
        changeHexVectorProperties(-1,-1,0,0,0);
        drawLine(() => hexHandlerArray[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
        }
    }

    void changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray(){
        hexY += hexX < hexSize-1+hexDifference ? yVectorHexLarger : yVectorHexSmaller;
        hexX += hexX < hexSize-1+hexDifference ? xVectorHexLarger : xVectorHexSmaller;
    }

    void deselectInALineTheRestOfHexesUntilOutOfBounds(){
        for(int i = 0; i <= hexSize*2; i++){
            if(lastClosestAxis!=panAndZoom.closestAxis){
                changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
            }
            if(checkIfHandlerArrayIsNotOutOfBounds()){
                hexHandlerArray[hexX][hexY].SetSpriteMaybe();
            }
            if(lastClosestAxis==panAndZoom.closestAxis){
                changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
            }
        }
    }

    int yVectorHexSmaller, yVectorHexLarger, xVectorHexSmaller, xVectorHexLarger, hexDifference;
    void changeHexVectorProperties(int yVectorHexSmaller, int yVectorHexLarger, int xVectorHexSmaller, int xVectorHexLarger, int hexDifference){
        this.yVectorHexSmaller = yVectorHexSmaller;
        this.yVectorHexLarger = yVectorHexLarger;
        this.xVectorHexSmaller = xVectorHexSmaller;
        this.xVectorHexLarger = xVectorHexLarger;
        this.hexDifference = hexDifference;
    }

    public void deselectEntireLineOnPreviousAxisIfAxisWasChanged(){
        if(lastClosestAxis!=panAndZoom.closestAxis){
            switch(lastClosestAxis){
                case 0:
                    changeHexVectorProperties(1,0,-1,-1,1);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();  
                break;
                case 1:
                    changeHexVectorProperties(1,1,0,0,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 2:
                    changeHexVectorProperties(0,1,1,1,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 3:
                    changeHexVectorProperties(-1,0,1,1,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 4:
                    changeHexVectorProperties(-1,-1,0,0,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 5:
                    changeHexVectorProperties(0,-1,-1,-1,1);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
            }   
        }
    }

    void selectHexesDependingOnClosestAxisForLineLengthAndThenDeselectTheRestOfTheLine(){
        switch(panAndZoom.closestAxis){
            case 0:
                changeHexVectorProperties(1,0,-1,-1,1);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 1:
                changeHexVectorProperties(1,1,0,0,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 2:
                changeHexVectorProperties(0,1,1,1,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 3:
                changeHexVectorProperties(-1,0,1,1,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 4:
                changeHexVectorProperties(-1,-1,0,0,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 5:
                changeHexVectorProperties(0,-1,-1,-1,1);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break; 
            
        }
    }
    public int hexX; public int hexY;
    public void selectDraggedOverHexes(){
        deselectEntireLineOnPreviousAxisIfAxisWasChanged();
        selectHexesDependingOnClosestAxisForLineLengthAndThenDeselectTheRestOfTheLine();
        lastClosestAxis = panAndZoom.closestAxis;
    }

    public bool checkIfHexIsTrueOrFalse(int x, int y, bool leftClickClicked){
        if(hexGridStatesX[x][y] == leftClickClicked){
            return true;
        }
        else{
            return false;
        }
    }

    public GameObject numberObject;
    public GameObject canvasObject;
    GameObject numberPrefab;

    void movePositionBeforeAndAfterLeftSideNumberIsPlaced(){
        numPosX += (streak >= 10 ? -0.369f : -0.228f)-0.3f;
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
        numPosX += streak >= 10 ? -0.1f : 0;
    }

    void placeLeftSideNumberOnScene(){
        setNumberGameobjectProperties();
        movePositionBeforeAndAfterLeftSideNumberIsPlaced();
        streak = 0;
    }

    void movePositionBeforeTopSideNumberIsPlaced(){
        numPosX += 0.27f;
        numPosY += 0.4f;
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
    }

    void placeTopSideNumberOnScene(){
        setNumberGameobjectProperties();
        movePositionBeforeTopSideNumberIsPlaced();
        streak = 0;
    }

    void setNumberGameobjectProperties(){
        numberPrefab = Instantiate(numberObject);
        numberPrefab.name = "num|"+numPosX+"|"+numPosY;
        numberPrefab.GetComponent<TextMeshProUGUI>().text = streak.ToString();
        numberPrefab.transform.SetParent(canvasObject.transform);
    }

    void movePositionAfterRightSideNumberIsPlaced(){
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
        numPosX += 0.27f;
        numPosY -= 0.4f;
    }

    void placeRightSideNumberOnScene(){
        setNumberGameobjectProperties();
        movePositionAfterRightSideNumberIsPlaced();
        streak = 0;
    }

    void changeRightSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = (hexSize*1f)-((i+1)*0.5f)-0.1f;
            numPosY = -0.78f-(i*0.76f);
        }
        else{
            numPosX = (hexSize*1f)-(hexSize*0.5f)-((hexSize-1-i)*-1f);
            numPosY = (hexSize*-0.76f);
        }
    }

    void fillOutNumbersOnRightSide(){
        for(int i = 0; i < hexGridStatesZ.Count; i++){
            
            changeRightSideNumberPositionAfterNewLine(i);
            for(int j = hexGridStatesZ[i].Count-1; j >= 0  ; j--){ 
                if(hexGridStatesZ[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeRightSideNumberOnScene();
                }
            }
            if(streak != 0){
                placeRightSideNumberOnScene();
            }
        }
    }

    void changeTopSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = startingPlaceX+(i*1f)+1.07f;
            numPosY = startingPlaceY+0.13f;
        }
        else{
            numPosX = startingPlaceX+((i-hexSize-1)*0.5f)+(hexSize*1f)+1.05f;
            numPosY = startingPlaceY-0.62f+(i-hexSize)*-0.76f;
        }
    }    

    void fillOutNumbersOnTopSide(){
        for(int i = 0; i < hexGridStatesY.Count; i++){

            changeTopSideNumberPositionAfterNewLine(i);
            for(int j = hexGridStatesY[i].Count-1; j >= 0  ; j--){ 
                if(hexGridStatesY[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeTopSideNumberOnScene();//napraw translacje
                }
            }
            if(streak != 0){
                placeTopSideNumberOnScene();
            }
        }
    }

    void changeLeftSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = startingPlaceX+(i*-0.5f)+0.8f;
            numPosY = startingPlaceY-(0.76f*i)-0.13f;
        }
        else{
            numPosX = startingPlaceX+((i-hexSize+1)*0.5f)+0.8f-(hexSize-1)*0.5f;
            numPosY = startingPlaceY-(0.76f*i)-0.13f;
        }
    }   

    int streak = 0;
    float numPosX, numPosY;
    void fillOutNumbersOnLeftSide(){
        for(int i = 0; i < hexGridStatesX.Count; i++){

            changeLeftSideNumberPositionAfterNewLine(i);
            for(int j = hexGridStatesX[i].Count-1; j >= 0  ; j--){
                if(hexGridStatesX[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeLeftSideNumberOnScene();
                }
            }
            if(streak != 0){
                placeLeftSideNumberOnScene();
            }
        }
    }


    void translateTopRightHexStatesFromYtoZ(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0;j < hexSize+1+i; j++){
                hexGridStatesZ[i].Add(hexGridStatesY[2*hexSize-1-j][j > hexSize ? i-j+hexSize : i]);
            }
        }
    }


    void translateBottomLeftHexStatesFromYtoZ(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0; j < (hexSize*2)-i; j++){
                hexGridStatesZ[hexSize+i].Add(hexGridStatesY[(hexSize*2)-i-j-1][j>hexSize-i ? (2*hexSize)-j : hexSize +i]);
            }
        }
    }

    public List<List<bool>> hexGridStatesZ = new List<List<bool>>();

    void translateTopLeftHexStatesFromYtoZ(){
        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0; j < hexSize+i; j++){
                int x = hexSize*2-2-j;
                int y = j >= hexSize ? i-j+hexSize-1 : i;
                hexGridStatesZ[i].Add(hexGridStatesY[x][y]);
            }
        }
    }

    void translateBottomRightHexStatesFromYtoZ(){
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0; j < hexSize*2-2-i; j++){
                int x = hexSize*2-3-i-j;
                int y = j >= hexSize-1-i ? hexSize+hexSize-2-j : i+hexSize;
                hexGridStatesZ[i+hexSize].Add(hexGridStatesY[x][y]);
            }
        }     
    }

    void translateHexStatesFromYtoZ(){
        translateTopLeftHexStatesFromYtoZ();
        translateBottomRightHexStatesFromYtoZ();
    }

    void translateTopLeftHexStatesFromXtoY(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesY.Add(new List<bool>());
            for(int j = 0; j < hexSize+i; j++){
                int y = j >= hexSize ? hexSize+i-j-1 : i;
                int x = j;
                hexGridStatesY[i].Add(hexGridStatesX[x][y]);
            }
        }
    }
    void translateBottomRightHexStatesFromXtoY(){
        
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesY.Add(new List<bool>());
            for(int j = 0; j < (hexSize*2)-2-i; j++){

                int y = j >= hexSize-i-1 ? hexSize+hexSize-j-2 : hexSize + i;
                int x = j+i+1;
                hexGridStatesY[hexSize+i].Add(hexGridStatesX[x][y]);
            }
        }        
    }

    public List<List<bool>> hexGridStatesY = new List<List<bool>>();

    void translateHexStatesFromXtoY(){

        translateTopLeftHexStatesFromXtoY();
        translateBottomRightHexStatesFromXtoY();
        
    }

    public GameObject hexObject, hoverLightObject;
    public List<List<Hex_Handler>> hexHandlerArray = new List<List<Hex_Handler>>();

    void placeNewHexOnScene(Vector3 hexPosition, int x, int y){
        GameObject hexPrefab = Instantiate(hexObject);
        hexPrefab.name = "hex-"+x+"-"+y;
        hexPrefab.transform.position = hexPosition;

        hexHandlerArray[x].Add(hexPrefab.GetComponent<Hex_Handler>());

        hexHandlerArray[x][y].X = x;
        hexHandlerArray[x][y].Y = y;
        hexHandlerArray[x][y].hexState = hexState;

        GameObject hoverLightPrefab = Instantiate(hoverLightObject);        
        hoverLightPrefab.transform.position = hexPosition;
        hoverLightPrefab.transform.SetParent(hexPrefab.transform);
        hoverLightPrefab.SetActive(false);
    }

    public List<List<bool>> hexGridStatesX = new List<List<bool>>();
    float startingPlaceX, startingPlaceY;

    System.Random random = new System.Random(Environment.TickCount);
    
    bool hexState;
    void randomizeHexState(){
        hexState = random.Next(100) < percentOfTrueStates;
    }

    void createAndPlaceTopHalfOfHexagons(){

        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArray.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize+i;j++){
                randomizeHexState();
                hexGridStatesX[i].Add(hexState);
                placeNewHexOnScene(new Vector3((j*2-i+2)*0.5f+startingPlaceX,i*-0.76f+startingPlaceY,0f), i, j);
            }
        }
    }

    void createAndPlaceMiddleLineOfHexagons(){

        hexGridStatesX.Add(new List<bool>());
        hexHandlerArray.Add(new List<Hex_Handler>());
        for(int i = 0; i < ((hexSize-1)*2)+1; i++){
            randomizeHexState();
            hexGridStatesX[hexSize-1].Add(hexState);
            placeNewHexOnScene(new Vector3((i*2-hexSize+3)*0.5f+startingPlaceX,(hexSize-1)*-0.76f+startingPlaceY,0f), hexSize-1, i);
        }
    }

    void createAndPlaceBottomHalfOfHexagons(){

        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArray.Add(new List<Hex_Handler>());
            for(int j = 0; j < (hexSize-1)*2-i;j++){
                randomizeHexState();
                hexGridStatesX[hexSize+i].Add(hexState);
                placeNewHexOnScene(new Vector3((i+j*2-hexSize+4)*0.5f+startingPlaceX,(hexSize+i)*-0.76f+startingPlaceY,0f), hexSize+i, j);
            }
        }
    }


    void placeNumbers(){
        if(leftNumbers == 1){fillOutNumbersOnLeftSide();}
        if(topRightNumbers == 1){fillOutNumbersOnTopSide();}
        if(bottomRightNumbers == 1){fillOutNumbersOnRightSide();}
    }

    void translateHexagons(){
        translateHexStatesFromXtoY();
        translateHexStatesFromYtoZ();
    }

    void placeReadHexOnScene(int x, int y){
        GameObject hexPrefab = Instantiate(hexObject);
        hexPrefab.name = "hex-"+x+"-"+y;
        hexPrefab.transform.position = hexPropertiesArray[x][y].position;

        GameObject hoverLightPrefab = Instantiate(hoverLightObject);        
        hoverLightPrefab.transform.position = hexPropertiesArray[x][y].position;
        hoverLightPrefab.transform.SetParent(hexPrefab.transform);
        hoverLightPrefab.SetActive(false);

        hexHandlerArray[x].Add(hexPrefab.GetComponent<Hex_Handler>());
        hexHandlerArray[x][y].X = x;
        hexHandlerArray[x][y].Y = y;
        hexHandlerArray[x][y].hexState = hexPropertiesArray[x][y].hexState;
        hexHandlerArray[x][y].isStateKnown = hexPropertiesArray[x][y].isStateKnown;
        hexHandlerArray[x][y].setSprite(hexPropertiesArray[x][y].spriteNum);


    }

    void readAndPlaceHexesFromArray(){
        for(int i = 0; i < hexPropertiesArray.Count; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArray.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexPropertiesArray[i].Count; j++){
                hexGridStatesX[i].Add(hexPropertiesArray[i][j].state);
                placeReadHexOnScene(i, j);
            }
        }

    }

    List<List<Save_And_Load.HexProperties>> hexPropertiesArray = new List<List<Save_And_Load.HexProperties>>();

    void placeHexagons(){

        startingPlaceX = hexSize*-0.5f-0.5f;
        startingPlaceY = (hexSize-1)*0.76f;

        if(newGame == 1){
            createAndPlaceTopHalfOfHexagons();
            createAndPlaceMiddleLineOfHexagons();
            createAndPlaceBottomHalfOfHexagons();
        }
        else{
            hexPropertiesArray = saveAndLoad.getHexProperties();      
            readAndPlaceHexesFromArray();
        }
    }

    void OnApplicationQuit(){

    }


}



