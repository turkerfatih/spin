using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Component
{
    public class GridViewTest:MonoBehaviour
    {
        public GameObject Prefab;
        public int Count=30;
        public GridView View;

        private List<Transform> list;
        
        private void Awake()
        {
            list = new List<Transform>(Count);
            var parent=View.Content;
            for (int i = 0; i < Count; i++)
            {
                var go = Instantiate(Prefab, parent);
                go.SetActive(true);
                list.Add(go.transform);
            }
            View.Setup(list);
        }
        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                View.Scroll(Input.mouseScrollDelta.y);
            }
        }
    }
}