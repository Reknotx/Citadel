using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimap
{
    public class MiniMapManager : MonoBehaviour
    {
        public static MiniMapManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            Instance = this;

        }

        public void MoveMinimapCamera(Vector3 pos)
        {
            
        }


    }
}