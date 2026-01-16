using System;
using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Boot:MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            LayerHelper.Setup();
            SortLayerHelper.Setup();
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}