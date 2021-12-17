/* Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 *
 * Overhauled on 12/1/2021. Overhaul was needed in order to make the generator
 * work with different room types and to make it more flexible overall.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
//Included this statement to help shorten code character count.
using static Map.NewMapGenerator.ConceptGrid.NewGridNode;
using Random = UnityEngine.Random;

namespace Map
{
    /// <summary> The map generator that spawns in the rooms in a 6x6 grid. </summary>
    public class NewMapGenerator : MonoBehaviour
    {
        #region Fields
        #region Public
        public static NewMapGenerator Instance;

        public static List<Room> specialRooms;

        public NewGridInfo gridInfo = new NewGridInfo();

        [Header("Enable this if we want to only have one row on the map.")]
        public bool OneRowOnly;
        #endregion

        #region Private
        /// <summary>
        /// <para>
        /// Grid size is set as follows.
        /// </para>
        /// Max Y value will be set to gridSize.
        /// Max X value will be set to gridSize + 1.
        /// <para>
        /// Grid pos set as follows: grid[y, x]
        /// </para>
        /// </summary>
        private Room[,] map;

        /// <summary>
        /// This array is used to lay out the conceptual framework of the map, and
        /// will be used to aid in choosing what rooms we actually need to spawn.
        /// Here we can also figure out later on what rooms will remain empty.
        /// </summary>
        private ConceptGrid conceptGrid;

        private int columns;

        private int rows;

        private int roomSize = 31;

        public RoomContainer roomCont;

        private Vector2 SpawnRoomPos, SpawnRoomGridPos;

        private Vector2 BossRoomPos, BossRoomGridPos;
        private Vector2 trueGridSize;

        #endregion
        #endregion

        public void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(Instance);

            Instance = this;

            if (specialRooms == null)
                specialRooms = new List<Room>();
            else if (specialRooms.Count > 0 && specialRooms[0] == null)
                specialRooms.Clear();

            if (roomCont == null)
                Debug.LogError("Room Container is empty. Please insert the scriptable object.");

        }

        public void Start()
        {
            trueGridSize = OneRowOnly
                ? new Vector2(gridInfo.gridSize + 1, 1)
                : new Vector2(gridInfo.gridSize + 1, gridInfo.gridSize);
            columns = (int) trueGridSize.x;
            rows = (int) trueGridSize.y;

            SpawnMap();
        }

        /// <summary> Function which spawns in all of the rooms on the map. </summary>
        private void SpawnMap()
        {
            map = new Room[(int) trueGridSize.y, (int) trueGridSize.x];
            conceptGrid = new ConceptGrid(trueGridSize, gridInfo);
            //NOTES:
            //1. Now we want to use the paths that were generated in the concept grid
            //to help us determine the rooms we want to spawn.
            //This will help maintain a lot of the current code structure that we have in place.
            //2. Levels need to be picked based on the number of openings they have on each side
            //by just comparing the room openings we can preplan certain rooms so we don't
            //have to delete and respawn prefabs.

            foreach (List<ConceptGrid.NewGridNode> path in conceptGrid.Paths)
            {
                CreatePath(path);
            }

            #region Spawning in filled rooms
            ConceptGrid.NewGridNode[,] tempConceptGrid = conceptGrid.grid;

            //Spawning a row
            for (int y = 0; y < rows; y++)
            {

                GameObject row = new GameObject("Row " + y)
                    { transform = { parent = transform } };

                //Spawning in a column
                for (int x = 0; x < columns; x++)
                {
                    if (map[y, x] != null && tempConceptGrid[y,x].roomType != (RoomType.Filled | RoomType.EMPTY)) continue;

                    GameObject tempFilledRoom = SpawnRoom(roomCont.filledRoom, new Vector2(x, y), "Filled Room");
                    
                    tempFilledRoom.transform.parent = row.transform;
                }
            }
            #endregion 
           
            #region Create Path Function
            // <summary>
            // Spawns the necessary rooms along a path by following the layout of the
            // conceptual grid.
            // </summary>
            // <param name="path">The list of nodes along a path on the grid.</param>
            void CreatePath(List<ConceptGrid.NewGridNode> path)
            {
                //Logic notes for spawning.
                //1. Each room has a certain number of openings each side
                //2. Each door is located in one of three positions on each side.
                //3. Rooms must match their neighbors in terms of the positioning of their
                //doorways that lead to the neighboring rooms.

                GameObject prevRoom = null;

                //Ok this needs to be redone, this isn't nice and just makes clutter.
                for (int pathIndex = 0; pathIndex < path.Count - 1; pathIndex++)
                {
                    ConceptGrid.NewGridNode currNode = path[pathIndex];

                    List<GameObject> examinedRooms = new List<GameObject>();
                    do
                    {
                        if (map[(int) currNode.gridPos.y, (int) currNode.gridPos.x] != null) break;
                        
                        #region Local Scope Variables
                        GameObject roomObj = GetRoom(currNode.roomType, examinedRooms);
                        
                        if (examinedRooms.Contains(roomObj.GetComponent<GameObject>()))
                            continue;

                        Room room = roomObj.GetComponent<Room>();
                        RoomInfo tempInfo = room.roomInfo;
                        string roomName = GetRoomName(currNode.roomType);
                        #endregion

                        examinedRooms.Add(roomObj);
                        
                        if (currNode.roomType == RoomType.Spawn)
                        {
                            SpawnRoom(roomObj, currNode.gridPos, roomName);
                            room.fogEnabledOnStart = false;
                            prevRoom = roomObj;
                            break;
                        }
                        else if (CompareNodeToRoom(currNode, tempInfo)
                            && EnsureEntrancesLineUp(tempInfo,
                                map[(int) path[pathIndex - 1].gridPos.y, (int) path[pathIndex - 1].gridPos.x].roomInfo,
                                GetPrevRoomDir(currNode.gridPos, path[pathIndex - 1].gridPos))
                        )
                        {
                            // Spawn the room
                            SpawnRoom(roomObj, currNode.gridPos, roomName);
                            prevRoom = roomObj;
                            break;
                        }

                        if (examinedRooms.Count == GetListCount(currNode.roomType))
                        {
                            Debug.LogError("Ran out of rooms of type " + currNode.roomType +
                                           ", about to enter infinite loop. Breaking from loop");
                            Debug.Log("The last room we looked at was " + prevRoom.name);

                            #if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
                            #else
                            Application.Quit();
                            #endif
                        }
        
                    } while (true);

                    examinedRooms.Clear();
                }
                
                #region Helper Functions

                GameObject GetRoom(ConceptGrid.NewGridNode.RoomType type, List<GameObject> examinedRooms)
                {
                    GameObject room = null;

                    // while (true)
                    // {
                        switch (type)
                        {
                            case RoomType.Normal:
                                room = roomCont.RegularRooms[Random.Range(0, roomCont.RegularRooms.Count)];
                                break;
                            case RoomType.Spawn:
                                room = roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)];
                                break;
                            case RoomType.Boss:
                                room = roomCont.SpawnRooms[Random.Range(0, roomCont.BossRooms.Count)];
                                break;
                            case RoomType.Shop:
                                room = roomCont.ShopRooms[Random.Range(0, roomCont.ShopRooms.Count)];
                                break;
                        }
                    //     
                    //     if (!examinedRooms.Contains(room)) break;
                    // }

                    Debug.Log("Room found");

                    return room;
                }

                int GetListCount(ConceptGrid.NewGridNode.RoomType type)
                {
                    switch (type)
                    {
                        case RoomType.Spawn:
                            return roomCont.SpawnRooms.Count;
                        case RoomType.Boss:
                            return roomCont.BossRooms.Count;
                        case RoomType.Shop:
                            return roomCont.BossRooms.Count;
                        default:
                            return roomCont.RegularRooms.Count;
                    }
                }

                string GetRoomName(ConceptGrid.NewGridNode.RoomType type)
                {
                    switch (type)
                    {
                        case RoomType.Spawn:
                            return "Spawn Room";
                        case RoomType.Boss:
                            return "Boss Room";
                        case RoomType.Shop:
                            return "Shop Room";
                        case RoomType.Filled:
                            return "Filled Room";
                        default:
                            return "Room";
                    }
                }
                
                bool CompareNodeToRoom(ConceptGrid.NewGridNode node, RoomInfo roomInfo)
                {
                    if (node == null) Debug.LogError("Help node null");
                    //node.openings.ToString();
                    //roomInfo.ToString();
                    return node.openings.TotalOpenings() == roomInfo.numOpenings &&
                           node.openings.RightSide == roomInfo.CalcDoorsRightSide() &&
                           node.openings.LeftSide == roomInfo.CalcDoorsLeftSide() &&
                           node.openings.TopSide == roomInfo.CalcDoorsTopSide() &&
                           node.openings.BottomSide == roomInfo.CalcDoorsBottomSide();
                }

                bool EnsureEntrancesLineUp(RoomInfo currRoom, RoomInfo prevRoom, Dir prevDir)
                {
                    bool lineUp = false;

                    if (currRoom == null) Debug.LogError("CurrRoomInfo is null");

                    if (prevRoom == null) Debug.LogError("PrevRoomInfo is null");

                    foreach (DoorPositions currPos in currRoom.openDoors)
                    {
                        foreach (DoorPositions prevPos in prevRoom.openDoors)
                        {
                            if (prevDir == Dir.Left)
                            {
                                //Curr must have openings on left
                                if ((prevPos == DoorPositions.RightTop && currPos == DoorPositions.LeftTop) ||
                                    (prevPos == DoorPositions.RightMiddle && currPos == DoorPositions.LeftMiddle) ||
                                    (prevPos == DoorPositions.RightBottom && currPos == DoorPositions.LeftBottom))
                                {
                                    lineUp = true;
                                }
                            }
                            else if (prevDir == Dir.Right)
                            {
                                //Curr must have openings on right
                                if ((prevPos == DoorPositions.LeftTop && currPos == DoorPositions.RightTop) ||
                                    (prevPos == DoorPositions.LeftMiddle && currPos == DoorPositions.RightMiddle) ||
                                    (prevPos == DoorPositions.LeftBottom && currPos == DoorPositions.RightBottom))
                                {
                                    lineUp = true;
                                }
                            }
                            else if (prevDir == Dir.Below)
                            {
                                //Curr must have openings on bottom
                                if ((prevPos == DoorPositions.TopLeft && currPos == DoorPositions.BottomLeft) ||
                                    (prevPos == DoorPositions.TopMiddle && currPos == DoorPositions.BottomMiddle) ||
                                    (prevPos == DoorPositions.TopRight && currPos == DoorPositions.BottomRight))
                                {
                                    lineUp = true;
                                }
                            }
                            else
                            {
                                // Debug.Log("Prev room above current");
                                // Debug.Log(prevPos.ToString());
                                //Curr must have openings on top
                                if ((prevPos == DoorPositions.BottomLeft && currPos == DoorPositions.TopLeft) ||
                                    (prevPos == DoorPositions.BottomMiddle && currPos == DoorPositions.TopMiddle) ||
                                    (prevPos == DoorPositions.BottomRight && currPos == DoorPositions.TopRight))
                                {
                                    lineUp = true;
                                }
                            }

                            if (lineUp) break;
                        }
                    }

                    return lineUp;
                }

                Dir GetPrevRoomDir(Vector2 currRoomPos, Vector2 prevRoomPos)
                {
                    if (currRoomPos.x > prevRoomPos.x)
                        //Curr room to right of prev room so the prev room dir is left
                        return Dir.Left;

                    if (currRoomPos.x < prevRoomPos.x)
                        return Dir.Right;
                    
                    return currRoomPos.y > prevRoomPos.y ? Dir.Below : Dir.Top;
                }
                #endregion

            }
            #endregion
        }

        /// <summary> 
        /// This function will spawn in the room, assign it to the grid, 
        /// and assign the rest of the values. 
        /// </summary>
        /// <param name="room">The room prefab that will be spawned in on the map.</param>
        /// <param name="gridCord">The grid coordinate for the <paramref name="room"/>.</param>
        /// <param name="name">The name of the room that will be spawned. The rooms grid coords are also
        /// included in the name automatically.</param>
        private GameObject SpawnRoom(GameObject room, Vector2 gridCord, string name = "")
        {
            if (map[(int) gridCord.y, (int) gridCord.x] != null) return null;

            GameObject spawnedRoom = Instantiate(room, new Vector3(gridCord.x * roomSize, gridCord.y * roomSize),
                Quaternion.identity);

            spawnedRoom.GetComponent<Room>().gridPos = gridCord;

            map[(int) gridCord.y, (int) gridCord.x] = spawnedRoom.GetComponent<Room>();

            if (!name.Equals(""))
            {
                spawnedRoom.name = name + ": " + gridCord;
                switch (name)
                {
                    case "Spawn Room":
                        SpawnRoomGridPos = gridCord;
                        SpawnRoomPos = spawnedRoom.transform.position;
                        break;
                    case "Boss Room":
                        BossRoomGridPos = gridCord;
                        BossRoomPos = spawnedRoom.transform.position;
                        break;
                }
            }

            return spawnedRoom;
        }


        private void AddSpecialRoomToList(GameObject roomObj) => specialRooms.Add(roomObj.GetComponent<Room>());

        public void ExposeSpecialRooms()
        {
            foreach (Room room in specialRooms) room.TurnOffFog();
        }

        /// <summary> A small directional enum for aiding the algorithm. </summary>
        enum Dir
        {
            Top,
            Below,
            Left,
            Right
        }

        #region Local classes
        
        [System.Serializable]
        public class NewGridInfo
        {
            [Tooltip("Represents the size of the grid"), Range(7, 15)] 
            public int gridSize = 7;

            [Tooltip("Represents the minimum distance from the spawn room to the boss room."), Range(4f, 15f)] 
            public float minDistSTB;

            public NewGridInfo() => minDistSTB = 4f;

            public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 bossRoomPos) =>
                Vector3.Distance(SpawnRoomPos, bossRoomPos) / 31 > minDistSTB;
        }

        #region New Concept Grid Class

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
            
            private NewGridInfo gridInfo;

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

                } while (!gridInfo.CheckSpawnBossDist(spawnRoomPos, bossRoomPos) && !GridSpotEmpty(bossRoomPos));

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
            
            #region Grid Node Class

            /// <summary>
            /// A class that makes a node for the grid that will be used to help in conceptualizing
            /// the map as it is being generated.
            /// </summary>
            public class NewGridNode
            {
                public struct Openings
                {
                    [Range(0, 3)] public int LeftSide;

                    [Range(0, 3)] public int RightSide;

                    [Range(0, 3)] public int TopSide;

                    [Range(0, 3)] public int BottomSide;

                    public void AssignOpeningValues(int left = 0, int right = 0, int top = 0, int bottom = 0)
                    {
                        LeftSide = left;
                        RightSide = right;
                        TopSide = top;
                        BottomSide = bottom;
                    }
                    
                    public override string ToString()
                    {
                        Debug.LogFormat("Node openings - Top: {0}, Bottom: {1}, Left: {2}, Right: {3}", TopSide,
                            BottomSide, LeftSide, RightSide);
                        return "";
                    }

                    public int TotalOpenings()
                    {
                        return LeftSide + RightSide + TopSide + BottomSide;
                    }
                }

                public enum RoomType
                {
                    EMPTY,
                    Normal,
                    Spawn,
                    Boss,
                    Shop,
                    Filled
                }

                public Vector2 gridPos;
                public Openings openings;
                public RoomType roomType = RoomType.EMPTY;

                public int gCost, hCost;

                public int fCost => gCost + hCost;

                public NewGridNode parent;

                public NewGridNode(Vector2 gridPos)
                {
                    Initialization();
                    this.gridPos = gridPos;
                }

                /// <summary> Construct that allows specifying room type as well as openings on the sides if known. </summary>
                /// <param name="roomType">The type of room that is placed on the grid.</param>
                /// <param name="leftSide">Number of openings on the left side.</param>
                /// <param name="rightSide">Number of openings on the right side.</param>
                /// <param name="topSide">Number of openings on the top side.</param>
                /// <param name="bottomSide">Number of openings on the bottom side.</param>
                public NewGridNode(Vector2 gridPos, RoomType roomType, int leftSide = 0, int rightSide = 0,
                    int topSide = 0, int bottomSide = 0)
                {
                    Initialization();
                    this.gridPos = gridPos;
                    this.roomType = roomType;
                    openings.AssignOpeningValues(leftSide, rightSide, topSide, bottomSide);

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
        }
        #endregion
        #endregion
    }
}