using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Do_On_Parent_Delete : MonoBehaviour
{
    Save_And_Load saveAndLoad;
    Hexgrid_Handler hexgrid;

    void Start()
    {
        saveAndLoad = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Save_And_Load>();
        hexgrid = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Hexgrid_Handler>();
    }

    void OnDestroy(){
        Debug.Log("kablooei");
        saveAndLoad.hexHandlerComponentArray = hexgrid.hexHandlerArray;
        saveAndLoad.hexStateArray = hexgrid.hexGridStatesX;
        saveAndLoad.saveGame();
        saveAndLoad.saveUIStats();

        //todo: put child in this bitch and make ondestroy do something like this ^^^^^^^^^^^^^^^^^^^^^^^^^^^
    }

}
