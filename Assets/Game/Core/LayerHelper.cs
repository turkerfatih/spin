using UnityEngine;

namespace Game.Core
{
    public static class LayerHelper
    {
        public static int Default;
        public static int UI;

        public static void Setup()
        {
            Default=LayerMask.NameToLayer("Default");
            UI=LayerMask.NameToLayer("UI");
        }

    }
}