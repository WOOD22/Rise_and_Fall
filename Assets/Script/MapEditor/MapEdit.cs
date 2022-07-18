using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.EventSystems;

public class MapEdit : MonoBehaviour
{
    public GameObject DataManager;

    public GameObject MapSaveFile_content;
    public GameObject map_save_file;

    public GameObject MapLoadFile_content;
    public GameObject map_load_file;

    public InputField save_map_name;

    public string file_name;

    public Text new_map_name;
    public Text new_map_size_x;
    public Text new_map_size_y;

    public Map map;

    public GameObject tile_map;
    public GameObject test_tile;

    public Sprite null_tile;

    //  지형
    public Sprite ocean_tile;               //  0번 바다 타일
    public Sprite coast_tile;               //  1번 해안 타일
    public Sprite lake_tile;                //  2번 호수 타일
    public Sprite river_tile;               //  3번 큰강 타일
    public Sprite flat_tile;                //  4번 평지 타일
    public Sprite hill_tile;                //  5번 언덕 타일
    public Sprite mountain_tile;            //  6번 산 타일
    //  바이옴
    public Sprite tropical_tile;            //  0번 열대 타일
    public Sprite dry_tile;                 //  1번 건조 타일
    public Sprite desert_tile;              //  2번 사막 타일
    public Sprite temperate_tile;           //  3번 온대 타일
    public Sprite subarctic_tile;           //  4번 냉대 타일
    public Sprite polar_tile;               //  5번 한대 타일
    //  자연환경
    public Sprite jungle_tile;              //  1번 정글 타일
    public Sprite forest_tile;              //  2번 숲 타일
    public Sprite marsh_tile;               //  3번 습지 타일
    //  작은 강
    public Sprite river_e_tile;             //  1번 동
    public Sprite river_ne_tile;            //  2번 북동
    public Sprite river_nw_tile;            //  3번 북서
    public Sprite river_w_tile;             //  4번 서
    public Sprite river_sw_tile;            //  5번 남서
    public Sprite river_se_tile;            //  6번 남동

    public Sprite select_tile_sprite;
    public Tile[] select_layer;
    public int select_tile_type;

    public bool is_tool_pen = true;

    void Start()
    {
        select_tile_sprite = null_tile;
        select_tile_type = 0;
    }

    void Update()
    {
        Tool_Pen(select_layer, select_tile_type);
    }

    //  새로운 맵 만들기(1레이어, 바다 타일로 초기화)******************************************************************
    public void New_Map()
    {
        map = DataManager.GetComponent<MapData>().map;

        map.name = new_map_name.text;
        map.size_x = int.Parse(new_map_size_x.text);
        map.size_y = int.Parse(new_map_size_y.text);
        map.map_info = new Tile[map.size_x * map.size_y];

        select_layer = map.map_info;

        //  바다 타일로 초기화
        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                map.map_info[j + i * map.size_y] = new Tile();
                map.map_info[j + i * map.size_y].terrain = 0;
            }
        }
                //  필요한 수 만큼 타일 추가
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            if (tile_map.transform.childCount <= i)
            {
                GameObject instance = Instantiate(test_tile, tile_map.transform);
                instance.name = "Tile_" + i;
            }
        }
        //  필요한 수 만큼 타일 추가 및 활성화, 나머지 비활성화
        int k = 0;
        for (int i = 0; i < tile_map.transform.childCount; i++)
        {
            if (i >= map.size_x * map.size_y)
            {
                tile_map.transform.GetChild(i).gameObject.SetActive(false);
                tile_map.transform.GetChild(i).localPosition = new Vector3(-1000f, -1000f, 0);
            }
            else if (i < map.size_x * map.size_y)
            {
                if (i % map.size_y == 0 && i != 0)
                {
                    k++;
                }
                tile_map.transform.GetChild(i).gameObject.SetActive(true);
                tile_map.transform.GetChild(i).localPosition = new Vector3((float)(i % map.size_y * 0.5f + k), (float)(i % map.size_y * 0.75f), 0);
            }
        }
        Matching_Tile_Sprite_Terrain();
    }

    //  맵 저장하기
    public void Save_Map()
    {
        DataManager.GetComponent<MapData>().Save_File(save_map_name.text);
    }

    //  맵 불러오기
    public void Load_Map()
    {
        DataManager.GetComponent<MapData>().Load_File(file_name);
        map = DataManager.GetComponent<MapData>().map;
        select_layer = map.map_info;

        //  필요한 수 만큼 타일 추가
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            if (tile_map.transform.childCount <= i)
            {
                GameObject instance = Instantiate(test_tile, tile_map.transform);
                instance.name = "Tile_" + i;
            }
        }
        //  필요한 수 만큼 타일 활성화, 나머지 비활성화
        int k = 0;
        for (int i = 0; i < tile_map.transform.childCount; i++)
        {
            if (i >= map.size_x * map.size_y)
            {
                tile_map.transform.GetChild(i).gameObject.SetActive(false);
            }
            else if (i < map.size_x * map.size_y)
            {
                if (i % map.size_y == 0 && i != 0)
                {
                    k++;
                }
                tile_map.transform.GetChild(i).gameObject.SetActive(true);
                tile_map.transform.GetChild(i).localPosition = new Vector3((float)(i % map.size_y * 0.5f + k), (float)(i % map.size_y * 0.75f), 0);
            }
        }
        Matching_Tile_Sprite_Terrain();
    }
    //  지형 맵 이미지 매칭하기
    void Matching_Tile_Sprite_Terrain()
    {
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            tile_map.transform.GetChild(i).gameObject.SetActive(true);
            //  지형 배치
            switch (map.map_info[i].terrain)
            {
                case 0:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = ocean_tile;
                    break;
                case 1:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = coast_tile;
                    break;
                case 2:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = lake_tile;
                    break;
                case 3:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = river_tile;
                    break;
                case 4:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = flat_tile;
                    break;
                case 5:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile;
                    break;
                case 6:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile;
                    break;
            }
            /*
            //  자연 환경 존재 시 배치(자연 환경은 평지 타일에만 배치됨)
            switch (map.map_info[i].nature)
            {
                case 1:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = jungle_tile;
                    break;
                case 2:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = forest_tile;
                    break;
                case 3:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = marsh_tile;
                    break;
            }*/
        }
    }
    //  바이옴 맵 이미지 매칭하기
    void Matching_Tile_Sprite_Biome()
    {
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            if(map.map_info[i].terrain > 3)
            {
                map.map_info[i].biome = 4;
            }
            else
            {
                tile_map.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            switch (map.map_info[i].biome)
            {
                case 1:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = tropical_tile;
                    break;
                case 2:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dry_tile;
                    break;
                case 3:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = desert_tile;
                    break;
                case 4:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = temperate_tile;
                    break;
                case 5:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = subarctic_tile;
                    break;
                case 6:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = polar_tile;
                    break;
            }
        }
    }

    //  세이브 페이지 오픈
    public void OpenSavePage()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Map"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Map");
        }

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Map");
        FileInfo[] fi = di.GetFiles("*.*");
        for (int i = 0; i < fi.Length; i++)
        {
            //  생성된 맵세이브파일 프리팹이 실제 맵세이브 파일보다 적을 경우 새로운 프리팹 생성
            if (MapSaveFile_content.transform.childCount < fi.Length)
            {
                GameObject instance = Instantiate(map_save_file, MapSaveFile_content.transform);
                instance.transform.GetChild(0).GetComponent<Text>().text = fi[i].Name.Substring(0, fi[i].Name.IndexOf('.'));
            }
        }
    }

    //  로드 페이지 오픈
    public void OpenLoadPage()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Map"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Map");
        }

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Map");
        FileInfo[] fi = di.GetFiles("*.*");
        for (int i = 0; i < fi.Length; i++)
        {
            //  생성된 맵세이브파일 프리팹이 실제 맵세이브 파일보다 적을 경우 새로운 프리팹 생성
            if (MapLoadFile_content.transform.childCount < fi.Length)
            {
                GameObject instance = Instantiate(map_load_file, MapLoadFile_content.transform);
                instance.transform.GetChild(0).GetComponent<Text>().text = fi[i].Name.Substring(0, fi[i].Name.IndexOf('.'));
            }
        }
    }

    //  색칠할 타일 선택
    public void Select_Tile_Sprite(int tile_type)
    {
        switch(tile_type)
        {
            case 0:
                select_tile_sprite = ocean_tile;
                select_tile_type = 0;
                break;
            case 1:
                select_tile_sprite = coast_tile;
                select_tile_type = 1;
                break;
            case 2:
                select_tile_sprite = lake_tile;
                select_tile_type = 2;
                break;
            case 3:
                select_tile_sprite = river_tile;
                select_tile_type = 3;
                break;
            case 4:
                select_tile_sprite = flat_tile;
                select_tile_type = 4;
                break;
            case 5:
                select_tile_sprite = hill_tile;
                select_tile_type = 5;
                break;
            case 6:
                select_tile_sprite = mountain_tile;
                select_tile_type = 6;
                break;
        }
    }
    
    //  하이라이트 변경
    public void Select_Layer(int layer_num)
    {
        switch (layer_num)
        {
            case 1:
                Matching_Tile_Sprite_Terrain();
                break;
            case 2:
                Matching_Tile_Sprite_Biome();
                break;
        }
    }
    //  펜 기능
    public void Tool_Pen(Tile[] map_layer, int tile_type)
    {
        if (is_tool_pen && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse_position, transform.forward, 15);
            if (hit)
            {
                if(hit.transform.gameObject.name.Contains("Tile"))
                {
                    string tile_num = hit.transform.gameObject.name;
                    tile_num = tile_num.Replace("Tile_", "");

                    map_layer[int.Parse(tile_num)].terrain = tile_type;
                    hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = select_tile_sprite;
                }
            }
        }
    }
    //  펜 활성화
    public void Tool_Use_Pen()
    {
        if (!is_tool_pen)
            is_tool_pen = true;
        else
            is_tool_pen = false;
    }
    //  바이옴 적용하기
    /*
    public void Apply_Biome()
    {
        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                int ten_latitude = map.size_y / 18;     //  위도10
                int equator = map.size_y / 2;           //  적도

                if (map.map_layer_1[j + i * map.size_y] != 1 && map.map_layer_2[j + i * map.size_y] == 0)
                {
                    if (j <= equator + ten_latitude && j >= equator - ten_latitude)
                    {
                        map.map_layer_2[j + i * map.size_y] = 5;
                    }
                    else if (j <= equator + ten_latitude * 3 && j >= equator - ten_latitude * 3)
                    {
                        map.map_layer_2[j + i * map.size_y] = 6;
                    }
                    else if (j <= equator + ten_latitude * 5 && j >= equator - ten_latitude * 5)
                    {
                        map.map_layer_2[j + i * map.size_y] = 7;
                    }
                    else if (j <= equator + ten_latitude * 7 && j >= equator - ten_latitude * 7)
                    {
                        map.map_layer_2[j + i * map.size_y] = 8;
                    }
                    else if (j <= equator + ten_latitude * 9 && j >= equator - ten_latitude * 9)
                    {
                        map.map_layer_2[j + i * map.size_y] = 9;
                    }
                    else
                    {
                        map.map_layer_2[j + i * map.size_y] = 9;
                    }
                }
            }
        }
    }*/
}
