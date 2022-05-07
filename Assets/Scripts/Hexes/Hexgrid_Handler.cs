using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Hexgrid_Handler : MonoBehaviour
{

    public int hexSize;
    PanAndZoom panAndZoom;
    
    void Start()
    {  
        panAndZoom = gameObject.GetComponent<PanAndZoom>();

        placeHexagons();
        translateHexagons();
        placeNumbers();
    }

    void Update(){
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            draggedSquareOrigin = "";
        }
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

    void selectInALineANumberOfHexesFromAStartingPosition(){
        for(int i = 0; i < panAndZoom.howManyHexesSelected; i++){
            changeHexStateDependingOnMouseUpInput();
            changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
        }
    }

    void changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray(){
        hexY += hexX < hexSize+hexDifference ? yVectorHexLarger : yVectorHexSmaller;
        hexX += hexX < hexSize+hexDifference ? xVectorHexLarger : xVectorHexSmaller;
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
    void changeHexVectorPropertiesByClosestAxis(int yVectorHexSmaller, int yVectorHexLarger, int xVectorHexSmaller, int xVectorHexLarger, int hexDifference){
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
                    changeHexVectorPropertiesByClosestAxis(1,0,-1,-1,1);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();  
                break;
                case 1:
                    changeHexVectorPropertiesByClosestAxis(1,1,0,0,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 2:
                    changeHexVectorPropertiesByClosestAxis(0,1,1,1,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 3:
                    changeHexVectorPropertiesByClosestAxis(-1,0,1,1,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 4:
                    changeHexVectorPropertiesByClosestAxis(-1,-1,0,0,0);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
                case 5:
                    changeHexVectorPropertiesByClosestAxis(0,-1,-1,-1,1);
                    deselectInALineTheRestOfHexesUntilOutOfBounds();
                break;
            }   
        }
    }

    void selectHexesDependingOnClosestAxisForLineLengthAndThenDeselectTheRestOfTheLine(){
        switch(panAndZoom.closestAxis){
            case 0:
                changeHexVectorPropertiesByClosestAxis(1,0,-1,-1,1);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 1:
                changeHexVectorPropertiesByClosestAxis(1,1,0,0,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 2:
                changeHexVectorPropertiesByClosestAxis(0,1,1,1,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 3:
                changeHexVectorPropertiesByClosestAxis(-1,0,1,1,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 4:
                changeHexVectorPropertiesByClosestAxis(-1,-1,0,0,0);
                selectInALineANumberOfHexesFromAStartingPosition();
                deselectInALineTheRestOfHexesUntilOutOfBounds();
            break;
            case 5:
                changeHexVectorPropertiesByClosestAxis(0,-1,-1,-1,1);
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
            numPosX = (hexSize*1f)-(i*0.5f)-0.1f;
            numPosY = -0.78f-(i*0.76f);
        }
        else{
            numPosX = (hexSize*1f)-(hexSize*0.5f)-((hexSize-i)*-1f);
            numPosY = -0.78f-(hexSize*0.76f);
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
                    placeTopSideNumberOnScene();
                }
            }
            if(streak != 0){
                placeTopSideNumberOnScene();
            }
        }
    }

    void changeLeftSideNumberPositionAfterNewLine(int i){
        if(i <= hexSize){
            numPosX = startingPlaceX+(i*-0.5f)+0.8f;
            numPosY = startingPlaceY-(0.76f*i)-0.13f;
        }
        else{
            numPosX = startingPlaceX+((i-hexSize-1)*0.5f)+0.8f-(hexSize-1)*0.5f;
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

    void translateHexStatesFromYtoZ(){
        translateTopRightHexStatesFromYtoZ();
        translateBottomLeftHexStatesFromYtoZ();
    }

    void translateTopLeftHexStatesFromXtoY(){

        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesY.Add(new List<bool>());
            for(int j = 0; j < hexSize+1+i; j++){
                hexGridStatesY[i].Add(hexGridStatesX[j][j > hexSize ? hexSize-j+i : i]);
            }
        }
    }

    void translateMiddleHexStatesFromXtoY(){

        for(int i = 0; i < 2; i++){
            hexGridStatesY.Add(new List<bool>());
            for(int j = 0; j < hexSize*2; j++){
                hexGridStatesY[hexSize-1+i].Add(hexGridStatesX[j%(hexSize*2)+i][j > hexSize - i ? hexSize-j+hexSize-1 : hexSize-1+i]);
            }
        }
    }

    void translateBottomRightHexStatesFromXtoY(){

        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesY.Add(new List<bool>());
            for(int j = 0; j < (hexSize*2)-1-i; j++){
                hexGridStatesY[hexSize+1+i].Add(hexGridStatesX[j+2+i][j > hexSize-i-2 ? (hexSize*2)-1-j : hexSize+1+i]);
            }
        }
    }

    public List<List<bool>> hexGridStatesY = new List<List<bool>>();

    void translateHexStatesFromXtoY(){

        translateTopLeftHexStatesFromXtoY();
        translateMiddleHexStatesFromXtoY();
        translateBottomRightHexStatesFromXtoY();
        
    }

    public GameObject hexGameObject;
    public List<List<Hex_Handler>> hexHandlerArray = new List<List<Hex_Handler>>();

    void placeHexOnScene(Vector3 hexPosition, int x, int y){
        GameObject hexPrefab = Instantiate(hexGameObject);
        hexPrefab.name = "hex-"+x+"-"+y;
        hexPrefab.transform.position = hexPosition;

        hexHandlerArray[x].Add(hexPrefab.GetComponent<Hex_Handler>());

        hexHandlerArray[x][y].X = x;
        hexHandlerArray[x][y].Y = y;
        hexHandlerArray[x][y].hexState = hexState;     
    }

    public List<List<bool>> hexGridStatesX = new List<List<bool>>();
    float startingPlaceX, startingPlaceY;

    System.Random random = new System.Random(Environment.TickCount);
    
    bool hexState;
    public int percentOfTrueStates = 50;
    void randomizeHexState(){
        hexState = random.Next(100) < percentOfTrueStates;
    }

    void placeTopHalfOfHexagons(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArray.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize+i;j++){
                randomizeHexState();
                hexGridStatesX[i].Add(hexState);
                placeHexOnScene(new Vector3((j*2-i+2)*0.5f+startingPlaceX,i*-0.76f+startingPlaceY,0f), i, j);
            }
        }
    }

    void placeMiddleLineOfHexagons(){

        hexGridStatesX.Add(new List<bool>());
        hexHandlerArray.Add(new List<Hex_Handler>());
        for(int i = 0; i < hexSize*2; i++){
            randomizeHexState();
            hexGridStatesX[hexSize].Add(hexState);
            placeHexOnScene(new Vector3((i*2-hexSize+2)*0.5f+startingPlaceX,(hexSize)*-0.76f+startingPlaceY,0f), hexSize, i);
        }
    }

    void placeBottomHalfOfHexagons(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArray.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize*2-i-1;j++){
                randomizeHexState();
                hexGridStatesX[hexSize+i+1].Add(hexState);
                placeHexOnScene(new Vector3((i+j*2-hexSize+3)*0.5f+startingPlaceX,(hexSize+i+1)*-0.76f+startingPlaceY,0f), hexSize+i+1, j);
            }
        }
    }

    void placeNumbers(){
        fillOutNumbersOnLeftSide();
        fillOutNumbersOnTopSide();
        fillOutNumbersOnRightSide();
    }

    void translateHexagons(){
        translateHexStatesFromXtoY();
        translateHexStatesFromYtoZ();
    }

    void placeHexagons(){

        startingPlaceX = hexSize*-0.5f-0.5f;
        startingPlaceY = hexSize*0.76f;

        placeTopHalfOfHexagons();
        placeMiddleLineOfHexagons();
        placeBottomHalfOfHexagons();

    }

}



