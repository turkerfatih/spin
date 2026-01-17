using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class DarabaController:MonoBehaviour
    {
        [SerializeField] private Daraba[]  darabas;

        private List<UniTask> tasks = new List<UniTask>(5);
        private float moveUpAmount = 8.32f;

        private void Awake()
        {
            Services.Darabas = this;
        }

        public UniTask AnimateOpen()
        {
            tasks.Clear();
            for (var i = 1; i <= 3; i++)
            {
                var daraba = darabas[i];
                tasks.Add(daraba.Open(moveUpAmount));
            }

            return UniTask.WhenAll(tasks);
        }
    }
}