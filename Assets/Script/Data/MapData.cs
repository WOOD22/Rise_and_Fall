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
    public int altitude = 0;            //  고도      0 : 평지, 1 : 언덕, 2 : 산
    public int[] river = new int[6];    //  강
    public bool is_river;
    public int[] road = new int[6];     //  도로
    public bool is_road;
    public Tile_Property property       //  타일의 속성
        = new Tile_Property();
    public List<Holding> holding       //  타일의 홀딩슬롯 (크기는 랜덤, 해안 & 강 & 호수와 접하면 수면타일 생성, 평지는 경사 거의 없음, 언덕은 경사가 완만, 산은 경사가 심함)
        = new List<Holding>();
}
[Serializable]
public class Tile_Property
{
    public string name;                 //  타일 이름(인구 100 이상부터 이름 생성)
    public string owner_code;           //  타일 소유세력 코드
    public string ruler_code;           //  타일 통치자 코드
    public List<Pop> population
        = new List<Pop>();              //  인구 리스트(계급, 문화별로 분류), 계급은 상류층, 중류층, 하류층으로 나뉜다.
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
public class Tile_Production
{
    public string tile_production_code; //  생산 코드
    public float tile_production_amount;//  생산량
}
[Serializable]
public class Tile_Resource
{
    public string tile_resource_code;   //  자원 코드
    public string tile_resource_name;   //  자원 이름
    public float tile_resource_value;   //  자원 가치
    public float tile_resource_amount;  //  자원량
}
[Serializable]
public class Holding
{
    public string holding_code;            //  슬롯에 건축된 건축물의 코드 (빈 슬롯 : B000)
    public int holding_altitude;           //  건설 슬롯의 높이 (0 : 수면, 1 : 매우 낮음, 2 : 낮음, 3 : 중간, 4 : 높음, 5 : 매우 높음)
}