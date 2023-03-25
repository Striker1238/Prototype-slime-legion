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
        private static FillCell _fillCell;
        public GameObject cellPref;
        public Transform perentForPref;


        
        public square SquareArea;
        public Vector2 CellSize;
        public Vector2 Spacing;

        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            _fillCell = FindObjectOfType<FillCell>();
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
                Vector3[] test = new Vector3[_gameController.Len];
                for (int column = 0; column < _gameController.Len; column++)
                {
                    test[column] = new Vector3(line, column, 0);
                    _gameController.Matrix[line, column] = new GameController.cellStruct
                    { 
                        cell = null,
                        X = line,
                        Y = column,
                        x_pos = (SquareArea.StartPos.x + (CellSize.x + Spacing.x) * line),
                        y_pos = (SquareArea.StartPos.y - (CellSize.y + Spacing.y) * column)
                    };
                }
                _fillCell.Fill(test);
            }
        }


        
    }
}