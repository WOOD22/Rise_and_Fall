using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

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
    public GameObject test_tile;

    public Sprite null_tile;
    //  1레이어(고저)
    public Sprite ocean_tile;       //  1번 바다 타일
    public Sprite plain_tile;       //  2번 평지 타일
    public Sprite hill_tile;        //  3번 언덕 타일
    public Sprite mountain_tile;    //  4번 산 타일

    public GameObject select_layer;
    public Sprite select_tile_sprite;
    public int select_tile_type;

    public bool is_tool_pen = false;

    void Start()
    {
        select_tile_sprite = null_tile;
        select_tile_type = 0;
    }

    void Update()
    {
        Tool_Pen(map.map_layer_1, select_tile_sprite, select_tile_type);
    }
    //  새로운 맵 만들기
    public void New_Map()
    {
        map = DataManager.GetComponent<MapData>().map;

        map.name = new_map_name.text;
        map.size_x = int.Parse(new_map_size_x.text);
        map.size_y = int.Parse(new_map_size_y.text);
        map.map_layer_1 = new int[map.size_x * map.size_y];

        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                GameObject instance = Instantiate(test_tile, tile_layer_1.transform);
                instance.name = "Tile_" + (j + i * map.size_y);
                instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                map.map_layer_1[j + i * map.size_y] = 1;
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

    public void Select_Tile_Sprite(int tile_type)
    {
        switch(tile_type)
        {
            case 0:
                select_tile_sprite = null_tile;
                break;
            case 1:
                select_tile_sprite = ocean_tile;
                break;
            case 2:
                select_tile_sprite = plain_tile;
                break;
            case 3:
                select_tile_sprite = hill_tile;
                break;
            case 4:
                select_tile_sprite = mountain_tile;
                break;
        }
    }

    //펜 기능
    public void Tool_Pen(int[] map_layer, Sprite tile_sprite, int tile_type)
    {
        if (is_tool_pen && Input.GetMouseButton(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse_position, transform.forward, 15);
            if (hit)
            {
                hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = tile_sprite;
                if(hit.transform.gameObject.name.Contains("Tile"))
                {
                    string tile_num = hit.transform.gameObject.name;
                    tile_num = tile_num.Replace("Tile_", "");
                    map.map_layer_1[int.Parse(tile_num)] = tile_type;
                }
            }
        }
    }

    public void Tool_Use_Pen()
    {
        is_tool_pen = true;
    }
}
