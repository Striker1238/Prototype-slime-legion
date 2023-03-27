using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Match3 {
    public class FillCell : MonoBehaviour
    {
        public GameObject[] attackPref = new GameObject[4];
        public GameObject cellPref;
        public Transform perentForPref;

        
        private static GameController _gameController;
        private static StartMatch3 _startMath3;

        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            _startMath3 = FindObjectOfType<StartMatch3>();
        }
        ///Возвращает Vector3, в котором находятся значения: X - indexX, Y - indexY, Z - количество рядом стоящих идентичных(по типу героя) ячеек
        public async Task Fill(Vector3[] indexCell)
        {
            //await Task.Delay((int)(_gameController.time*3000));
            foreach (var pos in indexCell)
            {
                if (pos.z == 0)
                {
                    var cellObj = Instantiate(cellPref, perentForPref);
                    var cell = cellObj.GetComponent<Cell>();
                    
                    cellObj.transform.localScale = new Vector2(_startMath3.CellSize.x-0.5f, _startMath3.CellSize.y - 0.5f);
                    cellObj.transform.position = new Vector3(_gameController.Matrix[(int)pos.x, (int)pos.y].x_pos, _gameController.Matrix[(int)pos.x, (int)pos.y].y_pos, perentForPref.position.z);

                    cell.thisCellObj = GenerateCell((int)pos.x, (int)pos.y);
                    cell.type = (typeHero)Random.Range(0, 4);

                    cell.attackPref = attackPref[(int)cell.type];

                    cell.levelHero = 1;

                    _gameController.Matrix[(int)pos.x, (int)pos.y].cell = cell;

                    //_gameController.CheckNewCell(_gameController.Matrix[(int)pos.x, (int)pos.y].cell);
                }
                else if(pos.z > 0 && _gameController.Matrix[(int)pos.x, (int)pos.y].cell.levelHero < _gameController.MaxLevelHero)
                {
                    _gameController.Matrix[(int)pos.x, (int)pos.y].cell.levelHero++;
                }
                _gameController.SetSpriteToObject(new Vector2(pos.x, pos.y));
                
            }
        }

        private CellPos GenerateCell(int _X, int _Y)
        {
            CellPos _cellObj = new CellPos
            {
                X = _X,
                Y = _Y,
            };
            return _cellObj;
        }
    }
}