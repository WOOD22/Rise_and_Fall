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
    public Tile[] map_info;     //맵의 정보
}
[Serializable]
public class Tile
{
    public int terrain = 0;             //  지형      0 : 바다, 1 : 해안, 2 : 호수, 3 : 강, 4 : 열대, 5 : 건조, 6 : 사막, 7 : 온대, 8 : 냉대, 8 : 한대
    public int altitude = 0;            //  고도      0 : 없음, 1 : 언덕, 2 : 산
    public bool is_city;                //  도시 존재여부
    public City city                    //  도시
        = new City();
}
[Serializable]
public class City
{
    public string name;                 //  도시 이름
    public string owner_code;           //  도시 소유세력 코드
    public string ruler_code;           //  도시 통치자 코드
    public List<Pop> population         //  인구
        = new List<Pop>();
    public Development development      //  개발도
        = new Development();
}
[Serializable]
public class Pop
{
    public string culture_code;         //  문화 코드
    public string class_code;           //  계급 코드
    public int pop_num;                 //  인구 수
}
[Serializable]
public class Development
{
    public float agriculture = 0;       //  농업
    public float agriculture_cap = 0;   //  상한치
    public float agriculture_exp = 0;   //  경험치 100 채우면 개발도 상승
    public float industry = 0;          //  공업
    public float industry_cap = 0;
    public float industry_exp = 0;
    public float commerce = 0;          //  상업
    public float commerce_cap = 0;
    public float commerce_exp = 0;
}