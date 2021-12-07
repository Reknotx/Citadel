/* Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;

//Notes:
//Chase, please note that for the grid node array, you are just conceptualizing
//the rooms and don't need too much information.
//I can randomly select the number of doors on each side that are needed to refer to
//when I actually spawn the rooms.
//Once I start actually spawning rooms use the conceptual grid to find rooms that have
//the stated number of openings on a particular side that I determined earlier to choose
//which rooms to actually spawn. Then once I start spawning rooms I'll use their room info to
//figure out what rooms I need to spawn afterwards that will fit that description
//


namespace Map
{

    /// <summary> The map generator that spawns in the rooms in a 6x6 grid. </summary>
    public class MapGenerator : MonoBehaviour
    {
        public static MapGenerator Instance;

        /// <summary>
        /// Grid size is set as follows.
        /// Max Y value will be set to gridSize.
        /// Max X value will be set to gridSize + 1.
        /// 
        /// Grid pos set as follows: grid[y, x]
        /// </summary>
        private Room[,] grid;

        /// <summary>
        /// This array is used to lay out the conceptual framework of the map, and
        /// will be used to aid in choosing what rooms we actually need to spawn.
        /// Here we can also figure out later on what rooms will remain empty.
        /// </summary>
        private GridNode[,] conceptGrid;

        private int roomSize = 31;

        private int columns;

        private int rows;

        public RoomContainer roomCont;

        private Vector2 SpawnRoomPos, SpawnRoomGridPos;

        private Vector2 BossRoomGridPos;
        private Vector2 trueGridSize;

        public GridInfo gridInfo = new GridInfo();

        private RoomInfo bossRoomInfo;
        private GameObject bossRoom;

        public static List<Room> specialRooms;

        [Header("Enable this if we want to only have one row on the map.")]
        public bool OneRowOnly;


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

            grid = new Room[(int) trueGridSize.y, (int) trueGridSize.x];
            conceptGrid = new GridNode[(int) trueGridSize.y, (int) trueGridSize.x];
            columns = (int) trueGridSize.x;
            rows = (int) trueGridSize.y;


            //Initializing the concept grid.
            Debug.Log("Initializing the concept grid.");
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    conceptGrid[y, x] = new GridNode(new Vector2(x, y));
                }
            }

            SpawnMap();
        }

        /// <summary> Function which spawns in all of the rooms on the map. </summary>
        private void SpawnMap()
        {
            //NOTES:
            // 1. The spawn room will be in column zero so that it can remain in the
            // array and to ensure that my algorithm for the path making will be consistent
            // and will work better.
            //     a. What this means is that nothing will be populating the grid on any row
            //     at column position zero except for the spawn room.
            //     

            RoomInfo tempRoomInfo;

            #region Spawning the spawn room

            int SRY = Random.Range(0, rows);

            GameObject spawnRoom = SpawnRoom(roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)],
                new Vector2(0, SRY), "Spawn Room");
            for (int y = 0; y < rows; y++)
            {
                if (y == SRY) continue;

                conceptGrid[y, 0].roomType = GridNode.RoomType.Filled;
            }
            AddSpecialRoomToList(spawnRoom);

            tempRoomInfo = spawnRoom.GetComponent<Room>().roomInfo;
            spawnRoom.GetComponent<Room>().fogEnabledOnStart = false;

            conceptGrid[(int) SpawnRoomGridPos.y, 0] = new GridNode(SpawnRoomGridPos,
                GridNode.RoomType.Spawn,
                tempRoomInfo.CalcDoorsLeftSide(),
                tempRoomInfo.CalcDoorsRightSide(),
                tempRoomInfo.CalcDoorsTopSide(),
                tempRoomInfo.CalcDoorsBottomSide());

            #endregion

            #region Spawning Boss Room

            Vector3 BRPos;
            do
            {
                int BRGX = Random.Range(4, columns - 1);
                int BRGY = Random.Range(1, rows - 1);

                BRPos = new Vector3(BRGX * roomSize, BRGY * roomSize);
                
                BossRoomGridPos = new Vector2(BRGX, BRGY);
            } while (!gridInfo.CheckSpawnBossDist(SpawnRoomPos, BRPos));
            
            bossRoom = roomCont.BossRooms[Random.Range(0, roomCont.BossRooms.Count)];
            bossRoomInfo = bossRoom.GetComponent<Room>().roomInfo;

            conceptGrid[(int) BossRoomGridPos.y, (int) BossRoomGridPos.x] = new GridNode(BossRoomGridPos,
                GridNode.RoomType.Boss,
                bossRoomInfo.CalcDoorsLeftSide(),
                bossRoomInfo.CalcDoorsRightSide(),
                bossRoomInfo.CalcDoorsTopSide(),
                bossRoomInfo.CalcDoorsBottomSide());
            #region 2a. Put filled rooms around the boss room except for the entrance side


            if (bossRoomInfo.CalcDoorsTopSide() == 0)
                conceptGrid[(int) BossRoomGridPos.y + 1, (int) BossRoomGridPos.x].roomType = GridNode.RoomType.Filled; // Top
            if (bossRoomInfo.CalcDoorsBottomSide() == 0)
                conceptGrid[(int) BossRoomGridPos.y - 1, (int) BossRoomGridPos.x].roomType = GridNode.RoomType.Filled; // Bottom
            if (bossRoomInfo.CalcDoorsLeftSide() == 0)
                conceptGrid[(int) BossRoomGridPos.y, (int) BossRoomGridPos.x - 1].roomType = GridNode.RoomType.Filled; // Left
            if (bossRoomInfo.CalcDoorsRightSide() == 0)
                conceptGrid[(int) BossRoomGridPos.y, (int) BossRoomGridPos.x + 1].roomType = GridNode.RoomType.Filled; // Right
            #endregion

            // bossRoom = SpawnRoom(bossRoom,
            //                      BossRoomGridPos,
            //                      "Boss Room");
            // AddSpecialRoomToList(bossRoom);
            
            #endregion

            //To remind myself as to what a room needs:
            //1. A room on the path will have at least two connections.
            //2. Rooms can have a max of four.
            //3. Dead end rooms will obviously have one connection.
            //
            //Time to adjust the Astar algorithm to utilize my internal class rather than the room class.

            //Spawning steps
            //1. Create the spawn and boss rooms and assign them in the grid.
            //Force filled rooms around the boss room to avoid having neighbors to said room
            //2. Create the treasure and shop rooms and surround them with filled rooms except on the
            //side with the entrance
            //3. Create the path from the spawn room to all special rooms using the astar algorithm

            //The locations of all of the special rooms can actually be assigned at random
            //when the conceptual grid is created as we only need to do it once.

            #region Spawning Shops

            //Something else to note here for spawning the shop rooms and the treasure rooms is that we
            //can ensure that there will be forced wrapping by spawning filled rooms around the special
            //rooms, effectively forcing the astar algorithm to path around the rooms, and take a longer
            //path

            //AddSpecialRoomToList(shopRoom);

            #endregion

            CreatePath(AStar(conceptGrid[(int) SpawnRoomGridPos.y, 0],
                conceptGrid[(int) BossRoomGridPos.y, (int) BossRoomGridPos.x]));


            #region Spawn in rooms that will go on the path




            #endregion



            #region Spawning in filled rooms

            //Spawning a row
            for (int y = 0; y < rows; y++)
            {

                GameObject row = new GameObject("Row " + y);
                row.transform.parent = transform;

                //Spawning in a column
                for (int x = 0; x < columns; x++)
                {
                    if (grid[y, x] != null) continue;

                    GameObject temp = Instantiate(roomCont.filledRoom,
                        new Vector3(roomSize * x, roomSize * y),
                        Quaternion.identity);

                    temp.GetComponent<Room>().gridPos = new Vector2(x, y);

                    temp.transform.parent = row.transform;

                    grid[y, x] = temp.GetComponent<Room>();
                }
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
            if (grid[(int) gridCord.y, (int) gridCord.x] != null) return null;

            GameObject spawnedRoom = Instantiate(room, new Vector3(gridCord.x * roomSize, gridCord.y * roomSize),
                Quaternion.identity);

            spawnedRoom.GetComponent<Room>().gridPos = gridCord;

            grid[(int) gridCord.y, (int) gridCord.x] = spawnedRoom.GetComponent<Room>();

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
                        break;
                }
            }

            return spawnedRoom;
        }


        private void AddSpecialRoomToList(GameObject roomObj)
        {
            specialRooms.Add(roomObj.GetComponent<Room>());
        }

        public void ExposeSpecialRooms()
        {
            foreach (Room room in specialRooms)
            {
                room.TurnOffFog();
            }
        }

        /// <summary> A small directional enum for aiding the algorithm. </summary>
        enum Dir
        {
            Top,
            Below,
            Left,
            Right
        }

        /// <summary>
        /// Spawns the necessary rooms along a path by following the layout of the
        /// conceptual grid.
        /// </summary>
        /// <param name="path">The list of nodes along a path on the grid.</param>
        private void CreatePath(List<GridNode> path)
        {
            //Future plans will be to change this up to include a varrying degree of lists
            //as the parameter rather than just one path as multiple paths will need to be
            //constructed. The algorithm should work just fine with the addition of multiple
            //paths and theoretically shouldn't be troubled by it.

            //This is where we'll do the necessary actions on that critical path.
            //As of right now I want this function to only work on the critical path.
            //It will be edited and updated later in order to work with successful
            //pathways towards the shops, tresure rooms, and to dead ends.
            //
            //Remember to pull the room info scriptable object off of the prefab
            //so that I can appropriately tell which rooms have what I need.
            //For each grid spot I will need to make sure that the prefab matches
            //EXACTLY what it is that is needed.

            if (path == null)
            {
                Debug.LogError("Huh");
            }

            // foreach (GridNode node in path)
            // {
            //     
            // }
            for (int index = 0; index < path.Count; index++)
            {
                if (path[index].roomType == GridNode.RoomType.Spawn) continue;

                if (path[index - 1].roomType == GridNode.RoomType.Spawn)
                {
                    //perform the actions for the spawn room.
                    DetermineDir(path[index], path[index - 1], true);
                }
                else
                {
                    //we need to determine what direction the last room was in relation to the current room
                    //the current room being the current index.
                    DetermineDir(path[index], path[index - 1]);
                }
            }

            //Now spawn in the rooms
            //Logic notes for spawning.
            //1. Each room has a certain number of openings each side
            //2. Each door is located in one of three positions on each side.
            //3. Rooms must match their neighbors in terms of the positioning of their
            //doorways that lead to the neighboring rooms.
            //
            //For the time being rooms will only have a max of one entrance on each side.
            //In the future there will be rooms that have multiple entrances on one side,
            //and will need to be updated appropriately.
            //

            GameObject prevRoom = null;
            GridNode currNode = null;
            
            Debug.Log(path[path.Count - 1].gridPos);

            //Ok this needs to be redone, this isn't nice and just makes clutter.
            //Has an issue with the LAST node in the path. I will just make a small easy fix
            //for the boss room
            //Currently this only spawns regular rooms
            for (int pathIndex = 1; pathIndex < path.Count - 1; pathIndex++)
            {
                currNode = path[pathIndex];

                List<Room> examinedRooms = new List<Room>();
                do
                {
                    int listIndex = Random.Range(0, roomCont.RegularRooms.Count);

                    if (roomCont.RegularRooms[listIndex] == null) continue;

                    //Apply change here to make it use all the lists
                    if (examinedRooms.Contains(roomCont.RegularRooms[listIndex].GetComponent<Room>()))
                    {
                        continue;
                    }

                    GameObject roomObj = roomCont.RegularRooms[listIndex];
                    Room room = roomObj.GetComponent<Room>();
                    RoomInfo tempInfo = room.roomInfo;
                    examinedRooms.Add(room);

                    if (CompareNodeToRoom(currNode, tempInfo)
                        && EnsureEntrancesLineUp(tempInfo,
                            grid[(int) path[pathIndex - 1].gridPos.y, (int) path[pathIndex - 1].gridPos.x].roomInfo,
                            GetPrevRoomDir(currNode.gridPos, path[pathIndex - 1].gridPos))
                    )
                    {
                        // Spawn the room
                        SpawnRoom(roomObj, new Vector3(currNode.gridPos.x, currNode.gridPos.y), "Room");
                        prevRoom = roomObj;
                        break;
                    }

                    if (examinedRooms.Count == roomCont.RegularRooms.Count)
                    {
                        Debug.LogError("Ran out of rooms, about to enter infinite loop. Breaking from loop");
                        Debug.Log("The last room we looked at was " + prevRoom.name);

#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }

                } while (examinedRooms.Count != roomCont.RegularRooms.Count);

                examinedRooms.Clear();
            }

            Debug.Log(path[path.Count - 1].roomType);

            if (path[path.Count - 1].roomType == GridNode.RoomType.Boss)
            {
                List<Room> examinedRooms = new List<Room>();
                do
                {
                    int roomListIndex = Random.Range(0, roomCont.BossRooms.Count);
                    int pathIndex = path.Count - 1;
                    currNode = path[pathIndex];
                    
                    if (roomCont.BossRooms[roomListIndex] == null) continue;

                    if (examinedRooms.Contains(roomCont.BossRooms[roomListIndex].GetComponent<Room>()))
                    {
                        continue;
                    }

                    GameObject roomObj = roomCont.BossRooms[roomListIndex];
                    Room room = roomObj.GetComponent<Room>();
                    RoomInfo tempInfo = room.roomInfo;
                    examinedRooms.Add(room);

                    if (CompareNodeToRoom(currNode, tempInfo)
                        && EnsureEntrancesLineUp(tempInfo,
                            grid[(int) path[pathIndex - 1].gridPos.y, (int) path[pathIndex - 1].gridPos.x].roomInfo,
                            GetPrevRoomDir(currNode.gridPos, path[pathIndex - 1].gridPos))
                    )
                    {
                        // Spawn the room
                        SpawnRoom(roomObj, new Vector3(currNode.gridPos.x, currNode.gridPos.y), "Boss Room");
                        Debug.Log("Found a valid room");
                        break;
                    }
                } while (examinedRooms.Count != roomCont.BossRooms.Count);
                
            }

            #region Helper Functions

            void DetermineDir(GridNode currNode, GridNode prevNode, bool onlyOneOpening = false)
            {
                int numOpenings = 1;

                //if (onlyOneOpening == false)
                //{
                //    numOpenings = Random.Range(1, 4);
                //}

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

            bool CompareNodeToRoom(GridNode node, RoomInfo roomInfo)
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

            //I need to figure out the positioning of the previous room in relation
            //to the current room so I can limit the amount of doors I'm actually looking at 
            //to ensure they line up properly
            //
            //I also need to ensure that the room has doors al appropriate places

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
                //Debug.Log(currRoomPos);
                //Debug.Log(prevRoomPos);

                if (currRoomPos.x > prevRoomPos.x)
                    //Curr room to right of prev room so the prev room dir is left
                    return Dir.Left;
                
                if (currRoomPos.x < prevRoomPos.x)
                    return Dir.Right;

                return currRoomPos.y > prevRoomPos.y ? Dir.Below : Dir.Top;
            }


            #endregion

        }

        #region Grid concept Astar

        public List<GridNode> AStar(GridNode start, GridNode end)
        {
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
            int distZ = (int) Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);
            int distX = (int) Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);

            if (distX > distZ) return 10 * distZ + 10 * (distX - distZ);
            else return 10 * distX + 10 * (distZ - distX);

        }

        /// <summary> Returns a list of the node surrounding the node we are looking at. </summary>
        /// <param name="node">The node whose neighbors we want to get.</param>
        /// <returns>A list of nodes that neighbor the <paramref name="node"/></returns>
        private List<GridNode> Neighbors(GridNode node)
        {
            List<GridNode> neighbors = new List<GridNode>();

            int gridPosX = (int) node.gridPos.x;
            int gridPosY = (int) node.gridPos.y;

            //Checking left
            if (gridPosX > 1 && conceptGrid[gridPosY, gridPosX - 1].roomType != GridNode.RoomType.Filled)
                neighbors.Add(conceptGrid[gridPosY, gridPosX - 1]);
            // else Debug.Log("Left filled");

            //Checking right
            if (gridPosX < columns - 1 && conceptGrid[gridPosY, gridPosX + 1].roomType != GridNode.RoomType.Filled)
                neighbors.Add(conceptGrid[gridPosY, gridPosX + 1]);
            // else Debug.Log("Right filled");

                //This is used for when we are looking at the neighbors of the spawn room to avoid
            //looking above or below it as we can never go that way
            if (gridPosX != 0)
            {
                //Checking up
                if (gridPosY < rows - 1 && conceptGrid[gridPosY + 1, gridPosX].roomType != GridNode.RoomType.Filled)
                    neighbors.Add(conceptGrid[gridPosY + 1, gridPosX]);
                // else Debug.Log("Top filled");

                //Checking down
                if (gridPosY > 0 && conceptGrid[gridPosY - 1, gridPosX].roomType != GridNode.RoomType.Filled)
                    neighbors.Add(conceptGrid[gridPosY - 1, gridPosX]);
                // else Debug.Log("Bottom filled");
            }

            return neighbors;
        }

        #endregion


        #region Local classes

        [System.Serializable]
        public class GridInfo
        {
            [Tooltip("Represents the size of the grid")] [Range(1, 15)]
            public int gridSize = 7;

            [Tooltip("Represents the minimum distance from the spawn room to the boss room.")] [Range(1f, 10f)]
            public float minDistSTB;

            public GridInfo()
            {
                minDistSTB = 4f;
            }

            public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 BRPos)
            {
                //Debug.Log("Dist of STB = " + Vector3.Distance(SpawnRoomPos, BRPos) / 31);
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
                [Range(0, 3)] public int LeftSide;

                [Range(0, 3)] public int RightSide;

                [Range(0, 3)] public int TopSide;

                [Range(0, 3)] public int BottomSide;

                public override string ToString()
                {
                    Debug.LogFormat("Node openings - Top: {0}, Bottom: {1}, Left: {2}, Right: {3}", TopSide, BottomSide,
                        LeftSide, RightSide);
                    return "";
                }

                public int TotalOpenings()
                {
                    return LeftSide + RightSide + TopSide + BottomSide;
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

            public int fCost => gCost + hCost;

            public GridNode parent;

            /// <summary> Parameterless constructor that just gives a basic initialization of node. </summary>
            public GridNode()
            {
                Initialization();
            }

            public GridNode(Vector2 gridPos)
            {
                Initialization();
                this.gridPos = gridPos;
            }

            /// <summary> Construct that allows specificying room type as well as openings on the sides if known. </summary>
            /// <param name="roomType">The type of room that is placed on the grid.</param>
            /// <param name="leftSide">Number of openings on the left side.</param>
            /// <param name="rightSide">Number of openings on the right side.</param>
            /// <param name="topSide">Number of openings on the top side.</param>
            /// <param name="bottomSide">Number of openings on the bottom side.</param>
            public GridNode(Vector2 gridPos, RoomType roomType, int leftSide = 0, int rightSide = 0, int topSide = 0,
                int bottomSide = 0)
            {
                Initialization();
                this.gridPos = gridPos;
                this.roomType = roomType;
                openings.LeftSide = leftSide;
                openings.RightSide = rightSide;
                openings.TopSide = topSide;
                openings.BottomSide = bottomSide;

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
}