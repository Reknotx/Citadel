/*
 * Author: Chase O'Connor
 *
 *  Brief: Manages the minimap so that the player can tell where they are.
 *
 */

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
            gameObject.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }

        public void ExpandMap()
        {
            //Take minimap from top right corner and expand it so that the player can see the whole
            //map of the castle.
        }
        
    }
}