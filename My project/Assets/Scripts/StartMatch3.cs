using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class StartMatch3 : MonoBehaviour
    {
        public int StartSteps = 5;
        
        [System.Serializable]
        public struct square
        {
            public Vector2 StartPos;
            public Vector2 EndPos;
        }

        private static GameController _gameController;
        private static FillCell _fillCell;
        public square SquareArea;
        public Vector2 CellSize;
        public Vector2 Spacing;
        public GameObject cellBackground;

        [Header("Battle")]
        public Camera mainCamera;
        public GameObject testObj;
        

        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            _fillCell = FindObjectOfType<FillCell>();
        }
        public void Start()
        {
            _gameController.CountSteps = StartSteps;
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
                    var background = Instantiate(cellBackground);
                    test[column] = new Vector3(line, column, 0);
                    _gameController.Matrix[line, column] = new GameController.cellStruct
                    { 
                        cell = null,
                        X = line,
                        Y = column,
                        x_pos = (SquareArea.StartPos.x + (CellSize.x) * line),
                        y_pos = (SquareArea.StartPos.y - (CellSize.y) * column)
                    };

                    background.transform.position = new Vector3(_gameController.Matrix[line, column].x_pos, _gameController.Matrix[line, column].y_pos, 15);
                    background.transform.localScale = CellSize;

                    background.GetComponent<SpriteRenderer>().color = ((line+column)%2==0)?new Color(0.6431373f, 0.6392157f, 0.1411765f) :new Color(0.5372549f, 0.5921569f, 0.1411765f);
                }
                _fillCell.Fill(test);
            }
        }

        public async Task StartBattle(float recedingSpeed)
        {
            _gameController.BattleIsActive = true;
            for (int x = 0; x < _gameController.Len; x++)
            {
                for (int y = 0; y < _gameController.Len; y++)
                {
                    _gameController.Matrix[x,y].cell.GetComponentInChildren<CircleCollider2D>().radius = (_gameController.Matrix[x, y].cell.type == typeHero.Swordsman)?1.5f:3;
                }
                
            }
            //отдал€ть камеру
            for (float t = 0; t <= 1 * recedingSpeed; t += Time.deltaTime)
            {
                Camera.main.fieldOfView = Mathf.Lerp(45,70, t / recedingSpeed);
                await Task.Delay(1);
            }
            

            //спавнить врагов
            for (int i = 0; i < _gameController.CountWaves; i++)
            {
                Vector3 startPos = new Vector3(Random.Range(-4, 4), 6.5f, 15);
                var enemy = Instantiate(testObj, startPos, Quaternion.identity);
                _gameController.AllEnemies.Add(enemy.GetComponent<Enemy>());
                await Task.Delay(Random.Range(50,300));
                //enemy.transform.position = new Vector3(Random.Range(-4, 4), -6, 15);
            }
            

            
        }


        //ѕеределать
        public async Task EndBattle()
        {
            _gameController.CountSteps = 5;
            for (float t = 0; t <= 1 * _gameController.RecedingSpeed; t += Time.deltaTime)
            {
                Camera.main.fieldOfView = Mathf.Lerp(70, 45, t / _gameController.RecedingSpeed);
                await Task.Delay(1);
            }
            for (int x = 0; x < _gameController.Len; x++)
            {
                for (int y = 0; y < _gameController.Len; y++)
                {
                    _gameController.Matrix[x, y].cell.GetComponentInChildren<CircleCollider2D>().radius = 0.5f;
                }

            }
        }
    }
}