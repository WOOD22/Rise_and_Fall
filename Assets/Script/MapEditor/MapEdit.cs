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

    public GameObject tile_layer_1;
    public GameObject tile_layer_2;
    public GameObject test_tile;

    public Sprite null_tile;
    //  1레이어(고저)
    public Sprite ocean_tile;       //  1번 바다 타일
    public Sprite plain_tile;       //  2번 평지 타일
    public Sprite hill_tile;        //  3번 언덕 타일
    public Sprite mountain_tile;    //  4번 산 타일
    //  2레이어(바이옴)
    public Sprite a_tile;           //  5번 열대 타일
    public Sprite b_tile;           //  6번 건조 타일
    public Sprite c_tile;           //  7번 온대 타일
    public Sprite d_tile;           //  8번 냉대 타일
    public Sprite e_tile;           //  9번 한대 타일
    //  3레이어(수계)
    public Sprite river_T_tile;     //  10번 T형 강 타일
    public Sprite river_V_1_tile;   //  11번 V형 강 타일1
    public Sprite river_V_2_tile;   //  12번 V형 강 타일2
    public Sprite river_V_3_tile;   //  13번 V형 강 타일3
    public Sprite river_I_1_tile;   //  14번 I형 강 타일1
    public Sprite river_I_2_tile;   //  15번 I형 강 타일2
    public Sprite river_I_3_tile;   //  16번 I형 강 타일3


    public Sprite select_tile_sprite;
    public int[] select_layer;
    public int select_tile_type;

    public bool is_tool_pen = false;

    void Start()
    {
        select_tile_sprite = null_tile;
        select_tile_type = 1;
    }

    void Update()
    {
        Tool_Pen(select_layer, select_tile_sprite, select_tile_type);
    }
    //  새로운 맵 만들기(1레이어, 바다 타일로 초기화)
    public void New_Map()
    {
        map = DataManager.GetComponent<MapData>().map;

        map.name = new_map_name.text;
        map.size_x = int.Parse(new_map_size_x.text);
        map.size_y = int.Parse(new_map_size_y.text);
        map.map_layer_1 = new int[map.size_x * map.size_y];
        map.map_layer_2 = new int[map.size_x * map.size_y];
        map.map_layer_3 = new int[map.size_x * map.size_y];
        map.map_layer_4 = new int[map.size_x * map.size_y];
        map.map_layer_5 = new int[map.size_x * map.size_y];
        map.map_layer_6 = new int[map.size_x * map.size_y];
        map.map_layer_7 = new int[map.size_x * map.size_y];

        select_layer = map.map_layer_1;

        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                map.map_layer_1[j + i * map.size_y] = 1;
            }
        }
        for (int i = 0; i < tile_layer_1.transform.childCount; i++)
        {
            if (i > map.size_x * map.size_y)
            {
                tile_layer_1.transform.GetChild(i).gameObject.SetActive(false);
            }
            else if (i <= map.size_x * map.size_y)
            {
                tile_layer_1.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                //  1레이어********************************************************************************************
                Matching_Tile(map.map_layer_1, tile_layer_1, 0, null_tile);
                Matching_Tile(map.map_layer_1, tile_layer_1, 1, ocean_tile);
                //  로드 시 map의 정보와 보여지는 타일의 이미지를 일치시킴
                void Matching_Tile(int[] map_layer, GameObject layer, int tile_type, Sprite tile_sprite)
                {
                    if (map_layer[j + i * map.size_y] == tile_type)
                    {
                        if (layer.transform.childCount <= (j + i * map.size_y))
                        {
                            GameObject instance = Instantiate(test_tile, layer.transform);
                            instance.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                            instance.name = "Tile_" + (j + i * map.size_y);
                            instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                        }
                        else
                        {
                            layer.transform.GetChild(j + i * map.size_y).gameObject.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                            layer.transform.GetChild(j + i * map.size_y).localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                        }
                    }
                }
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
        select_layer = map.map_layer_1;
        //맵 초기화(남는 타일은 비활성화 한다.)
        for (int i = 0; i < tile_layer_1.transform.childCount; i++)
        {
            if (i > map.size_x * map.size_y)
            {
                tile_layer_1.transform.GetChild(i).gameObject.SetActive(false);
            }
            else if (i <= map.size_x * map.size_y)
            {
                tile_layer_1.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        //  맵 구성하기, 이미 타일이 존재할 경우 재활용
        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                //  1레이어********************************************************************************************
                Matching_Tile(map.map_layer_1, tile_layer_1, 0, null_tile);
                Matching_Tile(map.map_layer_1, tile_layer_1, 1, ocean_tile);
                Matching_Tile(map.map_layer_1, tile_layer_1, 2, plain_tile);
                Matching_Tile(map.map_layer_1, tile_layer_1, 3, hill_tile);
                Matching_Tile(map.map_layer_1, tile_layer_1, 4, mountain_tile);
                //  2레이어********************************************************************************************
                //  로드 시 map의 정보와 보여지는 타일의 이미지를 일치시킴
                void Matching_Tile(int[] map_layer, GameObject layer,int tile_type, Sprite tile_sprite)
                {
                    if (map_layer[j + i * map.size_y] == tile_type)
                    {
                        if (layer.transform.childCount <= (j + i * map.size_y))
                        {
                            GameObject instance = Instantiate(test_tile, layer.transform);
                            instance.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                            instance.name = "Tile_" + (j + i * map.size_y);
                            instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                        }
                        else
                        {
                            layer.transform.GetChild(j + i * map.size_y).gameObject.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                            layer.transform.GetChild(j + i * map.size_y).localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                        }
                    }
                }
            }
        }
    }

    //맵 채우기
    void Fill_Map(int _x, int _y, GameObject fill_tile)
    {
        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                GameObject instance = Instantiate(fill_tile, tile_layer_1.transform);
                instance.name = "Tile_" + (j + i * _y);
                instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                map.map_layer_1[j + i * _y] = 1;
            }
        }
    }

    //  색칠할 타일 선택
    public void Select_Tile_Sprite(int tile_type)
    {
        switch(tile_type)
        {
            case 0:
                select_tile_sprite = null_tile;
                select_tile_type = 0;
                break;
            case 1:
                select_tile_sprite = ocean_tile;
                select_tile_type = 1;
                break;
            case 2:
                select_tile_sprite = plain_tile;
                select_tile_type = 2;
                break;
            case 3:
                select_tile_sprite = hill_tile;
                select_tile_type = 3;
                break;
            case 4:
                select_tile_sprite = mountain_tile;
                select_tile_type = 4;
                break;
            case 5:
                select_tile_sprite = a_tile;
                select_tile_type = 5;
                break;
            case 6:
                select_tile_sprite = b_tile;
                select_tile_type = 6;
                break;
            case 7:
                select_tile_sprite = c_tile;
                select_tile_type = 7;
                break;
            case 8:
                select_tile_sprite = d_tile;
                select_tile_type = 8;
                break;
            case 9:
                select_tile_sprite = e_tile;
                select_tile_type = 9;
                break;
            case 10:
                select_tile_sprite = river_T_tile;
                select_tile_type = 10;
                break;
            case 11:
                select_tile_sprite = river_V_1_tile;
                select_tile_type = 11;
                break;
            case 12:
                select_tile_sprite = river_V_2_tile;
                select_tile_type = 12;
                break;
            case 13:
                select_tile_sprite = river_V_3_tile;
                select_tile_type = 13;
                break;
            case 14:
                select_tile_sprite = river_I_1_tile;
                select_tile_type = 14;
                break;
            case 15:
                select_tile_sprite = river_I_2_tile;
                select_tile_type = 15;
                break;
            case 16:
                select_tile_sprite = river_I_3_tile;
                select_tile_type = 16;
                break;
        }
    }

    //  하이라이트 될 레이어 선택 
    public void Select_Layer(int layer_num)
    {
        switch (layer_num)
        {
            case 1:
                select_layer = map.map_layer_1;
                break;
            case 2:
                select_layer = map.map_layer_2;
                break;
            case 3:
                select_layer = map.map_layer_3;
                break;
            case 4:
                select_layer = map.map_layer_4;
                break;
        }
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            //  1레이어********************************************************************************************
            Matching_Tile(select_layer, tile_layer_1, 0, null_tile);
            Matching_Tile(select_layer, tile_layer_1, 1, ocean_tile);
            Matching_Tile(select_layer, tile_layer_1, 2, plain_tile);
            Matching_Tile(select_layer, tile_layer_1, 3, hill_tile);
            Matching_Tile(select_layer, tile_layer_1, 4, mountain_tile);
            //  2레이어********************************************************************************************
            Matching_Tile(select_layer, tile_layer_1, 5, a_tile);
            Matching_Tile(select_layer, tile_layer_1, 6, b_tile);
            Matching_Tile(select_layer, tile_layer_1, 7, c_tile);
            Matching_Tile(select_layer, tile_layer_1, 8, d_tile);
            Matching_Tile(select_layer, tile_layer_1, 9, e_tile);
            //  3레이어********************************************************************************************
            Matching_Tile(select_layer, tile_layer_1, 10, river_T_tile);
            Matching_Tile(select_layer, tile_layer_1, 11, river_V_1_tile);
            Matching_Tile(select_layer, tile_layer_1, 12, river_V_2_tile);
            Matching_Tile(select_layer, tile_layer_1, 13, river_V_3_tile);
            Matching_Tile(select_layer, tile_layer_1, 14, river_I_1_tile);
            Matching_Tile(select_layer, tile_layer_1, 15, river_I_2_tile);
            Matching_Tile(select_layer, tile_layer_1, 16, river_I_3_tile);
            //  로드 시 map의 정보와 보여지는 타일의 이미지를 일치시킴
            void Matching_Tile(int[] map_layer, GameObject layer, int tile_type, Sprite tile_sprite)
            {
                if (map_layer[i] == tile_type)
                {
                    layer.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                }
            }
        }
    }
    //  펜 기능
    public void Tool_Pen(int[] map_layer, Sprite tile_sprite, int tile_type)
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
                    if (map_layer[int.Parse(tile_num)] != 0)
                    {
                        map_layer[int.Parse(tile_num)] = tile_type;
                        hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                    }
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
    }
}
