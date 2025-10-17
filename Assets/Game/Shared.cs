using System;
using UnityEngine;

namespace Game
{
    public class Shared:MonoBehaviour
    {
        public static Shared Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this);
                return;
            } 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}