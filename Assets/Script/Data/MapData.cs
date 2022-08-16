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
    public int[] river = new int[6];    //  강
    public bool is_river;
    public int[] road = new int[6];     //  도로
    public bool is_road;
    public Tile_Property property       //  타일의 속성
        = new Tile_Property();
}
[Serializable]
public class Tile_Property
{
    public string name;                 //  타일 이름(인구 100 이상부터 이름 생성)
    public string owner_code;           //  타일 소유세력 코드
    public string ruler_code;           //  타일 통치자 코드
    public List<Pop> population
        = new List<Pop>();              //  인구 리스트(계급, 문화별로 분류), 계급은 상류층, 중류층, 하류층으로 나뉜다.
    /*public Development development
        = new Development();            //  개발도*/
    public List<Tile_Resource> tile_resource
         = new List<Tile_Resource>();   //  자원 리스트
    public List<Tile_Production> tile_production
         = new List<Tile_Production>(); //  생산 리스트
    public int tile_soil_fertility;     //  토지비옥도 (0 : 매우 나쁨, 1 : 나쁨, 2 : 조금 나쁨, 3 : 보통, 4 : 조금 좋음, 5 : 좋음, 6 : 매우 좋음)
}
[Serializable]
public class Pop
{
    public string culture_code;         //  문화 코드
    public string class_code;           //  계급 코드
    public int pop_cap;                 //  인구 한계치
    public int pop_num;                 //  인구 수
    public int pop_employed;            //  노동 인구
    public int pop_unemployed;          //  실업 인구
}
[Serializable]
public class Development
{
    public float primary_industry_dev;  //  1차 산업
    public float primary_industry_cap;  //  상한
    public float secondary_industry_dev;//  2차 산업
    public float secondary_industry_cap;//  상한
    public float tertiary_industry_dev; //  3차 산업
    public float tertiary_industry_cap; //  상한
}
[Serializable]
public class Tile_Production
{
    public string tile_production_code; //  생산 코드
    public float tile_production_amount;//  생산량
    /*
    //  1차 산업
    public float grain;                 //  곡물
    public float fish;                  //  생선
    public float livestock;             //  가축
    public float tree;                  //  목재
    //  2차 산업
    //  광업
    public float stone;                 //  석재
    public float coal;                  //  석탄
    public float copper;                //  구리
    public float iron;                  //  철
    public float silver;                //  은
    public float gold;                  //  금
    public float jewelry;               //  보석
    public float elohium;               //  엘로히움
    //  공업
    public float material;              //  자재
    public float consumer;              //  소비재(가공된 식료품(빵, 소세지 등)을 뜻함), 계급 순으로 소비량 차이가 있으나 차이가 크지 않다.
    public float durable;               //  내구재(의류, 개인 장구류, 마차 등), 계급 순으로 소비량 차이가 있고 차이가 크다.
    public float luxury;                //  사치재(보석 장신구, 고급 의류 등), 하류층은 소비하지 않고 중류층과 상류층 간의 소비 차이가 크다.
    public float supply;                //  보급품(병영에서 소비)
    //  3차 산업
    public float service;               //  서비스업
    public float commerce;              //  상업
    public float finance;               //  금융업
    public float transport;             //  운송업
    public float art;                   //  예술
    public float science;               //  과학
    public float humanity;              //  인문학
    */
}
[Serializable]
public class Tile_Resource
{
    public string tile_resource_code;   //  자원 코드
    public float tile_resource_amount;  //  자원량
    /*
    public float tile_tree;             //  수목량, 수량+, 인구 캡+
    public float tile_stone;            //  석재량
    public float tile_coal;             //  석탄 매장량
    public float tile_copper;           //  구리 매장량
    public float tile_iron;             //  철 매장량
    public float tile_silver;           //  은 매장량
    public float tile_gold;             //  금 매장량
    public float tile_jewelry;          //  보석 매장량
    public float tile_elohium;          //  엘로히움 매장량(엘로힘의 파편, 신적인 힘을 가진 금속, 가공자의 의지에 따라 가공품의 효과가 다르다.)
    */
}