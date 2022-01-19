using System.Collections.Generic;
using UnityEngine;

/*
 *  Notes:
 *  1. The concept grid class makes up the whole grid
 *  and will have all of the positions of the doorways
 *  already set up. So that the map generator will only have to look
 *  at each node and compare the rooms that match the requirements
 *  listed.
 */
public class ConceptGrid
{
    enum DoorSide
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3
    }
    
    private readonly NewGridInfo gridInfo;

    public NewGridNode[,] grid;

    /// <summary> Y value for the grid size. </summary>
    private readonly int rows;

    /// <summary> X value for the grid size. </summary>
    private readonly int columns;

    public readonly Vector2 spawnRoomPos, bossRoomPos, shopRoomPos;

    public List<List<NewGridNode>> Paths { get; private set; }

    /// <summary>
    /// Consctructor for the concept grid class.
    /// </summary>
    /// <param name="gridDimension"></param>
    /// <param name="info"></param>
    /// <remarks>Spawn room, shop room, and boss room locations are all determined in the
    /// constructor when the concept grid is created.</remarks>
    public ConceptGrid(Vector2 gridDimension, NewGridInfo info)
    {
        #region Initialization
        gridInfo = info;

        columns = (int) gridDimension.x;
        rows = (int) gridDimension.y;
        grid = new NewGridNode[rows, columns];
        
        //Initializing the concept grid.
        // Debug.Log("Initializing the concept grid.");
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                grid[y, x] = new NewGridNode(new Vector2(x, y));
            }
        }
        #endregion
        
        #region 1. Determine spawn room location
        int SRY = Random.Range(0, rows);

        grid[SRY, 0] = new NewGridNode(new Vector2(0, SRY), RoomType.Spawn);
        
        for (int y = 0; y < rows; y++)
        {
            if (y == SRY) continue;

            grid[y, 0].roomType = RoomType.Filled;
        }

        spawnRoomPos = new Vector2(0, SRY);
        #endregion

        #region 2. Determine boss room location
        do
        {
            //Subtracting 1 from col and rows helps ensure that the boss room will never spawn on the edges
            //of the map and will help with simplifying testing which side we will have the entrance on 
            bossRoomPos = new Vector2(Random.Range(4, columns - 1), Random.Range(1, rows - 1));

        } while (!CheckSpawnBossDist(spawnRoomPos, bossRoomPos) && !GridSpotEmpty(bossRoomPos));

        grid[(int) bossRoomPos.y, (int) bossRoomPos.x].roomType = RoomType.Boss;

        #region 2a. Put filled rooms around the boss room except for the entrance side
        
        int bossEntranceSide = Random.Range(0, 4);
        
        if (bossEntranceSide != (int) DoorSide.Top && (int)bossRoomPos.y + 1 != columns)
            grid[(int) bossRoomPos.y + 1, (int) bossRoomPos.x].roomType = RoomType.Filled; // Top
        
        if (bossEntranceSide != (int) DoorSide.Bottom && (int)bossRoomPos.y != 0)
            grid[(int) bossRoomPos.y - 1, (int) bossRoomPos.x].roomType = RoomType.Filled; // Bottom
        
        if (bossEntranceSide != (int) DoorSide.Left)
            grid[(int) bossRoomPos.y, (int) bossRoomPos.x - 1].roomType = RoomType.Filled; // Left
        
        if (bossEntranceSide != (int) DoorSide.Right && (int)bossRoomPos.x + 1 != rows)
            grid[(int) bossRoomPos.y, (int) bossRoomPos.x + 1].roomType = RoomType.Filled; // Right
        #endregion
        
        #endregion

        #region 3. Determine shop room location
        do
        {
            shopRoomPos = new Vector2(Random.Range(2, columns - 1), Random.Range(1, rows - 1));
        } while (!GridSpotEmpty(shopRoomPos));

        grid[(int)shopRoomPos.y, (int)shopRoomPos.x].roomType = RoomType.Shop;
        
        #region 3a. Put filled rooms around the shop room except for the entrance side
        int shopEntranceSide = Random.Range(0, 4);
        
        if (shopEntranceSide != (int) DoorSide.Top && (int)shopRoomPos.y + 1 != columns)
            grid[(int) shopRoomPos.y + 1, (int) shopRoomPos.x].roomType = RoomType.Filled; // Top
        
        if (shopEntranceSide != (int) DoorSide.Bottom && (int)shopRoomPos.y != 0)
            grid[(int) shopRoomPos.y - 1, (int) shopRoomPos.x].roomType = RoomType.Filled; // Bottom
        
        if (shopEntranceSide != (int) DoorSide.Left)
            grid[(int) shopRoomPos.y, (int) shopRoomPos.x - 1].roomType = RoomType.Filled; // Left
        
        if (shopEntranceSide != (int) DoorSide.Right && (int)shopRoomPos.x + 1 != rows)
            grid[(int) shopRoomPos.y, (int) shopRoomPos.x + 1].roomType = RoomType.Filled; // Right
        #endregion

        #endregion

        #region 4. Generate paths to these special locations from spawn room

        //   Add their grid node infos into the grid.
        //   The map generator will utilize this information to select the rooms that are desired
        GeneratePath();
        #endregion

        //Returns true if the grid node is empty
        bool GridSpotEmpty(Vector2 gridCord) =>
            grid[(int) gridCord.y, (int) gridCord.x].roomType == RoomType.EMPTY;
    }

    public NewGridNode GetGridNode(Vector2 position) => grid[(int)position.y, (int)position.x];

    public NewGridNode GetGridNode(int x, int y) => grid[y, x];

    //Instead of spawning the map by scanning through the concept grid, I can instead
    //use the paths created to determine the spots that will have rooms spawned
    //I can then use checks to ensure that rooms that are located on junctions
    //have the appropriate line up of rooms and that everything remains consistent.
    //this will probably provide me with the simplest method of spawning the map.
    //Then I just scan like normal to fill up all of the filled rooms.
    private void GeneratePath()
    {
        Paths = new List<List<NewGridNode>>
        {
            AStar(grid[(int) spawnRoomPos.y, 0], grid[(int) bossRoomPos.y, (int) bossRoomPos.x]),
            AStar(grid[(int) spawnRoomPos.y, 0], grid[(int) shopRoomPos.y, (int) shopRoomPos.x])
        };

        foreach (List<NewGridNode> path in Paths)
        {
            for (int index = 1; index < path.Count; index++)
            {
                if (path[index].roomType == RoomType.EMPTY)
                    path[index].roomType = RoomType.Normal;

                switch (path[index - 1].roomType)
                {
                    case RoomType.Spawn:
                        //perform the actions for the spawn room.
                        DetermineNumOpenings(path[index], path[index - 1], true);
                        break;
                    default:
                        //we need to determine what direction the last room was in relation to the current room
                        //the current room being the current index.
                        DetermineNumOpenings(path[index], path[index - 1]);
                        break;
                }
            }
        }
        
        //Determines the direction between the two grid nodes and assigns a value to the openings
        //on each side appropriately
        void DetermineNumOpenings(NewGridNode currNode, NewGridNode prevNode,
            bool onlyOneOpening = false)
        {
            int numOpenings = 1;

            //if (!onlyOneOpening)
            //    numOpenings = Random.Range(1, 4);

            if (currNode.gridPos.y > prevNode.gridPos.y)
            {
                //curr node above prev node
                prevNode.openings.TopSide = numOpenings;
                currNode.openings.BottomSide = numOpenings;

            }
            else if (currNode.gridPos.y < prevNode.gridPos.y)
            {
                //curr node below prev node
                prevNode.openings.BottomSide = numOpenings;
                currNode.openings.TopSide = numOpenings;
            }
            else if (currNode.gridPos.x > prevNode.gridPos.x)
            {
                //curr node to right of prev node
                prevNode.openings.RightSide = numOpenings;
                currNode.openings.LeftSide = numOpenings;
            }
            else
            {
                //curr node to left of prev node
                prevNode.openings.LeftSide = numOpenings;
                currNode.openings.RightSide = numOpenings;
            }
        }
    }
    
    #region Grid concept Astar

    public List<NewGridNode> AStar(NewGridNode start, NewGridNode end)
    {
        List<NewGridNode> frontier = new List<NewGridNode>(0);

        HashSet<NewGridNode> explored = new HashSet<NewGridNode>();

        frontier.Add(start);

        foreach (NewGridNode node in grid)
        {
            if (node != null) node.gCost = 0;
        }

        while (frontier.Count > 0)
        {
            NewGridNode currNode = frontier[0];

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

            foreach (NewGridNode neighbor in Neighbors(currNode))
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
    private List<NewGridNode> RetracePath(NewGridNode startNode, NewGridNode endNode)
    {
        List<NewGridNode> path = new List<NewGridNode>();

        NewGridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        if (currentNode == startNode)
            path.Add(currentNode);

        path.Reverse();

        return path;
    }

    /// <summary> Returns the distance cost between two nodes. </summary>
    /// <param name="nodeA">The node we are looking at.</param>
    /// <param name="nodeB">The goal node that we want the distance too.</param>
    /// <returns>The distance cost between <paramref name="nodeA"/> and <paramref name="nodeB"/>.</returns>
    private int GetDistCost(NewGridNode nodeA, NewGridNode nodeB)
    {
        int distZ = (int) Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);
        int distX = (int) Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);

        return distX > distZ ? 10 * distZ + 10 * (distX - distZ) : 10 * distX + 10 * (distZ - distX);
    }

    /// <summary> Returns a list of the node surrounding the node we are looking at. </summary>
    /// <param name="node">The node whose neighbors we want to get.</param>
    /// <returns>A list of nodes that neighbor the <paramref name="node"/></returns>
    private List<NewGridNode> Neighbors(NewGridNode node)
    {
        List<NewGridNode> neighbors = new List<NewGridNode>();

        int gridPosX = (int) node.gridPos.x;
        int gridPosY = (int) node.gridPos.y;

        //Checking left
        if (gridPosX > 1 && grid[gridPosY, gridPosX - 1].roomType != RoomType.Filled)
            neighbors.Add(grid[gridPosY, gridPosX - 1]);

        //Checking right
        if (gridPosX < columns - 1 && grid[gridPosY, gridPosX + 1].roomType != RoomType.Filled)
            neighbors.Add(grid[gridPosY, gridPosX + 1]);

        //This is used for when we are looking at the neighbors of the spawn room to avoid
        //looking above or below it as we can never go that way
        if (gridPosX != 0)
        {
            //Checking up
            if (gridPosY < rows - 1 && grid[gridPosY + 1, gridPosX].roomType != RoomType.Filled)
                neighbors.Add(grid[gridPosY + 1, gridPosX]);

            //Checking down
            if (gridPosY > 0 && grid[gridPosY - 1, gridPosX].roomType != RoomType.Filled)
                neighbors.Add(grid[gridPosY - 1, gridPosX]);
        }

        return neighbors;
    }

    #endregion
    
    #region Helper Functions
    
    /// <summary>
    /// Checks to see if the positions we currently have will meet out criteria for room placement.
    /// </summary>
    /// <param name="SpawnRoomPos">The grid coordinates for the spawn room.</param>
    /// <param name="bossRoomPos">The grid coordinates for the boss room.</param>
    /// <returns></returns>
    public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 bossRoomPos) =>
        Vector3.Distance(SpawnRoomPos, bossRoomPos) / 31 > gridInfo.minDistSTB;
    #endregion
}