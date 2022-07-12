using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMapFile : MonoBehaviour
{
    public GameObject EditManager;
    public GameObject DataManager;

    public string file_name;

    public Map map;

    private void Start()
    {
        EditManager = GameObject.Find("Manager").transform.Find("EditManager").gameObject;
        DataManager = GameObject.Find("Manager").transform.Find("DataManager").gameObject;
        file_name = this.transform.GetChild(0).GetComponent<Text>().text;
    }
    public void Save_File_Name()
    {
        EditManager.GetComponent<MapEdit>().save_map_name.text = file_name;
    }
}
