using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Mouse : MonoBehaviour
{

    bool isDragActive = false;
    bool downInPreviousFrame = false;
    bool mouseOver = false;

    void OnMouseExit(){
        mouseOver = false;
    }

    void OnMouseOver(){
        mouseOver = true;
    }
    
    void Update () {
        if (Input.GetMouseButton(1) && (mouseOver || downInPreviousFrame))
        {
            if (downInPreviousFrame)
            {
                if (isDragActive)
                {
                    gameObject.SendMessage("OnRightMouseDrag",SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    isDragActive = true;
                    gameObject.SendMessage("OnRightMouseDown",SendMessageOptions.DontRequireReceiver);
                }
            }
            downInPreviousFrame = true;
        }
        else
        {
            if (isDragActive)
            {
                isDragActive = false;
                gameObject.SendMessage("OnRightMouseUp",SendMessageOptions.DontRequireReceiver);
            }
            downInPreviousFrame = false;
        }
    }
}
