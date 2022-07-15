using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MapData : MonoBehaviour
{
    public Map map;
    /*  세이브  */
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
/*
[Serializable]
public class Map
{
    public string name;         //맵의 이름
    public int size_x, size_y;  //맵의 사이즈
    public Tile[] map_layer_1;   //고저
}*/
[Serializable]
public class Tile
{
    public int terrain = 0;             //  지형           0 : 바다, 1 : 해안, 2 : 호수, 3 : 평지, 4 : 언덕, 5 : 산
    public int biome = 0;               //  바이옴         0 : 없음, 1 : 열대, 2 : 건조, 3 : 온대, 4 : 냉대, 5 : 한대
    public int nature = 0;              //  자연 환경      0 : 없음, 1 : 숲, 2 : 정글, 3 : 습지
    public bool is_river = false;       //  강 존재여부
    public List<Pop> population;
}

[Serializable]
public class Pop
{
    public string culture_code;         //  문화 코드
    public string class_code;           //  계급 코드
    public int number;                  //  숫자
}