using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class StartMath3 : MonoBehaviour
    {
        
        [System.Serializable]
        public struct square
        {
            public Vector2 StartPos;
            public Vector2 EndPos;
        }

        private static GameController _gameController;
        public GameObject cellPref;
        public Transform perentForPref;


        
        public square SquareArea;
        public Vector2 CellSize;
        public Vector2 Spacing;

        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }
        public void Start()
        {
            GenerateMatrix();
        }
        public void GenerateMatrix()
        {
            //—оздание матрицы €чеек
            _gameController.Matrix = new GameController.cellStruct[_gameController.Len, _gameController.Len];
            for (int line = 0; line < _gameController.Len; line++)
            {
                for (int column = 0; column < _gameController.Len; column++)
                {
                    var cellObj = Instantiate(cellPref, perentForPref);
                    var cell = cellObj.GetComponent<Cell>();
                    cellObj.GetComponent<RectTransform>().sizeDelta = CellSize;
                    cellObj.GetComponent<RectTransform>().localPosition = new Vector3((SquareArea.StartPos.x + (CellSize.x + Spacing.x) * line), (SquareArea.StartPos.y - (CellSize.y + Spacing.y) * column), 0);
                    cell.X = line;
                    cell.Y = column;
                    cell.type = (typeHero)Random.Range(0, 4);
                    _gameController.Matrix[line, column] = new GameController.cellStruct
                    { 
                        cell = cellObj.GetComponent<Cell>(),
                        x_pos = cellObj.transform.position.x,
                        y_pos = cellObj.transform.position.y
                    };
                    //¬ременный код
                    switch (_gameController.Matrix[line, column].cell.type)
                    {
                        case typeHero.Shooting:
                            cellObj.GetComponent<Image>().color = Color.yellow;
                            break;
                        case typeHero.Swordsman:
                            cellObj.GetComponent<Image>().color = Color.grey;
                            break;
                        case typeHero.Poisoing:
                            cellObj.GetComponent<Image>().color = Color.green;
                            break;
                        case typeHero.Freezing:
                            cellObj.GetComponent<Image>().color = Color.cyan;
                            break;
                    }
                }
            }
        }
    }
}