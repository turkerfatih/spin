using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Create Reel Configuration", fileName = "ReelConfiguration", order = 0)]
    public class ReelConfiguration:ScriptableObject
    {
        public List<ReelConfigurationItem> Items;
    }
}