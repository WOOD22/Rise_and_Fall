using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdit : MonoBehaviour
{
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
        map.name = "Test_Map";
        map.size_x = 20;
        map.size_y = 10;
        map.map_layer_1 = new int[map.size_x * map.size_y];
        Fill_Map(map.size_x, map.size_y, test_tile);
    }

    void Update()
    {
        Tool_Pen();
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
