using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Match3
{
    public class GameController : MonoBehaviour
    {
        [System.Serializable]
        public struct cellStruct
        {
            public Cell cell;
            public int X;
            public int Y;
            public float x_pos;
            public float y_pos;
        }
        public int Len;
        [SerializeField]
        public cellStruct[,] Matrix;

        public float time;
        private static FillCell _fillCell;

        private Cell pressedCell;
        private Cell enteredCell;

        public void Awake()
        {
            _fillCell = FindObjectOfType<FillCell>();
        }
        public void OnPointerEnter(Cell cell) => enteredCell = cell;
        public void OnPointerDown(Cell cell) => pressedCell = cell;
        private bool IsAdjacent(Cell startCell, Cell endCell) =>
            (startCell.thisCellObj.X == endCell.thisCellObj.X && Mathf.Abs(startCell.thisCellObj.Y - endCell.thisCellObj.Y) == 1) ||
            (startCell.thisCellObj.Y == endCell.thisCellObj.Y && Mathf.Abs(startCell.thisCellObj.X - endCell.thisCellObj.X) == 1);
        public void OnPointerUp()
        {
            if (IsAdjacent(pressedCell, enteredCell))
            {
                //����� ����
                SwapCell(pressedCell, enteredCell);
            }
        }


        public async void SwapCell(Cell firstCell, Cell secondCell)
        {
            float cell1X = firstCell.transform.position.x;
            float cell1Y = firstCell.transform.position.y;


            Matrix[firstCell.thisCellObj.X, firstCell.thisCellObj.Y].cell = secondCell;
            Matrix[secondCell.thisCellObj.X, secondCell.thisCellObj.Y].cell = firstCell;


            CellPos test = firstCell.thisCellObj;
            firstCell.thisCellObj = secondCell.thisCellObj;
            secondCell.thisCellObj = test;


            firstCell.Move(secondCell.transform.position.x, secondCell.transform.position.y, time);
            await secondCell.Move(cell1X, cell1Y, time);
            var firstLine = LineAssembled(firstCell).Result.ToArray();
            var secondLine = LineAssembled(secondCell).Result.ToArray();

            DestroyCellLine(firstLine);
            DestroyCellLine(secondLine);


            pressedCell = null;
            enteredCell = null;
        }
        

        /// <returns>���������� Vector3, � ������� ��������� ��������: X - indexX, Y - indexY, Z - ���������� ����� ������� ����������(�� ���� �����) �����</returns>
        public async Task<List<Vector3>> LineAssembled(Cell swapCell)
        {
            List<Vector3> Line = new List<Vector3>();
            int[] XPos;
            int[] YPos;
            int countAdjecentOnMainCell = 0;
            if (swapCell.NeighboringSimilarX(out XPos) > 0 || swapCell.NeighboringSimilarY(out YPos) > 0)
            {
                List<int> Pos = new List<int>();
                swapCell.NeighboringSimilarX(out XPos);
                swapCell.NeighboringSimilarY(out YPos);
                
                
                Pos.AddRange(XPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    int countAdjecent = Matrix[Pos[i], swapCell.thisCellObj.Y].cell.NeighboringSimilarX(out XPos, (Pos[i] < swapCell.thisCellObj.X) ? 1 : 2);
                    Pos.AddRange(XPos);
                    Debug.Log($"XPos:({Pos[i]},{swapCell.thisCellObj.Y}) <-- �������� ������ |Iam: {swapCell.type}| MyPos:({swapCell.thisCellObj.X},{swapCell.thisCellObj.Y})");

                    Line.Add(new Vector3(Pos[i], swapCell.thisCellObj.Y, countAdjecent));
                }
                Pos.Clear();

                Pos.AddRange(YPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    int countAdjecent = Matrix[swapCell.thisCellObj.X, Pos[i]].cell.NeighboringSimilarY(out YPos, (Pos[i] < swapCell.thisCellObj.Y) ? 1 : 2);
                    Pos.AddRange(YPos);
                    Debug.Log($"YPos:({swapCell.thisCellObj.X},{Pos[i]}) <-- �������� ������ | MyPos:({swapCell.thisCellObj.X},{swapCell.thisCellObj.Y})");

                    Line.Add(new Vector3(swapCell.thisCellObj.X, Pos[i], countAdjecent));
                }
                
            }

            Line.Add(new Vector3(swapCell.thisCellObj.X, swapCell.thisCellObj.Y, countAdjecentOnMainCell));


            await Task.CompletedTask;
            if (Line.Count > 2)
            {
                return Line;
            }
            else return new List<Vector3>();
        }
        private void DestroyCellLine(Vector3[] Line)
        {
            //Line.ToList().Sort();
            foreach (var cell in Line)
            {
                if (cell.z < 1)
                {
                    Destroy(Matrix[(int)cell.x, (int)cell.y].cell.gameObject);
                    Matrix[(int)cell.x, (int)cell.y].cell = null;
                }
            }
            _fillCell.Fill(Line);
        }







        public void CheckNewCell(Cell swapCell)
        {
            LineAssembled(swapCell);
        }
    }
}
