﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange(string nextScene)
    {
        LoadingSceneManager.LoadScene(nextScene);
    }
}
