/* Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Notes:
///Chase, please note that for the grid node array, you are just conceptualizing
///the rooms and don't need too much information.
///I can randomly select the number of doors on each side that are needed to refer to
///when I actually spawn the rooms.
///Once I start actually spawning rooms use the conceptual grid to find rooms that have
///the stated number of openings on a particular side that I determined earlier to choose
///which rooms to actually spawn. Then once I start spawning rooms I'll use their room info to
///figure out what rooms I need to spawn afterwards that will fit that description
///




/// <summary> The map generator that spawns in the rooms in a 6x6 grid. </summary>
public class MapGenerator : MonoBehaviour
{
    #region Temporary Variables
    public GameObject basePrefab;

    /// <summary>
    /// Grid size is set as follows. 
    /// Max Y value will be set to gridSize.
    /// Max X value will be set to gridSize + 1.
    /// 
    /// Grid pos set as follows: grid[y, x]
    /// </summary>
    public Room[,] grid;

    /// <summary>
    /// This array is used to lay out the conceptual framework of the map, and
    /// will be used to aid in choosing what rooms we actually need to spawn.
    /// Here we can also figure out later on what rooms will remain empty.
    /// </summary>
    private GridNode[,] conceptGrid;

    private int roomSize = 31;

    private int columns;
    
    private int rows;
    #endregion

    public RoomContainer roomCont;

    private Vector2 SpawnRoomPos, SpawnRoomGridPos;
    private Vector2 BossRoomPos, BossRoomGridPos;
    private Vector2 trueGridSize;

    public GridInfo gridInfo = new GridInfo();

    public void Awake()
    {
        if (roomCont == null)
            Debug.LogError("Room Container is empty. Please insert the scriptable object.");
    }

    public void Start()
    {
        grid = new Room[gridInfo.gridSize, gridInfo.gridSize + 1];
        conceptGrid = new GridNode[gridInfo.gridSize, gridInfo.gridSize + 1];
        columns = gridInfo.gridSize + 1;
        rows = gridInfo.gridSize;

        trueGridSize = new Vector2(gridInfo.gridSize + 1, gridInfo.gridSize);

        ///Initializing the concept grid.
        Debug.Log("Initializing the concept grid.");
        for (int y = 0; y < trueGridSize.y; y++)
        {
            for (int x = 0; x < trueGridSize.x; x++)
            {
                conceptGrid[y, x] = new GridNode(x, y);
            }
        }

        SpawnMap();
    }

    /// <summary> Function which spawns in all of the rooms on the map. </summary>
    public void SpawnMap()
    {
        ///NOTES:
        /// 1. The spawn room will be in column zero so that it can remain in the
        /// array and to ensure that my algorithm for the path making will be consistent
        /// and will work better.
        ///     a. What this means is that nothing will be populating the grid on any row
        ///     at column position zero except for the spawn room.
        ///     
        
        Vector2 tempGridPos;

        #region Spawning the spawn room
        int SRY = Random.Range(0, (int)trueGridSize.y);

        GameObject spawnRoom = Instantiate(roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)],
                                           new Vector3(0, SRY * roomSize),
                                           Quaternion.identity);

        tempGridPos = new Vector2(0, SRY);
        SpawnRoomGridPos = tempGridPos;

        spawnRoom.GetComponent<Room>().gridPos = tempGridPos;
        spawnRoom.name = "Spawn Room";
        
        SpawnRoomPos = spawnRoom.transform.position;


        grid[(int)tempGridPos.y, 0] = spawnRoom.GetComponent<Room>();
        conceptGrid[(int)tempGridPos.y, 0] = new GridNode(GridNode.RoomType.Spawn);

        ///Add spawn room position to concept grid.
        //RoomInfo tempRoomInfo = spawnRoom.GetComponent<Room>().roomInfo;


        //conceptGrid[(int)tempGridPos.y, 0] = new GridNode(GridNode.RoomType.Spawn,
        //                                                  tempRoomInfo.CalcDoorsLeftSide(),
        //                                                  tempRoomInfo.CalcDoorsRightSide(),
        //                                                  tempRoomInfo.CalcDoorsTopSide(),
        //                                                  tempRoomInfo.CalcDoorsBottomSide());

        #endregion

        #region Spawning Boss Room

        int BRGX;
        int BRGY;
        Vector3 BRPos;
        do
        {
            BRGX = Random.Range(1, (int)trueGridSize.x);
            BRGY = Random.Range(0, (int)trueGridSize.y);

            BRPos = new Vector3(BRGX * roomSize, BRGY * roomSize);

            Debug.Log(BRPos);
        } while (!gridInfo.CheckSpawnBossDist(SpawnRoomPos, BRPos));
        

        GameObject bossRoom = Instantiate(roomCont.BossRooms[Random.Range(0, roomCont.BossRooms.Count)],
                                          BRPos,
                                          Quaternion.identity);


        tempGridPos = new Vector2(BRPos.x / roomSize, BRPos.y / roomSize);
        BossRoomGridPos = tempGridPos;

        bossRoom.GetComponent<Room>().gridPos = tempGridPos;
        bossRoom.name = "Boss Room: " + tempGridPos;

        BossRoomPos = bossRoom.transform.position;

        grid[(int)tempGridPos.y, (int)tempGridPos.x] = bossRoom.GetComponent<Room>();
        
        ///Add boss room to concept grid.
        conceptGrid[(int)tempGridPos.y, (int)tempGridPos.x] = new GridNode(GridNode.RoomType.Boss);

        Debug.Log(Vector3.Distance(SpawnRoomPos, BossRoomPos));

        //conceptGrid[(int)tempGridPos.y, (int)tempGridPos.x] = new GridNode(GridNode.RoomType.Boss,
        //                                                  tempRoomInfo.CalcDoorsLeftSide(),
        //                                                  tempRoomInfo.CalcDoorsRightSide(),
        //                                                  tempRoomInfo.CalcDoorsTopSide(),
        //                                                  tempRoomInfo.CalcDoorsBottomSide());

        #endregion

        ///Need to write down notes on how to implement this randomness into generation. 
        ///Perhaps a RNG value that will determine if we follow the quickest path or 
        ///perhaps a slightly less cost effective path.
        ///
        /// Steps: create the path --> Select the rooms that will go on the path --> spawn rooms
        /// 
        ///I think that a decent idea here for creating the pathways will be to figure out first
        ///what directions I need to go first for the openings. For example: any data value in a list
        ///will contain information on the previous direction we came from, and the direction we need
        ///to follow next. This might mean that I can swap out the astar algorithm to instead return
        ///a list of a custom data value rather than a list of room scripts.
        ///
        ///Another useful tool would be having a secondary 2D array which will contain this information
        ///so that when we are spawning in the rooms we can cross reference that data and make the
        ///apropriate decisions on what rooms to spawn then.
        ///
        ///What's helpful about the above note is that I can instead use this custom class here to 
        ///make up the baseline of the map conceptually, then I use it to choose the rooms to spawn.
        ///So first and foremost I should populate a 2D array with the conceptual stuff, then populate
        ///the actual grid with the spawned in rooms that match the requirements. It will also
        ///probably mean that the Room scripts won't even be used at all for determing
        ///the pathing, and thus the functions need to be adjusted to no longer use the Room scripts.
        ///
        ///To remind myself as to what a room needs:
        ///1. A room on the path will have at least two connections.
        ///2. Rooms can have a max of four.
        ///3. Dead end rooms will obviously have one connection.
        ///
        ///Time to adjust the Astar algorithm to utilize my internal class rather than the room class.

        CreatePath(AStar(conceptGrid[(int)SpawnRoomGridPos.y, (int)SpawnRoomGridPos.x],
                         conceptGrid[(int)BossRoomGridPos.y, (int)BossRoomGridPos.x]));


        #region Spawn in rooms that will go on the path




        #endregion


        #region Spawning Shops



        #endregion

        #region Spawning in normal rooms

        ///Spawning a row
        for (int y = 0; y < trueGridSize.y; y++)
        {

            GameObject row = new GameObject("Row " + y);
            row.transform.parent = transform;

            ///Spawning in a column
            for (int x = 1; x < trueGridSize.x; x++)
            {
                if (grid[y, x] != null) continue;

                GameObject temp = Instantiate(basePrefab,
                                              new Vector3(roomSize * x, roomSize * y),
                                              Quaternion.identity);

                temp.name = "Room: (" + x + ", " + y + ")";
                
                temp.GetComponent<Room>().gridPos = new Vector2(x, y);
                
                temp.transform.parent = row.transform;
                
                grid[y, x] = temp.GetComponent<Room>();
            }
        }

        #endregion
    }

    private void CreatePath(List<GridNode> path)
    {
        ///This is where we'll do the necessary actions on that critical path.
        ///As of right now I want this function to only work on the critical path.
        ///It will be edited and updated later in order to work with successful
        ///pathways towards the shops, tresure rooms, and to dead ends.
        ///
        ///Remember to pull the room info scriptable object off of the prefab
        ///so that I can appropriately tell which rooms have what I need.
        ///For each grid spot I will need to make sure that the prefab matches
        ///EXACTLY what it is that is needed.

        if (path == null) { Debug.LogError("Huh"); }

        for (int index = 0; index < path.Count; index++)
        {
            if (path[index].roomType == GridNode.RoomType.Spawn) continue;

            if (path[index - 1].roomType == GridNode.RoomType.Spawn)
            {
                ///perform the actions for the spawn room.
                DetermineDir(path[index], path[index - 1], true);
            }
            else
            {
                ///we need to determine what direction the last room was in relation to the current room
                ///the current room being the current index.
                DetermineDir(path[index], path[index - 1]);
            }
        }

        ///Now spawn in the rooms
        ///Logic notes for spawning.
        ///1. Each room has a certain number of openings each side
        ///2. Each door is located in one of three positions on each side.
        ///3. Rooms must match their neighbors in terms of the positioning of their
        ///doorways that lead to the neighboring rooms.
        ///
        ///For the time being rooms will only have a max of one entrance on each side.
        ///In the future there will be rooms that have multiple entrances on one side,
        ///and will need to be updated appropriately.
        ///
        ///

        
        foreach (GridNode node in path)
        {
            node.openings.ToString();
        }

        for (int index = 1; index < path.Count; index++)
        {
            GridNode currNode = path[index];

            List<Room> examinedRooms = new List<Room>();

            foreach (GameObject obj in roomCont.RegularRooms)
            {
                Room room = obj.GetComponent<Room>();

                RoomInfo tempInfo = room.roomInfo;
                
                

            }

            examinedRooms.Clear();
        }

        void DetermineDir(GridNode currNode, GridNode prevNode, bool onlyOneOpening = false)
        {
            int numOpenings = 1;

            //if (onlyOneOpening == false)
            //{
            //    numOpenings = Random.Range(1, 4);
            //}

            if (currNode.gridPos.y > prevNode.gridPos.y)
            {
                ///curr node above prev node
                prevNode.openings.TopSide = numOpenings;
                currNode.openings.BottomSide = numOpenings;

            }
            else if (currNode.gridPos.y < prevNode.gridPos.y)
            {
                ///curr node below prev node
                prevNode.openings.BottomSide = numOpenings;
                currNode.openings.TopSide = numOpenings;
            }
            else if (currNode.gridPos.x > prevNode.gridPos.x)
            {
                ///curr node to right of prev node
                prevNode.openings.RightSide = numOpenings;
                currNode.openings.LeftSide = numOpenings;
            }
            else
            {
                ///curr node to left of prev node
                prevNode.openings.LeftSide = numOpenings;
                currNode.openings.RightSide = numOpenings;
            }
        }

        bool CompareNodeToRoom(GridNode node, RoomInfo roomInfo)
        {

        }

    }

    #region Grid concept Astar
    public List<GridNode> AStar(GridNode start, GridNode end)
    {
        //Debug.Log("Starting at " + start.gridPos.ToString());
        //Debug.Log("Looking for " + end.gridPos.ToString());

        List<GridNode> frontier = new List<GridNode>(0);

        HashSet<GridNode> explored = new HashSet<GridNode>();

        frontier.Add(start);

        foreach (GridNode node in conceptGrid)
        {
            if (node != null) node.gCost = 0;
        }

        while (frontier.Count > 0)
        {
            GridNode currNode = frontier[0];

            for (int index = 1; index < frontier.Count; index++)
            {
                if (frontier[index].fCost < currNode.fCost
                    || (frontier[index].fCost == currNode.fCost && frontier[index].hCost < currNode.hCost))
                {
                    currNode = frontier[index];
                }
            }

            frontier.Remove(currNode);
            explored.Add(currNode);

            if (currNode == end)
            {
                //Debug.Log("We have found the end, time to retrace.");
                return RetracePath(start, end);
            }

            foreach (GridNode neighbor in Neighbors(currNode))
            {
                if (explored.Contains(neighbor) || neighbor == null) continue;

                int currCost = currNode.gCost;

                if (currCost < neighbor.gCost || !frontier.Contains(neighbor))
                {
                    neighbor.gCost = currCost;
                    neighbor.hCost = GetDistCost(neighbor, end);
                    neighbor.parent = currNode;
                    if (!explored.Contains(neighbor)) frontier.Add(neighbor);
                }
            }
        }

        Debug.LogError("We shouldn't be getting here.");
        return null;
    }

    /// <summary> Traces the path that we have created. </summary>
    /// <param name="startNode">The node to start the retrace at.</param>
    /// <param name="endNode">The end goal node.</param>
    /// <returns>A list representing the path from <paramref name="startNode"/> to <paramref name="endNode"/>.</returns>
    private List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();

        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        if (currentNode == startNode)
        {
            path.Add(currentNode);
        }

        path.Reverse();

        return path;
    }

    /// <summary> Returns the distance cost between two nodes. </summary>
    /// <param name="nodeA">The node we are looking at.</param>
    /// <param name="nodeB">The goal node that we want the distance too.</param>
    /// <returns>The distance cost between <paramref name="nodeA"/> and <paramref name="nodeB"/>.</returns>
    public int GetDistCost(GridNode nodeA, GridNode nodeB)
    {
        int distZ = (int)Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);
        int distX = (int)Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);

        if (distX > distZ) return 10 * distZ + 10 * (distX - distZ);
        else return 10 * distX + 10 * (distZ - distX);

    }

    /// <summary> Returns a list of the node surrounding the node we are looking at. </summary>
    /// <param name="node">The node whose neighbors we want to get.</param>
    /// <returns>A list of nodes that neighbor the <paramref name="node"/></returns>
    private List<GridNode> Neighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        int gridPosX = (int)node.gridPos.x;
        int gridPosY = (int)node.gridPos.y;

        ///Checking left
        if (gridPosX > 0) neighbors.Add(conceptGrid[gridPosY, gridPosX - 1]);

        ///Checking right
        if (gridPosX < columns - 1) neighbors.Add(conceptGrid[gridPosY, gridPosX + 1]);

        ///Checking up
        if (gridPosY < rows - 1) neighbors.Add(conceptGrid[gridPosY + 1, gridPosX]);

        ///Checking down
        if (gridPosY > 0) neighbors.Add(conceptGrid[gridPosY - 1, gridPosX]);

        return neighbors;
    }

    #endregion

}

#region Local classes
[System.Serializable]
public class GridInfo
{
    [Tooltip("Represents the size of the grid")]
    [Range(1, 15)]
    public int gridSize = 7;

    [Tooltip("Represents the minimum distance from the spawn room to the boss room.")]
    [Range(1f, 10f)]
    public float minDistSTB;

    public GridInfo() { minDistSTB = 4f; }

    public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 BRPos)
    {
        Debug.Log("Dist of STB = " + Vector3.Distance(SpawnRoomPos, BRPos) / 31);
        return Vector3.Distance(SpawnRoomPos, BRPos) / 31 > minDistSTB;
    }
}

/// <summary>
/// A class that makes a node for the grid that will be used to help in conceptualizing
/// the map as it is being generated.
/// </summary>
public class GridNode
{
    public struct Openings
    {
        [Range(0, 3)]
        public int LeftSide;

        [Range(0, 3)]
        public int RightSide;

        [Range(0, 3)]
        public int TopSide;

        [Range(0, 3)]
        public int BottomSide;

        public override string ToString()
        {
            Debug.LogFormat("Top: {0}, Bottom: {1}, Left: {2}, Right: {3}", TopSide, BottomSide, LeftSide, RightSide);
            return "";
        }
    }

    public enum RoomType
    {
        Normal,
        Spawn,
        Boss,
        Shop,
        Treasure,
        Filled
    }

    public Vector2 gridPos;
    public Openings openings;
    public RoomType roomType = RoomType.Normal;

    public int gCost;

    public int hCost;

    public int fCost { get => gCost + hCost; }

    public GridNode parent;

    /// <summary> Parameterless constructor that just gives a basic initialization of node. </summary>
    public GridNode()
    {
        Initialization();
    }

    /// <summary> Constructor that allows specifying room type if anything other than normal. </summary>
    /// <param name="roomType">The type of the room that is placed on the grid.</param>
    public GridNode(RoomType roomType)
    {
        Initialization();
        this.roomType = roomType;
    }

    /// <summary> Construct that allows specificying room type as well as openings on the sides if known. </summary>
    /// <param name="roomType">The type of room that is placed on the grid.</param>
    /// <param name="leftSide">Number of openings on the left side.</param>
    /// <param name="rightSide">Number of openings on the right side.</param>
    /// <param name="topSide">Number of openings on the top side.</param>
    /// <param name="bottomSide">Number of openings on the bottom side.</param>
    public GridNode(RoomType roomType, int leftSide = 0, int rightSide = 0, int topSide = 0, int bottomSide = 0)
    {
        Initialization();
        this.roomType = roomType;
        openings.LeftSide = leftSide;
        openings.RightSide = rightSide;
        openings.TopSide = topSide;
        openings.BottomSide = bottomSide;

    }

    public GridNode(int xCor, int yCor)
    {
        Initialization();
        gridPos = new Vector2(xCor, yCor);
    }

    /// <summary> Little helper function for constructors that initializes the basic variables. </summary>
    private void Initialization()
    {
        gridPos = Vector2.zero;
        gCost = 0;
        hCost = 0;
        parent = null;
    }
}
#endregion