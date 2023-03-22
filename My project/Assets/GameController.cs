using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GameController : MonoBehaviour
    {
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

            
            firstCell.Move(secondCell.transform.position.x, secondCell.transform.position.y, time);
            secondCell.Move(cell1X, cell1Y, time);


            int X = firstCell.X;
            int Y = firstCell.Y;
            firstCell.X = secondCell.X;
            firstCell.Y = secondCell.Y;
            secondCell.X = X;
            secondCell.Y = Y;


            pressedCell = null;
            enteredCell = null;
        }
        
    }
}
