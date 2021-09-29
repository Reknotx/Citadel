using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMover : MonoBehaviour
{
    public static GraphMover Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void MoveGraph(Vector3 pos)
    {
        AstarPath.active.data.gridGraph.center = new Vector3(pos.x + 15, pos.y + 15, 0);
        //path.data.gridGraph.
    }
}
