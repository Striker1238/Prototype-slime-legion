using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Match3
{
    public class GameController : MonoBehaviour
    {
        [System.Serializable]
        public struct cellSprite
        {
            public List<Sprite> _sprite;
        }
        [System.Serializable]
        public struct cellStruct
        {
            public Cell cell;
            public int X;
            public int Y;
            public float x_pos;
            public float y_pos;
        }
        public struct hero
        {
            HeroAttack.Swordsman swordsman;
        }

        [Header("Matrix")]
        public int Len;
        [Tooltip("Point-to-point move time")]
        public float MoveTime;
        [SerializeField]
        public cellStruct[,] Matrix;

        [Header("Hero")]
        public int MaxLevelHero;
        public cellSprite[] HeroSprites;

        [Header("Game settings")]
        [Tooltip("Camera zoom speed")]
        public int RecedingSpeed;
        private int countSteps;
        public int CountSteps
        {
            get => countSteps;
            set 
            {
                countSteps = value;
                countStepsText.text = countSteps.ToString();
                if(countSteps <= 0) _startMatch3.StartBattle(RecedingSpeed);
            }
        }
        public int CountWaves;
        //public int EnemyMoveSpeed;

        public bool BattleIsActive = false;
        [Header("Objects")]
        public TextMeshProUGUI countStepsText;
        [HideInInspector]
        public List<Enemy> AllEnemies = new List<Enemy>();
        

        private static FillCell _fillCell;
        private static StartMatch3 _startMatch3;
        private Cell pressedCell;
        private Cell enteredCell;
        




        public void Awake()
        {
            _fillCell = FindObjectOfType<FillCell>();
            _startMatch3 = FindObjectOfType<StartMatch3>();
        }
        public void SetSpriteToObject(Vector2 indices) => 
            Matrix[(int)indices.x, (int)indices.y].cell.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = 
            HeroSprites[(int)Matrix[(int)indices.x, (int)indices.y].cell.type]._sprite[Matrix[(int)indices.x, (int)indices.y].cell.levelHero-1];
        public void OnPointerEnter(Cell cell) => enteredCell = cell;
        public void OnPointerDown(Cell cell) => pressedCell = cell;
        private bool IsAdjacent(Cell startCell, Cell endCell) =>
            (startCell.thisCellObj.X == endCell.thisCellObj.X && Mathf.Abs(startCell.thisCellObj.Y - endCell.thisCellObj.Y) == 1) ||
            (startCell.thisCellObj.Y == endCell.thisCellObj.Y && Mathf.Abs(startCell.thisCellObj.X - endCell.thisCellObj.X) == 1);
        public void OnPointerUp()
        {
            if (IsAdjacent(pressedCell, enteredCell))
            {
                
                SwapCell(pressedCell, enteredCell);
                CountSteps--;
                pressedCell = null;
                enteredCell = null;
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


            firstCell.Move(secondCell.transform.position.x, secondCell.transform.position.y, MoveTime);
            await secondCell.Move(cell1X, cell1Y, MoveTime);
            var firstLine = LineAssembled(firstCell).Result.ToArray();
            var secondLine = LineAssembled(secondCell).Result.ToArray();

            DestroyCellLine(firstLine);
            DestroyCellLine(secondLine);
        }

        /// <returns>¬озвращает Vector3, в котором наход€тс€ значени€: X - indexX, Y - indexY, Z - количество р€дом сто€щих идентичных(по типу геро€) €чеек</returns>
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
                countAdjecentOnMainCell = XPos.Length + YPos.Length;


                Pos.AddRange(XPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    int countAdjecent = Matrix[Pos[i], swapCell.thisCellObj.Y].cell.NeighboringSimilarX(out XPos, (Pos[i] < swapCell.thisCellObj.X) ? 1 : 2);
                    Pos.AddRange(XPos);
                    Debug.Log($"XPos:({Pos[i]},{swapCell.thisCellObj.Y}) <-- соседн€€ €чейка |Iam: {swapCell.type}| MyPos:({swapCell.thisCellObj.X},{swapCell.thisCellObj.Y})");

                    Line.Add(new Vector3(Pos[i], swapCell.thisCellObj.Y, countAdjecent));
                }
                Pos.Clear();

                Pos.AddRange(YPos);
                for (int i = 0; i < Pos.Count; i++)
                {
                    int countAdjecent = Matrix[swapCell.thisCellObj.X, Pos[i]].cell.NeighboringSimilarY(out YPos, (Pos[i] < swapCell.thisCellObj.Y) ? 1 : 2);
                    Pos.AddRange(YPos);
                    Debug.Log($"YPos:({swapCell.thisCellObj.X},{Pos[i]}) <-- соседн€€ €чейка | MyPos:({swapCell.thisCellObj.X},{swapCell.thisCellObj.Y})");

                    Line.Add(new Vector3(swapCell.thisCellObj.X, Pos[i], countAdjecent));
                }
                
            }

            Line.Add(new Vector3(swapCell.thisCellObj.X, swapCell.thisCellObj.Y, countAdjecentOnMainCell-1));


            await Task.CompletedTask;
            if (Line.Count > 2)
            {
                CountSteps += Line.Count-2;
                return Line;
            }
            else return new List<Vector3>();
        }
        private void DestroyCellLine(Vector3[] Line)
        {
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
        
    }
}
