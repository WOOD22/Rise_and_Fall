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
    public Tile_Resource resource       //  타일의 자원
    = new Tile_Resource();
    public Tile_Property property       //  타일의 속성
        = new Tile_Property();
}
[Serializable]
public class Tile_Resource
{
    public float tile_fertility;        //  비옥도, 농업 생산량+
    public float tile_water;            //  강수량 & 저수지 등에 비축된 물이 포함된 수량, 인구 캡+, 농업 생산량+
    public float tile_tree;             //  수목량, 수량+, 인구 캡+
    public float tile_coal;             //  석탄매장량
    public float tile_copper;           //  구리매장량
    public float tile_iron;             //  철매장량
    public float tile_silver;           //  은매장량
    public float tile_gold;             //  금매장량
    public float tile_jewelry;          //  보석매장량
    public float tile_elohium;          //  엘로히움매장량(엘로힘의 파편, 신적인 힘을 가진 금속, 가공자의 의지에 따라 가공품의 효과가 다르다.)
}
[Serializable]
public class Tile_Property
{
    public string name;                 //  타일 이름(인구 100 이상의 마을 부터 이름 생성)
    public string owner_code;           //  타일 소유세력 코드
    public string ruler_code;           //  타일 통치자 코드
    public List<Pop> population         //  인구(계급, 문화별로 분류 )
        = new List<Pop>();
    public Development development      //  개발도
        = new Development();
}
[Serializable]
public class Pop
{
    public string culture_code;         //  문화 코드
    public string class_code;           //  계급 코드
    public int pop_cap;                 //  인구 한계치
    public int pop_num;                 //  인구 수
}
[Serializable]
public class Development
{
    public float primary_industry_dev;  //  1차 산업
    public float primary_industry_cap;  //  상한
    public float secondary_industry_dev;//  공업
    public float secondary_industry_cap;
    public float tertiary_industry_dev; //  상업
    public float tertiary_industry_cap;
}
[Serializable]
public class Industry
{

}