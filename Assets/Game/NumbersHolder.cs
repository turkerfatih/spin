using System;
using UnityEngine;

namespace Game
{
    public class NumbersHolder:MonoBehaviour
    {
        [SerializeField]
        private Sprite[] NumberSprites;

        private void Awake()
        {
            Services.Numbers = this;
        }

        public Sprite GetNumber(int number)
        {
            var index=Mathf.Clamp(number-1,0,NumberSprites.Length-1);
            return NumberSprites[index];
        }
    }
}