using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Handler : MonoBehaviour
{

    public int X;
    public int Y;
    public bool hexState;
    public bool isStateKnown;
    public int spriteNum = 2;
    Hexgrid_Handler hagrid;
    Pan_And_Zoom panAndZoom;
    UI_Stats_Handler hexAmountCalc;
    Sound_Handler soundHandler;
    GameObject lightHover;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        hagrid = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Hexgrid_Handler>();
        panAndZoom = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Pan_And_Zoom>();
        hexAmountCalc = GameObject.Find("!SCRIPT HOLDER!").GetComponent<UI_Stats_Handler>();
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
        lightHover = gameObject.transform.GetChild(0).gameObject;
    }

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public void setSprite(int spriteNum){
        this.spriteNum = spriteNum;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[spriteNum];
    }

    void makeGuessNotPerfect(){
        if(!isStateKnown){
            hagrid.isPerfect = false;
        }
    }

    void checkIfHexIsUnknown(){
        if(!isStateKnown){
            hagrid.anyUnknownHexes = true;
        }
    }

    public void guessHexIsTrue(){
        checkIfHexIsUnknown();
        if(hexState){
            SetSpriteYes();
        }
        else{
            makeGuessNotPerfect();
            SetSpriteNoIncorrect();
        }
    }
    public void guessHexIsFalse(){
        checkIfHexIsUnknown();
        if(!hexState){
            SetSpriteNo();
        }
        else{
            makeGuessNotPerfect();
            SetSpriteYesIncorrect();   
        }
    }

    public void SetSpriteYes(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[0];
            spriteNum = 0;
            isStateKnown = true;
            hexAmountCalc.correctGuess();
        }  
    }

    public void SetSpriteYesIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[1];
            spriteNum = 1;
            isStateKnown = true;
            hexAmountCalc.wrongGuess();
        }  
    }
    
    public void SetSpriteMaybe(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[2];
        }  
    }
    public void SetSpriteNo(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[3];
            spriteNum = 3;
            isStateKnown = true;
            hexAmountCalc.correctGuess();
        }  
    }
    public void SetSpriteNoIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[4];
            spriteNum = 4;
            isStateKnown = true;
            hexAmountCalc.wrongGuess();
        }  
    }
    public void SetSpriteHoldAndDrag(){
        if(!isStateKnown && spriteRenderer.sprite == sprites[2]){
            spriteRenderer.sprite = sprites[6];
            soundHandler.playHover();
        }  
    }
    public void enableHoverLight(){
        lightHover.SetActive(true);
    }
    public void disableHoverLight(){
        lightHover.SetActive(false);
    }    

    void ifBlockedClearLineIfNotSelectHexes(){
        hagrid.hexX = X; hagrid.hexY = Y;
        if(panAndZoom.blockedPlacement){
            hagrid.deselectEntireLineOnPreviousAxisIfAxisWasChanged();
            SetSpriteMaybe();     
        }
        else if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }

    void OnMouseOver(){
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            hagrid.hexX = X; hagrid.hexY = Y;
            hagrid.DrawThreeGuidingLines();
        }
    }
    
    void OnMouseExit(){
        panAndZoom.checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis();
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            hagrid.UndrawThreePreviousGuidingLines();
        }
    }

    void OnMouseUp(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnMouseDrag(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnRightMouseDrag(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnRightMouseUp(){
        ifBlockedClearLineIfNotSelectHexes();
    }
}
