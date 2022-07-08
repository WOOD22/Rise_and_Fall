using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MapData : MonoBehaviour
{
    public Map map;

    public void Save_File(string file_name)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Map"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Map");
        }

        string save = JsonUtility.ToJson(map, true);
        string path = Path.Combine(Application.persistentDataPath + "/Map", file_name + ".json");
        File.WriteAllText(path, save);
    }
    /*  로드  */
    public void Load_File(string file_name)
    {
        string path = Path.Combine(Application.persistentDataPath + "/Map", file_name + ".json");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            string save = File.ReadAllText(path);
            map = JsonUtility.FromJson<Map>(save);
        }
    }
}

[Serializable]
public class Map
{
    public string name;         //맵의 이름
    public int size_x, size_y;  //맵의 사이즈
    public int[] map_layer_1;   //고저
    public int[] map_layer_2;   //바이옴
    public int[] map_layer_3;   //수계
    public int[] map_layer_4;   //환경
    public int[] map_layer_5;   //영향력
    public int[] map_layer_6;   //문화
    public int[] map_layer_7;   //종교
}