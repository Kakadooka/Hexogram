using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan_And_Zoom : MonoBehaviour
{

    void setParameters(){
        hexgridHandler = gameObject.GetComponent<Hexgrid_Handler>();
        zoomOutMax = hexgridHandler.hexSize+1f;
        zoomOutMin = zoomOutMax <= 4f ? 1f : 4f;
        borderX = hexgridHandler.hexSize;
        borderY = hexgridHandler.hexSize*0.76f;
    }

    Hexgrid_Handler hexgridHandler;
    void Start(){
        setParameters();
    }

    Vector3 touchStart, touchDrag, draggedLine;
    Vector3 leftClickStart;
    float zoomOutMax, borderX, borderY, zoomOutMin, angle;

    public int howManyHexesSelected;
    public int closestAxis;
    public float gapToHexEdge = 0f;

    public bool blockedPlacement = false;

    int howManySelected(float differenceInX){      
        return (int)Mathf.Floor(draggedLine.x/differenceInX) + 1;
        
    }

    void checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis(){
        if(angle >= 0f && angle  <= 62.5f){
            howManyHexesSelected = howManySelected(0.5f);
            closestAxis = 0;
        }
        else if(angle >= 62.5f && angle  <= 117.5f){
            howManyHexesSelected = howManySelected(1f);
            closestAxis = 1;
        }
        else if(angle >= 117.5f && angle  <= 180f){
            howManyHexesSelected = howManySelected(0.5f);
            closestAxis = 2;
        }
        else if(angle >= 180f && angle  <= 242.5f){
            howManyHexesSelected = howManySelected(-0.5f);
            closestAxis = 3;
        }
        else if(angle >= 242.5f && angle  <= 297.5f){
            howManyHexesSelected = howManySelected(-1f);
            closestAxis = 4;
        }
        else if(angle >= 297.5f && angle  <= 360f){
            howManyHexesSelected = howManySelected(-0.5f);
            closestAxis = 5;
        }
    }

    void calculateVariablesForDragSelectingUsindLeftMouseButton(){
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            leftClickStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
            calculateAngleAndLengthOfDraggedLine();
            checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis();
            Debug.DrawRay(new Vector3(0,0,0), draggedLine, Color.green);
        }
    }

    void calculateAngleAndLengthOfDraggedLine(){
        touchDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchDrag.z = leftClickStart.z = 0;
        draggedLine = touchDrag - leftClickStart;
        angle = Vector2.Angle(new Vector3(0f, 10f, 0f), draggedLine);
        if(draggedLine.x < 0f){
            angle = 360f - angle;
        }
    }

    void panCameraUsingMiddleMouseButton(){
        if(Input.GetMouseButtonDown(2)){
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(2)){
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x,borderX*-1f, borderX), Mathf.Clamp(Camera.main.transform.position.y,borderY*-1f, borderY), -10f);
        }
    }

    void zoomUsingScroll(){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel")*8f, zoomOutMin, zoomOutMax);
    }

    void blockAndUnblockPlacementDependingOnMouseButtonInputs(){
        if((Input.GetMouseButton(0) && Input.GetMouseButtonDown(1)) || (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))){
            blockedPlacement = true;
            hexgridHandler.draggedSquareOrigin = "";
        }
        else if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            blockedPlacement = false;
        }
    }

	void Update () {
        calculateVariablesForDragSelectingUsindLeftMouseButton();
        panCameraUsingMiddleMouseButton();
        zoomUsingScroll();
        blockAndUnblockPlacementDependingOnMouseButtonInputs();

	}


}
