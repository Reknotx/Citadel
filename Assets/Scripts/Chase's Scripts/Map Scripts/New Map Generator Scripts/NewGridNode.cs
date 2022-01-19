using UnityEngine;


namespace Map
{
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

        public Vector2 gridPos;
        public Openings openings;
        public RoomType roomType = RoomType.EMPTY;

        public int gCost, hCost;

        public int fCost => gCost + hCost;

        /// <summary>The parent to this node. </summary>
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
}