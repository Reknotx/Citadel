/* Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 */

using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

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
    public class NewMapGenerator : MonoBehaviour
    {
        public static NewMapGenerator Instance;

        /// <summary>
        /// Grid size is set as follows.
        /// Max Y value will be set to gridSize.
        /// Max X value will be set to gridSize + 1.
        /// 
        /// Grid pos set as follows: grid[y, x]
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

        /// <summary>
        /// 
        /// </summary>
        private Vector2 SpawnRoomPos, SpawnRoomGridPos;

        private Vector2 BossRoomPos, BossRoomGridPos;
        private Vector2 trueGridSize;

        public NewGridInfo gridInfo = new NewGridInfo();

        [HideInInspector] public static List<Room> specialRooms;

        [Header("Enable this if we want to only have one row on the map.")]
        public bool OneRowOnly = false;


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

            map = new Room[(int) trueGridSize.y, (int) trueGridSize.x];
            conceptGrid = new ConceptGrid(trueGridSize, gridInfo);
            columns = (int) trueGridSize.x;
            rows = (int) trueGridSize.y;

            SpawnMap();
        }

        /// <summary> Function which spawns in all of the rooms on the map. </summary>
        private void SpawnMap()
        {
            //NOTES:
            //1. Now we want to use the paths that were generated in the concept grid
            //to help us determine the rooms we want to spawn.
            //This will help maintain a lot of the current code structure that we have in place.

            RoomInfo tempRoomInfo;

            #region Spawning the spawn room

            //int SRY = Random.Range(0, (int) trueGridSize.y);

            GameObject spawnRoom = SpawnRoom(roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)],
                new Vector2(0, 0), "Spawn Room");
            AddSpecialRoomToList(spawnRoom);

            tempRoomInfo = spawnRoom.GetComponent<Room>().roomInfo;
            spawnRoom.GetComponent<Room>().fogEnabledOnStart = false;

            // conceptGrid[(int) SpawnRoomGridPos.y, 0] = new ConceptGrid.NewGridNode(SpawnRoomGridPos,
            //     ConceptGrid.NewGridNode.RoomType.Spawn,
            //     tempRoomInfo.CalcDoorsLeftSide(),
            //     tempRoomInfo.CalcDoorsRightSide(),
            //     tempRoomInfo.CalcDoorsTopSide(),
            //     tempRoomInfo.CalcDoorsBottomSide());

            #endregion

            #region Spawning Boss Room

            // Vector3 BRPos;
            // do
            // {
            //     int BRGX = Random.Range(1, (int) trueGridSize.x);
            //     int BRGY = Random.Range(0, (int) trueGridSize.y);
            //
            //     BRPos = new Vector3(BRGX * roomSize, BRGY * roomSize);
            // } while (!gridInfo.CheckSpawnBossDist(SpawnRoomPos, BRPos));

            //BossRoomGridPos = new Vector2(BRPos.x / roomSize, BRPos.y / roomSize);

            GameObject bossRoom = SpawnRoom(roomCont.BossRooms[Random.Range(0, roomCont.BossRooms.Count)],
                BossRoomGridPos,
                "Boss Room");
            AddSpecialRoomToList(bossRoom);

            BossRoomPos = bossRoom.transform.position;

            // conceptGrid[(int) BossRoomGridPos.y, (int) BossRoomGridPos.x] = new ConceptGrid.NewGridNode(BossRoomGridPos,
            //     ConceptGrid.NewGridNode.RoomType.Boss,
            //     tempRoomInfo.CalcDoorsLeftSide(),
            //     tempRoomInfo.CalcDoorsRightSide(),
            //     tempRoomInfo.CalcDoorsTopSide(),
            //     tempRoomInfo.CalcDoorsBottomSide());

            #endregion
            
            #region Spawning Shops

            //Something else to note here for spawning the shop rooms and the treasure rooms is that we
            //can ensure that there will be forced wrapping by spawning filled rooms around the special
            //rooms, effectively forcing the astar algorithm to path around the rooms, and take a longer
            //path

            //AddSpecialRoomToList(shopRoom);

            #endregion

            // CreatePath(AStar(conceptGrid[(int)SpawnRoomGridPos.y, 0],
            //                  conceptGrid[(int)BossRoomGridPos.y, (int)BossRoomGridPos.x]));


            #region Spawn in rooms that will go on the path




            #endregion



            #region Spawning in filled rooms
            //1. Instead of above function we just loop over the the whole grid and spawn in the appropriate
            // rooms based off of the room type.
            //This might be better to do if I didn't have it set up the way that I did
            //Actually this will make it easier to set up but I need to do some overhaul.
            //Time to work on that
            
            ConceptGrid.NewGridNode[,] tempConceptGrid = conceptGrid.grid;

            //Spawning a row
            for (int y = 0; y < rows; y++)
            {

                GameObject row = new GameObject("Row " + y)
                    { transform = { parent = transform } };

                //Spawning in a column
                for (int x = 0; x < columns; x++)
                {
                    if (map[y, x] != null) continue;

                    GameObject roomToSpawn = null;
                    ConceptGrid.NewGridNode.RoomType tempRoomType = tempConceptGrid[y, x].roomType;

                    if (tempRoomType == ConceptGrid.NewGridNode.RoomType.Filled)
                    {
                        roomToSpawn = roomCont.filledRoom;
                    }
                    else
                    {
                        
                    }


                    GameObject temp = Instantiate(roomToSpawn,
                    new Vector3(roomSize * x, roomSize * y),
                    Quaternion.identity);

                    temp.GetComponent<Room>().gridPos = new Vector2(x, y);

                    temp.transform.parent = row.transform;

                    map[y, x] = temp.GetComponent<Room>();
                }
            }
            #endregion

            
            
            #region Helper Functions
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
                            if (prevPos == DoorPositions.RightTop && currPos == DoorPositions.LeftTop)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.RightMiddle && currPos == DoorPositions.LeftMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.RightBottom && currPos == DoorPositions.LeftBottom)
                            {
                                lineUp = true;
                            }
                        }
                        else if (prevDir == Dir.Right)
                        {
                            //Curr must have openings on right
                            if (prevPos == DoorPositions.LeftTop && currPos == DoorPositions.RightTop)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.LeftMiddle && currPos == DoorPositions.RightMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.LeftBottom && currPos == DoorPositions.RightBottom)
                            {
                                lineUp = true;
                            }
                        }
                        else if (prevDir == Dir.Below)
                        {
                            //Curr must have openings on bottom
                            if (prevPos == DoorPositions.TopLeft && currPos == DoorPositions.BottomLeft)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.TopMiddle && currPos == DoorPositions.BottomMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.TopRight && currPos == DoorPositions.BottomRight)
                            {
                                lineUp = true;
                            }
                        }
                        else
                        {
                            // Debug.Log("Prev room above current");
                            // Debug.Log(prevPos.ToString());
                            //Curr must have openings on top
                            if (prevPos == DoorPositions.BottomLeft && currPos == DoorPositions.TopLeft)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.BottomMiddle && currPos == DoorPositions.TopMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.BottomRight && currPos == DoorPositions.TopRight)
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
        private void CreatePath(List<ConceptGrid.NewGridNode> path)
        {
            //Future plans will be to change this up to include a varying degree of lists
            //as the parameter rather than just one path as multiple paths will need to be
            //constructed. The algorithm should work just fine with the addition of multiple
            //paths and theoretically shouldn't be troubled by it.

            //This is where we'll do the necessary actions on that critical path.
            //As of right now I want this function to only work on the critical path.
            //It will be edited and updated later in order to work with successful
            //pathways towards the shops, treasure rooms, and to dead ends.
            //
            //Remember to pull the room info scriptable object off of the prefab
            //so that I can appropriately tell which rooms have what I need.
            //For each grid spot I will need to make sure that the prefab matches
            //EXACTLY what it is that is needed.

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

            //Ok this needs to be redone, this isn't nice and just makes clutter.
            for (int pathIndex = 1; pathIndex < path.Count - 1; pathIndex++)
            {
                ConceptGrid.NewGridNode currNode = path[pathIndex];

                List<Room> examinedRooms = new List<Room>();
                do
                {
                    int listIndex = Random.Range(0, roomCont.RegularRooms.Count);

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
                            map[(int) path[pathIndex - 1].gridPos.y, (int) path[pathIndex - 1].gridPos.x].roomInfo,
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
            
            #region Helper Functions
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
                            if (prevPos == DoorPositions.RightTop && currPos == DoorPositions.LeftTop)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.RightMiddle && currPos == DoorPositions.LeftMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.RightBottom && currPos == DoorPositions.LeftBottom)
                            {
                                lineUp = true;
                            }
                        }
                        else if (prevDir == Dir.Right)
                        {
                            //Curr must have openings on right
                            if (prevPos == DoorPositions.LeftTop && currPos == DoorPositions.RightTop)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.LeftMiddle && currPos == DoorPositions.RightMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.LeftBottom && currPos == DoorPositions.RightBottom)
                            {
                                lineUp = true;
                            }
                        }
                        else if (prevDir == Dir.Below)
                        {
                            //Curr must have openings on bottom
                            if (prevPos == DoorPositions.TopLeft && currPos == DoorPositions.BottomLeft)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.TopMiddle && currPos == DoorPositions.BottomMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.TopRight && currPos == DoorPositions.BottomRight)
                            {
                                lineUp = true;
                            }
                        }
                        else
                        {
                            // Debug.Log("Prev room above current");
                            // Debug.Log(prevPos.ToString());
                            //Curr must have openings on top
                            if (prevPos == DoorPositions.BottomLeft && currPos == DoorPositions.TopLeft)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.BottomMiddle && currPos == DoorPositions.TopMiddle)
                            {
                                lineUp = true;
                            }
                            else if (prevPos == DoorPositions.BottomRight && currPos == DoorPositions.TopRight)
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


        #region Local classes

        [System.Serializable]
        public class NewGridInfo
        {
            [Tooltip("Represents the size of the grid")] [Range(7, 15)]
            public int gridSize = 7;

            [Tooltip("Represents the minimum distance from the spawn room to the boss room.")] [Range(4f, 15f)]
            public float minDistSTB;

            public NewGridInfo()
            {
                minDistSTB = 4f;
            }

            public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 BRPos)
            {
                //Debug.Log("Dist of STB = " + Vector3.Distance(SpawnRoomPos, BRPos) / 31);
                return Vector3.Distance(SpawnRoomPos, BRPos) / 31 > minDistSTB;
            }
        }
        #endregion

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

            private readonly Vector2 spawnRoomPos, bossRoomPos, shopRoomPos;

            public List<List<NewGridNode>> paths;

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

                grid[SRY, 0] = new NewGridNode(new Vector2(0, SRY), NewGridNode.RoomType.Spawn);
                    for (int y = 0; y < rows; y++)
                    {
                        if (y == SRY) continue;

                        grid[y, 0].roomType = NewGridNode.RoomType.Filled;
                    }

                spawnRoomPos = new Vector2(0, SRY);

                #endregion

                #region 2. Determine boss room location

                Vector3 BRPos;
                do
                {
                    int BRGX = Random.Range(4, columns);
                    int BRGY = Random.Range(0, rows);

                    BRPos = new Vector3(BRGX, BRGY);
                } while (!gridInfo.CheckSpawnBossDist(new Vector2(0, SRY), BRPos));

                grid[(int) BRPos.y, (int) BRPos.x].roomType = NewGridNode.RoomType.Boss;

                #region 2a. Put filled rooms around the boss room except for the entrance side
                int entranceSide = Random.Range(0, 4);
                
                if (entranceSide != (int) DoorSide.Top && (int)BRPos.y + 1 != columns)
                    grid[(int) BRPos.y + 1, (int) BRPos.x].roomType = NewGridNode.RoomType.Filled; // Top
                
                if (entranceSide != (int) DoorSide.Bottom && (int)BRPos.y != 0)
                    grid[(int) BRPos.y - 1, (int) BRPos.x].roomType = NewGridNode.RoomType.Filled; // Bottom
                
                if (entranceSide != (int) DoorSide.Left)
                    grid[(int) BRPos.y, (int) BRPos.x - 1].roomType = NewGridNode.RoomType.Filled; // Left
                
                if (entranceSide != (int) DoorSide.Right && (int)BRPos.x + 1 != rows)
                    grid[(int) BRPos.y, (int) BRPos.x + 1].roomType = NewGridNode.RoomType.Filled; // Right
                #endregion

                bossRoomPos = BRPos;

                #endregion

                #region 3. Determine shop room location

                //1. Determine a location for the shop room but place it on the left side of the map


                #region 3a. Put filled rooms around the shop room except for the entrance side

                #endregion

                #endregion

                #region 4. Generate paths to these special locations from spawn room

                //   Add their grid node infos into the grid.
                //   The map generator will utilize this information to select the rooms that are desired
                GeneratePath();
                #endregion
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
                paths = new List<List<NewGridNode>>
                {
                    AStar(grid[(int) spawnRoomPos.y, 0], grid[(int) bossRoomPos.y, (int) bossRoomPos.x]),
                    AStar(grid[(int) spawnRoomPos.y, 0], grid[(int) shopRoomPos.y, (int) shopRoomPos.x])
                };

                foreach (List<NewGridNode> path in paths)
                {
                    for (int index = 0; index < path.Count; index++)
                    {
                        if (path[index].roomType == NewGridNode.RoomType.Spawn) continue;

                        if (path[index - 1].roomType == NewGridNode.RoomType.Spawn)
                        {
                            //perform the actions for the spawn room.
                            DetermineNumOpenings(path[index], path[index - 1], true);
                        }
                        else
                        {
                            //we need to determine what direction the last room was in relation to the current room
                            //the current room being the current index.
                            DetermineNumOpenings(path[index], path[index - 1]);
                        }
                    }
                }
                
                //Determines the direction between the two grid nodes and assigns a value to the openings
                //on each side appropriately
                void DetermineNumOpenings(NewGridNode currNode, NewGridNode prevNode,
                    bool onlyOneOpening = false)
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
                if (gridPosX > 1 && grid[gridPosY, gridPosX - 1].roomType != NewGridNode.RoomType.Filled)
                    neighbors.Add(grid[gridPosY, gridPosX - 1]);

                //Checking right
                if (gridPosX < columns - 1 && grid[gridPosY, gridPosX + 1].roomType != NewGridNode.RoomType.Filled)
                    neighbors.Add(grid[gridPosY, gridPosX + 1]);

                //This is used for when we are looking at the neighbors of the spawn room to avoid
                //looking above or below it as we can never go that way
                if (gridPosX != 0)
                {
                    //Checking up
                    if (gridPosY < rows - 1 && grid[gridPosY + 1, gridPosX].roomType != NewGridNode.RoomType.Filled)
                        neighbors.Add(grid[gridPosY + 1, gridPosX]);

                    //Checking down
                    if (gridPosY > 0 && grid[gridPosY - 1, gridPosX].roomType != NewGridNode.RoomType.Filled)
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
                    Treasure,
                    Filled
                }

                public Vector2 gridPos;
                public Openings openings;
                public RoomType roomType = RoomType.EMPTY;

                public int gCost, hCost;

                public int fCost => gCost + hCost;

                public NewGridNode parent;

                /// <summary> Parameterless constructor that just gives a basic initialization of node. </summary>
                public NewGridNode()
                {
                    Initialization();
                }

                /// <summary> Constructor that allows specifying room type if anything other than normal. </summary>
                /// <param name="roomType">The type of the room that is placed on the grid.</param>
                public NewGridNode(RoomType roomType)
                {
                    Initialization();
                    this.roomType = roomType;
                }

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
    }
}