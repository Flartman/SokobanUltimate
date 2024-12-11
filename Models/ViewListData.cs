using System.Collections.Generic;
using static Sokoban.Shared;

namespace Sokoban.Models
{

    public class ViewMapData
    {     
        public GameElement[,] GroundLayer { get; }

        public GameElement[,] CollisionLayer { get; }

        public int ColumnsNumber => GroundLayer.GetLength(0);

        public int RowsNumber => GroundLayer.GetLength(1);

        public int SelectedItemRow { get; }

        public int SelectedItemColumn { get; }

        public ViewMapData(GameElement[,] groundLayer, GameElement[,] collisionLayer, int selectedItemColumn = 0, int selectedItemRow = 0)
        {
            GroundLayer = groundLayer;
            CollisionLayer = collisionLayer;
            SelectedItemColumn = selectedItemColumn;
            SelectedItemRow = selectedItemRow;
        }
    }

    public class ViewListData
    {
        public IReadOnlyList<string> Items { get; }

        public int SelectedItemNumber { get; }

        public ViewListData(IReadOnlyList<string> itemList, int selectedItemNumber)
        {
            Items = itemList;
            SelectedItemNumber = selectedItemNumber;
        }
    }
}
