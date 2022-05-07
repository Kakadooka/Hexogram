using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Mouse : MonoBehaviour
{

    bool isRightMouseDragActive = false;
    bool rightMouseDownInPreviousFrame = false;
    bool mouseOver = false;
    bool rightMouseDragActive = false;

    void OnMouseExit(){
        mouseOver = false;
    }

    void OnMouseOver(){
        mouseOver = true;
    }
    
    void Update () {

        if (Input.GetMouseButton(1) && (mouseOver || rightMouseDownInPreviousFrame))
        {
            if (rightMouseDownInPreviousFrame)
            {
                if (isRightMouseDragActive)
                {
                    gameObject.SendMessage("OnRightMouseDrag",SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    isRightMouseDragActive = true;
                    gameObject.SendMessage("OnRightMouseDown",SendMessageOptions.DontRequireReceiver);
                }
            }
            rightMouseDownInPreviousFrame = true;
        }
        else
        {
            if (isRightMouseDragActive)
            {
                isRightMouseDragActive = false;
                gameObject.SendMessage("OnRightMouseUp",SendMessageOptions.DontRequireReceiver);
            }
            rightMouseDownInPreviousFrame = false;
        }
    }
}
