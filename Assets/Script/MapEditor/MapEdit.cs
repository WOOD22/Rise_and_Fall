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

    public Text new_map_name;
    public Text new_map_size_x;
    public Text new_map_size_y;

    public Map map;

    public GameObject tile_layer_1;
    public GameObject test_tile;
    //  레이어 1
    public Sprite ocean_tile;
    public Sprite plain_tile;
    public Sprite hill_tile;
    public Sprite mountain_tile;

    public bool is_tool_pen = false;

    void Start()
    {
        //map = DataManager.GetComponent<MapData>().map;
    }

    void Update()
    {
        Tool_Pen();
    }

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
    //세이브페이지 오픈
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
            //생성된 맵세이브파일 프리팹이 실제 맵세이브 파일보다 적을 경우 새로운 프리팹 생성
            if (MapSaveFile_content.transform.childCount < fi.Length)
            {
                GameObject instance = Instantiate(map_save_file, MapSaveFile_content.transform);
                instance.transform.GetChild(0).GetComponent<Text>().text = fi[i].Name;
            }
        }
    }

    public void Save_Map()
    {
        DataManager.GetComponent<MapData>().Save_File(map.name);
    }

    public void Load_Map(string map_name)
    {
        DataManager.GetComponent<MapData>().Load_File(map_name);
        map = DataManager.GetComponent<MapData>().map;

        for (int i = 0; i < map.size_x; i++)
        {
            for (int j = 0; j < map.size_y; j++)
            {
                if (map.map_layer_1[j + i * map.size_y] == 1)
                {
                    if (tile_layer_1.transform.childCount < (j + i * map.size_y))
                    {
                        GameObject instance = Instantiate(test_tile, tile_layer_1.transform);
                        instance.GetComponent<SpriteRenderer>().sprite = ocean_tile;
                        instance.name = "Tile_" + (j + i * map.size_y);
                        instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                    }
                    else
                    {
                        tile_layer_1.transform.GetChild(j + i * map.size_y).gameObject.GetComponent<SpriteRenderer>().sprite = ocean_tile;
                    }
                }
                if (map.map_layer_1[j + i * map.size_y] == 2)
                {
                    if (tile_layer_1.transform.childCount < (j + i * map.size_y))
                    {
                        GameObject instance = Instantiate(test_tile, tile_layer_1.transform);
                        instance.GetComponent<SpriteRenderer>().sprite = plain_tile;
                        instance.name = "Tile_" + (j + i * map.size_y);
                        instance.transform.localPosition = new Vector3((float)i + (float)j * 0.5f, (float)j * 0.75f, 0);
                    }
                    else
                    {
                        tile_layer_1.transform.GetChild(j + i * map.size_y).gameObject.GetComponent<SpriteRenderer>().sprite = plain_tile;
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

    //펜 기능
    public void Tool_Pen()
    {
        if (is_tool_pen && Input.GetMouseButton(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouse_position, transform.forward, 15);
            if (hit)
            {
                hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = plain_tile;
                if(hit.transform.gameObject.name.Contains("Tile"))
                {
                    string tile_num = hit.transform.gameObject.name;
                    tile_num = tile_num.Replace("Tile_", "");
                    map.map_layer_1[int.Parse(tile_num)] = 2;
                }
            }
        }
    }

    public void Tool_Use_Pen()
    {
        is_tool_pen = true;
    }
}
