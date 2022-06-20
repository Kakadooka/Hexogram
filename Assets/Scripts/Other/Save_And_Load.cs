using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Save_And_Load : MonoBehaviour
{

    [Serializable]
    public class HexProperties{
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public int spriteNum;
        [SerializeField]
        public bool state, isStateKnown, hexState;
        public void addProperties(bool hexState, Vector3 position, int spriteNum, bool isStateKnown, bool state){ 
            this.hexState = hexState;
            this.position = position;
            this.spriteNum = spriteNum;
            this.state = state;
            this.isStateKnown = isStateKnown;
        }
    }

    [Serializable]
    public class HexPropertiesList{
        [SerializeField]
        public List<HexProperties> hexProperties = new List<HexProperties>();
    }
    [Serializable]
    public class HexPropertiesListOfLists{
        [SerializeField]
        public List<HexPropertiesList> hexPropertiesList = new List<HexPropertiesList>();

    }
    [Serializable]
    public class HexgridData{
        [SerializeField]
        public int hexSize, leftNumbers, topRightNumbers, bottomRightNumbers;             
        public HexgridData(int hexSize, int leftNumbers, int topRightNumbers, int bottomRightNumbers){
            this.hexSize = hexSize;
            this.leftNumbers = leftNumbers;
            this.topRightNumbers = topRightNumbers;
            this.bottomRightNumbers = bottomRightNumbers;
        }
        public HexgridData(){

        }
    }

    [Serializable]
    public class UIStatsInfo{
        [SerializeField]
        public int amountOfAllHexes, amountOfGuessedHexes, amountOfWrongGuesses, percentComplete;
        [SerializeField]
        public float timeClock;
        public UIStatsInfo(int amountOfAllHexes,int amountOfGuessedHexes,int amountOfWrongGuesses,int percentComplete, float timeClock){
            this.amountOfAllHexes = amountOfAllHexes;
            this.amountOfGuessedHexes = amountOfGuessedHexes;
            this.amountOfWrongGuesses = amountOfWrongGuesses;
            this.percentComplete = percentComplete;
            this.timeClock = timeClock;
        }
    }

    public bool checkIfSaveFilesExist(){
        setSaveDirectory();
        return (Directory.Exists(saveDirectory) && File.Exists(saveDirectory+"/ui.json") && File.Exists(saveDirectory+"/save.json"));
    }

    void putObjectIntoJsonAndSaveIt(HexPropertiesListOfLists hexPropertiesListOfLists){
        if (!Directory.Exists(saveDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(saveDirectory);
        }
        StreamWriter writer = new StreamWriter(saveDirectory+"/save.json");
        writer.WriteLine(JsonUtility.ToJson(hexPropertiesListOfLists));
        writer.Close();
    }
    void putObjectIntoJsonAndSaveIt(UIStatsInfo uIStatsInfo){
        if (!Directory.Exists(saveDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(saveDirectory);
        }
        StreamWriter writer = new StreamWriter(saveDirectory+"/ui.json");
        writer.WriteLine(JsonUtility.ToJson(uIStatsInfo));
        writer.Close();
    }
    void putObjectIntoJsonAndSaveIt(HexgridData hexgridData){
        if (!Directory.Exists(saveDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(saveDirectory);
        }
        StreamWriter writer = new StreamWriter(saveDirectory+"/hexgridData.json");
        writer.WriteLine(JsonUtility.ToJson(hexgridData));
        writer.Close();
    }



    public List<List<Hex_Handler>> hexHandlerComponentArray = new List<List<Hex_Handler>>();
    public List<List<bool>> hexStateArray = new List<List<bool>>();

    HexPropertiesListOfLists hexPropertiesListOfLists = new HexPropertiesListOfLists();

    HexPropertiesListOfLists getObjectFromHexProperties(){
        hexHandlerComponentArray = hexgrid.hexHandlerArray;
        hexStateArray = hexgrid.hexGridStatesX;
        for(int i = 0; i < hexHandlerComponentArray.Count; i++){
            
            for(int j = 0; j < hexHandlerComponentArray[i].Count; j++){
                
                Hex_Handler hexHandler = hexHandlerComponentArray[i][j];
                hexPropertiesListOfLists.hexPropertiesList[i].hexProperties[j].addProperties(hexHandler.hexState, hexHandler.transform.position, hexHandler.spriteNum, hexHandler.isStateKnown, hexStateArray[i][j]);     
            }
        }
        return hexPropertiesListOfLists;
    }

    void setSaveDirectory(){
        if(saveDirectory == null){
            saveDirectory = Application.persistentDataPath+"/Saved Game";
        }
    }

    UI_Stats_Handler uiStats;
    Hexgrid_Handler hexgrid;
    void Start(){
        uiStats = gameObject.GetComponent<UI_Stats_Handler>();
        hexgrid = gameObject.GetComponent<Hexgrid_Handler>();
        setSaveDirectory();

        if(hexgrid != null){
            for(int i = 0; i < hexgrid.hexHandlerArray.Count; i++){
                hexPropertiesListOfLists.hexPropertiesList.Add(new HexPropertiesList());
                for(int j = 0; j < hexgrid.hexHandlerArray[i].Count; j++){
                    hexPropertiesListOfLists.hexPropertiesList[i].hexProperties.Add(new HexProperties());
                }
            }
        }

    }
    string saveDirectory;

    public void saveUIStats(){
        UIStatsInfo uiStatsInfo = new UIStatsInfo(uiStats.amountOfAllHexes,uiStats.amountOfGuessedHexes,uiStats.amountOfWrongGuesses,uiStats.percentComplete,uiStats.timeClock);
        putObjectIntoJsonAndSaveIt(uiStatsInfo);
    }
    
    public UIStatsInfo getUIStats(){
        string jsonText = File.ReadAllText(saveDirectory+"/ui.json");
        return JsonUtility.FromJson<UIStatsInfo>(jsonText);
    }

    public void saveGame(){
        putObjectIntoJsonAndSaveIt(getObjectFromHexProperties());
        saveUIStats();
        saveHexgridData();
    }

    public void saveHexgridData(){
        HexgridData hexgridData = new HexgridData(hexgrid.hexSize, hexgrid.leftNumbers, hexgrid.topRightNumbers, hexgrid.bottomRightNumbers);
        putObjectIntoJsonAndSaveIt(hexgridData);
    }

    public void setHexgridDataIntoPlayerPrefs(){

        string jsonText = File.ReadAllText(saveDirectory+"/hexgridData.json");
        HexgridData hexgridData = JsonUtility.FromJson<HexgridData>(jsonText);

        PlayerPrefs.SetInt("size", hexgridData.hexSize);
        PlayerPrefs.SetInt("leftNumbers", hexgridData.leftNumbers);
        PlayerPrefs.SetInt("topRightNumbers", hexgridData.topRightNumbers);
        PlayerPrefs.SetInt("bottomRightNumbers", hexgridData.bottomRightNumbers);
    }

    List<List<HexProperties>> translateStupidClassOfClassesIntoBeautiful2DList(HexPropertiesListOfLists hexPropertiesListOfLists){
        List<List<HexProperties>> hexPropertiesArray = new List<List<HexProperties>>();
        for(int i = 0; i < hexPropertiesListOfLists.hexPropertiesList.Count; i++){
            hexPropertiesArray.Add(new List<HexProperties>());
            for(int j = 0; j < hexPropertiesListOfLists.hexPropertiesList[i].hexProperties.Count; j++){
                hexPropertiesArray[i].Add(hexPropertiesListOfLists.hexPropertiesList[i].hexProperties[j]);
            }            
        }
        return hexPropertiesArray;
    }

    public List<List<HexProperties>> getHexProperties(){
        setSaveDirectory();

        HexPropertiesListOfLists hexPropertiesListOfLists = new HexPropertiesListOfLists();
       
        string jsonText = File.ReadAllText(saveDirectory+"/save.json");
        hexPropertiesListOfLists = JsonUtility.FromJson<HexPropertiesListOfLists>(jsonText);

        return translateStupidClassOfClassesIntoBeautiful2DList(hexPropertiesListOfLists);
    }

}
