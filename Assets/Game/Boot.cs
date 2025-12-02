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
            LayerHelper.Setup();
            SortLayerHelper.Setup();
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}