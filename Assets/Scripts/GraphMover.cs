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
        var graph = AstarPath.active.data.gridGraph;
        graph.center = pos;
        graph.SetDimensions(58, 58, 0.5f);
        AstarPath.active.Scan(graph);
    }
}
