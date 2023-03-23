using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3
{
    public class GameController : MonoBehaviour
    {
        [System.Serializable]
        public struct cellStruct
        {
            public Cell cell;
            public float x_pos;
            public float y_pos;
        }
        public int Len;
        public cellStruct[,] Matrix;

        public float time;
        private Cell pressedCell;
        private Cell enteredCell;
        public void OnPointerEnter(Cell cell) => enteredCell = cell;
        public void OnPointerDown(Cell cell) => pressedCell = cell;

        private bool IsAdjacent(Cell startCell, Cell endCell) =>
            (startCell.X == endCell.X && Mathf.Abs(startCell.Y - endCell.Y) == 1) ||
            (startCell.Y == endCell.Y && Mathf.Abs(startCell.X - endCell.X) == 1);

        public void OnPointerUp()
        {
            LineAssembled(pressedCell);
            if (IsAdjacent(pressedCell, enteredCell))
            {
                //здесь свап
                SwapCell(ref pressedCell, ref enteredCell);
            }
        }

        public void SwapCell(ref Cell firstCell, ref Cell secondCell)
        {
            float cell1X = firstCell.transform.position.x;
            float cell1Y = firstCell.transform.position.y;

            int X = firstCell.X;
            int Y = firstCell.Y;
            firstCell.X = secondCell.X;
            firstCell.Y = secondCell.Y;
            secondCell.X = X;
            secondCell.Y = Y;

            firstCell.Move(secondCell.transform.position.x, secondCell.transform.position.y, time);
            secondCell.Move(cell1X, cell1Y, time);


            

            LineAssembled(firstCell);
            LineAssembled(secondCell);

            pressedCell = null;
            enteredCell = null;
        }
        
        public void LineAssembled(Cell swapCell)
        {
            List<Vector2> Line = new List<Vector2>();
            int[] XPos;
            int[] YPos;
            if (swapCell.NeighboringSimilarX(out XPos) > 0 || swapCell.NeighboringSimilarY(out YPos) > 0)
            {
                List<int> Pos = new List<int>();
                swapCell.NeighboringSimilarX(out XPos);
                swapCell.NeighboringSimilarY(out YPos);
                
                
                Pos.AddRange(XPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    Matrix[Pos[i], swapCell.Y].cell.NeighboringSimilarX(out XPos, (Pos[i] < swapCell.X) ? 1 : 2);
                    Pos.AddRange(XPos);
                    Debug.Log($"Pos:({Pos[i]},{swapCell.Y}) <-- соседн€€ €чейка | MyPos:({swapCell.X},{swapCell.Y})");

                    Line.Add(new Vector2(Pos[i], swapCell.Y));
                }
                Pos.Clear();

                Pos.AddRange(YPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    Matrix[swapCell.X, Pos[i]].cell.NeighboringSimilarY(out YPos, (Pos[i] < swapCell.Y) ? 1 : 2);
                    Pos.AddRange(YPos);
                    Debug.Log($"Pos:({swapCell.X},{Pos[i]}) <-- соседн€€ €чейка | MyPos:({swapCell.X},{swapCell.Y})");

                    Line.Add(new Vector2(swapCell.X, Pos[i]));
                }
                
            }
            Line.Add(new Vector2(swapCell.X, swapCell.Y));
            if (Line.Count > 2) DestroyCellLine(Line.ToArray());
        }
        
        private void DestroyCellLine(Vector2[] Line)
        {
            foreach (var cell in Line)
            {
                Debug.Log($"Destroy cell: ({cell.x};{cell.y})");
                Destroy(Matrix[(int)cell.x, (int)cell.y].cell.gameObject);
            }
        }
    }
}
