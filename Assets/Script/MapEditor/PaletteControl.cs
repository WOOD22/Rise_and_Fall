using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteControl : MonoBehaviour
{
    public bool open_palette;

    private void OnEnable()
    {
        open_palette = false;
    }

    public void Open_Palette()
    {
        if (!open_palette)
        {
            this.transform.localPosition = new Vector3(-256, -180, 0);
            open_palette = true;
        }
        else if (open_palette)
        {
            this.transform.localPosition = new Vector3(-320, -180, 0);
            open_palette = false;
        }
    }

    public void Up_Palette(GameObject bt)
    {
        if (bt.GetComponent<Toggle>().isOn)
        {
            bt.transform.GetChild(0).localPosition = new Vector3(0, 8, 0);
        }
        else if (!bt.GetComponent<Toggle>().isOn)
        {
            bt.transform.GetChild(0).localPosition = new Vector3(0, -200, 0);
        }
    }
}
