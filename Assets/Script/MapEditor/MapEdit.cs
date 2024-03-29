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
    public Sprite tropical_tile;            //  4번 열대 타일
    public Sprite dry_tile;                 //  5번 건조 타일
    public Sprite desert_tile;              //  6번 사막 타일
    public Sprite temperate_tile;           //  7번 온대 타일
    public Sprite subarctic_tile;           //  8번 냉대 타일
    public Sprite polar_tile;               //  9번 한대 타일
    //  고저
    public Sprite[] hill_tile;              //  1번 언덕 타일
    public Sprite[] mountain_tile;          //  2번 산 타일
    //  토지비옥도
    public Sprite[] soil_fertility_tile;    //  토지비옥도 (0 : 매우 나쁨, 1 : 나쁨, 2 : 조금 나쁨, 3 : 보통, 4 : 조금 좋음, 5 : 좋음, 6 : 매우 좋음)

    public int select_layer = 1;
    public int select_tile_type;

    public bool is_tool_pen = false;

    void Start()
    {
        select_tile_type = 0;
    }

    void Update()
    {
        Tool_Pen(select_tile_type);
    }

    //  새로운 맵 만들기(1레이어, 바다 타일로 초기화)******************************************************************
    public void New_Map()
    {
        map = DataManager.GetComponent<MapData>().map;

        map.name = new_map_name.text;
        map.size_x = int.Parse(new_map_size_x.text);
        map.size_y = int.Parse(new_map_size_y.text);
        map.map_info = new Tile[map.size_x * map.size_y];

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
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = tropical_tile;
                    break;
                case 5:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dry_tile;
                    break;
                case 6:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = desert_tile;
                    break;
                case 7:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = temperate_tile;
                    break;
                case 8:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = subarctic_tile;
                    break;
                case 9:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = polar_tile;
                    break;
            }
            //  땅 존재 시 고저 타일 배치
            if (map.map_info[i].terrain < 4)
            {
                map.map_info[i].altitude = 0;
            }
            switch (map.map_info[i].altitude)
            {
                case 1:
                    switch(map.map_info[i].terrain)
                    {
                        case 4:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[0];
                            break;
                        case 5:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[1];
                            break;
                        case 6:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[2];
                            break;
                        case 7:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[3];
                            break;
                        case 8:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[4];
                            break;
                        case 9:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = hill_tile[5];
                            break;
                    }
                    break;
                case 2:
                    switch (map.map_info[i].terrain)
                    {
                        case 4:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[0];
                            break;
                        case 5:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[1];
                            break;
                        case 6:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[2];
                            break;
                        case 7:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[3];
                            break;
                        case 8:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[4];
                            break;
                        case 9:
                            tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = mountain_tile[5];
                            break;
                    }
                    break;
            }
            //  강타일 배치
            if (map.map_info[i].terrain >= 4)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (map.map_info[i].river[j] == 1)
                        tile_map.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                    else
                        tile_map.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
            }
            //  도로타일 배치
            if (map.map_info[i].terrain >= 4)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (map.map_info[i].road[j] == 1)
                        tile_map.transform.GetChild(i).GetChild(j + 6).gameObject.SetActive(true);
                    else
                        tile_map.transform.GetChild(i).GetChild(j + 6).gameObject.SetActive(false);
                }
            }
        }
    }
    //  비옥도 맵 이미지 매칭하기
    void Matching_Tile_Sprite_Soil_Fertility()
    {
        for (int i = 0; i < map.size_x * map.size_y; i++)
        {
            tile_map.transform.GetChild(i).gameObject.SetActive(true);
            //  비옥도 표기
            switch (map.map_info[i].property.tile_soil_fertility)
            {
                case 0:
                    tile_map.transform.GetChild(i).gameObject.SetActive(false);
                    break;
                case 1:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[1];
                    break;
                case 2:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[2];
                    break;
                case 3:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[3];
                    break;
                case 4:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[4];
                    break;
                case 5:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[5];
                    break;
                case 6:
                    tile_map.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = soil_fertility_tile[6];
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
                select_tile_type = 0;
                break;
            case 1:
                select_tile_type = 1;
                break;
            case 2:
                select_tile_type = 2;
                break;
            case 3:
                select_tile_type = 3;
                break;
            case 4:
                select_tile_type = 4;
                break;
            case 5:
                select_tile_type = 5;
                break;
            case 6:
                select_tile_type = 6;
                break;
            case 7:
                select_tile_type = 7;
                break;
            case 8:
                select_tile_type = 8;
                break;
            case 9:
                select_tile_type = 9;
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
                select_layer = 1;
                break;
            case 2:
                Matching_Tile_Sprite_Terrain();
                select_layer = 2;
                break;
            case 3:
                Matching_Tile_Sprite_Terrain();
                select_layer = 3;
                break;
            case 4:
                Matching_Tile_Sprite_Terrain();
                select_layer = 4;
                break;
            case 5:
                Matching_Tile_Sprite_Soil_Fertility();
                select_layer = 5;
                break;
        }
    }

    //  타일 색칠 시 비옥도 & 홀딩 크기, 고도
    public void Tile_Soil_Fertility(int tile_num)
    {
        switch (select_tile_type)
        {
            case 0:
                map.map_info[tile_num].property.tile_soil_fertility = 0;
                break;
            case 1:
                map.map_info[tile_num].property.tile_soil_fertility = 0;
                break;
            case 2:
                map.map_info[tile_num].property.tile_soil_fertility = 0;
                break;
            case 3:
                map.map_info[tile_num].property.tile_soil_fertility = 0;
                break;
            case 4:
                map.map_info[tile_num].property.tile_soil_fertility = 2;
                break;
            case 5:
                map.map_info[tile_num].property.tile_soil_fertility = 2;
                break;
            case 6:
                map.map_info[tile_num].property.tile_soil_fertility = 1;
                break;
            case 7:
                map.map_info[tile_num].property.tile_soil_fertility = 3;
                break;
            case 8:
                map.map_info[tile_num].property.tile_soil_fertility = 2;
                break;
            case 9:
                map.map_info[tile_num].property.tile_soil_fertility = 1;
                break;
        }
        //  홀딩 초기 설정
        void holding_setting(int altitude_min, int altitude_max)
        {
            int holding_size = UnityEngine.Random.Range(10, 20);
            for (int i = 0; i < holding_size; i++)
            {
                Holding new_holding = new Holding();
                new_holding.holding_code = "B000";
                new_holding.holding_altitude = UnityEngine.Random.Range(altitude_min, altitude_max);
                map.map_info[tile_num].holding.Add(new_holding);
            }
        }
    }

    //  펜 기능
    public void Tool_Pen(int tile_type)
    {
        if (is_tool_pen && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse_position, transform.forward, 15);
            Vector2 vec = new Vector2();
            //  case 3,4 충동제외
            float a = 0;        //  Mathf.Abs(hit.point.x - vec.x)
            float b = 0;        //  Mathf.Abs(hit.point.y - vec.y)
            float c = 0.1f;     //  충돌 제외 크기
            if (hit)
            {
                if (hit.transform.gameObject.name.Contains("Tile"))
                {
                    string tile_num = hit.transform.gameObject.name;
                    tile_num = tile_num.Replace("Tile_", "");
                    //
                    switch (select_layer)
                    {
                        case 1:
                            map.map_info[int.Parse(tile_num)].terrain = tile_type;
                            //  육지타일 배치 시 주변 타일 해안 타일로 바꿈
                            if (tile_type >= 4)
                            {
                                if (int.Parse(tile_num) + 1 < map.size_x * map.size_y && int.Parse(tile_num) % map.size_y != map.size_y - 1)
                                {
                                    if (map.map_info[int.Parse(tile_num) + 1].terrain == 0)
                                        map.map_info[int.Parse(tile_num) + 1].terrain = 1;
                                }
                                if (int.Parse(tile_num) - 1 >= 0 && int.Parse(tile_num) % map.size_y != 0)
                                {
                                    if (map.map_info[int.Parse(tile_num) - 1].terrain == 0)
                                        map.map_info[int.Parse(tile_num) - 1].terrain = 1;
                                }
                                if (int.Parse(tile_num) + map.size_y < map.size_x * map.size_y)
                                {
                                    if (map.map_info[int.Parse(tile_num) + map.size_y].terrain == 0)
                                        map.map_info[int.Parse(tile_num) + map.size_y].terrain = 1;
                                }
                                if (int.Parse(tile_num) - map.size_y >= 0)
                                {
                                    if (map.map_info[int.Parse(tile_num) - map.size_y].terrain == 0)
                                        map.map_info[int.Parse(tile_num) - map.size_y].terrain = 1;
                                }
                                if (int.Parse(tile_num) - map.size_y + 1 >= 0)
                                {
                                    if (map.map_info[int.Parse(tile_num) - map.size_y + 1].terrain == 0 && int.Parse(tile_num) % map.size_y != map.size_y - 1)
                                        map.map_info[int.Parse(tile_num) - map.size_y + 1].terrain = 1;
                                }
                                if (int.Parse(tile_num) + map.size_y - 1 < map.size_x * map.size_y && int.Parse(tile_num) % map.size_y != 0)
                                {
                                    if (map.map_info[int.Parse(tile_num) + map.size_y - 1].terrain == 0)
                                        map.map_info[int.Parse(tile_num) + map.size_y - 1].terrain = 1;
                                }
                            }
                            Matching_Tile_Sprite_Terrain();
                            Tile_Soil_Fertility(int.Parse(tile_num));
                            break;
                        case 2:
                            map.map_info[int.Parse(tile_num)].altitude = tile_type;
                            Matching_Tile_Sprite_Terrain();
                            break;
                        case 3:
                            vec = hit.transform.localPosition;
                            a = Mathf.Abs(hit.point.x - vec.x);
                            b = Mathf.Abs(hit.point.y - vec.y);
                            //  가장자리에서 6방향으로 레이 충돌
                            if (map.map_info[int.Parse(tile_num)].altitude == 0 && map.map_info[int.Parse(tile_num)].terrain >= 4)
                            {
                                //  북서
                                if (hit.point.x - vec.x > 0 && hit.point.y - vec.y > +(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[0] = tile_type;
                                    if(int.Parse(tile_num) + 1 < map.size_x * map.size_y && int.Parse(tile_num) % map.size_y != map.size_y - 1)
                                    {
                                        map.map_info[int.Parse(tile_num) + 1].river[3] = tile_type;
                                    }
                                }
                                //  남서
                                else if (hit.point.x - vec.x > 0 && hit.point.y - vec.y < -(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[2] = tile_type;
                                    if (int.Parse(tile_num) + map.size_y - 1 < map.size_x * map.size_y && int.Parse(tile_num) % map.size_y != 0)
                                    {
                                        map.map_info[int.Parse(tile_num) + map.size_y - 1].river[5] = tile_type;
                                    }
                                }
                                //  서
                                else if (hit.point.x - vec.x > 0 && a > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[1] = tile_type;
                                    if (int.Parse(tile_num) + map.size_y < map.size_x * map.size_y)
                                    {
                                        map.map_info[int.Parse(tile_num) + map.size_y].river[4] = tile_type;
                                    }
                                }

                                //  북동
                                if (hit.point.x - vec.x < 0 && hit.point.y - vec.y > -(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[5] = tile_type;
                                    if (int.Parse(tile_num) - map.size_y + 1 >= 0)
                                    {
                                        map.map_info[int.Parse(tile_num) - map.size_y + 1].river[2] = tile_type;
                                    }
                                }
                                //  남동
                                else if (hit.point.x - vec.x < 0 && hit.point.y - vec.y < +(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[3] = tile_type;
                                    if (int.Parse(tile_num) - 1 >= 0 && int.Parse(tile_num) % map.size_y != 0)
                                    {
                                        map.map_info[int.Parse(tile_num) - 1].river[0] = tile_type;
                                    }
                                }
                                //  동
                                else if (hit.point.x - vec.x < 0 && a > c)
                                {
                                    map.map_info[int.Parse(tile_num)].river[4] = tile_type;
                                    if (int.Parse(tile_num) - map.size_y >= 0)
                                    {
                                        map.map_info[int.Parse(tile_num) - map.size_y].river[1] = tile_type;
                                    }
                                }
                            }
                            Matching_Tile_Sprite_Terrain();
                            break;
                        case 4:
                            vec = hit.transform.localPosition;
                            a = Mathf.Abs(hit.point.x - vec.x);
                            b = Mathf.Abs(hit.point.y - vec.y);
                            if (map.map_info[int.Parse(tile_num)].terrain >= 4)
                            {
                                if (hit.point.x - vec.x > 0 && hit.point.y - vec.y > +(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[0] = tile_type;
                                }
                                else if (hit.point.x - vec.x > 0 && hit.point.y - vec.y < -(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[2] = tile_type;
                                }
                                else if (hit.point.x - vec.x > 0 && a > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[1] = tile_type;
                                }

                                if (hit.point.x - vec.x < 0 && hit.point.y - vec.y > -(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[5] = tile_type;
                                }
                                else if (hit.point.x - vec.x < 0 && hit.point.y - vec.y < +(hit.point.x - vec.x) / 2 && a > c && b > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[3] = tile_type;
                                }
                                else if (hit.point.x - vec.x < 0 && a > c)
                                {
                                    map.map_info[int.Parse(tile_num)].road[4] = tile_type;
                                }
                            }
                            Matching_Tile_Sprite_Terrain();
                            break;
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
}
