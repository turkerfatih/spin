using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.UI.Component
{
    public class GridViewTest:MonoBehaviour
    {
        public CardView Prefab;
        public int Count=30;
        public GridView View;

        private List<CardView> list;
        

        IEnumerator  Start()
        {
            yield return null; // wait one frame to let TMP initialize
            list = new List<CardView>(Count);
            var parent=View.Content;
            for (int i = 0; i < Count; i++)
            {
                var cardView = Instantiate(Prefab, parent);
                cardView.gameObject.SetActive(true);
                list.Add(cardView);
                cardView.SetMasked(true);
            }
            View.Setup(list);
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                View.Scroll(Input.mouseScrollDelta.y);
            }

            /*if (Input.GetMouseButtonDown(0))
            {
                foreach (var item in list)
                {
                    var view = item.GetComponent<CardView>();
                    view.SetMasked(!view.IsMasked);
                }
            }*/
        }
    }
}