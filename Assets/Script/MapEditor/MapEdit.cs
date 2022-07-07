using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdit : MonoBehaviour
{
    public GameObject tile_layer_1;
    public GameObject ocean_tile;
    //public GameObject plain_tile;
    //public GameObject hill_tile;
    //public GameObject mountain_tile;

    void Start()
    {
        Fill_Map(200, 100, ocean_tile);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
    }
}
