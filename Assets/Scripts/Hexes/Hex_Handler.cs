using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Handler : MonoBehaviour
{

    public int X;
    public int Y;
    public bool hexState;
    bool isStateKnown = false;
    Hexgrid_Handler hagrid;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        hagrid = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Hexgrid_Handler>();
    }

    public bool state;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public void guessHexIsTrue(){
        if(hexState){
            SetSpriteYes();
        }
        else{
            SetSpriteNoIncorrect();
        }
    }
    public void guessHexIsFalse(){
        if(!hexState){
            SetSpriteNo();
        }
        else{
            SetSpriteYesIncorrect();
        }
    }

    public void SetSpriteYes(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[0];
            isStateKnown = true;
        }  
    }
    public void SetSpriteYesIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[1];
            isStateKnown = true;
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
            isStateKnown = true;
        }  
    }
    public void SetSpriteNoIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[4];
            isStateKnown = true;
        }  
    }
    public void SetSpriteHover(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[5];
        }  
    }
    public void SetSpriteAndHoldDrag(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[6];
        }  
    }
    
    void Update(){
        
    }  

    void OnMouseUp(){
        if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.hexX = X; hagrid.hexY = Y;
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }

    void OnMouseDrag(){
        if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.hexX = X; hagrid.hexY = Y;
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }

    void OnRightMouseDrag(){
        if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.hexX = X; hagrid.hexY = Y;
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }

    void OnRightMouseUp(){
        if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.hexX = X; hagrid.hexY = Y;
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }


}
